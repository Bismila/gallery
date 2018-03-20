using Gallery.BAL.DTO;
using Gallery.BAL.Interfaces;
using Gallery.DAL.IRepository;
using Gallery.DAL.Models;
using System.Collections.Generic;
using System.Linq;

namespace Gallery.BAL.Services
{
    public class RoleService : IBaseService<RoleDTO>, IRoleService
    {
        private readonly IRoleRepository roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        public IEnumerable<RoleDTO> GetAllElements()
        {
            var roles = roleRepository.GetAllElements().Select(x => new RoleDTO
            {
                Id = x.Id,
                Name = x.Name
            });
            return roles;
        }
        public long RoleIdByRoleName(string roleName)
        {
            var roleId = roleRepository.GetRoleIdByRoleName(roleName);

            return roleId;
        }

        public RoleDTO Get(long id)
        {
            roleRepository.Get(id);

            var role = roleRepository.Get(id);

            return new RoleDTO { Id = role.Id, Name = role.Name};
        }

        public void Create(RoleDTO item)
        {
            var role = new Role
            {
                Id = item.Id,
                Name = item.Name
            };
            roleRepository.Create(role);
        }

        public void Delete(long id)
        {
            roleRepository.Delete(id);
        }

        public void Update(RoleDTO element)
        {
            var role = new Role
            {
                Id = element.Id,
                Name = element.Name
            };
            roleRepository.Update(role);
        }

    }
}
