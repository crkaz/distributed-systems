using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using DistSysACW.Models;
using System.IO;
using System.Linq;
using System.Globalization;
using System;
using System.Text;

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
            UserDatabaseAccess.Log(_context, apiKey, "/Protected/Hello");

            bool apiKeyInDb = UserDatabaseAccess.LookupApiKey(_context, apiKey);

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
            UserDatabaseAccess.Log(_context, apiKey, "/Protected/SHA1");

            bool apiKeyInDb = UserDatabaseAccess.LookupApiKey(_context, apiKey);

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

                    return Ok(sha1HashString.ToUpper()); // ToUpper simply to match test server.
                }
            }

            return Unauthorized("Unauthorized. Check ApiKey in Header is correct.");
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public IActionResult SHA256([FromHeader] string apiKey, [FromQuery] string message)
        {
            UserDatabaseAccess.Log(_context, apiKey, "/Protected/SHA256");

            bool apiKeyInDb = UserDatabaseAccess.LookupApiKey(_context, apiKey);

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

                    return Ok(sha256HashString.ToUpper()); // ToUpper simply to match test server.
                }
            }

            return Unauthorized("Unauthorized. Check ApiKey in Header is correct.");
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetPublicKey([FromHeader(Name = "ApiKey")] string apiKey)
        {
            UserDatabaseAccess.Log(_context, apiKey, "/Protected/GetPublicKey");

            bool apiKeyInDb = UserDatabaseAccess.LookupApiKey(_context, apiKey);

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
            UserDatabaseAccess.Log(_context, apiKey, "/Protected/Sign");

            bool apiKeyInDb = UserDatabaseAccess.LookupApiKey(_context, apiKey);

            if (apiKeyInDb)
            {
                string signedMessage = RSAService.Instance.SignData(message);
                return Ok(signedMessage);
            }

            return Unauthorized("Unauthorized. Check ApiKey in Header is correct.");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddFifty([FromHeader(Name = "ApiKey")] string apiKey, [FromQuery] string encryptedInteger, [FromQuery] string encryptedSymKey, [FromQuery] string encryptedIV)
        {
            UserDatabaseAccess.Log(_context, apiKey, "/Protected/AddFifty");

            bool apiKeyInDb = UserDatabaseAccess.LookupApiKey(_context, apiKey);
            if (apiKeyInDb)
            {
                try
                {
                    byte[] intBytes = RSAService.Instance.Decrypt(encryptedInteger); // RSA decrypt to get original message.
                    int msg = BitConverter.ToInt32(intBytes);
                    byte[] keyBytes = RSAService.Instance.Decrypt(encryptedSymKey); // RSA decrypt to get AES key.
                    byte[] ivBytes = RSAService.Instance.Decrypt(encryptedIV); // RSA decrypt to get AES IV.
                    int val = msg + 50; // Add fifty to original value in message.
                    byte[] valBytes = AESEncrypt(val.ToString(), keyBytes, ivBytes); // Encrypt new value with passed AES key.
                    string result = BitConverter.ToString(valBytes); // Convert new value into string to return.

                    return Ok(result);
                }
                catch (Exception e)
                {
                    return BadRequest("Bad Request"); // Doesn't specify a message.
                }
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

        private byte[] HexStringToByteArr(string hex)
        {
            return hex.Split('-').Select(hexStr => byte.Parse(hexStr, NumberStyles.HexNumber)).ToArray(); // Signed data converted back into byte array.
        }

        private byte[] AESEncrypt(string args, byte[] key, byte[] iv)
        {
            byte[] encryptedMessageBytes;
            using (var aes = new AesCryptoServiceProvider())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(key, iv);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(args);
                        }
                        encryptedMessageBytes = ms.ToArray();
                    }
                }
            }

            return encryptedMessageBytes;
        }
        #endregion
    }
}
