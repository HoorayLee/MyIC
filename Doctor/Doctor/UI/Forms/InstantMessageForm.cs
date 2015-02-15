using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Doctor.Forms
{
    public partial class InstantMessageForm : Form
    {
        public InstantMessageForm(string patientName)
        {
            InitializeComponent();
            this.Text = patientName;
        }

        /// <summary>
        /// 点击事件：发送信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_send_Click(object sender, EventArgs e)
        {

        }
    }
}
