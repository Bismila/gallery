using Gallery.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.DAL.IRepository
{
   public interface ILikeRepository
    {
        void AddLike(long ImageId, long UserId);
        long CountLikes(long ImageId);
        void DeleteLike(long ImageId, long UserId);
        IEnumerable<Like> GetLikesByUserId(long userId);

        bool IsLikeThisPhoto(long idImg, long idUser);
    }
}
