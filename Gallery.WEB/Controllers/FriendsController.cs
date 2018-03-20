using Gallery.BAL.Interfaces;
using Gallery.WEB.Models;
using Gallery.DAL.Models;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gallery.BAL.DTO;

namespace Gallery.WEB.Controllers
{
    //[Authorize]
    public class FriendsController : Controller
    {
        private readonly IUserService userService;
        private readonly IFriendService friendService;
        private readonly IImageService imageService;
        private readonly ILikeService likeService;

        public FriendsController(IUserService userService, 
                                 IFriendService friendService,
                                 IImageService imageService,
                                 ILikeService likeService)
        {
            this.userService = userService;
            this.imageService = imageService;
            this.friendService = friendService;
            this.likeService = likeService;
        }

        // GET: all users and Friends (search all users and friend on enter)
        public ActionResult Index(string searchLogin)
        {
            var currentUserName = User.Identity.Name;
            var currentUs = userService.GetCurrentUser(currentUserName);

            var users = userService.FriendsAndNotFriends(currentUs).ToList();

            return View(users.Select(x => new UserViewModel
            {
                Id = (int)x.Id,
                Name = x.Name,
                Login = x.Login,
                PhotoUser = x.PhotoUser,
                Friends = x.Friends
            }));
        }

        [HttpGet] //search some friend on keyup
        public ActionResult SearchedFriends(string searchLoginFriend)
        {
            var currentUserName = User.Identity.Name;
            var currentUs = userService.GetCurrentUser(currentUserName);
            var users = userService.SearchFriends(searchLoginFriend);
          
            return PartialView(users.Select(x => new UserViewModel
            {
                Id = (int)x.Id,
                Name = x.Name,
                Login = x.Login,
                PhotoUser = x.PhotoUser,
                Friends = x.Friends
            }));
        }

        [HttpGet] //search some friend on keyup
        public ActionResult SearchedUsers(string searchLogin)
        {
            var currentUserName = User.Identity.Name;
            var currentUs = userService.GetCurrentUser(currentUserName);
            var users = userService.SearchUsers(searchLogin);

            return PartialView(users.Select(x => new UserViewModel
            {
                Id = (int)x.Id,
                Name = x.Name,
                Login = x.Login,
                PhotoUser = x.PhotoUser,
                Friends = x.Friends
            }));
        }

        [HttpGet, ActionName("AddFriend")]
        public JsonResult AddFriend(int id)
        {
            var currentUser = userService.GetCurrentUser(User.Identity.Name);
            friendService.AddFriendService(currentUser.Id, id);
            if(IsFriend(id))
            {
                var str = "true";
                return Json(str.ToString(), JsonRequestBehavior.AllowGet);
            }
            var str1 = "is not your friend...";
            return Json(str1, JsonRequestBehavior.AllowGet);


        }

        public bool IsFriend(int id)
        {
            var maybeCurrentUser = userService.Get(id);
            var currUser = userService.GetCurrentUser(User.Identity.Name);
            if (userService.IsFriend(currUser, maybeCurrentUser))
            {
                return true;
            }
            return false;
        }

        [ActionName("DelFriend")]
        public ActionResult DelFriend(int id)
        {
            friendService.DelFriendService(id);

            return RedirectToAction("MyFriends", "Friends");
        }

        //GET: MyFriends
        public ActionResult MyFriends()
        {
            var currentUserName = User.Identity.Name;
            var currentUser = userService.GetCurrentUser(currentUserName);
            var allFriends = userService.GetAllFriendsForUser(currentUser);

            if (allFriends.Count() == 0)
            {
                ViewBag.isEmpty = "null";
                return View();
            }
            else
            {
                var showAllFriends = allFriends.Select(x => new UserViewModel
                {
                    Id = (int)x.Id,
                    Name = x.Name,
                    Login = x.Login,
                    Email = x.Email,
                    PhotoUser = x.PhotoUser,
                    Images = x.Images.Select(i => new CreateUpdateDto
                    {
                        Id = i.Id,
                        Name = i.Name,
                        ImageDate = i.ImageDate,
                        PathImage = i.PathImage,
                        User = i.User,
                        UserId = i.UserId,
                        CountLikes = likeService.CountLikes(i.Id),
                        isLike = likeService.IsLike(i.Id, currentUser.Id)
                    }).ToList(),
                    Friends = x.Friends
                });
                return View(showAllFriends);
            }
        }

        public ActionResult PageMyFriend(long id)
        {
            var currentUser = userService.GetCurrentUser(User.Identity.Name);
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var friend = userService.Get(id);
            var allImgFromCurrentUser = imageService.GetAllElementsFromUser(friend.Id).Select(i => new CreateUpdateDto
            {
                Id = (int)i.Id,
                Name = i.Name,
                PathImage = i.PathImage,
                UserId = (int)i.UserId,
                CountLikes = likeService.CountLikes(i.Id),
                isLike = likeService.IsLike(i.Id, currentUser.Id)

            }).ToList();
            ViewBag.MyFriendLogin = friend.Login;
            return View(new UserViewModel
            {
                Id = (int)friend.Id,
                Login = friend.Login,
                Name = friend.Name,
                Email = friend.Email,
                Friends = friend.Friends,
                Images = allImgFromCurrentUser,
                PhotoUser = friend.PhotoUser
            });
        }
    }
}