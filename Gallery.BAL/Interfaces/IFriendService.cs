using Gallery.BAL.DTO;
using System.Collections.Generic;


namespace Gallery.BAL.Interfaces
{
    public interface IFriendService
    {
        void AddFriendService(long IdUser, long IdFriend);
        void DelFriendService(long IdFriend);
        IEnumerable<FriendDTO> GetAllFriendsService(long userId);
        bool IsFriend(long userId, long friendId);
        IEnumerable<CreateUpdateDto> GetAllImageFriends(long currentUserId);
       
    }
}
