using Gallery.DAL.Models;
using System;
using System.Collections.Generic;

namespace Gallery.DAL.IRepository
{
    public interface IUserRepository : IBaseRepository <User>
    {
        User GetCurrentUser(string loginName);

        void DeleteAvatar(long userId);

        IEnumerable<User> GetFriendsAndNotFriends(User currentUser);

        IEnumerable<User> SearchFriends(string searchLogin);

        IEnumerable<User> SearchUsers(string searchLogin);

        IEnumerable<User> GetAllFriendsForUser(User currentUser);
    }
}
