using Doctor.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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
            if (null != LoginStatus.UserInfo && !string.IsNullOrEmpty(LoginStatus.UserInfo.PhotoPath))
            {
                HttpHelper.DownloadFile("PicDownloadHandler.ashx", LoginStatus.UserInfo.PhotoPath);                
                Bitmap image = new Bitmap(Environment.CurrentDirectory + @"\DownloadFiles\" + LoginStatus.UserInfo.PhotoPath);
                image = KiResizeImage(image, picBox_photo.Width, picBox_photo.Height);
                picBox_photo.BackgroundImage = image;
                HttpHelper.DownloadFile("PicDownloadHandler.ashx", LoginStatus.UserInfo.LicensePath);
                image = new Bitmap(Environment.CurrentDirectory + @"\DownloadFiles\" + LoginStatus.UserInfo.LicensePath);
                image = KiResizeImage(image, picBox_license.Width, picBox_license.Height);               
                picBox_license.BackgroundImage = image;
             }
       }
    }
}
