using Gallery.DAL.RepositoryClasses;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Security;

namespace Gallery.BAL.Providers
{
    public class CustomRoleProvider : RoleProvider
    {
       // IDbConnection conn;

        public CustomRoleProvider()
        {

        }

        public override string[] GetRolesForUser(string username)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                UserRepository userRepository = new UserRepository(db);

                RoleRepository roleRepository = new RoleRepository(db);

                var role = userRepository.GetUserRoleByUserName(username);
                string[] roles = { role };

                if (roles != null)
                {
                    return roles.ToArray();
                }
                else
                {
                    return new string[] { };
                }

            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {

                UserRepository userRepository = new UserRepository(db);
                RoleRepository roleRepository = new RoleRepository(db);

                var roleUser = userRepository.GetUserRoleByUserName(username);
                if (roleUser == roleName)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        public override string ApplicationName
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}
