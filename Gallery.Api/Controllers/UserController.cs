using Gallery.BAL.DTO;
using Gallery.BAL.Interfaces;
using Gallery.BAL.Providers;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Gallery.Api.Controllers
{
    public class UserController : ApiController
    {
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        CustomRoleProvider roleProvider;
        public UserController(IUserService userService, 
                              IRoleService roleService,
                              CustomRoleProvider roleProvider) 
        {
            this.userService = userService;
            this.roleService = roleService;
            this.roleProvider = roleProvider;
        }

        [Route("api/users")]
        [HttpGet]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public IEnumerable<UserDTO> GetAllUsers()
        {
            var users = userService.GetAllElements();
            return users;
        }



        //// POST api/values
        //public void Post([FromBody]string value)
        //{

        //}


        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}

    }
}