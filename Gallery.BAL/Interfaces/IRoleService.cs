using Gallery.BAL.DTO;
using Gallery.DAL.Models;
using System.Collections.Generic;

namespace Gallery.BAL.Interfaces
{
    public interface IRoleService : IBaseService<RoleDTO>
    {
       long RoleIdByRoleName(string roleName);
    }
}
