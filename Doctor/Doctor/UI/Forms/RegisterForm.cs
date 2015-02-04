using Doctor.DAL;
using Doctor.Model;
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
    public partial class RegisterForm : Form
    {
        private string photoPath;
        private string licensePath;

        public RegisterForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 点击事件：确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_confirm_Click(object sender, EventArgs e)
        {
            //基本信息
            //1.用户名
            string username = tb_username.Text.Trim();
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("请输入用户名");
                tb_username.Focus();
                return;
            }

            //2.密码
            string password = tb_password.Text;
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("请输入密码");
                tb_password.Focus();
                return;
            }

            //3.再次输入密码
            string passwordAgain = tb_passwordAgain.Text;
            if (string.IsNullOrEmpty(passwordAgain))
            {
                MessageBox.Show("请再次输入密码");
                tb_passwordAgain.Focus();
                return;
            }
            if (!password.Equals(passwordAgain))
            {
                MessageBox.Show("两次输入密码不一致");
                tb_passwordAgain.Focus();
                return;
            }
            
            //认证信息
            //1.真实姓名
            string realName = tb_realname.Text.Trim();
            if (string.IsNullOrEmpty(realName))
            {
                MessageBox.Show("请输入真实姓名");
                tb_realname.Focus();
                return;
            }

            //2.省市县医院选择
            if (cb_province.SelectedIndex < 0)
            {
                MessageBox.Show("请选择省份");
                cb_province.Focus();
                return;
            }

            if (cb_city.SelectedIndex < 0)
            {
                MessageBox.Show("请选择城市");
                cb_city.Focus();
                return;
            }

            if (cb_area.SelectedIndex < 0)
            {
                MessageBox.Show("请选择区域");
                cb_area.Focus();
                return;
            }

            if (cb_hospital.SelectedIndex < 0)
            {
                MessageBox.Show("请选择医院");
                cb_hospital.Focus();
                return;
            }

            //3.执业医师证编码
            string licenseNo = tb_license.Text.Trim();
            if (string.IsNullOrEmpty(licenseNo))
            {
                MessageBox.Show("请输入执业医师证编码");
                tb_license.Focus();
                return;
            }

            if (string.IsNullOrEmpty(licensePath))
            {
                MessageBox.Show("请选择执业医师证图片");
                return;
            }

            DoctorModel model = new DoctorModel();
            model.Name = username;
            model.Password = password;
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

        /// <summary>
        /// 点击事件：选择相片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_selectPhoto_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                photoPath = openFileDialog.FileName;

                picBox_photo.Image = Image.FromFile(photoPath);
                lbl_photo.Text = photoPath;
                lbl_photo.ForeColor = Color.Black;
            }
        }

        /// <summary>
        /// 点击事件：选择医师证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_selectLicense_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                licensePath = openFileDialog.FileName;

                picBox_license.Image = Image.FromFile(licensePath);
                lbl_license.Text = licensePath;
                lbl_license.ForeColor = Color.Black;
            }
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            Hat_provinceModel[] provinces = Hat_provinceDAL.GetAll();
            cb_province.DataSource = provinces;
            cb_province.DisplayMember = "province";
        }

        /// <summary>
        /// 选择省
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_province_SelectedIndexChanged(object sender, EventArgs e)
        {
            Hat_provinceModel province = (Hat_provinceModel)cb_province.SelectedItem;
            Hat_cityModel[] cities = Hat_cityDAL.GetAllByProvinceId(province.Id);
            cb_city.DataSource = cities;
            cb_city.DisplayMember = "city";
        }

        /// <summary>
        /// 选择市
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_city_SelectedIndexChanged(object sender, EventArgs e)
        {
            Hat_cityModel city = (Hat_cityModel)cb_city.SelectedItem;
            Hat_areaModel[] areas = Hat_areaDAL.GetAllByCityId(city.Id);
            cb_area.DataSource = areas;
            cb_area.DisplayMember = "area";
        }
    }
}
