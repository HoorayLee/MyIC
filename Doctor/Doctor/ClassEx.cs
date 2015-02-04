using System;
using System.Collections.Generic;
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
    }
}
