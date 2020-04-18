using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CoreExtensions;

namespace DistSysACW
{
    public class RSAService
    {
        private static RSAService instance; // Singleton.
        public static RSAService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RSAService();
                }
                return instance;
            }
        }

        private readonly string keyStoreName; // Guid for keystore.
        private RSACryptoServiceProvider RSA { get { return GetRSA(); } }

        private RSAService()
        {
            // Generate key in machine storage.
            keyStoreName = Guid.NewGuid().ToString();
            CspParameters cspParams = new CspParameters();
            cspParams.Flags = CspProviderFlags.UseMachineKeyStore;
            cspParams.KeyContainerName = keyStoreName;
            new RSACryptoServiceProvider(cspParams);
        }

        ~RSAService()
        {
            // Re-init rsa and remove from machine storage.
            RSA.PersistKeyInCsp = false;
            RSA.Clear();
        }

        public string GetPublicKey()
        {
            // Re-init rsa with key stored in container created with instance and get key.
            return RSA.ToXmlStringCore22();
        }

        public string SignData(string message)
        {
            byte[] asciiByteMessage = Encoding.ASCII.GetBytes(message);
            byte[] signed = RSA.SignData(asciiByteMessage, new SHA1CryptoServiceProvider());

            return BitConverter.ToString(signed);

        }

        public byte[] Decrypt(string hex)
        {
            byte[] encryption = hex.Split('-').Select(hexStr => byte.Parse(hexStr, NumberStyles.HexNumber)).ToArray(); // Signed data converted back into byte array.
            byte[] decrypted = RSA.Decrypt(encryption, true);
            //var val = Encoding.ASCII.GetString(decrypted);
            //var val = BitConverter.ToInt32(decrypted);

            return decrypted;
        }

        private string GetPrivateKey()
        {
            // Re-init rsa with key stored in container created with instance and get key.
            return RSA.ToXmlStringCore22(true);
        }

        private RSACryptoServiceProvider GetRSA()
        {
            // Get rsa from key in machine storage.
            CspParameters cspParams = new CspParameters();
            cspParams.Flags = CspProviderFlags.UseMachineKeyStore;
            cspParams.KeyContainerName = keyStoreName;
            return new RSACryptoServiceProvider(cspParams);
        }
    }
}
