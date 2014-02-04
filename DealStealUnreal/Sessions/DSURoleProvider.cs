using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DealStealUnreal
{
    public class DSURoleProvider : RoleProvider
    {
        private DSURepository repository;

        public DSURoleProvider()
        {
            repository = new DSURepository();
        }

        public override string[] GetRolesForUser(string username)
        {           
            ICollection<Role> roles = repository.GetRolesForUser(username);
            string[] nameRoles = new string[roles.Count()];

            foreach (Role role in roles)
            {
                nameRoles[nameRoles.Length] = role.Name;
            }

            return nameRoles;
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
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

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            Role role = repository.GetRole(roleName);
            int roleId = (role != null) ? role.Id : -1;
            return (repository.Users.SingleOrDefault(user => (user.UserName == username || user.Email == username) && user.RoleId == roleId) != null);
        }
    }
}