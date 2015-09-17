using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Kahia.Common.Extensions.ConversionExtensions;
using Kahia.Common.Extensions.GeneralExtensions;
using Kahia.Common.Extensions.StringExtensions;

namespace FormsAuthTest.Application.Helpers
{
    public class EncryptionHelper
    {
        #region Parameters from Config

        static String PassPhrase = AppSettingHelper.Get("System.Encryption.PassPhrase", "'{0}' parameter is invalid. \n Passphrase from which a pseudo-random password will be derived. The derived password will be used to generate the encryption key. Passphrase can be any string.");
        static String SaltValue = AppSettingHelper.Get("System.Encryption.SaltValue", "'{0}' parameter is invalid. \n Salt value used along with passphrase to generate password. Salt can be any string.");
        static String InitVector = AppSettingHelper.Get("System.Encryption.InitVector", "'{0}' parameter is invalid. \n Initialization vector (or IV). This value is required to encrypt the first block of plaintext data. For RijndaelManaged class IV must be exactly 16 ASCII characters long.");
        static String HashAlgorithm = AppSettingHelper.Get("System.Encryption.HashAlgorithm");
        static int PasswordIterations = AppSettingHelper.Get("System.Encryption.PasswordIterations").ToNullableInt().ThrowIfNull("System.Encryption.PasswordIterations parameter is invalid. \n Number of iterations used to generate password. One or two iterations should be enough.").Value;
        static int KeySize = AppSettingHelper.Get("System.Encryption.KeySize").ToInt();

        #endregion

        private static void PreCheckParameters()
        {
            if (HashAlgorithm.IsNullOrEmptyString() || (!HashAlgorithm.Equals("MD5") && !HashAlgorithm.Equals("SHA1")))
                throw new ArgumentException("System.Encryption.HashAlgorithm parameter is invalid. \n Hash algorithm used to generate password. Allowed values are: \"MD5\" and \"SHA1\". SHA1 hashes are a bit slower, but more secure than MD5 hashes.");

            if (KeySize.IsNullOrEmptyString() || (KeySize != 128 && KeySize != 192 && KeySize != 256))
                throw new ArgumentException("System.Encryption.KeySize parameter is invalid. \n Size of encryption key in bits. Allowed values are: 128, 192, and 256. Longer keys are more secure than shorter keys.");
        }

        /// <summary>
        /// Encrypts specified plaintext using Rijndael symmetric key algorithm and returns a base64-encoded result.
        /// </summary>
        /// <param name="parameter">
        /// Plaintext value to be encrypted.
        /// </param>
        /// <returns>
        /// Encrypted value formatted as a base64-encoded string.
        /// </returns>
        public static string Encrypt(String parameter)
        {
            PreCheckParameters();

            var plainText = parameter;
            var initVectorBytes = Encoding.UTF8.GetBytes(InitVector);
            var saltValueBytes = Encoding.UTF8.GetBytes(SaltValue);
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            var password = new PasswordDeriveBytes(PassPhrase, saltValueBytes, HashAlgorithm, PasswordIterations);
            var keyBytes = password.GetBytes(KeySize / 8);
            var symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            var cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            string cipherText = Convert.ToBase64String(cipherTextBytes);
            return cipherText;
        }

        /// <summary>
        /// Decrypts specified ciphertext using Rijndael symmetric key algorithm.
        /// </summary>
        /// <param name="parameter">
        /// Base64-formatted ciphertext value.
        /// </param>
        /// <returns>
        /// Decrypted string value.
        /// </returns>
        /// <remarks>
        /// Most of the logic in this function is similar to the Encrypt logic. In order for decryption to work, all parameters of this function
        /// - except cipherText value - must match the corresponding parameters of the Encrypt function which was called to generate the ciphertext.
        /// </remarks>
        public static String Decrypt(String parameter)
        {
            PreCheckParameters();

            var cipherText = parameter;
            var initVectorBytes = Encoding.UTF8.GetBytes(InitVector);
            var saltValueBytes = Encoding.UTF8.GetBytes(SaltValue);
            var cipherTextBytes = Convert.FromBase64String(cipherText);
            var password = new PasswordDeriveBytes(PassPhrase, saltValueBytes, HashAlgorithm, PasswordIterations);
            var keyBytes = password.GetBytes(KeySize / 8);
            var symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            var decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            var plainTextBytes = new byte[cipherTextBytes.Length];
            var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            var plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            return plainText;
        }
    }
}
