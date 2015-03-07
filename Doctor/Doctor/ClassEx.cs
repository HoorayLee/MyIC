using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Doctor
{
    public static class ClassEx
    {
        /// <summary>
        /// 检查TextBox为空的扩展方法
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="trim">是否去除空格再检查</param>
        /// <param name="msg">为空时的信息</param>
        /// <returns></returns>
        public static bool CheckTextBoxEmpty(this TextBox textBox, bool trim, string msg)
        {
            string content = textBox.Text;
            if (trim)
            {
                content = content.Trim();
            }

            if (string.IsNullOrEmpty(content))
            {
                MessageBox.Show(msg);
                textBox.Focus();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除StringBuilder尾部的一个回车
        /// </summary>
        /// <param name="builder"></param>
        public static void RemoveLine(this StringBuilder builder)
        {
            string newLine = Environment.NewLine;
            string str = builder.ToString();
            if (str.Length < newLine.Length)
            {
                return;
            }

            if(newLine.Equals(str.Substring(str.Length - newLine.Length)))
            {
                builder.Remove(builder.Length - newLine.Length, newLine.Length);
            }
        }

        /// <summary>
        /// 将流用UTF8解析为字符串
        /// </summary>
        /// <param name="stream"></param>
        public static string ToUTF8String(this Stream stream)
        {
            using(StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// 将字符串以UTF8写入流
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="content"></param>
        public static void WriteUTF8String(this Stream stream, string content)
        {
            byte[] buf = Encoding.UTF8.GetBytes(content);
            stream.Write(buf, 0, buf.Length);
        }
    }
}
