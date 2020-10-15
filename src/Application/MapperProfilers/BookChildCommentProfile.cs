﻿using System;
using System.Globalization;
using AutoMapper;
using NoSqlEntities = Domain.NoSQL.Entities;
namespace Application.MapperProfilers
{
    public class BookChildCommentProfile : Profile
    {
        public BookChildCommentProfile()
        {
            CreateMap<NoSqlEntities.BookChildComment, Dto.Comment.Book.ChildDto>()
               .ForMember(dto => dto.Date, opt => opt.MapFrom(entity => DateTime.Parse(entity.Date).ToLocalTime()))
               .ForMember(dto => dto.Comments, opt => opt.MapFrom(entity => entity.Comments))
               .ForMember(dto => dto.Owner, opt => opt.MapFrom(entity => new Dto.Comment.OwnerDto() { Id = entity.OwnerId }))
               .ReverseMap();
        }
    }
}
