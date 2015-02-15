using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doctor.Util
{
    public class MD5 
    {
        /// <summary>
        /// 返回指定字符串的消息摘要（32位加密），用户密码要MD5后保存在服务器上
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMD5(string input)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input);
            bytes = md5.ComputeHash(bytes);
            md5.Clear();

            string result = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                result += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
            }

            return result.PadLeft(32, '0');
        }
    }
}
