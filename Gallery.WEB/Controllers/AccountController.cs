using Gallery.BAL.DTO;
using Gallery.BAL.Interfaces;
using Gallery.WEB.Models;
using System.Web.Mvc;
using System.Web.Security;
using System.Linq;
using Gallery.BAL.Encryption;
using System.Security.Cryptography;
using Gallery.BAL.Providers;
using System.Web;
using System;

namespace Gallery.WEB.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IUserService userService;
        private readonly IFileService fileService;
        CustomRoleProvider rPr;

        public AccountController(IUserService userService, CustomRoleProvider role, IFileService fileService)
        {
            this.userService = userService;
            this.fileService = fileService;
            rPr = role;
        }

        // GET: Registration
        public ActionResult Index()
        {
            return View();
        }

        // GET: Registration
        public ActionResult LogIn()
        {
            ModelState.Clear();
            return View();
        }

        // POST: /Register/LogIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                var login = userService.GetAllElements().FirstOrDefault(log => log.Login == user.Login);
                if (login == null)
                {
                    TempData["Message"] = "Login or Password is wrong";
                    return View(user);
                }
                string source = user.Password;
                using (MD5 md5Hash = MD5.Create())
                {
                    if (EncryptPassword.VerifyMd5Hash(md5Hash, source, login.Password))
                    {
                        if (User.Identity.IsAuthenticated)
                        {
                            var s = User.Identity.Name;
                        }
                        else
                        {
                            FormsAuthentication.SetAuthCookie(user.Login, false);
                            TempData["currentUserId"] = user.Login;
                        }

                        FormsAuthentication.SetAuthCookie(user.Login, false);
                    }
                    else
                    {
                        TempData["Message"] = "Login or Password is wrong";
                        return RedirectToAction("Login", "Account");
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("login", "Account");
        }

        //
        // GET: /Register/SignUp
        public ActionResult SignUp()
        {
            return View();
        }

        // POST: /Register/SignUp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(RegisterViewModel user, HttpPostedFileBase file)
        {
            file = Request.Files[0];
            if (ModelState.IsValid)
            {

                var login = userService.GetAllElements().FirstOrDefault(log => log.Login == user.Login);
                if (login != null)
                {
                    ViewBag.Message = "This login is used";
                    return View(user);
                }
              
                string source = user.Password;
                using (MD5 md5Hash = MD5.Create())
                {
                    string hash = EncryptPassword.GetMd5Hash(md5Hash, source);
                    var us = new UserDTO
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Login = user.Login,
                        Password = hash,
                        Email = user.Email,
                        PhotoUser = user.PhotoUser,
                        Roles = user.Roles,
                        RoleId = 3
                    };

                    userService.Create(us);
                    var userWithId = userService.GetCurrentUser(us.Login); 
                    if (!string.IsNullOrWhiteSpace(file.FileName))
                    {
                        user.PhotoUser = @"~\" + fileService.UploadFile(file.InputStream, file.FileName, userWithId.Id)
                                                             .Replace(HttpContext.Request.PhysicalApplicationPath, String.Empty);
                        us.Id = userWithId.Id;
                        us.PhotoUser = user.PhotoUser;
                        userService.Update(us);
                    }
                    FormsAuthentication.SetAuthCookie(user.Login, false);

                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }
    }
}