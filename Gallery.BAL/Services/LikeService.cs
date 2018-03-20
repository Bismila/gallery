using Gallery.BAL.DTO;
using Gallery.BAL.Interfaces;
using Gallery.DAL.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BAL.Services
{
    public class LikeService : ILikeService
    {
        private readonly ILikeRepository likeRepository;

        public LikeService(ILikeRepository likeRepository)
        {
            this.likeRepository = likeRepository;
        }

        public void AddLike(long ImageId, long UserId)
        {
            likeRepository.AddLike(ImageId, UserId);
        }

        public long CountLikes(long ImageId)
        {
            var count = likeRepository.CountLikes(ImageId);
            return count;
        }

        public void DeleteLike(long ImageId, long UserId)
        {
            likeRepository.DeleteLike(ImageId, UserId);
        }

        public IEnumerable<LikeDTO> GetLikesByUserId(long userId)
        {
            var allLikes = likeRepository.GetLikesByUserId(userId).Select(x=> new LikeDTO
            {
                ImageId = x.ImageId,
                UserId = x.UserId
            });
          
            return allLikes;
        }

        public bool ToggleLike(long ImageId, long UserId)
        {
            var allLikes = GetLikesByUserId(UserId);
            var likeImageFromCurrentUser = allLikes.Where(x => x.ImageId == ImageId).FirstOrDefault();
            if(likeImageFromCurrentUser != null)
            {
                likeRepository.DeleteLike(ImageId, UserId);
                return false;
            }
            likeRepository.AddLike(ImageId, UserId);
            return true;

        }

        public bool IsLike(long imgId, long currentUserId)
        {

            var allLikes = GetLikesByUserId(currentUserId);
            var likesImageFromCurrentUser = allLikes.Where(x => x.ImageId == imgId).FirstOrDefault();

            if (likesImageFromCurrentUser == null)
            {
                return false;
            }
            return true;
        }
    }
}
