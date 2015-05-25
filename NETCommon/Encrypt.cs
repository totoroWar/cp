using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NETCommon
{
    public enum MD5Bit { L16 = 16, L32 = 32 }
    public enum SHA1Bit { L160 = 160, L256 = 256, L384 = 384 }
    /// <summary>
    /// MD5编码
    /// </summary>
    public class MD5
    {
        /// <summary>
        /// MD5编码
        /// </summary>
        /// <param name="value">明文</param>
        /// <param name="bit">位长</param>
        /// <returns></returns>
        public static string Get(string value, MD5Bit bit)
        {
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();
            if (bit == MD5Bit.L32)
            {
                // Create a new instance of the MD5CryptoServiceProvider object.
                System.Security.Cryptography.MD5 md5Hasher = System.Security.Cryptography.MD5.Create();

                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
            }
            if (bit == MD5Bit.L16)
            {
                // This is one implementation of the abstract class MD5.
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

                string temp = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(value)), 4, 8);
                temp = temp.Replace("-", "");
                sBuilder.Append(temp);
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static string Get(string value)
        {
            return Get(value, MD5Bit.L32);
        }
    }

    /// <summary>
    /// SHA1编码
    /// </summary>
    public class SHA1
    {
        /// <summary>
        /// SHA1编码
        /// </summary>
        /// <param name="value">明文</param>
        /// <param name="bit">位长</param>
        /// <returns></returns>
        public static string Get(string value, SHA1Bit bit)
        {
            StringBuilder sBuilder = new StringBuilder();
            if (bit == SHA1Bit.L160)
            {
                System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
                // This is one implementation of the abstract class SHA1.
                byte[] result = sha.ComputeHash(Encoding.Default.GetBytes(value));
                sBuilder.Append(BitConverter.ToString(result).Replace("-", ""));
            }
            if (bit == SHA1Bit.L256)
            {
                System.Security.Cryptography.SHA256 sha256 = new System.Security.Cryptography.SHA256CryptoServiceProvider();
                // This is one implementation of the abstract class SHA1.
                byte[] result = sha256.ComputeHash(Encoding.Default.GetBytes(value));
                sBuilder.Append(BitConverter.ToString(result).Replace("-", ""));
            }
            if (bit == SHA1Bit.L384)
            {
                System.Security.Cryptography.SHA384 sha384 = new System.Security.Cryptography.SHA384CryptoServiceProvider();
                // This is one implementation of the abstract class SHA1.
                byte[] result = sha384.ComputeHash(Encoding.Default.GetBytes(value));
                sBuilder.Append(BitConverter.ToString(result).Replace("-", ""));
            }
            return sBuilder.ToString();
        }
    }

    public class DESString
    {
        static byte[] bytes = ASCIIEncoding.ASCII.GetBytes(ConfigHelper.GetConfigString("CJLSoftDEKey"));
        /// <summary>
        /// Encrypt a string.
        /// </summary>
        /// <param name="originalString">The original string.</param>
        /// <returns>The encrypted string.</returns>
        /// <exception cref="ArgumentNullException">This exception will be thrown when the original string is null or empty.</exception>
        public static string Encrypt(string originalString)
        {
            if (String.IsNullOrEmpty(originalString))
            {
                throw new ArgumentNullException("The string which needs to be encrypted can not be null.");
            }

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);

            StreamWriter writer = new StreamWriter(cryptoStream);
            writer.Write(originalString);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();

            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }

        /// <summary>
        /// Decrypt a crypted string.
        /// </summary>
        /// <param name="cryptedString">The crypted string.</param>
        /// <returns>The decrypted string.</returns>
        /// <exception cref="ArgumentNullException">This exception will be thrown when the crypted string is null or empty.</exception>
        public static string Decrypt(string cryptedString)
        {
            if (String.IsNullOrEmpty(cryptedString))
            {
                throw new ArgumentNullException("The string which needs to be decrypted can not be null.");
            }

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cryptedString));
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(cryptoStream);

            return reader.ReadToEnd();
        }
    }
}
