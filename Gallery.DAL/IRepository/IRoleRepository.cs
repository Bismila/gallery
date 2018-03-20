using Gallery.DAL.Models;
using System.Collections.Generic;

namespace Gallery.DAL.IRepository
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        long GetRoleIdByRoleName(string roleName);
    }
}
