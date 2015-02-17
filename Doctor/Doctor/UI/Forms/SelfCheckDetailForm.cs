using Doctor.Model;
using Doctor.UI.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Doctor.Forms
{
    public partial class SelfCheckDetailForm : Form
    {
        private RecordModel record;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="record"></param>
        public SelfCheckDetailForm(RecordModel record)
        {
            InitializeComponent();
            this.record = record;
        }

        /// <summary>
        /// 窗口载入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelfCheckDetailForm_Load(object sender, EventArgs e)
        {
            //获取与病人有关的信息
            string result = HttpHelper.ConnectionForResult("PatientInfoHandler.ashx", 
                record.User_id.ToString());
            if (result != null)
            {
                UserModel patient = JsonConvert.DeserializeObject<UserModel>(result);
                lbl_patientName.Text = patient.Name;
                if(patient.Date_of_birth != null)
                {
                    TimeSpan timeSpan = DateTime.Now - (DateTime)patient.Date_of_birth;
                    int age = timeSpan.Days / 365;
                    lbl_age.Text = age + "岁";
                }
                else
                {
                    lbl_age.Text = "";
                }
            }

            //获取自检图片
            string photosResult = HttpHelper.ConnectionForResult("SelfCheckPhotosHandler.ashx",
                record.Record_id.ToString());
            if (photosResult != null)
            {
                JObject jObj = JObject.Parse(photosResult);
                int nbPhotos = (int)jObj["count"];

                for (int i = 0; i < nbPhotos; i++)
                {
                    string photoPath = jObj["content"][i].ToString();
                    if(!HttpHelper.DownloadFile("SelfCheckPhotoDownloadHandler.ashx", photoPath))
                    {
                        MessageBox.Show("获取图片失败");
                    }
                    else
                    {
                        PictureBox picBox = new PictureBox();
                        picBox.Image = Image.FromFile(Path.Combine(GeneralHelper.DownloadPicFolder,
                            photoPath));
                        picBox.SizeMode = PictureBoxSizeMode.Zoom;
                        picBox.Cursor = Cursors.Hand;
                        picBox.MouseClick += (obj, ev) => { new PicLargeForm(photoPath).ShowDialog(); };
                        flowLayoutPanel.Controls.Add(picBox);
                    }

                }
            }
            
            //获取其他医生的历史诊断
            string prevCommentResult = HttpHelper.ConnectionForResult("DiagnosisHandler.ashx", 
                record.Record_id.ToString());
            if (prevCommentResult != null)
            {
                JObject jObjComment = JObject.Parse(prevCommentResult);
                int nbDiagnoses = (int)jObjComment["count"];

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < nbDiagnoses; i++)
                {
                    var item = jObjComment["content"][i];
                    string docName = (string)item["realname"];
                    string comment = (string)item["comment"];
                    DateTime time = (DateTime)item["time"];

                    builder.AppendFormat("{0} {1}", time.ToString(), docName).AppendLine();
                    builder.AppendLine(comment);
                    builder.AppendLine();
                }
                builder.RemoveLine();
                tb_prevComment.Text = builder.ToString();
            }


            tb_description.Text = record.Description;

            if (LoginStatus.UserInfo.IfAuth)
            {
                tb_comment.ReadOnly = false;
                lbl_ifAuth.Visible = false;
            }
            else
            {
                tb_comment.ReadOnly = true;
                lbl_ifAuth.Visible = true;
            }

        }
        /// <summary>
        /// 点击事件：提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_submit_Click(object sender, EventArgs e)
        {
            //检查输入的诊断是否为空
            string comment = tb_comment.Text;
            if (string.IsNullOrEmpty(comment))
            {
                MessageBox.Show("请输入您的意见");
                tb_comment.Focus();
                return;
            }

            //组装诊断信息类实例
            DiagnosisModel model = new DiagnosisModel();
            //model.Diagnosis_id = 1;
            model.Record_id = record.Record_id;
            model.Result = comment;
            model.Time = System.DateTime.Now;
            model.Doc_id = LoginStatus.UserInfo.Doc_id;
            
            string result = HttpHelper.ConnectionForResult("DiagnosisHandler.ashx", JsonConvert.SerializeObject(model));
            if (null == result)
            {
                MessageBox.Show("连接失败");
                return;
            }
            
            JObject jObjResult = JObject.Parse(result);
            string state = (string)jObjResult.Property("state");
            if (state != "success")
            {
                MessageBox.Show("提交失败！");
            }
            else
            {
                MessageBox.Show("提交成功！");
                this.Close();
            }
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
