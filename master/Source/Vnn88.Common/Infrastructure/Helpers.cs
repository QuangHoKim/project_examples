using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Vnn88.Common.Infrastructure
{
    public static class Helpers
    {
        private static readonly Random Random = new Random();
        public static IConfiguration Configuration { get; set; }

        public static void EnsureMapLengthString(ref string str1, ref string str2)
        {
            if (str1.Length > str2.Length)
            {
                var subLength = str1.Length - str2.Length;
                if (subLength == 0)
                    return;

                for (var i = 0; i < subLength; i++)
                {
                    str2 += str2[i];
                }
                EnsureMapLengthString(ref str1, ref str2);
            }
            else
            {
                var subLength = str2.Length - str1.Length;
                if (subLength == 0)
                    return;

                for (var i = 0; i < subLength; i++)
                {
                    str1 += str1[i];
                }
                EnsureMapLengthString(ref str2, ref str1);
            }
        }
        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.Claims.FirstOrDefault(m => m.Type == Constants.ClaimName.AccountId)?.Value);
        }

        public static int GetRole(this ClaimsPrincipal user)
        {
            return int.Parse(user.Claims.FirstOrDefault(m => m.Type == Constants.ClaimName.Role)?.Value);
        }

        /// <summary>
        /// Mã hóa chuỗi có mật khẩu
        /// </summary>
        /// <param name="toEncrypt">Chuỗi cần mã hóa</param>
        /// <returns>Chuỗi đã mã hóa</returns>
        public static string Encrypt(string toEncrypt)
        {
            const string key = "vshop";
            byte[] keyArray;
            var toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

            {
                var hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
            }

            var tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            var cTransform = tdes.CreateEncryptor();
            var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// Giải mã
        /// </summary>
        /// <param name="toDecrypt">Chuỗi đã mã hóa</param>
        /// <returns>Chuỗi Giải mã</returns>
        public static string Decrypt(string toDecrypt)
        {
            const string key = "vshop";
            byte[] keyArray;
            var toEncryptArray = Convert.FromBase64String(toDecrypt);

            {
                var hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
            }

            var tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            var cTransform = tdes.CreateDecryptor();
            var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }
    }
}
