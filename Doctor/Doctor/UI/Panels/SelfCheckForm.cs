using Doctor.Forms;
using Doctor.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            //查看本地是否存在缓存，如果缓存文件上次写入时间超过5分钟则重新获取
            string selfCheck = null;
            if (File.Exists(GeneralHelper.SelfCheckCache))
            {
                string fileName = GeneralHelper.SelfCheckCache;
                DateTime timeLastModified = File.GetCreationTime(fileName);
                TimeSpan span = DateTime.Now - timeLastModified;
                if(span.Minutes >= 5)
                {
                    this.Cursor = Cursors.WaitCursor;
                    selfCheck = HttpHelper.ConnectionForResult("SelfCheckHandler.ashx", "ListAll");
                    this.Cursor = Cursors.Default;
                    using (FileStream stream = new FileStream(GeneralHelper.SelfCheckCache, FileMode.OpenOrCreate))
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(selfCheck);
                        stream.Write(bytes, 0, bytes.Length);
                    }
                }
                else
                {
                    using (FileStream stream = new FileStream(fileName, FileMode.Open))
                    {
                        selfCheck = stream.ToUTF8String();
                    }
                }
            }
            else
            {
                this.Cursor = Cursors.WaitCursor;
                selfCheck = HttpHelper.ConnectionForResult("SelfCheckHandler.ashx", "ListAll");
                this.Cursor = Cursors.Default;
                using (FileStream stream = new FileStream(GeneralHelper.SelfCheckCache, FileMode.OpenOrCreate))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(selfCheck);
                    stream.Write(bytes, 0, bytes.Length);
                }
            }

            JObject jObjResult = JObject.Parse(selfCheck);
            int count = (int)jObjResult["count"];
            if (count != 0)
            {
                List<ExRecordModel> list = new List<ExRecordModel>();
                JArray jlist = JArray.Parse(jObjResult["content"].ToString());
                for (int i = 0; i < jlist.Count; ++i)
                {
                    ExRecordModel record = new ExRecordModel(JsonConvert.DeserializeObject<RecordModel>(jlist[i].ToString()));
                    list.Add(record);
                }

                //JArray str_content = (JArray)jObjResult.Property("content");
                // JArray ja = (JArray)JsonConvert.DeserializeObject(str_content);

                if (null != LoginStatus.UserIP)
                {
                    string localId = LoginStatus.GetLocalId();
                    //string localId = "510100";
                    list.Sort(new ExRecordComparer(localId));
                }

                this.dataGridView1.AutoGenerateColumns = false;
                this.dataGridView1.DataSource = list;
            }
        }

        private class ExRecordComparer : IComparer<ExRecordModel>
        {
            private int? local;

            public ExRecordComparer(string localId)
            {
                int temp;
                if (int.TryParse(localId, out temp))
                {
                    local = temp;
                }
                else
                {
                    local = null;
                }
            }

            public int Compare(ExRecordModel x, ExRecordModel y)
            {
                if (null == local)
                {
                    return 0;
                }

                ExRecordModel r1 = x as ExRecordModel;
                ExRecordModel r2 = y as ExRecordModel;
                return Math.Abs((int)local - int.Parse(r1.AreaId)) - Math.Abs((int)local - int.Parse(r2.AreaId));
            }
        }

        private class ExRecordModel : RecordModel
        {
            public ExRecordModel(RecordModel record)
            {
                this.Description = record.Description;
                this.Hat_area_id = record.Hat_area_id;
                this.Record_id = record.Record_id;
                this.Time = record.Time;
                this.User_id = record.User_id;
                this.Area = GeneralHelper.GetAreaName(this.Hat_area_id);
                this.AreaId = GeneralHelper.GetAreaId(this.Hat_area_id);
            }

            public string Area { get; set; }
            public string AreaId { get; set; }
        }

        /// <summary>
        /// 双击事件：点击某一自检项查看详细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int index = e.RowIndex;
            if (index < 0)
            {
                return;
            }

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
