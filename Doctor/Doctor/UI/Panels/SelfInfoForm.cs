using Doctor.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Doctor.Panels
{
    public partial class SelfInfoForm : Form
    {
        /// <summary>
        /// 窗体：医生个人信息
        /// </summary>
        public SelfInfoForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 点击事件：修改密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void link_modifyPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ModifyPasswordForm form = new ModifyPasswordForm();
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog();
        }
    }
}
