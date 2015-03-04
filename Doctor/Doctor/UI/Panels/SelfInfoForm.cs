using Doctor.Forms;
using Doctor.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Doctor.Panels
{
    public partial class SelfInfoForm : Form
    {
        /// <summary>
        /// 窗体：医生个人信息
        /// </summary>
        public SelfInfoForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 点击事件：修改密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void link_modifyPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ModifyPasswordForm form = new ModifyPasswordForm();
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog();
        }
        public static Bitmap KiResizeImage(Bitmap bmp, int newW, int newH)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);

                // 插值算法的质量
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();

                return b;
            }
            catch
            {
                return null;
            }
        }
        private void SelfInfoForm_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (null != LoginStatus.UserInfo && !string.IsNullOrEmpty(LoginStatus.UserInfo.PhotoPath))
            {
                DoctorModel userInfo = LoginStatus.UserInfo;
                Bitmap image = null;

                //下载并显示个人照片
                string photoPath = Path.Combine(GeneralHelper.DownloadPicFolder, userInfo.PhotoPath);
                if (!File.Exists(photoPath))
                {
                    HttpHelper.DownloadFile("PicDownloadHandler.ashx", userInfo.PhotoPath);
                }
                image = new Bitmap(photoPath);
                image = KiResizeImage(image, picBox_photo.Width, picBox_photo.Height);
                picBox_photo.BackgroundImage = image;
                
                //下载并显示医师证
                string licensePath = Path.Combine(GeneralHelper.DownloadPicFolder, userInfo.LicensePath);
                if (!File.Exists(licensePath))
                {
                    HttpHelper.DownloadFile("PicDownloadHandler.ashx", userInfo.LicensePath);         
                }
                image = new Bitmap(licensePath);
                image = KiResizeImage(image, picBox_license.Width, picBox_license.Height);               
                picBox_license.BackgroundImage = image;

                //其他UI控件显示
                lbl_username.Text = userInfo.Name;

                if (userInfo.Hospital_id == null)
                {
                    lbl_hospital.Text = "未填写";
                }
                else
                {
                    string responseStr = null;
                    bgWorker.DoWork += (a, b) =>
                    {
                        responseStr = HttpHelper.ConnectionForResult("HospitalInfoHandler.ashx", userInfo.Hospital_id.ToString());
                    };
                    bgWorker.RunWorkerCompleted += (a, b) =>
                    {
                        HospitalModel hospital = JsonConvert.DeserializeObject<HospitalModel>(responseStr);
                        lbl_hospital.Text = hospital.Name;
                    };
                    bgWorker.RunWorkerAsync();
                }

                lbl_license.Text = (userInfo.LicenseNo == null ? "未填写" : userInfo.LicenseNo);
                lbl_realname.Text = (userInfo.RealName == null ? "未填写" : userInfo.RealName);

                //认证与否
                if (userInfo.IfAuth)
                {
                    lbl_ifAuth.Text = "已认证";
                    lbl_ifAuth.ForeColor = Color.Black;
                }
                else
                {
                    lbl_ifAuth.Text = "未认证";
                    lbl_ifAuth.ForeColor = Color.Red;
                }
            }
            this.Cursor = Cursors.Default;
        }
    }
}
