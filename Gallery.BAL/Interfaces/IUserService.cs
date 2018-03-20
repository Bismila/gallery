using Gallery.BAL.DTO;
using System.Collections.Generic;

namespace Gallery.BAL.Interfaces
{
    public interface IUserService :IBaseService<UserDTO>
    {
        UserDTO GetCurrentUser(string loginName);

        IEnumerable<UserDTO> SearchFriends(string searchLogin);

        IEnumerable<UserDTO> SearchUsers(string searchLogin);

        void DeleteAvatar(long userId);

        IEnumerable<UserDTO> GetAllFriendsForUser(UserDTO currentUser);

        IEnumerable<FriendDTO> GetAllFriends(UserDTO currentUser);

        IEnumerable<UserDTO> FriendsAndNotFriends(UserDTO currentUser);

        bool IsFriend(UserDTO currentUser, UserDTO mabyCurrUser);

    }
}
