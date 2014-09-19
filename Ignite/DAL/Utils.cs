using System;
using System.Security.Cryptography;
using System.Text;
using Custom.String.Random;

namespace Ignite.DAL
{
    internal static class Utils
    {        
        private const int saltValueSize = 8;
        private static readonly RandomStringGenerator rsgPW;

        private static readonly RandomStringGenerator rsg;

        /// <summary>
        /// 
        /// </summary>
        static Utils()
        {
            rsg = new RandomStringGenerator(1, "ABCDEF0123456789");
            rsgPW = new RandomStringGenerator(1, "ABCDEFGHIJKLMNPRXTUVWXYZabcdefghijklmnpqrxtuvwxyz123456789#&$"); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        internal static string GetRandomStringPW(int size)
        {
            return rsgPW.NextString(size, size);
        }

        public static string GenerateRandomPassword()
        {
            return GetRandomStringPW(7);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal static string GenerateUserToken()
        {
            DateTime now = DateTime.Now;

            string token = String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}"
                                         , rsg.NextString()
                                         , now.ToString("yy")
                                         , rsg.NextString()
                                         , now.ToString("MM")
                                         , rsg.NextString()
                                         , now.ToString("dd")
                                         , rsg.NextString()
                                         , now.ToString("HH")
                                         , rsg.NextString()
                                         , now.ToString("mm")
                                         , rsg.NextString()
                                         , now.ToString("ss")
                                         , rsg.NextString());

            return token;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        internal static string GetRandomString(int size)
        {
            return rsg.NextString(size, size);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clearProperty"></param>
        /// <param name="saltValue">8 lenght hexadecimal string</param>
        /// <returns></returns>
        internal static string HashProperty(string clearProperty, string saltValue)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            UnicodeEncoding encoding = new UnicodeEncoding();            

            byte[] binarySaltValue = new byte[saltValueSize];

            binarySaltValue[0] = byte.Parse(saltValue.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            binarySaltValue[1] = byte.Parse(saltValue.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            binarySaltValue[2] = byte.Parse(saltValue.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            binarySaltValue[3] = byte.Parse(saltValue.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

            byte[] valueToHash = new byte[saltValueSize + encoding.GetByteCount(clearProperty)];
            byte[] binaryProperty = encoding.GetBytes(clearProperty);

            binarySaltValue.CopyTo(valueToHash, 0);
            binaryProperty.CopyTo(valueToHash, saltValueSize);

            byte[] hashValue = md5.ComputeHash(valueToHash);

            string hashedProperty = saltValue;

            foreach (byte hexDigit in hashValue)
            {
                hashedProperty += hexDigit.ToString("X2");
            }

            return hashedProperty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hashedProperty"></param>
        /// <param name="clearProperty"></param>
        /// <returns></returns>
        internal static bool IsEqual(string hashedProperty, string clearProperty)
        {
            string saltValue = ReturnSaltValue(hashedProperty);
            string hashedClearProperty = HashProperty(clearProperty, saltValue);

            if (hashedProperty == hashedClearProperty)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hashedProperty"></param>
        /// <returns></returns>
        internal static string ReturnSaltValue(string hashedProperty)
        {
            return hashedProperty.Substring(0, saltValueSize);
        }
    }
}