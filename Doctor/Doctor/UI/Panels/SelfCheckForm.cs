using Doctor.Forms;
using Doctor.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            SelfCheckDetailForm.Record_id = 1;
            new SelfCheckDetailForm().Show(); 
        }

        private void SelfCheckForm_Load(object sender, EventArgs e)
        {
          string selfcheck =   HttpHelper.ConnectionForResult("SelfCheckHandler.ashx", "ListAll");         
          JObject jObjResult = JObject.Parse(selfcheck);          
          int count = (int)jObjResult["count"];
          if (count != 0)
          {
              List<Doctor.Model.RecordModel> list = new List<Model.RecordModel>();
              JArray jlist = JArray.Parse(jObjResult["content"].ToString());
              for (int i = 0; i < jlist.Count; ++i)
              {
                  Doctor.Model.RecordModel rm = JsonConvert.DeserializeObject<RecordModel>(jlist[i].ToString());
                  list.Add(rm);
              }
              this.dataGridView1.DataSource = list;
              this.dataGridView1.AutoGenerateColumns = false;
              //JArray str_content = (JArray)jObjResult.Property("content");
             // JArray ja = (JArray)JsonConvert.DeserializeObject(str_content);
          }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int index = e.RowIndex;
            DataGridViewRow dgr = this.dataGridView1.Rows[index];
            if (null != dgr && !string.IsNullOrEmpty(dgr.Cells[0].Value.ToString()))
            {
                SelfCheckDetailForm.Record_id = int.Parse(dgr.Cells[0].Value.ToString());
                new SelfCheckDetailForm().Show();
            }
            else
            {
                MessageBox.Show("请选择数据后重试！");
            }
          
        }
    }
}
