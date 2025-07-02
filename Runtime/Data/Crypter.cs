using System;
using System.Text;

namespace UnityUtils.Data
{
    /// <summary>
    /// Provides methods for encrypting and decrypting strings using a simple XOR-based algorithm.
    /// </summary>
    public class Crypter
    {
        /// <summary>
        /// The encryption key used for the XOR operation.
        /// </summary>
        private static string encryptionKey = "";

        /// <summary>
        /// Encrypts the given input string using the encryption key.
        /// </summary>
        /// <param name="input">The input string to encrypt.</param>
        /// <returns>The encrypted string, encoded in Base64.</returns>
        public static string Encrypt(string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] keyBytes = Encoding.UTF8.GetBytes(encryptionKey);

            byte[] encryptedBytes = new byte[inputBytes.Length];
            for (int i = 0; i < inputBytes.Length; i++)
            {
                encryptedBytes[i] = (byte)(inputBytes[i] ^ keyBytes[i % keyBytes.Length]);
            }

            return Convert.ToBase64String(encryptedBytes);
        }

        /// <summary>
        /// Decrypts the given input string using the encryption key.
        /// </summary>
        /// <param name="input">The encrypted string, encoded in Base64, to decrypt.</param>
        /// <returns>The decrypted string.</returns>
        public static string Decrypt(string input)
        {
            byte[] encryptedBytes = Convert.FromBase64String(input);
            byte[] keyBytes = Encoding.UTF8.GetBytes(encryptionKey);

            byte[] decryptedBytes = new byte[encryptedBytes.Length];
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                decryptedBytes[i] = (byte)(encryptedBytes[i] ^ keyBytes[i % keyBytes.Length]);
            }

            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}