using System;
using System.Security.Cryptography;
using System.Text;

namespace DistSysACWClient
{
    class ProtectedRequest : Request
    {
        public static void Hello()
        {
            string endpoint = "Protected/Hello";

            if (Connection.ApiKeySet())
            {
                GetEndpoint(endpoint, null, "Unauthorized. Check ApiKey in Header is correct.");
            }
            else
            {
                Console.WriteLine("You need to do a User Post or User Set first");
            }
        }

        public static void SHA1(string args)
        {
            string endpoint = "Protected/SHA1?message=";

            if (Connection.ApiKeySet())
            {

                bool validArgs = true;

                if (string.IsNullOrWhiteSpace(args))
                {
                    validArgs = false;
                }

                if (validArgs)
                {
                    endpoint += args;
                    GetEndpoint(endpoint, null, "Unauthorized. Check ApiKey in Header is correct.");
                }
                else
                {
                    Console.WriteLine("Bad Request");
                }
            }
            else
            {
                Console.WriteLine("You need to do a User Post or User Set first");
            }
        }

        public static void SHA256(string args)
        {
            string endpoint = "Protected/SHA256?message=";

            if (Connection.ApiKeySet())
            {

                bool validArgs = true;

                if (string.IsNullOrWhiteSpace(args))
                {
                    validArgs = false;
                }

                if (validArgs)
                {
                    endpoint += args;
                    GetEndpoint(endpoint, null, "Unauthorized. Check ApiKey in Header is correct.");
                }
                else
                {
                    Console.WriteLine("Bad Request");
                }
            }
            else
            {
                Console.WriteLine("You need to do a User Post or User Set first");
            }
        }

        public static void GetPublicKey()
        {
            string endpoint = "Protected/GetPublicKey";

            if (Connection.ApiKeySet())
            {
                string successMsg = "Got Public Key";
                string errorMsg = "Couldn’t Get the Public Key";

                Connection.ServerKey = GetEndpoint(endpoint, successMsg, errorMsg);
            }
            else
            {
                Console.WriteLine("You need to do a User Post or User Set first");
            }
        }

        public static void Sign(string args)
        {
            string endpoint = "Protected/Sign?message=" + args;

            if (string.IsNullOrWhiteSpace(args))
            {
                Console.WriteLine("Invalid args.");
                return;
            }

            if (Connection.ApiKeySet())
            {
                if (Connection.PublicKeySet)
                {
                    string signedString = GetGetEndpoint(endpoint); // Signed data HH-HH-HH...

                    bool verified = CryptoOps.VerifyRSASigning(args, signedString);

                    if (verified)
                    {
                        Console.WriteLine("Message was successfully signed");
                    }
                    else
                    {
                        Console.WriteLine("Message was not successfully signed");
                    }
                }
                else
                {
                    Console.WriteLine("Client doesn’t yet have the public key");
                }
            }
            else
            {
                Console.WriteLine("You need to do a User Post or User Set first");
            }
        }

        public static void AddFifty(string args)
        {
            string endpoint = "Protected/AddFifty";

            if (Connection.ApiKeySet())
            {
                if (Connection.PublicKeySet)
                {
                    try
                    {
                        int resultInt;
                        bool validArgs = int.TryParse(args, out resultInt);
                        if (validArgs)
                        {
                            // Initialise Aes key and iv for the request.
                            using (var aes = new AesCryptoServiceProvider())
                            {
                                aes.GenerateKey();
                                aes.GenerateIV();

                                endpoint += "?encryptedInteger=" + CryptoOps.RSAEncrypt(BitConverter.GetBytes(resultInt));
                                endpoint += "&encryptedSymKey=" + CryptoOps.RSAEncrypt(aes.Key);
                                endpoint += "&encryptedIV=" + CryptoOps.RSAEncrypt(aes.IV);

                                string response = GetGetEndpoint(endpoint); // Get server response.
                                byte[] responseByte = CryptoOps.HexStringToByteArr(response); // Convert response to byte array.

                                string result = CryptoOps.AESDecrypt(responseByte, aes.Key, aes.IV); // AES decrypt response.

                                bool isInt = int.TryParse(result, out resultInt);
                                if (isInt)
                                {
                                    Console.WriteLine(result);
                                }
                                else
                                {
                                    throw new Exception("Result was not an integer."); // Caught by trycatch for desired response.
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("A valid integer must be given!");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("An error occurred!");
                    }
                }
                else
                {
                    Console.WriteLine("Client doesn’t yet have the public key");
                }
            }
            else
            {
                Console.WriteLine("You need to do a User Post or User Set first");
            }
        }
    }
}
