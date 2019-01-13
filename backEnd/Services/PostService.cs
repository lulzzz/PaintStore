﻿using System;
using System.Collections.Generic;
using System.Linq;
using backEnd.Controllers.CategoryControllers;
using backEnd.Interfaces;
using backEnd.Managers;
using backEnd.Models;
using backEnd.Models.ResultsModels;

namespace backEnd.Services
{
    public class PostService : IPostsService
    {
        private readonly PaintStoreContext _paintStoreContext;

        public PostService(PaintStoreContext ctx)
        {
            _paintStoreContext = ctx;
        }
        public PostDetailsResult GetPost(int userId, int postId)
        {
            using (var db = _paintStoreContext)
            {
                var post = db.Posts.First(b => b.Id == postId);
                var tagsIds = db.PostTags.Where(x => x.PostId == postId).Select(y => y.TagId).ToList();

                var tagsList = db.Tags.Where(tag =>
                    tagsIds.Contains(tag.Id)).Select(x => x.TagName).ToList();

                bool liked = db.PostLikes.Any(x => x.PostId == postId && x.UserId == userId);

                var postDetailsResult = new PostDetailsResult(post)
                {
                    CreationDate = post.CreationDate,
                    Description = post.Description,
                    TagsList = tagsList,
                    Liked = liked
                };

                return postDetailsResult;
            }
        }

        public List<PostsResults> GetFollowingPosts(int userId)
        {
            List<PostsResults> imagesResult = new List<PostsResults>();
            using (var db = _paintStoreContext)
            {
                var images = db.Posts.
                    Where(x => db.UserFollowers.
                        Where(y => y.FollowingUserId == userId).
                        Select(z => z.FollowedUserId).Contains(x.UserId)).
                    OrderByDescending(x => x.CreationDate);
                foreach (var image in images)
                {
                    var userOwnerImgLink = db.Users.First(x => x.Id == image.UserId).AvatarImgLink;
                    imagesResult.Add(new PostsResults(image){UserOwnerImgLink = userOwnerImgLink});
                }
                
            }
            return imagesResult;
        }

        public List<PostsResults> GetAllPosts(string message)
        {
            List<PostsResults> imagesResult = new List<PostsResults>();
            using (var db = _paintStoreContext)
            {
                IQueryable<IPosts> images = null;
                if (message == "most_popular")
                {
                    images = db.Posts.OrderByDescending(y => (db.PostComments.Count(x => x.PostId == y.Id)));
                }
                if (message == "the_newest")
                {
                    images = db.Posts.OrderByDescending(x => x.CreationDate);
                }
                foreach (var image in images)
                {
                    var userOwnerImgLink = db.Users.First(x => x.Id == image.UserId).AvatarImgLink;
                    imagesResult.Add(new PostsResults(image){UserOwnerImgLink = userOwnerImgLink});
                }
                
            }
            return imagesResult;
        }

        public List<PostsResults> GetPostsByTag(string tag)
        {
            List<PostsResults> imagesResult = new List<PostsResults>();
            using (var db = _paintStoreContext)
            {
                var tagId = db.Tags.First(x => x.TagName == tag).Id;
                var postsIds = db.PostTags.Where(x => x.TagId == tagId).Select(y => y.PostId).ToList();
                IQueryable<Posts> images = null;

                images = db.Posts.Where(post =>
                    postsIds.Contains(post.Id));
                    
                foreach (var image in images)
                {
                    var userOwnerImgLink = db.Users.First(x => x.Id == image.UserId).AvatarImgLink;
                    imagesResult.Add(new PostsResults(image){UserOwnerImgLink = userOwnerImgLink});
                }
            }

            return imagesResult;
        }

        public Posts AddImage(Posts post)
        {
            using(var db = _paintStoreContext)
            {
                UsersManager.UserPostsCountPlus(db, post.UserId);
                post.CreationDate = DateTime.Now;
                db.Posts.Add(post);
                var count = db.SaveChanges();
            }
            return post;
        }

        public Posts EditPost(Posts post)
        {
            using (var db = _paintStoreContext)
            {
                var postToUptade = db.Posts.First(x => x.Id == post.Id);

                if (post.Title != null) postToUptade.Title = post.Title;
                if (post.Description != null) postToUptade.Description = post.Description;
                postToUptade.Edited = true;
                db.SaveChanges();
                return postToUptade;
            }
        }

        public int PostRemover(int postId)
        {
            using (var db = _paintStoreContext)
            {
                var tempPost = db.Posts.First(x => x.Id == postId);

                UsersManager.UserPostsCountMinus(db, tempPost.UserId);

                var tagsToRemove = db.PostTags.Where(x => x.PostId == postId).ToList();
                foreach (var tagToRemove in tagsToRemove)
                {
                    db.PostTags.Remove(tagToRemove);
                    TagsManager.TagsCountMinus(db, tagToRemove.TagId);
                }

                db.Posts.Remove(db.Posts.First(x => x.Id == postId));
                foreach (var like in db.PostLikes.Where(x => x.PostId == postId))
                {
                    db.PostLikes.Remove(db.PostLikes.First(x => x.Id == like.Id));
                }

                foreach(var comment in db.PostComments.Where(x => x.PostId == postId))
                {
                    db.PostComments.Remove(db.PostComments.First(x => x.Id == comment.Id));
                    foreach (var like in db.CommentLikes.Where(x => x.CommentId == comment.Id))
                    {
                        db.CommentLikes.Remove(db.CommentLikes.First(x => x.Id == like.Id));
                    }
                }

                db.SaveChanges();
                return postId;
            }
        }
    }
}
