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
using CoreExtensions;

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
        public ProtectedController(UserContext context) : base(context) { }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public IActionResult Hello([FromHeader] string apiKey)
        {
            UserDatabaseAccess.Log(apiKey, "/Protected/Hello");

            bool apiKeyInDb = UserDatabaseAccess.LookupApiKey(apiKey);

            if (apiKeyInDb)
            {
                string username = UserDatabaseAccess.GetUserByApiKey(_context, apiKey).UserName;

                return Ok("Hello " + username);
            }

            return Unauthorized("Unauthorized. Check ApiKey in Header is correct.");
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public IActionResult SHA1([FromHeader] string apiKey, [FromQuery] string message)
        {
            UserDatabaseAccess.Log(apiKey, "/Protected/SHA1");

            bool apiKeyInDb = UserDatabaseAccess.LookupApiKey(apiKey);

            if (apiKeyInDb)
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

            return Unauthorized("Unauthorized. Check ApiKey in Header is correct.");
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public IActionResult SHA256([FromHeader] string apiKey, [FromQuery] string message)
        {
            UserDatabaseAccess.Log(apiKey, "/Protected/SHA256");

            bool apiKeyInDb = UserDatabaseAccess.LookupApiKey(apiKey);

            if (apiKeyInDb)
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

            return Unauthorized("Unauthorized. Check ApiKey in Header is correct.");
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetPublicKey([FromHeader(Name = "ApiKey")] string apiKey)
        {
            UserDatabaseAccess.Log(apiKey, "/Protected/GetPublicKey");

            bool apiKeyInDb = UserDatabaseAccess.LookupApiKey(apiKey);

            if (apiKeyInDb)
            {
                string key = RSAService.Instance.GetPublicKey();
                return Ok(key);
            }

            return Unauthorized("Unauthorized. Check ApiKey in Header is correct.");
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public IActionResult Sign([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery] string message)
        {
            UserDatabaseAccess.Log(apiKey, "/Protected/Sign");

            bool apiKeyInDb = UserDatabaseAccess.LookupApiKey(apiKey);

            if (apiKeyInDb)
            {
                string signedMessage = RSAService.Instance.SignData(message);
                return Ok(signedMessage);
            }

            return Unauthorized("Unauthorized. Check ApiKey in Header is correct.");
        }

        #region Utils
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
        #endregion
    }


}
