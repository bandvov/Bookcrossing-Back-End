﻿namespace Application.Dto.Comment.Book
{
    public class RootUpdateDto
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public int CommentOwnerId { get; set; }
    }
}
