using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Lab6
{
    public static class ExtensionMethods
    {
        public static byte[] GetBytes(this string message)
        {
            return Encoding.UTF8.GetBytes(message);
        }
        public static string ToHexString(this byte[] data)
        {
            return BitConverter.ToString(data).Replace("-", string.Empty).ToLower();
        }
        public static string GetString(this byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }

        public static string ToBase64(this byte[] data)
        {
            return Convert.ToBase64String(data);
        }

        public static byte[] FromBase64(this string data)
        {
            return Convert.FromBase64String(data);
        }
    }
}
