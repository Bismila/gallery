using Gallery.BAL.DTO;
using Gallery.BAL.Interfaces;
using Gallery.DAL.IRepository;
using Gallery.DAL.Models;
using System.Collections.Generic;
using System.Linq;

namespace Gallery.BAL.Services
{
    public class UserService : IBaseService<UserDTO>, IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IFriendRepository friendRepository;
        private readonly IRoleRepository roleRepository;

        public UserService(IUserRepository userRepository, IFriendRepository friendRepository, IRoleRepository roleRepository)
        {
            this.userRepository = userRepository;
            this.friendRepository = friendRepository;
            this.roleRepository = roleRepository;
        }

        public UserDTO GetCurrentUser(string loginName)
        {
            var roles = roleRepository.GetAllElements().ToList();
            var currentUser = userRepository.GetCurrentUser(loginName);
            return new UserDTO
            {
                Id = currentUser.Id,
                Name = currentUser.Name,
                Login = currentUser.Login,
                Email = currentUser.Email,
                Password = currentUser.Password,
                RoleId = currentUser.RoleId,
                PhotoUser = currentUser.PhotoUser,
                Images = currentUser.Images,
                Friends = currentUser.Friends,
                Roles = roles
            };
        }

        public IEnumerable<UserDTO> SearchFriends(string searchLogin)
        {
            var allFoundFriends = userRepository.SearchFriends(searchLogin);
            IEnumerable<UserDTO> searchingUsers = allFoundFriends.Select(x => new UserDTO
            {
                Id = x.Id,
                Name = x.Name,
                Login = x.Login,
                Email = x.Email,
                Password = x.Password,
                PhotoUser = x.PhotoUser,
                RoleId = x.RoleId,
                Friends = x.Friends,
                Images = x.Images
            });
            return searchingUsers;
        }

        public IEnumerable<UserDTO> SearchUsers(string searchLogin)
        {
            var allFoundFriends = userRepository.SearchUsers(searchLogin);
            IEnumerable<UserDTO> searchingUsers = allFoundFriends.Select(x => new UserDTO
            {
                Id = x.Id,
                Name = x.Name,
                Login = x.Login,
                Email = x.Email,
                Password = x.Password,
                PhotoUser = x.PhotoUser,
                RoleId = x.RoleId,
                Friends = x.Friends,
                Images = x.Images
            });
            return searchingUsers;
        }

        public IEnumerable<UserDTO> FriendsAndNotFriends(UserDTO currentUser)
        {
            var user = userRepository.Get(currentUser.Id);
            var fr = friendRepository.GetAllFriends(user).ToList();

            IEnumerable<UserDTO> users = userRepository.GetAllElements().Select(x => new UserDTO
            {
                Id = x.Id,
                Name = x.Name,
                Login = x.Login,
                PhotoUser = x.PhotoUser,
                RoleId = x.RoleId,
                Friends = fr,
                Images = x.Images
            });

            return users;
        }

        public bool IsFriend(UserDTO currentUser, UserDTO mabyCurrUser)
        {
            var allUsers = GetAllFriends(currentUser);
            foreach (var item in allUsers)
            {
                if(item.FriendId == mabyCurrUser.Id)
                    return true;
            }
            return false;
        }

        public IEnumerable<UserDTO> GetAllElements()
        {
            var roles = roleRepository.GetAllElements().ToList();
            var users = userRepository.GetAllElements().Select(x => new UserDTO
            {
                Id = x.Id,
                Name = x.Name,
                Login = x.Login,
                Email = x.Email,
                Password = x.Password,
                PhotoUser = x.PhotoUser,
                RoleId = x.RoleId,
                Friends = x.Friends,
                Roles = roles,
                Images = x.Images
                
            });
            return users;
        }

        public UserDTO Get(long id)
        {
            var user = userRepository.Get(id);
            var roles = roleRepository.GetAllElements().ToList();
            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Login = user.Login,
                Email = user.Email,
                Password = user.Password,
                RoleId = user.RoleId,
                PhotoUser = user.PhotoUser,
                Friends = user.Friends,
                Roles = roles,
                Images = user.Images
            };
        }

        public void Create(UserDTO item)
        {
            var user = new User
            {
                Id = item.Id,
                Name = item.Name,
                Login = item.Login,
                Email = item.Email,
                Password = item.Password,
                PhotoUser = item.PhotoUser,
                RoleId = item.RoleId,
                Friends = item.Friends,
                Images = item.Images
            };
            userRepository.Create(user);
        }

        public void Delete(long id)
        {
            friendRepository.DeleteAllFriendsByUser(id);
            userRepository.Delete(id);
        }

        public void DeleteAvatar(long userId)
        {
            userRepository.DeleteAvatar(userId);
        }

        public void Update(UserDTO element)
        {
            var user = new User
            {
                Id = element.Id,
                Name = element.Name,
                Login = element.Login,
                Email = element.Email,
                Password = element.Password,
                PhotoUser = element.PhotoUser,
                RoleId = element.RoleId,
                Friends = element.Friends,
                 Images = element.Images
            };
            userRepository.Update(user);
        }

        public IEnumerable<FriendDTO> GetAllFriends(UserDTO currentUser)
        {
            var friends = friendRepository.GetAllFriend(currentUser.Id).Select(x => new FriendDTO
            {
                UserId = x.UserId,
                FriendId = x.FriendId
            });
            return friends;
        }

        public IEnumerable<UserDTO> GetAllFriendsForUser(UserDTO currentUser)
        {
            var user = userRepository.Get(currentUser.Id);
            IEnumerable<UserDTO> users = userRepository.GetAllFriendsForUser(user).Select(x => new UserDTO
            {
                Id = x.Id,
                Name = x.Name,
                Login = x.Login,
                PhotoUser = x.PhotoUser,
                RoleId = x.RoleId,
                Friends = x.Friends,
                Images = x.Images
            });
            return users;
        }

    }
}
