using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace EmployeeTrackingWebApp.Models
{
    public class UserRoleProvider : RoleProvider
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
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

        public override string[] GetRolesForUser(string username)
        {
            string[] UserRoles = (from user in db.AppUsers
                                  join role in db.AppRoles on user.AppRoleId equals role.AppRoleId
                                  where user.EmailId == username
                                  select role.AppRoleName).ToArray();

            return UserRoles;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            string[] UserRoles = (from user in db.AppUsers
                                  join role in db.AppRoles on user.AppRoleId equals role.AppRoleId
                                  where user.EmailId == username
                                  select role.AppRoleName).ToArray();

            if (UserRoles.Contains(roleName))
                return true;

            return false;
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