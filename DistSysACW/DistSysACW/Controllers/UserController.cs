using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DistSysACW.Controllers
{
    public class UserController : BaseController
    {
        /// <summary>
        /// Constructs a TalkBack controller, taking the UserContext through dependency injection
        /// </summary>
        /// <param name="context">DbContext set as a service in Startup.cs and dependency injected</param>
        public UserController(Models.UserContext context) : base(context) { }


        [ActionName("New")]
        public IActionResult Get([FromQuery] string username)
        {
            // localhost:< portnumber >/ api / user / new? username = UserOne
            // If a user with the username ‘UserOne’ exists in the database, the server should return "True - User Does Exist! Did you mean to do a POST to create a new user?" in the body of the result with a status code of OK(200)
            // If a user with the username ‘UserOne’ does not exist in the database, the server should return "False - User Does Not Exist! Did you mean to do a POST to create a new user?" in the body of the result with a status code of OK(200).
            // If there is no string submitted, the server should return "False - User Does Not Exist! Did you mean to do a POST to create a new user?" in the body of the result with a status code of OK(200).
            return Ok();
        }

        [ActionName("New")]
        public IActionResult Post([FromBody] string json)
        {
            // localhost:< portnumber >/ api / user / new with “UserOne” in the body of the request
            // Should create a new User with the username ‘UserOne’, generate a new GUID as the user’s API Key, and then add the new user to the database.Finally, the server should return the API Key as a string to the client with a status code of OK(200). If this is the first user they should be saved as Admin role, otherwise just with User role.
            // If there is no string submitted in the body, the result should be "Oops. Make sure your body contains a string with your username and your Content-Type is Content-Type:application/json" with a status code of BAD REQUEST(400)
            // If the username is alrady taken, the result should be "Oops. This username is already in use. Please try again with a new username." with a status code of FORBIDDEN(403)
            return Ok();
        }
    }
}
