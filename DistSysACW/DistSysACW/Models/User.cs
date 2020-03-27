using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DistSysACW.Models
{
    public class User
    {
        #region Task2
        // TODO: Create a User Class for use with Entity Framework
        // Note that you can use the [key] attribute to set your ApiKey Guid as the primary key 
        #endregion

        public enum UserRole { Admin, User };


        // DB fields.
        [Key] // Make primary key.
        public string ApiKey { get; set; } // Primary key.
        public string UserName { get; set; }
        public string Role { get; set; }
        public virtual ICollection<Log> Logs { get; set; }

        public User() { }
    }

    #region Task13?
    // TODO: You may find it useful to add code here for Logging
    #endregion

    public static class UserDatabaseAccess
    {
        #region Task3 
        // TODO: Make methods which allow us to read from/write to the database 
        #endregion

        // 1. Create a new user, using a username given as a parameter and creating a new GUID which is saved
        // as a string to the database as the ApiKey.This must return the ApiKey or the User object so that
        // the server can pass the Key back to the client.
        public static string CreateUser(string username)
        {
            string apiKey = Guid.NewGuid().ToString();
            string userRole = Enum.GetName(typeof(User.UserRole), User.UserRole.User);

            User user = new User() { ApiKey = apiKey, Role = userRole, UserName = username };

            using (var ctx = new UserContext())
            {
                // FROM TASK 4:
                // ...If this is the first user they should be saved as Admin role otherwise just with User role.
                if (ctx.Users.Count() == 0)
                {
                    user.Role = Enum.GetName(typeof(User.UserRole), User.UserRole.Admin);
                }
                ctx.Users.Add(user);
                ctx.SaveChanges();
            }

            return apiKey;
        }

        // 2. Check if a user with a given ApiKey string exists in the database, returning true or false.
        public static bool LookupApiKey(string apiKey)
        {
            using (var ctx = new UserContext())
            {
                foreach (var user in ctx.Users)
                {
                    if (user.ApiKey == apiKey)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        // 3. Check if a user with a given ApiKey and UserName exists in the database, returning true or false.
        /// Could be combined with method 2.
        public static bool LookupUsernameAndApiKey(string apiKey, string username)
        {
            using (var ctx = new UserContext())
            {
                foreach (var user in ctx.Users)
                {
                    if (user.UserName == username && user.ApiKey == apiKey)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        // 4. Check if a user with a given ApiKey string exists in the database, returning the User object.
        public static User GetUserByApiKey(string apiKey)
        {
            using (var ctx = new UserContext())
            {
                foreach (var user in ctx.Users)
                {
                    if (user.ApiKey == apiKey)
                    {
                        return user;
                    }
                }
                return null;
            }
        }

        // 5. Delete a user with a given ApiKey from the database.
        public static bool DeleteUserByApiKey(string apiKey)
        {
            using (var ctx = new UserContext())
            {
                User userToDelete = null; // Cannot modify collection in foreach.

                foreach (var user in ctx.Users)
                {
                    if (user.ApiKey == apiKey)
                    {
                        userToDelete = user;
                        break;
                    }
                }

                try
                {
                    ctx.Users.Remove(userToDelete);
                    ctx.SaveChanges();
                    return true;
                }
                catch (DbUpdateConcurrencyException) // Manage optimistic concurrency conflict.
                {
                    Console.WriteLine("Failed to fulfil action: database was modified by somebody else");
                }
            }
            return false;
        }

        // 6. Etc…
        // This is only possible if usernames are unique but not sure how else can check from usercontroller given query in GET.
        public static bool CheckUsernameExists(string username)
        {
            using (var ctx = new UserContext())
            {
                foreach (var user in ctx.Users)
                {
                    if (user.UserName == username)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public static bool ChangeRole(string username, string role)
        {
            using (var ctx = new UserContext())
            {
                User userToChange = null; // Cannot modify collection in foreach.
                foreach (var user in ctx.Users)
                {
                    if (user.UserName == username)
                    {
                        userToChange = user;
                        break;
                    }
                }

                try
                {
                    userToChange.Role = role;
                    ctx.SaveChanges();
                    return true;
                }
                catch (DbUpdateConcurrencyException) // Manage optimistic concurrency conflict.
                {
                    Console.WriteLine("Failed to fulfil action: database was modified by somebody else");
                }
            }
            return false;
        }

        public static void Log(string logString)
        {
            using (var ctx = new UserContext())
            {
                Log log = new Log(logString);
                ctx.Logs.Add(log);
                ctx.SaveChanges();
            }
        }
        ///.......
    }
}
