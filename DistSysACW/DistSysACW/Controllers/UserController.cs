using System;
using DistSysACW.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace DistSysACW.Controllers
{
    public class UserController : BaseController
    {
        /// <summary>
        /// Constructs a TalkBack controller, taking the UserContext through dependency injection
        /// </summary>
        /// <param name="context">DbContext set as a service in Startup.cs and dependency injected</param>
        public UserController(UserContext context) : base(context) { }


        [HttpGet]
        [ActionName("New")]
        public IActionResult Get([FromQuery] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return Ok("\"False - User Does Not Exist! Did you mean to do a POST to create a new user?\"");
            }
            bool usernameTaken = UserDatabaseAccess.CheckUsernameExists(_context, username);

            try
            {
                if (!usernameTaken) /*user with the username ‘UserOne’ does not exist in the database*/
                {
                    return Ok("\"False - User Does Not Exist! Did you mean to do a POST to create a new user?\"");
                }
                else if (usernameTaken) /*user with the username ‘UserOne’ exists in the database*/
                {
                    return Ok("\"True - User Does Exist! Did you mean to do a POST to create a new user?\"");
                }
            }
            catch
            {
                return BadRequest("Unexpected request format.");
            }
            return BadRequest("Unexpected error.");
        }

        [HttpPost]
        [ActionName("New")]
        public IActionResult Post([FromBody] string json)
        {
            // localhost:< portnumber >/ api / user / new with “UserOne” in the body of the request
            bool jsonIsEmpty = string.IsNullOrWhiteSpace(json);
            bool usernameExists = UserDatabaseAccess.CheckUsernameExists(_context, json);

            if (jsonIsEmpty)
            {
                #region // If there is no string submitted in the body, the result should be
                // "Oops. Make sure your body contains a string with your username and your Content-Type is Content-Type:application/json"
                // with a status code of BAD REQUEST(400)
                #endregion
                return BadRequest("Oops. Make sure your body contains a string with your username and your Content-Type is Content-Type:application/json");
            }
            else if (usernameExists)
            {
                #region // If the username is alrady taken, the result should be 
                // "Oops. This username is already in use. Please try again with a new username." with a status code of FORBIDDEN(403)
                #endregion
                //return Forbid();
                return StatusCode(403, "Oops. This username is already in use. Please try again with a new username.");
            }
            else
            {
                #region // Should create a new User with the username ‘UserOne’, generate a new GUID as the user’s API Key,
                // and then add the new user to the database.Finally, the server should return the API Key as a string
                // to the client with a status code of OK(200). If this is the first user they should be saved as Admin role,
                // otherwise just with User role.
                #endregion
                string apiKey = UserDatabaseAccess.CreateUser(_context, json);
                return Ok(apiKey);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin,User")]
        public bool RemoveUser([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery] string username)
        {
            UserDatabaseAccess.Log(_context, apiKey, "/User/RemoveUser");

            #region // If the server receives this request, it must extract the ApiKey string
            // from the header to see if the API Key is in the database and, if it is, it must
            // check that the username and API Key are the same user and if they are, it must
            // delete this user from the database. You should probably use your UserDatabaseAccess
            // class that you created in TASK3 to do the database access.
            #endregion
            bool usernameMatchesApiKey = UserDatabaseAccess.LookupUsernameAndApiKey(_context, apiKey, username);

            if (usernameMatchesApiKey)
            {
                bool success = UserDatabaseAccess.DeleteUserByApiKey(_context, apiKey);
                if (success)
                {
                    return true;
                }
            }
            return false;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult ChangeRole([FromHeader(Name = "ApiKey")] string apiKey, [FromBody] JObject json)
        {
            UserDatabaseAccess.Log(_context, apiKey, "/User/ChangeRole");

            bool apiKeyInDb = UserDatabaseAccess.LookupApiKey(_context, apiKey);

            if (apiKeyInDb)
            {
                // username, role
                string username = (string)json["username"];
                string role = ((string)json["role"]);

                bool roleExists = (role == "Admin" || role == "User");
                bool usernameExists = UserDatabaseAccess.CheckUsernameExists(_context, username);

                if (usernameExists && roleExists)
                {
                    //If success: Should return "DONE" in the body of the result, with a status code of OK(200)
                    bool success = UserDatabaseAccess.ChangeRole(_context, username, role);
                    if (success)
                    {
                        return Ok("DONE");
                    }
                }
                else if (!usernameExists)
                {
                    //If username does not exist: Should return "NOT DONE: Username does not exist" in the body of the result, with a status code of BAD REQUEST(400)
                    return BadRequest("NOT DONE: Username does not exist");
                }
                else if (!roleExists)
                {
                    //If role is not User or Admin: Should return "NOT DONE: Role does not exist" in the body of the result, with a status code of BAD REQUEST(400)
                    return BadRequest("NOT DONE: Role does not exist");
                }
            }

            //In all other error cases: Should return "NOT DONE: An error occured" in the body of the result, with a status code of BAD REQUEST(400)
            return BadRequest("NOT DONE: An error occured");
        }
    }
}
