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
    /// <summary>
    /// 窗体：即时通讯
    /// </summary>
    public partial class ContactsForm : Form
    {
        public ContactsForm()
        {
            InitializeComponent();
            LoginForm form = new LoginForm();
            form.TopLevel = false;
            
        }


        /// <summary>
        /// 双击事件：点击某个联系人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lv_contacts_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListView listView = sender as ListView;
            System.Windows.Forms.ListView.SelectedListViewItemCollection collection = listView.SelectedItems;
            string patientName = collection[0].Text;

            new InstantMessageForm(patientName).Show();
        }
    }
}
