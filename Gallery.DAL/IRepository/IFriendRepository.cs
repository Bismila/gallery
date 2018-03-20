using Gallery.DAL.Models;
using System.Collections.Generic;

namespace Gallery.DAL.IRepository
{
    public interface IFriendRepository
    {
        void AddFriend(long IdUser, long IdFriend);

        void DelFriend(long IdFriend);

        IEnumerable<Friend> GetAllFriend(long Id);

        void DeleteAllFriendsByUser(long userId);

        IEnumerable<Friend> GetAllFriends(User currentUser);
    }
}
