﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using BusinessFlow.Models;

namespace BusinessFlow.Models
{
    public class CodeFirstRoleProvider : RoleProvider
            {

                private string _ApplicationName;
                public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
                {
                    if (config == null)
                    {
                        throw new ArgumentNullException("config");
                    }
                    if (string.IsNullOrEmpty(name))
                    {
                        name = "CodeFirstRoleProvider";
                    }
                    if (string.IsNullOrEmpty(config["description"]))
                    {
                        config.Remove("description");
                        config.Add("description", "Code-First Role Provider");
                    }

                    base.Initialize(name, config);

                    _ApplicationName = config["applicationName"];
                }

                public override string ApplicationName
                {
                    get { return _ApplicationName; }
                    set { _ApplicationName = value; }
                }

                public override bool RoleExists(string roleName)
                {
                    if (string.IsNullOrEmpty(roleName))
                    {
                        throw CreateArgumentNullOrEmptyException("roleName");
                    }
                    using (BusinessFlowContext context = new BusinessFlowContext())
                    {
                        dynamic result = context.Roles.FirstOrDefault(Rl => Rl.RoleName == roleName);
                        if (result != null)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                public override bool IsUserInRole(string userName, string roleName)
                {
                    if (string.IsNullOrEmpty(userName))
                    {
                        throw CreateArgumentNullOrEmptyException("userName");
                    }
                    if (string.IsNullOrEmpty(roleName))
                    {
                        throw CreateArgumentNullOrEmptyException("roleName");
                    }
                    using (BusinessFlowContext context = new BusinessFlowContext())
                    {
                        dynamic user = context.User.FirstOrDefault(Usr => Usr.Username == userName);
                        if (user == null)
                        {
                            return false;
                        }
                        dynamic role = context.Roles.FirstOrDefault(Rl => Rl.RoleName == roleName);
                        if (role == null)
                        {
                            return false;
                        }
                        return user.Roles.Contains(role);
                    }
                }

                public override string[] GetAllRoles()
                {
                    using (BusinessFlowContext context = new BusinessFlowContext())
                    {
                        return context.Roles.Select(Rl => Rl.RoleName).ToArray();
                    }
                }

                public override string[] GetUsersInRole(string roleName)
                {
                    if (string.IsNullOrEmpty(roleName))
                    {
                        throw CreateArgumentNullOrEmptyException("roleName");
                    }
                    using (BusinessFlowContext context = new BusinessFlowContext())
                    {
                        var role = context.Roles.FirstOrDefault(Rl => Rl.RoleName == roleName);
                        if (role == null)
                        {
                            throw new InvalidOperationException("Role not found");
                        }
                        return role.Users.Select(Usr => Usr.Username).ToArray();
                    }
                }

                public override string[] FindUsersInRole(string roleName, string usernameToMatch)
{
	if (string.IsNullOrEmpty(roleName)) {
		throw CreateArgumentNullOrEmptyException("roleName");
	}
	if (string.IsNullOrEmpty(usernameToMatch)) {
		throw CreateArgumentNullOrEmptyException("usernameToMatch");
	}
	using (BusinessFlowContext context = new BusinessFlowContext()) {
		var query = from Rl in context.Roles from Usr in Rl.Users where Rl.RoleName == roleName && Usr.Username.Contains(usernameToMatch) select Usr.Username;
		return query.ToArray();
	}
}

                public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
                {
                    if (string.IsNullOrEmpty(roleName))
                    {
                        throw CreateArgumentNullOrEmptyException("roleName");
                    }
                    using (BusinessFlowContext context = new BusinessFlowContext())
                    {
                        dynamic role = context.Roles.FirstOrDefault(Rl => Rl.RoleName == roleName);
                        if (role == null)
                        {
                            throw new InvalidOperationException("Role not found");
                        }
                        if (throwOnPopulatedRole)
                        {
                            dynamic usersInRole = role.Users.Any;
                            if (usersInRole)
                            {
                                throw new InvalidOperationException(string.Format("Role populated: {0}", roleName));
                            }
                        }
                        else
                        {
                            foreach (User usr_loopVariable in role.Users)
                            {
                               var usr = usr_loopVariable;
                                context.User.Remove(usr);
                            }
                        }
                        context.Roles.Remove(role);
                        context.SaveChanges();
                        return true;
                    }
                }

                public override string[] GetRolesForUser(string userName)
                {
                    if (string.IsNullOrEmpty(userName))
                    {
                        throw CreateArgumentNullOrEmptyException("userName");
                    }
                    using (BusinessFlowContext context = new BusinessFlowContext())
                    {
                        var user = context.User.FirstOrDefault(Usr => Usr.Username == userName);
                        if (user == null)
                        {
                            throw new InvalidOperationException(string.Format("User not found: {0}", userName));
                        }
                        return user.Roles.Select(Rl => Rl.RoleName).ToArray();
                    }
                }

                public override void CreateRole(string roleName)
                {
                    if (string.IsNullOrEmpty(roleName))
                    {
                        throw CreateArgumentNullOrEmptyException("roleName");
                    }
                    using (BusinessFlowContext context = new BusinessFlowContext())
                    {
                        dynamic role = context.Roles.FirstOrDefault(Rl => Rl.RoleName == roleName);
                        if (role != null)
                        {
                            throw new InvalidOperationException(string.Format("Role exists: {0}", roleName));
                        }
                        Role NewRole = new Role
                        {
                            RoleId = Guid.NewGuid(),
                            RoleName = roleName
                        };
                        context.Roles.Add(NewRole);
                        context.SaveChanges();
                    }
                }

                public override void AddUsersToRoles(string[] usernames, string[] roleNames)
                {
                    using (BusinessFlowContext context = new BusinessFlowContext())
                    {
                        var users = context.User.Where(usr => usernames.Contains(usr.Username)).ToList();
                        var roles = context.Roles.Where(rl => roleNames.Contains(rl.RoleName)).ToList();
                        foreach (User user_loopVariable in users)
                        {
                           var user = user_loopVariable;
                            foreach (Role role_loopVariable in roles)
                            {
                              var  role = role_loopVariable;
                                if (!user.Roles.Contains(role))
                                {
                                    user.Roles.Add(role);
                                }
                            }
                        }
                        context.SaveChanges();
                    }
                }

                public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
                {
                    using (BusinessFlowContext context = new BusinessFlowContext())
                    {
                        foreach (string username_loopVariable in usernames)
                        {
                           var username = username_loopVariable;
                            string us = username;
                            User user = context.User.FirstOrDefault(u => u.Username == us);
                            if (user != null)
                            {
                                foreach (string rolename_loopVariable in roleNames)
                                {
                                   var rolename = rolename_loopVariable;
                                    var rl = rolename;
                                    Role role = user.Roles.FirstOrDefault(r => r.RoleName == rl);
                                    if (role != null)
                                    {
                                        user.Roles.Remove(role);
                                    }
                                }
                            }
                        }
                        context.SaveChanges();
                    }
                }

                private ArgumentException CreateArgumentNullOrEmptyException(string paramName)
                {
                    return new ArgumentException(string.Format("Argument cannot be null or empty: {0}", paramName));
                }

    }

}