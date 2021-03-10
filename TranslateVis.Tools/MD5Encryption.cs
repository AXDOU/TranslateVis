using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace TranslateVis.Tools
{
    public class MD5Encryption
    {
        /// <summary>
        /// MD5加密(UTF8编码)
        /// </summary>
        /// <param name="plaintext">明文</param>
        /// <returns></returns>
        public static string Encryption(string plaintext)
        {
            if (string.IsNullOrWhiteSpace(plaintext)) return plaintext;
            MD5CryptoServiceProvider service = new MD5CryptoServiceProvider();
            byte[] _bytes = Encoding.UTF8.GetBytes(plaintext);
            byte[] hashByte = service.ComputeHash(_bytes);
            string ciphertext = string.Join("", hashByte.Select(x => x.ToString("x2")));
            return ciphertext;
        }
        /// <summary>
        /// 文档摘要加密
        /// </summary>
        /// <param name="fileStream">文件流</param>
        /// <returns></returns>
        public static string FileSummaryEncryption(Stream fileStream)
        {
            MD5CryptoServiceProvider service = new MD5CryptoServiceProvider();
            byte[] _bytes = new byte[fileStream.Length];
            fileStream.Read(_bytes, 0, _bytes.Length);
            byte[] encryptionBytes = service.ComputeHash(_bytes);
            string ciphertext = string.Join("", encryptionBytes.Select(x => x.ToString("x2")));
            return ciphertext;
        }
    }
}
