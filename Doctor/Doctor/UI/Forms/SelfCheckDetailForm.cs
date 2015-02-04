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
    public partial class SelfCheckDetailForm : Form
    {
        public SelfCheckDetailForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 点击事件：提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_submit_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 点击事件：取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
