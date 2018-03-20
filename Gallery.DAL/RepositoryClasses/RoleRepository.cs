using System.Collections.Generic;
using System.Linq;
using Gallery.DAL.IRepository;
using Gallery.DAL.Models;
using System.Data;
using Dapper;
using System;

namespace Gallery.DAL.RepositoryClasses
{
    public class RoleRepository : BaseRepository, IBaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(IDbConnection conn) : base(conn)
        {

        }

        public void Create(Role element)
        {
            var sqlQuery = "INSERT INTO Roles (Name, UserId) VALUES (@Name, @UserId);" +
                " SELECT CAST(SCOPE_IDENTITY() as int)";
            connection.Execute(sqlQuery, element);
        }

        public void Delete(long id)
        {
            var sqlQuery = "DELETE FROM Roles WHERE Id = @id";
            connection.Execute(sqlQuery, new { id });
        }
        public Role Get(long id)
        {
            var selectQuery = "SELECT * FROM Roles WHERE Id = @id";
            var role = connection.Query<Role>(@selectQuery, new { id }).Single(s => s.Id == id);
            return role;
        }

        public IEnumerable<Role> GetAllElements()
        {
            var rolesList = connection.Query<Role>("SELECT * FROM Roles").ToList();
            return rolesList;
        }

        public void Update(Role element)
        {
            var sqlQuery = "UPDATE Roles SET Name = @Name, UserId = @UserId WHERE Id = @Id";
            connection.Execute(sqlQuery, element);
        }

        public bool ListEquals(List<Role> expected, List<Role> actual)
        {
            if (expected.Count != actual.Count)
                return false;
            else
            {
                for (var i = 0; i < expected.Count(); i++)
                {
                    if (expected[i].Id.Equals(actual[i].Id) &&
                        expected[i].Name.Equals(actual[i].Name))
                        return true;
                }
                return false;
            }
        }

        public long GetRoleIdByRoleName(string roleName)
        {
            var roleId = connection.Query<string>("SELECT Id FROM Roles r WHERE r.Name = @roleName", new { roleName }).FirstOrDefault();
            return Convert.ToInt64(roleId);
        }
    }
}
