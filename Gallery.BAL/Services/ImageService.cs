using Gallery.BAL.DTO;
using Gallery.BAL.DTO.ImagesDto;
using Gallery.BAL.Interfaces;
using Gallery.DAL.IRepository;
using Gallery.DAL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gallery.BAL.Services
{
    public class ImageService : IBaseService<CreateUpdateDto>, IImageService
    {
        private readonly IImageRepository imageRepository;
        private readonly IUserRepository userRepository;
        private readonly IFriendRepository friendRepository;
        private readonly ILikeRepository likeRepository;
        private readonly ICommentRepository commentRepository;

        public ImageService(IImageRepository _imageRepository,
                            IUserRepository _userRepository,
                            IFriendRepository _friendRepository,
                            ILikeRepository _likeRepository,
                            ICommentRepository _commentRepository)
        {
            imageRepository = _imageRepository;
            userRepository = _userRepository;
            friendRepository = _friendRepository;
            likeRepository = _likeRepository;
            commentRepository = _commentRepository;
        }
        public bool IsExistsFile(string fileName, long userId)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                    @"images\" + Directory.CreateDirectory(userId.ToString()),
                                    fileName);
            if (File.Exists(path))
            {
                return false;
            }
            return true;
        }
        public void Create(CreateUpdateImageDto element)
        {
            var currentUser = userRepository.GetCurrentUser(element.UserName);
            var elementNew = new Image
            {
                Id = element.Id,
                Name = element.Name,
                ImageDate = element.ImageDate,
                PathImage = element.PathImage,
                UserId = currentUser.Id,
                
            };
            imageRepository.Create(elementNew);
        }

        public void Create(CreateUpdateDto element)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(long id)
        {
            imageRepository.Delete(id);
        }

        public CreateUpdateDto Get(long id)
        {

            var image = imageRepository.Get(id);
            return new CreateUpdateDto
            {
                Id = image.Id,
                Name = image.Name,
                ImageDate = image.ImageDate,
                PathImage = image.PathImage,
                UserId = image.UserId,
                CountLikes = likeRepository.CountLikes(image.Id),
                isLike = likeRepository.IsLikeThisPhoto(image.Id, image.UserId),
                Comments = commentRepository.GetAllCommentsForImage(image.Id).Select(x => new CommentDTO
                {
                    Id = x.Id,
                    CommentData = x.CommentData,
                    ImageId = x.ImageId,
                    //IsEditComment = x.
                    ParentId = x.ParentId,
                    Text = x.Text,
                    UserId = x.UserId,
                    //UserLogin = userRepository.Get((long)x.UserId)
                    //UserPhoto = x.
                }).ToList()
            };
        }
        public CreateUpdateDto Get(long id, long currentUserId)
        {
            var image = imageRepository.Get(id);
            return new CreateUpdateDto
            {
                Id = image.Id,
                Name = image.Name,
                ImageDate = image.ImageDate,
                PathImage = image.PathImage,
                UserId = image.UserId,
                CountLikes = likeRepository.CountLikes(image.Id),
                isLike = likeRepository.IsLikeThisPhoto(image.Id, currentUserId),
                Comments = commentRepository.GetAllCommentsForImage(image.Id).Select(x => new CommentDTO() {
                    Id = x.Id,
                    //UserPhoto = x.
                    //UserLogin = x.
                    UserId = x.UserId,
                    Text = x.Text,
                    ParentId = x.ParentId,
                    //IsEditComment = x.
                    ImageId = x.ImageId,
                    CommentData = x.CommentData
                }).ToList()
            };
        }
        public CreateUpdateImageDto GetOneImage(long id)
        {
            var image = imageRepository.Get(id);
            var imageForUpdate = new CreateUpdateImageDto
            {
                Id = image.Id,
                Name = image.Name,
                ImageDate = image.ImageDate,
                PathImage = image.PathImage,
                UserName = image.userName,
                UserId = image.UserId,
                CountLikes = likeRepository.CountLikes(image.Id),
                isLike = likeRepository.IsLikeThisPhoto(image.Id, image.UserId),
                Comments = commentRepository.GetAllCommentsForImage(image.Id).Select(x => new CommentDTO {
                    Id = x.Id,
                    CommentData = x.CommentData,
                    ImageId = x.ImageId,
                    //IsEditComment = x.
                    ParentId = x.ParentId,
                    Text = x.Text,
                    UserId = x.UserId,
                    //UserLogin = userRepository.Get((long)x.UserId)
                    //UserPhoto = x.
                }).ToList()
            };
            return imageForUpdate;
        }

        public IEnumerable<CreateUpdateDto> GetAllElements()
        {
            var images = imageRepository.GetAllElements().Select(x => new CreateUpdateDto
            {
                Id = x.Id,
                Name = x.Name,
                ImageDate = x.ImageDate,
                PathImage = x.PathImage,
                UserId = x.UserId,
                User = x.User,
                CountLikes = likeRepository.CountLikes(x.Id),
                isLike = likeRepository.IsLikeThisPhoto(x.Id, x.UserId),
                Comments = commentRepository.GetAllCommentsForImage(x.Id).Select(img => new CommentDTO
                {
                    Id = img.Id,
                    CommentData = img.CommentData,
                    ImageId = img.ImageId,
                    //IsEditComment = x.
                    ParentId = img.ParentId,
                    Text = img.Text,
                    UserId = img.UserId,
                    //UserLogin = userRepository.Get((long)img.UserId.ToS)
                    //UserPhoto = x.
                }).ToList()
            });
            return images;
        }

        public bool IsUniqName(string name)
        {
            var allNames = GetAllElements().ToList();
            foreach (var element in allNames)
            {
                if (name == element.Name)
                    return false;
            }
            return true;
        }

        public void Update(CreateUpdateImageDto element)
        {
            var currentUser = userRepository.GetCurrentUser(element.UserName);
            var updateImage = new Image
            {
                Id = element.Id,
                Name = element.Name,
                ImageDate = element.ImageDate,
                PathImage = element.PathImage,
                UserId = currentUser.Id,
                
            };
            imageRepository.Update(updateImage);
        }

        public void Update(CreateUpdateDto element)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<CreateUpdateImageDto> GetAllElementsFromUser(long userId)
        {
            var images = imageRepository.GetAllElementsFromUser(userId).Select(x => new CreateUpdateImageDto
            {
                Id = x.Id,
                Name = x.Name,
                ImageDate = x.ImageDate,
                PathImage = x.PathImage,
                UserId = x.UserId,
                UserName = x.userName,
                CountLikes = likeRepository.CountLikes(x.Id),
                isLike = likeRepository.IsLikeThisPhoto(x.Id, userId),
                Comments = commentRepository.GetAllCommentsForImage(x.Id).Select(img => new CommentDTO
                {
                    Id = img.Id,
                    CommentData = img.CommentData,
                    ImageId = img.ImageId,
                    //IsEditComment = x.
                    ParentId = img.ParentId,
                    Text = img.Text,
                    UserId = img.UserId,
                    //UserLogin = userRepository.Get((long)x.UserId)
                    //UserPhoto = x.
                }).ToList()
            });
            return images;
        }

        public IEnumerable<CreateUpdateImageDto> GetAllElementsFromFriendsAndUser(long userId)
        {
            var allFriends = friendRepository.GetAllFriend(userId);
            var currentUser = userRepository.Get(userId);
            var imagesUser = imageRepository.GetAllElementsFromUser(userId).Select(x => new CreateUpdateImageDto
            {
                Id = x.Id,
                Name = x.Name,
                ImageDate = x.ImageDate,
                PathImage = x.PathImage,
                UserId = x.UserId,
                UserName = currentUser.Login,
                CountLikes = likeRepository.CountLikes(x.Id),
                isLike = likeRepository.IsLikeThisPhoto(x.Id, userId),
                Comments = commentRepository.GetAllCommentsForImage(x.Id).Select(comm => new CommentDTO
                {
                    Id = comm.Id,
                    CommentData = comm.CommentData,
                    ImageId = comm.ImageId,
                    ParentId = comm.ParentId,
                    Text = comm.Text,
                    UserId = comm.UserId,
                    UserLogin = currentUser.Login,
                    UserPhoto = currentUser.PhotoUser,

                }).ToList()

            });
            var imgsFrns = imageRepository.GetAllImagesFromFriends(userId).Select(x => new CreateUpdateImageDto
            {
                Id = x.Id,
                Name = x.Name,
                ImageDate = x.ImageDate,
                PathImage = x.PathImage,
                UserId = x.UserId,
                UserName = x.userName,
                CountLikes = likeRepository.CountLikes(x.Id),
                isLike = likeRepository.IsLikeThisPhoto(x.Id, userId),
                Comments = commentRepository.GetAllCommentsForImage(x.Id).Select(comm => new CommentDTO
                {
                    Id = comm.Id,
                    CommentData = comm.CommentData,
                    ImageId = comm.ImageId,
                    ParentId = comm.ParentId,
                    Text = comm.Text,
                    UserId = comm.UserId,
                    UserLogin = currentUser.Login,
                    UserPhoto = currentUser.PhotoUser
                }).ToList()
            });
            var allImages = imagesUser.Union(imgsFrns).OrderByDescending(i => i.ImageDate);
            return allImages;
        }

    }
}
