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
        /// <summary>
        /// 窗体：自检列表
        /// </summary>
        public SelfCheckForm()
        {
            InitializeComponent();
        }

        private void SelfCheckForm_Load(object sender, EventArgs e)
        {
            string selfcheck = HttpHelper.ConnectionForResult("SelfCheckHandler.ashx", "ListAll");
            JObject jObjResult = JObject.Parse(selfcheck);
            int count = (int)jObjResult["count"];
            if (count != 0)
            {
                List<RecordModel> list = new List<RecordModel>();
                JArray jlist = JArray.Parse(jObjResult["content"].ToString());
                for (int i = 0; i < jlist.Count; ++i)
                {
                    RecordModel record = JsonConvert.DeserializeObject<RecordModel>(jlist[i].ToString());
                    list.Add(record);
                }
                this.dataGridView1.AutoGenerateColumns = false;
                this.dataGridView1.DataSource = list;
                //JArray str_content = (JArray)jObjResult.Property("content");
                // JArray ja = (JArray)JsonConvert.DeserializeObject(str_content);
            }
        }

        /// <summary>
        /// 双击事件：点击某一自检项查看详细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int index = e.RowIndex;
            DataGridViewRow dgr = this.dataGridView1.Rows[index];
            if (null != dgr && !string.IsNullOrEmpty(dgr.Cells[0].Value.ToString()))
            {
                RecordModel record = dgr.DataBoundItem as RecordModel;
                //SelfCheckDetailForm.Record_id = int.Parse(dgr.Cells[0].Value.ToString());
                new SelfCheckDetailForm(record).Show();
            }
            else
            {
                MessageBox.Show("请选择数据后重试！");
            }
          
        }
    }
}
