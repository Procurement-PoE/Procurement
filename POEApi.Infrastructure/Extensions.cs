using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace POEApi.Infrastructure
{
    public static class Extensions
    {
        public static string GetHash(this string input)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(input)))
                sb.Append(item.ToString("x2").ToLower());

            return sb.ToString();
        }

        public static T GetEnum<T>(this XAttribute attribute)
        {
            return (T)Enum.Parse(typeof(T), attribute.Value);
        }

        public static byte[] ReadAllBytes(this StreamReader reader)
        {
            List<byte> bytes = new List<byte>();
            byte[] buffer = new byte[1024];

            int readBytes = -1;
            while (readBytes != 0)
            {
                readBytes = reader.BaseStream.Read(buffer, 0, buffer.Length);
                bytes.AddRange(buffer.Take(readBytes));
            }

            return bytes.ToArray();
        }

        public static string SafeSubString(this string text, int start, int length)
        {
            return text.SafeSubString(start, length, string.Empty);
        }

        public static string SafeSubString(this string text, int start, int length, string suffix)
        {
            if (length > text.Length)
                return text;

            return string.Concat(text.Substring(start, length), suffix);
        }

        public static string GetEntry(this Dictionary<string, string> dictionary, string key) 
        {
            if (!dictionary.ContainsKey(key))
                return string.Empty;

            return dictionary[key];
        }
    }
}
