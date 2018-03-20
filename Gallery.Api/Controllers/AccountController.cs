using Gallery.Api.Models;
using Gallery.BAL.DTO;
using Gallery.BAL.Encryption;
using Gallery.BAL.Interfaces;
using Gallery.BAL.Providers;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Security;

using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Text;
using Microsoft.AspNet.Hosting;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System;
using System.IO;

namespace Gallery.Api.Controllers
{
    [EnableCors("http://localhost:56238", "*", "*")]
    public class AccountController : ApiController
    {
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        private readonly IFileService fileService;
        private readonly IImageService imageService;
        CustomRoleProvider roleProvider;

        public AccountController(IUserService userService,
                            IRoleService roleService,
                            CustomRoleProvider roleProvider,
                            IFileService fileService,
                            IImageService imageService)
        {
            this.userService = userService;
            this.roleService = roleService;
            this.roleProvider = roleProvider;
            this.fileService = fileService;
            this.imageService = imageService;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("api/account/authorize")]
        public IHttpActionResult GetForAdmin()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            return Ok("Hello, " + identity.Name + " Role: " + string.Join(",", roles.ToList()));
        }

        //[Authorize(Roles = "moderator, user")]
        [HttpGet]
        [Route("api/account/getCurrentUser")]
        public HttpResponseMessage GetCurrentUserClaims()
        {
            var curUser = userService.GetAllElements().FirstOrDefault(log => log.Login == User.Identity.Name);
            var imagesListForCurrUser = imageService.GetAllElementsFromUser(curUser.Id).Select(x=> new ImageModel()
            {
                Id = (int)x.Id,
                ImageDate = x.ImageDate,
                Name = x.Name,
                PathImage = x.PathImage,
                UserId = (int)x.UserId,
                
            }).ToList();
            if (User.Identity.IsAuthenticated)
            {
                if (curUser != null)
                {
                    var identity = (ClaimsIdentity)User.Identity;
                    IEnumerable<Claim> claims = identity.Claims;
                    if (identity.IsAuthenticated)
                    {
                        if (curUser != null)
                        {
                            var user = new UserModel()
                            {
                                Id = (int)curUser.Id,
                                Name = identity.Name,
                                Login = curUser.Login,
                                Password = curUser.Password,
                                PhotoUser = curUser.PhotoUser,
                                //RoleId = (int)curUser.RoleId,
                                //Roles = curUser.Roles,
                                Email = curUser.Email,
                                Images = imagesListForCurrUser
                            };
                            return Request.CreateResponse(HttpStatusCode.OK, user);
                        }
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "error");
                }
            }
           
           
            return Request.CreateResponse(HttpStatusCode.InternalServerError, "error"); 

        }

        public void LogOff()
        {
            FormsAuthentication.SignOut();
        }

        [Route("api/account/register")]
        [HttpPost]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public async Task<HttpResponseMessage> Register()
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "ups");
            }
            Dictionary<string, RegisterUserModel> attributes = new Dictionary<string, RegisterUserModel>();
            Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            string fileName = "";
            Byte[] newAvatarForCurrUser = null;
            foreach (var file in provider.Contents)
            {
                if (file.Headers.ContentDisposition.FileName != null)
                {
                    fileName = file.Headers.ContentDisposition.FileName.Trim('\"');
                    var buffer = await file.ReadAsByteArrayAsync();
                    files.Add(fileName, buffer);
                    newAvatarForCurrUser = buffer;
                }
                else
                {
                    foreach (NameValueHeaderValue p in file.Headers.ContentDisposition.Parameters)
                    {
                        string name = p.Value;
                        if (name.StartsWith("\"") && name.EndsWith("\""))
                        {
                            name = name.Substring(1, name.Length - 2);
                        }
                        string value = await file.ReadAsStringAsync();
                        RegisterUserModel user = JsonConvert.DeserializeObject<RegisterUserModel>(value);
                        user.PhotoUser = fileName;
                        var login = userService.GetAllElements().FirstOrDefault(log => log.Login == user.Login);
                        if (login != null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "error");
                        }
                        else
                        {
                            string source = user.Password;
                            using (MD5 md5Hash = MD5.Create())
                            {
                                string hash = EncryptPassword.GetMd5Hash(md5Hash, source);
                               
                                var newUser = new UserDTO()
                                {
                                    Id = user.Id,
                                    Name = user.Name,
                                    Login = user.Login,
                                    Email = user.Email,
                                    Password = hash,
                                    RoleId = 3,
                                    PhotoUser = user.PhotoUser,
                                    //Images = 
                                    //Friends = 
                                };
                                userService.Create(newUser);
                              
                                var userWithId = userService.GetCurrentUser(newUser.Login);
                                if (!string.IsNullOrWhiteSpace(fileName) && newAvatarForCurrUser != null)
                                {
                                    MemoryStream stream = new MemoryStream(newAvatarForCurrUser);
                                    user.PhotoUser = fileService.UploadFile(stream, fileName, userWithId.Id);
                                    newUser.Id = userWithId.Id;
                                    newUser.PhotoUser = user.PhotoUser;
                                    userService.Update(newUser);
                                }
                            }
                        }
                    }
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, "OK");





            //}

        }
    }

}

