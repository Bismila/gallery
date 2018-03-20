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
    public class FriendService : IFriendService
    {
        private readonly IFriendRepository friendRepository;
        private readonly IImageRepository imageRepository;

        public FriendService(IFriendRepository friendRepository, IImageRepository imageRepository)
        {
            this.friendRepository = friendRepository;
            this.imageRepository = imageRepository;
        }

        public void AddFriendService(long IdUser, long IdFriend)
        {
            var friend = new Friend
            {
                UserId = IdUser,
                FriendId = IdFriend

            };
            friendRepository.AddFriend(IdUser, IdFriend);
        }

        public void DelFriendService(long IdFriend)
        {
            friendRepository.DelFriend(IdFriend);
        }

        public IEnumerable<FriendDTO> GetAllFriendsService(long userId)
        {
            var friends = friendRepository.GetAllFriend(userId).Select(x => new FriendDTO
            {
               UserId = x.UserId,
               FriendId = x.FriendId
            });
            return friends;
        }

        public bool IsFriend(long userId, long friendId)
        {
            var allFriends = GetAllFriendsService(userId).ToList();
            foreach (var item in allFriends)
            {
                if (item.FriendId == friendId)
                {
                    return true;
                }                 
            }
            return false;
        }

        public IEnumerable<CreateUpdateDto> GetAllImageFriends(long currentUserId)
        {
            var allImageFriends = imageRepository.GetAllImagesFromFriends(currentUserId).Select(x=> new CreateUpdateDto
            {
                Id = x.Id,
                Name = x.Name,
                PathImage = x.PathImage,
                User = x.User
            });
            return allImageFriends;
        }

     
    }
}
