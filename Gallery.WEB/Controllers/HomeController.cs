using Gallery.BAL.Providers;
using Gallery.BAL.DTO;
using Gallery.BAL.Interfaces;
using Gallery.WEB.Models;
using System.Linq;
using System.Web.Mvc;

namespace Gallery.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly IImageService imgService;
        private readonly IUserService userService;
        private readonly IFileService fileService;
        private readonly IFriendService friendService;
        private readonly ILikeService likeService;
        private readonly ICommentService commentService;
        CustomRoleProvider rPr;

        public HomeController(IFriendService _friendService,
                              IUserService _userService,
                              IImageService _imageService, 
                              IFileService _fileService,
                              ILikeService _likeService,
                              ICommentService _commentService,
                              CustomRoleProvider role)
        {
            friendService = _friendService;
            userService = _userService;
            imgService = _imageService;
            fileService = _fileService;
            likeService = _likeService;
            commentService = _commentService;
            rPr = role;
        }

        [Authorize]
        public ActionResult Index()
        {
            //get all images into home page
            UserDTO currentUser = new UserDTO();
            //var countLikesForImg = 
            if (User.Identity.IsAuthenticated)
            {
                var s = User.Identity.Name;
                currentUser = userService.GetCurrentUser(s);

                var allImage = imgService.GetAllElementsFromFriendsAndUser(currentUser.Id);

                var allImagesOrderByDate = allImage;
                ViewBag.PhotoUserUrl = currentUser.PhotoUser;
                ViewBag.CurrentUserId = currentUser.Id;
                //TempData["currentUser"] = currentUser;

                if (allImage.Count() == 0)
                {
                    ViewBag.isEmpty = "null";

                    return View();
                }

                else
                {
                    var showNineImageFromFriendsAndUser = allImagesOrderByDate.Select(x => new ImageViewModel
                    {
                        Id = (int)x.Id,
                        Name = x.Name,
                        ImageDate = x.ImageDate,
                        PathImage = x.PathImage,
                        UserId = (int)x.UserId,
                        UserName = x.UserName,
                        CountLikes = likeService.CountLikes(x.Id),
                        isLike = likeService.IsLike(x.Id, currentUser.Id),
                        Comments = commentService.GetAllCommentsForImage(x.Id).ToList()
                    });

                    return View(showNineImageFromFriendsAndUser);
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        //[HttpGet, ActionName("AddComment")]
        //public JsonResult AddComment(int imgId, string textComment)
        //{
        //    var currUser = userService.GetCurrentUser(User.Identity.Name);
        //    if (!string.IsNullOrWhiteSpace(textComment))
        //    {
        //        commentService.AddComment((long)imgId, textComment, currUser);
        //        var comm = commentService.Get(imgId);

        //        var result = new { Id = comm.Id, Comments = comm, UserWriter = currUser };
        //        //return this.Jsonp(result);

        //        return Json(comm, JsonRequestBehavior.AllowGet);
        //    }
        //    var str = "Your comments is empty!";
        //    return Json(str, JsonRequestBehavior.AllowGet);

        //}

    }
}