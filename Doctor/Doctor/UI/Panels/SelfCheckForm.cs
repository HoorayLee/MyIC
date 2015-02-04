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
    public partial class SelfCheckForm : Form
    {
        //窗体：自检列表
        public SelfCheckForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 双击事件：点击某一自检项查看详细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lv_selfCheck_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListView listView = sender as ListView;
            System.Windows.Forms.ListView.SelectedListViewItemCollection collection = listView.SelectedItems;
            string patientName = collection[0].Text;

            new SelfCheckDetailForm().Show(); 
        }
    }
}
