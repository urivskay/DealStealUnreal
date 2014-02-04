using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace DealStealUnreal.Security
{
        public static class Hash
        {
            /// <summary>
            /// Hash a string
            /// </summary>
            /// <param name="input">String to hash</param>
            /// <returns>Hashed string</returns>
            public static string HashString(string input)
            {
                var data = System.Text.Encoding.ASCII.GetBytes(input);
                data = System.Security.Cryptography.MD5.Create().ComputeHash(data);
                return Convert.ToBase64String(data);
            }
        }
}