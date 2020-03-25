using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
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


        [HttpGet] // Parses query obj to give raw string value.
        [ActionName("New")]
        public IActionResult Get([FromQuery] string username)
        {
            // localhost:< portnumber >/api/user/new?username=UserOne
            bool usernameTaken = UserDatabaseAccess.CheckUsernameExists(username);

            try
            {
                if (String.IsNullOrWhiteSpace(username) || !usernameTaken) /*user with the username ‘UserOne’ does not exist in the database*/
                {
                    return Ok("False - User Does Not Exist! Did you mean to do a POST to create a new user?");
                }
                else if (usernameTaken) /*user with the username ‘UserOne’ exists in the database*/
                {
                    return Ok("True - User Does Exist! Did you mean to do a POST to create a new user?");
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
        //[Authorize(Roles = "Admin")]
        // [Authorize(Roles = "Admin,User")]
        public IActionResult Post([FromBody] string json)
        //public IActionResult Post([FromBody] JRaw json)
        {
            // localhost:< portnumber >/ api / user / new with “UserOne” in the body of the request
            Console.WriteLine(json);
            bool jsonIsEmpty = String.IsNullOrWhiteSpace(json.ToString());
            bool usernameExists = UserDatabaseAccess.CheckUsernameExists(json.ToString());

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
                return Forbid("Oops. This username is already in use. Please try again with a new username.");
            }
            else
            {
                #region // Should create a new User with the username ‘UserOne’, generate a new GUID as the user’s API Key,
                // and then add the new user to the database.Finally, the server should return the API Key as a string
                // to the client with a status code of OK(200). If this is the first user they should be saved as Admin role,
                // otherwise just with User role.
                #endregion
                string apiKey = UserDatabaseAccess.CreateUser(json.ToString());
                return Ok(apiKey);
            }
        }

        [HttpDelete]
        [ActionName("RemoveUser")]
        [Authorize(Roles = "Admin, User")]
        public bool Delete([FromHeader] string apiKey, [FromQuery] string username)
        {
            #region // If the server receives this request, it must extract the ApiKey string
            // from the header to see if the API Key is in the database and, if it is, it must
            // check that the username and API Key are the same user and if they are, it must
            // delete this user from the database. You should probably use your UserDatabaseAccess
            // class that you created in TASK3 to do the database access.
            #endregion
            bool usernameMatchesApiKey = UserDatabaseAccess.LookupUsernameAndApiKey(apiKey, username);

            if (usernameMatchesApiKey)
            {
                //try
                //{
                UserDatabaseAccess.DeleteUserByApiKey(apiKey);
                return true;
                //}
                //catch { }
            }
            return false;
        }

        [HttpPost]
        [ActionName("ChangeRole")]
        [Authorize(Roles = "Admin")]
        //public IActionResult Post([FromHeader] string apiKey, [FromBody] string json)
        public IActionResult Post([FromHeader] string apiKey, [FromBody] JObject json)
        {
            // username, role
            string username = (string)json["username"];
            string role = (string)json["role"];

            return null;
        }

    }
}
