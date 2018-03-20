using Gallery.DAL.IRepository;
using Gallery.DAL.Models;
using Gallery.DAL.EFInfrastructure.EFContext;
using Gallery.DAL.RepositoryClasses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.DAL.EFInfrastructure.EFRepository
{
    public class EFUserRepository : IUserRepository
    {
        private readonly IDbSet<User> _userSet;
        private readonly IDbSet<Image> _imageSet;
        private readonly IDbSet<Role> _roleSet;
        private readonly IDbSet<Friend> _friendSet;
        private readonly DbContext _context;

        public EFUserRepository(DbContext context)
        {
            _context = context;
            _userSet = context.Set<User>();
            _imageSet = context.Set<Image>();
            _roleSet = context.Set<Role>();
            _friendSet = context.Set<Friend>();

        }

        public IEnumerable<User> GetAllElements()
        {
            List<User> users = new List<User>();
            try
            {
                users = _userSet.AsNoTracking().ToList();
            }
            catch (Exception e)
            {
                var ex = e.InnerException;
            }
            return users;
        }

        public void Create(User element)
        {
            var newUser = new User()
            {
                Name = element.Name,
                Login = element.Login,
                Password = element.Password,
                Email = element.Email,
                RoleId = element.RoleId,
                PhotoUser = element.PhotoUser,
            };
            _userSet.Add(newUser);

            _context.Entry(newUser).State = EntityState.Added;

            _context.SaveChanges();
        }

        public void Delete(long id)
        {
            var user = _userSet.Find(id);

            _context.Entry(user).State = EntityState.Deleted;

            _context.SaveChanges();
        }

        public void DeleteAvatar(long userId)
        {
            var userForDeletePhoto = _userSet.Find(userId);
            userForDeletePhoto.PhotoUser = "";

            _context.Entry(userForDeletePhoto).Property(img => img.PhotoUser).IsModified = true;

            _context.SaveChanges();
        }

        public User Get(long id)
        {
            var user = _userSet.Find(id);
            return user;
        }

        public IEnumerable<User> GetAllFriendsForUser(User currentUser)
        {
            var allFriends = _userSet.Join(_friendSet,
                                u => u.Id, 
                                f => f.FriendId, 
                                (u, f) => new { u, f })
                                        .Where(us=>us.f.UserId == currentUser.Id)
                                        .Select(x=> x.u);

            return allFriends.ToList();
        }

        public User GetCurrentUser(string loginName)
        {
            var user = _userSet.Where(name => name.Login == loginName).First();

            return user;
        }

        public IEnumerable<User> GetFriendsAndNotFriends(User currentUser)
        {
            var all = (from u in _userSet
                       join f in _friendSet on u.Id equals f.FriendId
                       where f.UserId == currentUser.Id
                       select u).ToList();
            return all;
        }

        public IEnumerable<User> SearchFriends(string searchLogin)
        {
            var searchedFriends = _friendSet.Join(_userSet,
                                     f => f.FriendId,
                                     u => u.Id,
                                     (f, u) => new { u })
                                             .Where(us => us.u.Login.Contains(searchLogin))
                                             .Select(x => x.u).ToList();

            return searchedFriends;
        }

        public IEnumerable<User> SearchUsers(string searchLogin)
        {
            return _userSet.Where(u => u.Login.Contains(searchLogin)).ToList();
        }

        public void Update(User element)
        {
            var updateUser = _userSet.Find(element.Id);

            _context.Entry(updateUser).CurrentValues.SetValues(element);

            _context.SaveChanges();
        }
    }
}
