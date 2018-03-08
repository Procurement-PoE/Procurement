using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.IO;

namespace POEApi.Infrastructure
{
    public static class DPAPI
    {
        private static RNGCryptoServiceProvider rngProvider = new RNGCryptoServiceProvider();

        public static string Encrypt(this SecureString secret)
        {
            if (secret == null)
                throw new ArgumentNullException("secret");

            IntPtr ptr = Marshal.SecureStringToCoTaskMemUnicode(secret);
            try
            {
                byte[] entropy = new byte[16];
                rngProvider.GetBytes(entropy);

                char[] buffer = new char[secret.Length];
                Marshal.Copy(ptr, buffer, 0, secret.Length);

                byte[] data = Encoding.Unicode.GetBytes(buffer);
                byte[] encrypted = ProtectedData.Protect(data, entropy, DataProtectionScope.CurrentUser);

                List<byte> saltInclusive = new List<byte>();
                saltInclusive.AddRange(entropy);
                saltInclusive.AddRange(encrypted);

                return Convert.ToBase64String(saltInclusive.ToArray());
            }
            finally
            {
                Marshal.ZeroFreeCoTaskMemUnicode(ptr);
            }
        }

        public static SecureString Decrypt(this string cipher)
        {
            if (cipher == null) throw new ArgumentNullException("cipher");

            byte[] saltInclusive = Convert.FromBase64String(cipher);
            MemoryStream ms;

            byte[] entropy;
            byte[] data;

            using (ms = new MemoryStream(saltInclusive))
            {
                BinaryReader reader = new BinaryReader(ms, Encoding.Unicode);
                entropy = reader.ReadBytes(16);
                data = reader.ReadBytes(saltInclusive.Length - 16);
            }

            byte[] decrypted = ProtectedData.Unprotect(data, entropy, DataProtectionScope.CurrentUser);

            SecureString secured = new SecureString();

            int count = Encoding.Unicode.GetCharCount(decrypted);
            if (count > 0)
            {
                int bc = decrypted.Length / count;

                for (int i = 0; i < count; i++)
                    secured.AppendChar(Encoding.Unicode.GetChars(decrypted, i * bc, bc)[0]);
            }

            secured.MakeReadOnly();

            return secured;
        }

        public static string UnWrap(this SecureString secret)
        {
            if (secret == null)
                throw new ArgumentNullException("secret");

            IntPtr ptr = Marshal.SecureStringToCoTaskMemUnicode(secret);
            try
            {
                return Marshal.PtrToStringUni(ptr);
            }
            finally
            {
                Marshal.ZeroFreeCoTaskMemUnicode(ptr);
            }
        }
    }
}
