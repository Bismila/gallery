using Gallery.BAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BAL.Interfaces
{
    public interface ILikeService
    {
        void AddLike(long ImageId, long UserId);
        long CountLikes(long ImageId);
        void DeleteLike(long ImageId, long UserId);

        bool ToggleLike(long ImageId, long UserId);
        bool IsLike(long imgId, long currentUserId);

        IEnumerable<LikeDTO> GetLikesByUserId(long userId);
    }
}
