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

namespace Doctor.Forms
{
    public partial class SelfCheckDetailForm : Form
    {
        public static int Record_id = 0;
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
            DiagnosisModel model = new DiagnosisModel();
           // model.Diagnosis_id = 1;
            model.Record_id = Record_id;
            model.Result = textBox1.Text;
            model.Time = System.DateTime.Now;
           string result =  HttpHelper.ConnectionForResult("DiagnosisHandler.ashx", JsonConvert.SerializeObject(model));
           JObject jObjResult = JObject.Parse(result);
           string state = (string)jObjResult.Property("state");
           if (state != "success")
           {
               MessageBox.Show("提交失败！");
           }
           else
           {

               MessageBox.Show("提交成功！");
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

        private void SelfCheckDetailForm_Load(object sender, EventArgs e)
        {
            if (LoginStatus.UserInfo.IfAuth == true)
            {
                textBox1.ReadOnly = false;
                label4.Visible = false;
            }                
            else
            {
                textBox1.ReadOnly = true;
                label4.Visible = true;
            }
                
        }
    }
}
