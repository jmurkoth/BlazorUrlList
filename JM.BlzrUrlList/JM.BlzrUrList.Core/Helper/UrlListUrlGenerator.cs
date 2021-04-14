using JM.BlzrUrlList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JM.BlzrUrList.Core.Helper
{
    public static class UrlListUrlGenerator
    {
        internal static readonly char[] chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
        public static void SetUrlId( this UrlList urlList)
        {
            if(string.IsNullOrEmpty(urlList.UrlId))
            {
                urlList.UrlId = GenerateId();
            }
        }
        private static string GenerateId()
        {
            int size = 8;
            
            byte[] data = new byte[4 * size];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }
            return result.ToString().ToLower();
        }
    }
}
