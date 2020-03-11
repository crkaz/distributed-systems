using System;
using System.Security.Cryptography;

/// <summary>
/// Exercise 1: API for Hashing and Asymmetric Cryptography
/// </summary>
namespace lab6
{
    class Program
    {
        static void Main(string[] args)
        {
            //OutputSha1Encryption("helloworld");
            //OutputSha256Encryption("helloworld");
            byte[] encryptedByteMessage;
            byte[] decryptedByteMessage;
            string message = "hello world";
            byte[] messageByteArray = System.Text.Encoding.ASCII.GetBytes(message);
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            {
                encryptedByteMessage = RSAEncrypt(messageByteArray, RSA.ExportParameters(false));
                Console.Write("Encrypted message: ");
                Console.WriteLine(ByteArrayToHexString(encryptedByteMessage));
                decryptedByteMessage = RSADecrypt(encryptedByteMessage, RSA.ExportParameters(true));
                Console.Write("Decrypted message: ");
                Console.WriteLine(System.Text.Encoding.ASCII.GetString(decryptedByteMessage));
            }

        }

        private static void OutputSha1Encryption(string message)
        {
            byte[] messageByteArray = System.Text.Encoding.ASCII.GetBytes(message);
            byte[] sha1ByteArray;

            SHA1 sha1Prov = new SHA1CryptoServiceProvider();
            sha1ByteArray = sha1Prov.ComputeHash(messageByteArray);
            Console.Write("\n" + message + " became:");
            Console.Write(ByteArrayToHexString(sha1ByteArray));
            //Console.Write(BitConverter.ToString(sha1ByteArray).Replace('-', ' '));
        }

        private static void OutputSha256Encryption(string message)
        {
            byte[] messageByteArray = System.Text.Encoding.ASCII.GetBytes(message);
            byte[] sha1ByteArray;

            SHA256 sha1Prov = new SHA256CryptoServiceProvider();
            sha1ByteArray = sha1Prov.ComputeHash(messageByteArray);
            Console.Write("\n" + message + " became:");
            Console.Write(ByteArrayToHexString(sha1ByteArray));
            //Console.Write(BitConverter.ToString(sha1ByteArray).Replace('-', ' '));
        }

        static string ByteArrayToHexString(byte[] byteArray)
        {
            string hexString = "";
            if (null != byteArray)
            {
                foreach (byte b in byteArray)
                {
                    hexString += b.ToString("x2");
                }
            }
            return hexString;
        }

        static public byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo)
        {
            try
            {
                byte[] encryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKeyInfo);
                    encryptedData = RSA.Encrypt(DataToEncrypt, false);
                }
                return encryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        static public byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo)
        {
            try
            {
                byte[] decryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKeyInfo);
                    decryptedData = RSA.Decrypt(DataToDecrypt, false);
                }
                return decryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
}
