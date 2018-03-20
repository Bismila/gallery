using Gallery.BAL.DTO;
using Gallery.BAL.Encryption;
using Gallery.BAL.Interfaces;
using Gallery.DAL.Models;
using Gallery.WEB.Models;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace Gallery.WEB.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IFileService fileService;
        private readonly IImageService imageService;
        private readonly IRoleService roleService;
        public UserController(IUserService userService, IFileService fileService, IImageService imageService, IRoleService roleService)
        {
            this.userService = userService;
            this.fileService = fileService;
            this.imageService = imageService;
            this.roleService = roleService;
        }

        // GET: User
        public ActionResult Index()
        {
            var users = userService.GetAllElements().ToList();
            var user = View(users.Select(x => new UserViewModel
            {
                Id = (int)x.Id,
                Name = x.Name,
                Login = x.Login,
                Email = x.Email,
                Password = x.Password,
                PhotoUser = x.PhotoUser,
                RoleId = (int)x.RoleId,
                Roles = x.Roles
            }));
            return user;
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost, ActionName("Create")]
        public ActionResult Create(UserViewModel user)
        {
            var us = new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Login = user.Login,
                Password = user.Password,
                Email = user.Email,
                PhotoUser = user.PhotoUser,
                RoleId = user.RoleId,
                Roles = user.Roles
            };
            userService.Create(us);
            return RedirectToAction("/Index");
        }

        // GET: Users/Delete
        public ActionResult Delete(long id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var currentUser = userService.Get(id);
            var currentUserImages = imageService.GetAllElementsFromUser(currentUser.Id);

            if (!string.IsNullOrWhiteSpace(currentUser.PhotoUser))
            {
                fileService.DeleteFile(currentUser.PhotoUser, currentUser.Id);
            }
            if (currentUserImages.Count() != 0)
            {
                fileService.DeleteAllFile(currentUser.Id);
            }
            userService.Delete(id);

            return RedirectToAction("/Index");
        }

        [Authorize]
        [HttpGet]
        public ActionResult MyPage()
        {
            var name = User.Identity.Name;

            var user = userService.GetCurrentUser(name);
            var userViewModel = new UserViewModel
            {
                Id = (int)user.Id,
                Name = user.Name,
                Login = user.Login,
                Email = user.Email,
                PhotoUser = user.PhotoUser,
                Password = user.Password,
                RoleId = (int)user.RoleId,
                Roles = user.Roles
            };
            return View(userViewModel);
        }

        [HttpGet]
        // GET: Users/Edit/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = userService.Get(id.Value);
            var userViewModel = new UserViewModel
            {
                Id = (int)user.Id,
                Name = user.Name,
                Login = user.Login,
                Email = user.Email,
                Password = user.Password,
                PhotoUser = user.PhotoUser,
                RoleId = (int)user.RoleId,
                Roles = user.Roles
            };
            return View(userViewModel);
        }

        [HttpGet]
        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = userService.Get(id.Value);

            var userViewModel = new EditUserViewModel
            {
                Id = (int)user.Id,
                Name = user.Name,
                Login = user.Login,
                Password = user.Password,
                OldLogin = user.Login,
                NewPassword = "",
                Email = user.Email,
                PhotoUser = user.PhotoUser,
                RoleId = (int)user.RoleId,
                Roles = user.Roles

            };

            if (user == null)
            {
                return HttpNotFound();
            }
            if (string.IsNullOrEmpty(userViewModel.PhotoUser))
            {
                ViewBag.PhotoUserUrl = "/images/user-icon.png";
            }
            else
            {
                ViewBag.PhotoUserUrl = userViewModel.PhotoUser;
            }

            return View(userViewModel);
        }

        // POST: Users/Edit/5
        [HttpPost, ActionName("Edit")]
        public ActionResult Edit(EditUserViewModel user, HttpPostedFileBase file)
        {
            file = Request.Files[0];
            var currentUser = userService.GetCurrentUser(User.Identity.Name);
            var userOld = userService.Get(user.Id);

            string hash = "";
            string photo = user.PhotoUser;

            if (User.IsInRole("admin"))
            {
                var roleId = roleService.RoleIdByRoleName(user.UpdateUserRole);
          
                if (string.IsNullOrWhiteSpace(user.Login))
                {
                    var r = roleService.GetAllElements().ToList();
                    @TempData["Message"] = "This is a required field";
                    return Redirect("/User/Edit/" + user.Id);
                }

                if (!user.Login.Equals(user.OldLogin))
                {
                    var login = userService.GetAllElements().FirstOrDefault(log => log.Login == user.Login);
                    if (login != null)
                    {
                        @TempData["Message"] = "This login is used";
                        return Redirect("/User/Edit/" + user.Id);
                    }
                }

                if (!string.IsNullOrWhiteSpace(user.NewPassword))
                {
                    if (user.NewPassword.Length <6)
                    {
                        @TempData["PasswordMessage"] = "The password must be at least 6 characters";
                        return Redirect("/User/Edit/" + user.Id);
                    }
                    string source = user.NewPassword;
                    using (MD5 md5Hash = MD5.Create())
                    {
                        hash = EncryptPassword.GetMd5Hash(md5Hash, source);
                    }
                }

                if (string.IsNullOrWhiteSpace(user.NewPassword))
                {
                    hash = user.Password;
                }

                if (file != null && !string.IsNullOrWhiteSpace(file.FileName))
                {
                    if (user.Name == userOld.Name && !string.IsNullOrWhiteSpace(userOld.PhotoUser))
                    {
                        fileService.DeleteFile(userOld.PhotoUser, user.Id);
                    }
                    photo = fileService.UploadFile(file.InputStream, file.FileName, user.Id);
                }

                if (file != null && string.IsNullOrWhiteSpace(file.FileName))
                {
                    photo = user.PhotoUser;
                }

                userService.Update(new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Login = user.Login,
                    Password = hash,
                    Email = user.Email,
                    PhotoUser = photo,
                    RoleId = roleId,
                    Roles = user.Roles
                });
                return Redirect("/User/Index");
            }

            else
            {
                if (file != null && !string.IsNullOrWhiteSpace(file.FileName))
                {
                    if (user.Name == userOld.Name && !string.IsNullOrWhiteSpace(userOld.PhotoUser))
                    {
                        fileService.DeleteFile(userOld.PhotoUser, currentUser.Id);
                    }
                    user.PhotoUser = fileService.UploadFile(file.InputStream, file.FileName, currentUser.Id);

                    userService.Update(new UserDTO
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Login = user.Login,
                        Password = user.Password,
                        Email = user.Email,
                        PhotoUser = user.PhotoUser,
                        RoleId = user.RoleId,
                        Roles = user.Roles
                    });
                }

                if (file != null && string.IsNullOrWhiteSpace(file.FileName))
                {
                    userService.Update(new UserDTO
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Login = user.Login,
                        Email = user.Email,
                        Password = user.Password,
                        PhotoUser = userOld.PhotoUser,
                        RoleId = user.RoleId,
                        Roles = user.Roles
                    });
                }
                return Redirect("/Home/Index");
            }
        }


        public ActionResult DeleteAvatar(long id)
        {
            var user = userService.Get(id);
            if (!string.IsNullOrWhiteSpace(user.PhotoUser))
            {
                userService.DeleteAvatar(id);
                fileService.DeleteFile(user.PhotoUser, user.Id);
                ViewBag.PhotoUserUrl = "~/images/user-icon.png";
            }
            return RedirectToAction("Index", "Home");
        }

    }
}