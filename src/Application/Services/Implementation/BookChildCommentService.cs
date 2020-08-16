﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dto.Comment.Book;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.NoSQL;
using Domain.NoSQL.Entities;
using MongoDB.Driver;

namespace Application.Services.Implementation
{
    public class BookChildCommentService : IBookChildCommentService
    {
        private readonly IChildRepository<BookRootComment, BookChildComment> _childCommentRepository;
        private readonly IBookRootCommentService _bookRootCommentService;
        private readonly IMapper _mapper;

        public BookChildCommentService(IChildRepository<BookRootComment, BookChildComment> childCommentRepository,
            IBookRootCommentService bookRootCommentService,
            IMapper mapper)
        {
            _childCommentRepository = childCommentRepository;
            _bookRootCommentService = bookRootCommentService;
            _mapper = mapper;
        }

        public async Task<int> Add(ChildInsertDto insertDto)
        {
            string rootId = insertDto.Ids.First();
            List<(string nestedArrayName, string itemId)> path = insertDto.Ids.Skip(1).Select(x => ("Comments", x)).ToList();

            var updateResult = await _childCommentRepository.PushAsync(
                rootId,
                new BookChildComment(true)
                {
                    Text = insertDto.Text,
                    OwnerId = insertDto.OwnerId,
                    Date = DateTime.Now.ToUniversalTime().ToString()
                },
                path,
                "Comments");

            return Convert.ToInt32(updateResult.ModifiedCount);
        }

        public async Task<int> Remove(IEnumerable<string> ids)
        {
            string rootId = ids.First();
            string childId = ids.Last();
            var rootComment = await _bookRootCommentService.GetById(rootId);
            var childComment = await FindChild(rootComment.Comments, 
                childId);
            if (childComment != null && childComment.Comments.Any())
            {
                return await SetAsDeleted(ids, childComment, rootId);
            }

            return await Delete(ids, rootId, childId);
        }

        private async Task<int> SetAsDeleted(IEnumerable<string> ids, ChildDto childComment, string rootId)
        {
            var children = childComment.Comments.Select(c => _mapper.Map<ChildDto, BookChildComment>(c));
            List<(string nestedArrayName, string itemId)> path = ids.Skip(1).Select(x => ("Comments", x)).ToList();
            UpdateResult updateResult = await _childCommentRepository.SetAsync(
                rootId,
                new BookChildComment() {IsDeleted = true, Text = childComment.Text, Comments = children},
                path
            );
            return (int) updateResult.ModifiedCount;
        }

        private async Task<int> Delete(IEnumerable<string> ids, string rootId, string childId)
        {
            List<(string nestedArrayName, string itemId)> path = ids.Skip(1).SkipLast(1).Select(x => ("Comments", x)).ToList();
            UpdateResult updateResult = await _childCommentRepository.PullAsync(
                rootId,
                childId,
                path,
                "Comments");
            RootDto rootComment = await _bookRootCommentService.GetById(rootId);
            if (!HasActiveComments(rootComment.Comments) && rootComment.IsDeleted)
            {
                await _bookRootCommentService.Remove(rootId);
            }

            if (rootComment.Comments.Any())
            {
                await ClearCommentBranch(rootComment, ids.Skip(1).SkipLast(1));
            }

            return (int) updateResult.ModifiedCount;
        }

        public async Task<int> Update(ChildUpdateDto updateDto)
        {
            string rootId = updateDto.Ids.First();
            string childId = updateDto.Ids.Last();

            var rootComment = await _bookRootCommentService.GetById(rootId);
            var childComment = await FindChild(rootComment.Comments,
                childId);
            var children = childComment.Comments.Select(c => _mapper.Map<ChildDto, BookChildComment>(c));

            List<(string nestedArrayName, string itemId)> path = updateDto.Ids.Skip(1).Select(x => ("Comments", x)).ToList();

            var updateResult = await _childCommentRepository.SetAsync(
                rootId,
                new BookChildComment() {Text = updateDto.Text, Comments = children},
                path);

            return Convert.ToInt32(updateResult.MatchedCount);
        }

        private async Task<ChildDto> FindChild(IEnumerable<ChildDto> children, string childId)
        {
            var searchedChild = children.FirstOrDefault(c => c.Id == childId);
            if (searchedChild != null)
            {
                return searchedChild;
            }

            foreach (var child in children)
            {
                if (child.Comments.Any())
                {
                    var result = await FindChild(child.Comments, childId);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }

        public async Task ClearCommentBranch(RootDto root, IEnumerable<string> ids)
        {
            var path = new List<(string nestedArrayName, string itemId)>();
            foreach (var id in ids)
            {
                var child = await FindChild(root.Comments, id);
                if (child.IsDeleted && !HasActiveComments(child.Comments))
                {
                    var rootId = root.Id;
                    var childId = child.Id;

                    await _childCommentRepository.PullAsync(
                        rootId,
                        childId,
                        path,
                        "Comments");

                    return;
                }

                path.Add(("Comments", id));
            }
        }

        private bool HasActiveComments(IEnumerable<ChildDto> children)
        {
            if (!children.Any())
            {
                return false;
            }

            if (children.Any(c => c.IsDeleted == false))
            {
                return true;
            }

            foreach (var child in children)
            {
                if (HasActiveComments(child.Comments))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
