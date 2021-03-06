﻿namespace PaintStore.Domain.Interfaces
{
    public interface IPosts
    {
        int Id { get; set; }
        int UserId { get; set; }
        string UserOwnerName { get; set; }
        string Title { get; set; }
        string ImgLink { get; set; }
        int LikeCount { get; set; }
        int ViewCount { get; set; }
        int CommentsCount { get; set; }
    }
}
