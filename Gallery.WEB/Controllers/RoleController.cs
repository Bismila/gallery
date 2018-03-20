using Gallery.BAL.DTO;
using Gallery.BAL.Interfaces;
using Gallery.WEB.Models;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Gallery.WEB.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService roleService;
        public RoleController(IRoleService roleService)
        {
            this.roleService = roleService;
        }

        // GET: Role
        public ActionResult Index()
        {
            var roles = roleService.GetAllElements().ToList();
            var role = View(roles.Select(x => new RoleViewModel
            {
                Id = (int)x.Id,
                Name = x.Name
            }));
            return role;
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: Roles/Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            roleService.Delete(id.Value);
            return RedirectToAction("/Index");
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = roleService.Get(id.Value);
            var roleViewModel = new RoleViewModel
            {
                Id = (int)role.Id,
                Name = role.Name
            };
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(roleViewModel);
        }

        // POST: Roles/Edit/5
        [HttpPost, ActionName("Edit")]
        public ActionResult Edit(RoleViewModel role)
        {
            var r = new RoleDTO
            {
                Id = role.Id,
                Name = role.Name
            };
            roleService.Update(r);
            return RedirectToAction("/Index");
        }
    }
}