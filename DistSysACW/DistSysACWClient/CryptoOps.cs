using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CoreExtensions;
using System.Globalization;
using System.IO;

namespace DistSysACWClient
{
    class CryptoOps
    {
        public static string RSAEncrypt(byte[] DataToEncrypt)
        {
            try
            {
                byte[] encryptedData;
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlStringCore22(Connection.ServerKey);
                    encryptedData = rsa.Encrypt(DataToEncrypt, false);
                }

                return ByteArrToHexString(encryptedData);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static bool VerifyRSASigning(string original, string signed)
        {
            try
            {
                bool verified = false;
                using (var rsa = new RSACryptoServiceProvider())
                {
                    byte[] signedBytes = HexStringToByteArr(signed);
                    byte[] asciiByteMessage = Encoding.ASCII.GetBytes(original); // Original message, encoded.
                    rsa.FromXmlStringCore22(Connection.ServerKey);
                    verified = rsa.VerifyData(asciiByteMessage, new SHA1CryptoServiceProvider(), signedBytes);
                }
                return verified;
            }
            catch
            {
                return false;
            }
        }

        public static byte[] AESEncrypt(string args, byte[] key, byte[] iv)
        {
            try
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
            catch
            {
                return null;
            }
        }

        public static string AESDecrypt(byte[] args, byte[] key, byte[] iv)
        {
            try
            {
                string plaintext;
                using (var aes = new AesCryptoServiceProvider())
                {
                    ICryptoTransform decryptor = aes.CreateDecryptor(key, iv);
                    using (MemoryStream ms = new MemoryStream(args))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cs))
                            {
                                plaintext = sr.ReadToEnd();
                            }
                        }
                    }
                }

                return plaintext;
            }
            catch
            {
                return null;
            }
        }

        public static byte[] HexStringToByteArr(string hex)
        {
            try
            {
                return hex.Split('-').Select(hexStr => byte.Parse(hexStr, NumberStyles.HexNumber)).ToArray(); // Signed data converted back into byte array.
            }
            catch
            {
                return null;
            }
        }

        public static string ByteArrToHexString(byte[] byteArray, bool includeHyphen = true)
        {
            string hexString = "";
            if (null != byteArray)
            {
                int i = 0;
                foreach (byte b in byteArray)
                {
                    hexString += b.ToString("x2");

                    // Add hyphen after all but final hex pair.
                    if (includeHyphen && i++ < byteArray.Length - 1)
                    {
                        hexString += "-";
                    }
                }
            }

            return hexString.ToUpper();
        }
    }
}
