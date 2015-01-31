using Doctor.Forms;
using Doctor.Panels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Doctor
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 点击事件：退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picBox_exit_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("确定要退出吗？", "退出", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                this.Close();
            }
        }

        /// <summary>
        /// 点击事件：登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picBox_login_Click(object sender, EventArgs e)
        {
            LoginForm form = new LoginForm();
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog();
        }

        /// <summary>
        /// 点击事件：注册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picBox_register_Click(object sender, EventArgs e)
        {
            RegisterForm form = new RegisterForm();
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog();
        }

        /// <summary>
        /// 点击事件：注销
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picBox_logout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要注销吗？", "注销", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                //注销后的操作

            }
        }

        /// <summary>
        /// 点击事件：个人信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picBox_selfInfo_Click(object sender, EventArgs e)
        {
            SelfInfoForm form = new SelfInfoForm();
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            form.TopLevel = false;
            this.panel.Controls.Clear();
            this.panel.Controls.Add(form);
            form.Show();
        }

        /// <summary>
        /// 点击事件：查看自检
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picBox_check_Click(object sender, EventArgs e)
        {
            SelfCheckForm form = new SelfCheckForm();
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            form.TopLevel = false;
            this.panel.Controls.Clear();
            this.panel.Controls.Add(form);
            form.Show();
        }

        /// <summary>
        /// 点击事件：即时通讯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picBox_message_Click(object sender, EventArgs e)
        {
            ContactsForm form = new ContactsForm();
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            form.TopLevel = false;
            this.panel.Controls.Clear();
            this.panel.Controls.Add(form);
            form.Show();
        }

        /// <summary>
        /// 点击事件：显示帮助
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picBox_help_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }
    }
}
