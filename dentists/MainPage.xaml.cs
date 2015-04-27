using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Shapes;
using System.Windows.Media;
using Com.AMap.Api.Maps.Model;
using Com.AMap.Api.Maps;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using Coding4Fun.Toolkit.Controls;
using IMClient_WinForm;
using dentists.Data.IMdata;
using dentists.Page.doctors;
using dentists.Page.im.IMdata;
using Model;
namespace dentists
{
    public partial class MainPage : PhoneApplicationPage
    {
        IsolatedStorageSettings iss = IsolatedStorageSettings.ApplicationSettings;
        bool isExit = false;
        DoctorList doctorlist;
        // 构造函数
        public MainPage()
        {
            InitializeComponent();
            iss["ID3"] = new List<BitmapImage>();
            if (!iss.Contains("DoctorList")) 
            {
                List<ClassList> DoctorList = new List<ClassList>();
                iss["DoctorList"] = new DoctorList(DoctorList);
            }
            MyIMClient.Login("365");
            doctorlist = iss["DoctorList"] as DoctorList;
            if(doctorlist.totalunread!=0)
            {
                this.docur.Visibility = System.Windows.Visibility.Visible;
                this.text.Text = doctorlist.totalunread.ToString();
            }
        }

        // 为 ViewModel 项加载数据
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (doctorlist.totalunread != 0)
            {
                this.docur.Visibility = System.Windows.Visibility.Visible;
                this.text.Text = doctorlist.totalunread.ToString();
            }
        }
        private void Button_Click_Scan(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ScanNow/scan.xaml", UriKind.Relative));
        }

        private void Button_Click_Doctor(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page/doctors/doctor.xaml", UriKind.Relative));
        }

        private void Button_Click_History(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_Book(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_Litter(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_Hints(object sender, RoutedEventArgs e)
        {

        }
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (!isExit)
            {
                isExit = true;
                var toast = new ToastPrompt { Message = "再按一次退出程序" };
                toast.Visibility = System.Windows.Visibility.Visible;
                toast.Completed += (o, ex) => { isExit = false; };
                toast.Show();
                e.Cancel = true;
            }
            else
            {
                NavigationService.RemoveBackEntry();
            }
        }
    }
}