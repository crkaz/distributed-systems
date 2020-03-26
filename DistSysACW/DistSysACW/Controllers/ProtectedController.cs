using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using DistSysACW.Models;

namespace DistSysACW.Controllers
{
    public class ProtectedController : BaseController
    {
        #region TASK 9
        // Create a new ProtectedController.Add the api/Protected/Hello method, api/Protected/SHA1 method and api/Protected/SHA256 method.
        //All of these requests must be authorised and all three must return strings to the client.
        //You may use the .NET SHA1CryptoServiceProvider and SHA256CryptoServiceProvider for SHA1 and SHA256 hashing respectively.
        //Both SHA1 and SHA256 methods must take a string message from the URI and both must return the hexadecimal hash as a string with no additional characters(e.g.no delimiters like -)
        #endregion

        /// <param name="context">DbContext set as a service in Startup.cs and dependency injected</param>
        public ProtectedController(Models.UserContext context) : base(context) { }
        //SHA1CryptoServiceProvider
        //SHA256CryptoServiceProvider

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public IActionResult Hello([FromHeader] string apiKey)
        {
            #region For Hello: localhost:< portnumber >/ api /protected/hello with an ApiKey in the header of the request
            //Should return "Hello <UserName>" in the body of the result, where UserName is the User’s UserName from the database, with a status code of OK(200). E.g. “Hello UserOne”.
            #endregion

            string username = UserDatabaseAccess.GetUserByApiKey(apiKey).UserName;

            return Ok("Hello " + username);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public IActionResult SHA1([FromQuery] string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return BadRequest("Bad Request");
            }
            else
            {
                byte[] byteMessage = System.Text.Encoding.ASCII.GetBytes(message);
                SHA1 sha1Provider = new SHA1CryptoServiceProvider();
                byte[] sha1Message = sha1Provider.ComputeHash(byteMessage);
                string sha1HashString = HashToString(sha1Message);

                return Ok(sha1HashString);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public IActionResult SHA256([FromQuery] string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return BadRequest("Bad Request");
            }
            else
            {
                byte[] byteMessage = System.Text.Encoding.ASCII.GetBytes(message);
                SHA256 sha256Provider = new SHA256CryptoServiceProvider();
                byte[] sha256Message = sha256Provider.ComputeHash(byteMessage);
                string sha256HashString = HashToString(sha256Message);

                return Ok(sha256HashString);
            }
        }

        // --
        private string HashToString(byte[] hash)
        {
            string hexString = "";
            if (hash != null)
            {
                foreach (byte b in hash)
                {
                    hexString += b.ToString("x2");
                }
            }
            return hexString;
        }
    }
}
