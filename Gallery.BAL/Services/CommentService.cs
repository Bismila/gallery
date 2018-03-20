using Gallery.BAL.DTO;
using Gallery.BAL.Interfaces;
using Gallery.DAL.IRepository;
using Gallery.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BAL.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository commentRepository;
        private readonly IUserRepository userRepository;

        public CommentService(ICommentRepository commentRepository, IUserRepository userRepository)
        {
            this.commentRepository = commentRepository;
            this.userRepository = userRepository;
        }

        public CommentDTO AddComment(CommentDTO comment)
        {
            var currUser = userRepository.GetCurrentUser(comment.UserLogin);
            commentRepository.AddComment(new Comment
            {
                ImageId = comment.ImageId,
                UserId = currUser.Id,
                Text = comment.Text,
                CommentData = comment.CommentData,
                ParentId = comment.ParentId
            });
            var comm = commentRepository.Get(comment.ImageId);
            var item = new CommentDTO
            {
                Id = comm.Id,
                ImageId = comment.ImageId,
                UserId = currUser.Id,
                UserLogin = currUser.Login,
                UserPhoto = currUser.PhotoUser,
                Text = comment.Text,
                CommentData = comment.CommentData,
                ParentId = comment.ParentId
            };
            return item;
        }

        public void DeleteComment(long Id)
        {
            commentRepository.DeleteComment(Id);
        }

        public void UpdateComment(CommentDTO comment)
        {
            var comm = commentRepository.Get(comment.Id);
            if (!comment.Text.Equals(comm.Text))
            {
                var item = new Comment
                {
                    Id = comment.Id,
                    UserId = comment.UserId,
                    ImageId = comment.ImageId,
                    Text = comment.Text,
                    CommentData = comm.CommentData,
                    ParentId = comment.ParentId,
                };
                commentRepository.UpdateComment(item);
            }
           
        }

        public IEnumerable<CommentDTO> GetAllCommentsForImage(long ImageId)
        {
            var allCommentForImage = commentRepository.GetAllCommentsForImage(ImageId).Select(x => new CommentDTO
            {
                Id = x.Id,
                UserId = x.UserId,
                ImageId = x.ImageId,
                UserLogin = userRepository.Get(x.UserId).Login,
                UserPhoto = userRepository.Get(x.UserId).PhotoUser,
                Text = x.Text,
                CommentData = x.CommentData,
                ParentId = x.ParentId,
                IsEditComment = IsEditDate(x.CommentData)
            });
            return allCommentForImage;
        }

        private bool IsEditDate(DateTime dateAddComment)
        {
            var currTime = DateTime.Now;
            var dateAddCommToMillSec = dateAddComment.AddMinutes(10);
            if ((dateAddCommToMillSec) > currTime)
                return true;
            return false;
        }

        public CommentDTO Get(long imgId)
        {
            var comment = commentRepository.Get(imgId);
            var currUser = userRepository.Get(comment.UserId);
            return new CommentDTO
            {
                Id = comment.Id,
                CommentData = comment.CommentData,
                ImageId = comment.ImageId,
                ParentId = comment.ParentId,
                Text = comment.Text,
                UserId = comment.UserId,
                UserLogin = currUser.Login,
                UserPhoto = currUser.PhotoUser,
                IsEditComment = IsEditDate(comment.CommentData)

            };
        }
    }
}
