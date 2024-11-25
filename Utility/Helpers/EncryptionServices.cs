using System.Text;
using System.Security.Cryptography;
using Serilog;

namespace Utility.Helpers
{
    public interface IEncryptionServices
    {
        #region Methods
        bool ComparePassword(string password, string inputPassword);
        string DecryptString(string text, byte[] key);
        string EncryptString(string text, byte[] key);
        string DecryptString(string text);
        string EncryptString(string text);
        bool IsEncrypted(string text);
        #endregion
    }

    /// <summary>
    /// Provides Encryption services.
    /// </summary>
    public class EncryptionServices : IEncryptionServices
    {
        #region Constants
        internal class DemoKey
        {
            #region Static Readonlys

            /// <summary>
            /// The encryption key value.
            /// </summary>
            internal static readonly byte[] Value = new byte[32] // 32 bytes = 256-bit.
            {
                140, 84, 28, 39, 182, 99, 193, 73,
                43, 71, 55, 142, 76, 16, 54, 19,
                21, 115, 23, 75, 15, 114, 41, 79,
                181, 196, 40, 148, 154, 81, 150, 56
            };
            #endregion
        }
        /// <summary>
        /// The prefix text added to the start of encrypted data, 
        /// to help identify that the data is encrypted.
        /// </summary>
        private const string EncryptedValuePrefix = "AMSKey:";
        #endregion

        #region Methods

        #region Public

        /// <summary>
        /// Decrypts the specified text.
        /// </summary>
        /// <param name="text">The text to decrypt</param>
        /// <param name="key">The encryption key</param>
        /// <returns>The decrypted text</returns>
        public string DecryptString(string text, byte[] key)
        {
            if (string.IsNullOrWhiteSpace(text) || !IsEncrypted(text))
            {
                // There is no need to decrypt null/empty or unencrypted text.
                return text;
            }

            // Parse the vector from the encrypted data.
            byte[] vector = Convert.FromBase64String(text.Split(';')[0].Split(':')[1]);

            // Decrypt and return the plain text.
            return Decrypt(Convert.FromBase64String(text.Split(';')[1]), key, vector);
        }

        /// <summary>
        /// Encrypts the specified text.
        /// </summary>
        /// <param name="text">The text to encrypt</param>
        /// <param name="key">The encryption key</param>
        /// <returns>The encrypted text</returns>
        public string EncryptString(string text)
        {
            if (string.IsNullOrWhiteSpace(text) || IsEncrypted(text))
            {
                // There is no need to encrypt null/empty or already encrypted text.
                return text;
            }

            // Create a new random vector.
            byte[] vector = GenerateInitializationVector();

            // Encrypt the text.
            string encryptedText = Convert.ToBase64String(Encrypt(text, DemoKey.Value, vector));

            // Format and return the encrypted data.
            return EncryptedValuePrefix + Convert.ToBase64String(vector) + ";" + encryptedText;
        }

        /// <summary>
        /// Decrypts the specified text.
        /// </summary>
        /// <param name="text">The text to decrypt</param>
        /// <param name="key">The encryption key</param>
        /// <returns>The decrypted text</returns>
        public string DecryptString(string text)
        {
            if (string.IsNullOrWhiteSpace(text) || !IsEncrypted(text))
            {
                // There is no need to decrypt null/empty or unencrypted text.
                return text;
            }

            // Parse the vector from the encrypted data.
            byte[] vector = Convert.FromBase64String(text.Split(';')[0].Split(':')[1]);

            // Decrypt and return the plain text.
            return Decrypt(Convert.FromBase64String(text.Split(';')[1]), DemoKey.Value, vector);
        }

        /// <summary>
        /// Encrypts the specified text.
        /// </summary>
        /// <param name="text">The text to encrypt</param>
        /// <param name="key">The encryption key</param>
        /// <returns>The encrypted text</returns>
        public string EncryptString(string text, byte[] key)
        {
            if (string.IsNullOrWhiteSpace(text) || IsEncrypted(text))
            {
                // There is no need to encrypt null/empty or already encrypted text.
                return text;
            }

            // Create a new random vector.
            byte[] vector = GenerateInitializationVector();

            // Encrypt the text.
            string encryptedText = Convert.ToBase64String(Encrypt(text, key, vector));

            // Format and return the encrypted data.
            return EncryptedValuePrefix + Convert.ToBase64String(vector) + ";" + encryptedText;
        }
        /// <summary>
        /// compare user supplied password and database EncryptedPassword
        /// </summary>
        /// <param name="plainPassword"></param>
        /// <param name="savePassword"></param>
        /// <returns>returns true or false</returns>
        public bool ComparePassword(string plainPassword, string savePassword)
        {
            var decryptSavePassword = "";
            try
            {
                decryptSavePassword = DecryptString(savePassword);
                return decryptSavePassword.Equals(plainPassword);

            }
            catch (Exception ex)
            {
                Log.Error(savePassword +" , "+ ex.Message.ToString());
                
            }
            //            var encryptedPassword = EncryptString(plainPassword);
            //          return encryptedPassword.Equals(savePassword);
            return false;
        }

        /// <summary>
        /// Determines if a specified text is encrypted.
        /// </summary>
        /// <param name="text">The text to check</param>
        /// <returns>True if the text is encrypted, otherwise false</returns>
        public bool IsEncrypted(string text) =>
            text.StartsWith(EncryptedValuePrefix, StringComparison.OrdinalIgnoreCase);

        #endregion

        #region Private

        /// <summary>
        /// Decrypts the specified byte array to plain text.
        /// </summary>
        /// <param name="encryptedBytes">The encrypted byte array</param>
        /// <param name="key">The encryption key</param>
        /// <param name="vector">The initialization vector</param>
        /// <returns>The decrypted text as a string</returns>
        private string Decrypt(byte[] encryptedBytes, byte[] key, byte[] vector)
        {
            using (var aesAlgorithm = Aes.Create())
            using (var decryptor = aesAlgorithm.CreateDecryptor(key, vector))
            using (var memoryStream = new MemoryStream(encryptedBytes))
            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            using (var streamReader = new StreamReader(cryptoStream, Encoding.UTF8))
            {
                return streamReader.ReadToEnd();
            }
        }

        /// <summary>
        /// Encrypts the specified text and returns an encrypted byte array.
        /// </summary>
        /// <param name="plainText">The text to encrypt</param>
        /// <param name="key">The encryption key</param>
        /// <param name="vector">The initialization vector</param>
        /// <returns>The encrypted text as a byte array</returns>
        private byte[] Encrypt(string plainText, byte[] key, byte[] vector)
        {
            using (var aesAlgorithm = Aes.Create())
            using (var encryptor = aesAlgorithm.CreateEncryptor(key, vector))
            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                using (var streamWriter = new StreamWriter(cryptoStream, Encoding.UTF8))
                {
                    streamWriter.Write(plainText);
                }

                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Generates a random initialization vector.
        /// </summary>
        /// <returns>The initialization vector as a byte array</returns>
        private byte[] GenerateInitializationVector()
        {
            var aesAlgorithm = Aes.Create();
            aesAlgorithm.GenerateIV();

            return aesAlgorithm.IV;
        }

        #endregion        
        #endregion
    }
}
