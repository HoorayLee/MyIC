using Doctor.Forms;
using Doctor.Panels;
using Doctor.Properties;
using Doctor.Util;
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
        /// 窗口载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            //载入时为用户登出状态
            Logout();

            //注册登录信息改变时事件
            LoginStatus.LoginStatusChanged += LoginStatus_LoginStatusChanged;
        }

        private void Login()
        {
            //状态栏显示
            lbl_status.Text = "用户名：" + LoginStatus.UserInfo.Name;

            //左侧按钮启用
            picBox_check.Enabled = true;
            picBox_message.Enabled = true;
            picBox_selfInfo.Enabled = true;
            picBox_logout.Enabled = true;

            picBox_check.Image = Resources.查看自检;
            picBox_message.Image = Resources.联系人;
            picBox_selfInfo.Image = Resources.个人信息;
            picBox_logout.Image = Resources.注销;

            //右侧Panel清除所有
            panel.Controls.Clear();
        }

        private void Logout()
        {
            //状态栏显示
            lbl_status.Text = "未登录";

            //左侧按钮禁用
            picBox_check.Enabled = false;
            picBox_message.Enabled = false;
            picBox_selfInfo.Enabled = false;
            picBox_logout.Enabled = false;

            picBox_check.Image = Resources.查看自检_灰_;
            picBox_message.Image = Resources.联系人_灰_;
            picBox_selfInfo.Image = Resources.个人信息_灰_;
            picBox_logout.Image = Resources.注销_灰_; 

            //右侧Panel清除所有
            panel.Controls.Clear();
        }

        /// <summary>
        /// 登录信息改变时事件
        /// </summary>
        /// <param name="e"></param>
        private void LoginStatus_LoginStatusChanged(EventArgs e)
        {
            if (LoginStatus.UserInfo != null)
            {
                //登录状态
                Login();
            }
            else
            {
                //登出状态
                Logout();
            }
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
                //清除保存的登录状态
                LoginStatus.Clear();

                //清除panel中的内容
                panel.Controls.Clear();
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
