using Gallery.BAL.DTO;
using Gallery.BAL.DTO.ImagesDto;
using Gallery.BAL.Interfaces;
using Gallery.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gallery.WEB.Controllers
{
    public class ImageController : Controller
    {
        private readonly IImageService imageService;
        private readonly IUserService userService;
        private readonly IFileService fileService;
        private readonly ILikeService likeService;

        public ImageController(IImageService _imageService,
                                IFileService _fileService,
                                IUserService _userService,
                                ILikeService _likeService)
        {
            imageService = _imageService;
            fileService = _fileService;
            userService = _userService;
            likeService = _likeService;
        }

        // GET: Image
        public ActionResult Index()
        {
            //get all images into home page
            UserDTO currentUser = null;

            if (User.Identity.IsAuthenticated)
            {
                var s = User.Identity.Name;
                currentUser = userService.GetCurrentUser(s);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            var allImage = imageService.GetAllElementsFromUser(currentUser.Id).OrderByDescending(i => i.ImageDate);

            var allImageCurrentUser = View(allImage.Select(x => new ImageViewModel
            {
                Id = (int)x.Id,
                Name = x.Name,
                PathImage = x.PathImage,
                UserId = (int)x.UserId,
                CountLikes = likeService.CountLikes(x.Id),
                isLike = likeService.IsLike(x.Id, currentUser.Id)
            }));

            if (allImage.Count() == 0)
            {
                ViewBag.isEmpty = "null";
                return View();
            }

            else
            {
                return View(allImage.Select(x => new ImageViewModel
                {
                    Id = (int)x.Id,
                    Name = x.Name,
                    PathImage = x.PathImage,
                    UserId = (int)x.UserId,
                    CountLikes = likeService.CountLikes(x.Id),
                    isLike = likeService.IsLike(x.Id, currentUser.Id)
                }));
            }
        }

        public ActionResult GetImageById(int id)
        {
            var image = imageService.Get(id);
            return View();
        }

        public ActionResult CreateImage()
        {
            return View("CreateImage");
        }

        [HttpPost]
        public ActionResult CreateImage(ImageViewModel image, HttpPostedFileBase file)
        {
            var currentUser = userService.GetCurrentUser(User.Identity.Name);
            file = Request.Files[0];

            if (string.IsNullOrWhiteSpace(file.FileName))
            {
                ViewBag.IsFile = "Choose file!";
                return View("CreateImage");
            }
            if (!imageService.IsExistsFile(file.FileName, currentUser.Id))
            {
                ViewBag.IsFile = "Such file already exists!!";
                return View("CreateImage");
            }
            if (string.IsNullOrWhiteSpace(image.Name))
            {
                ViewBag.IsNotFill = "Enter the name for new image!";
                return View("CreateImage");
            }
            if (!imageService.IsUniqName(image.Name))
            {
                ViewBag.IsNotFill = "Such name already exists!!";
                return View("CreateImage");
            }
         
            else
            {
                //comment part  .Replace(HttpContext.Request.PhysicalApplicationPath, String.Empty); if change
                //upload file on Azure Storage
                image.PathImage = @"~\" + fileService.UploadFile(file.InputStream, file.FileName, currentUser.Id)
                                            .Replace(HttpContext.Request.PhysicalApplicationPath, String.Empty);
            if (string.IsNullOrWhiteSpace(image.PathImage))
                {
                    ViewBag.IsFile = "Such file already exists!";
                    return View("CreateImage");
                }

                ViewBag.IsCurrentUrl = image.PathImage;

                imageService.Create(new CreateUpdateImageDto
                {
                    Id = image.Id,
                    Name = image.Name,
                    ImageDate = image.ImageDate,
                    PathImage = image.PathImage,
                    UserName = currentUser.Login,
                    CountLikes = likeService.CountLikes(image.Id),
                    isLike = likeService.IsLike(image.Id, currentUser.Id)
                });
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet, ActionName("DeleteImage")]
        public ActionResult DeleteImage(int id)
        {
            var currentImg = imageService.Get(id);
            var currentUser = userService.GetCurrentUser(User.Identity.Name);
            imageService.Delete(id);
            fileService.DeleteFile(System.Web.HttpContext.Current.Server.MapPath(currentImg.PathImage), currentUser.Id);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult EditImage(int id)
        {
            var image = imageService.GetOneImage(id);
            var currentUser = userService.GetCurrentUser(User.Identity.Name);
            if (image != null)
                return View(new ImageViewModel
                {
                    Id = (int)image.Id,
                    Name = image.Name,
                    ImageDate = image.ImageDate,
                    PathImage = image.PathImage,
                    CountLikes = likeService.CountLikes(image.Id),
                    isLike = likeService.IsLike(image.Id, currentUser.Id)
                });
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditImage(ImageViewModel elem, HttpPostedFileBase file)
        {
            file = Request.Files[0];

            var currentUser = userService.GetCurrentUser(User.Identity.Name);

            var imageOld = imageService.GetOneImage(elem.Id);

            if (file == null)
            {
                ViewBag.IsFile = "Choose file!";
                return View("EditImage");
            }
            if (elem.Name == null)
            {
                ViewBag.IsNotFill = "Enter the name for new image!";
                return View("EditImage");
            }

            else
            {
                if (string.IsNullOrWhiteSpace(file.FileName))
                {
                    imageOld = imageService.GetOneImage(elem.Id);
                    imageService.Update(new CreateUpdateImageDto
                    {
                        Id = elem.Id,
                        Name = elem.Name,
                        ImageDate = elem.ImageDate,
                        PathImage = imageOld.PathImage,
                        UserName = currentUser.Login,
                        UserId = currentUser.Id,
                        CountLikes = likeService.CountLikes(elem.Id),
                        isLike = likeService.IsLike(elem.Id, currentUser.Id)
                    });
                }
                else
                {
                    fileService.DeleteFile(System.Web.HttpContext.Current.Server.MapPath(imageOld.PathImage), currentUser.Id);

                    elem.PathImage = @"~\" + fileService.UploadFile(file.InputStream, file.FileName, currentUser.Id)
                                                    .Replace(HttpContext.Request.PhysicalApplicationPath, String.Empty);
                    imageService.Update(new CreateUpdateImageDto
                    {
                        Id = elem.Id,
                        Name = elem.Name,
                        ImageDate = elem.ImageDate,
                        PathImage = elem.PathImage,
                        UserName = currentUser.Login,
                        UserId = currentUser.Id,
                        CountLikes = likeService.CountLikes(elem.Id),
                        isLike = likeService.IsLike(elem.Id, currentUser.Id)
                    });
                }
            }
            var x = imageService.Get(elem.Id);

            return RedirectToAction("Index", "Home", x);
        }

        [HttpGet, ActionName("CountLikes")]
        public JsonResult CountLikes(long id)
        {
            long countLikes = 0;
            var currUser = userService.GetCurrentUser(User.Identity.Name);
            var currImg = imageService.Get(id, currUser.Id);

            if (likeService.ToggleLike(id, currUser.Id))
            {
                countLikes = likeService.CountLikes(id);
                currImg = imageService.Get(id, currUser.Id);
                return Json(currImg, JsonRequestBehavior.AllowGet);
            }
            currImg = imageService.Get(id, currUser.Id);
            return Json(currImg, JsonRequestBehavior.AllowGet);

        }
    }
}