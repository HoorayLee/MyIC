using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;
using sendImg;
using System.IO.IsolatedStorage;
namespace dentists
{
    public partial class Page1 : PhoneApplicationPage
    {
        private bool iscontinue = true;
        BitmapImage images;
        public Page1()
        {
            InitializeComponent();
            image.Stretch = System.Windows.Media.Stretch.Fill;
            ThreadStart start = new ThreadStart(getpicture);
            Thread thread = new Thread(start);
            thread.Start();
        }
        
        public void getpicture() 
        {
            while (iscontinue)
            {
                WebClient webclient = new WebClient(); 
                webclient.OpenReadAsync(new Uri("http://192.168.1.1:8080/?action=snapshot"));
                webclient.OpenReadCompleted += new OpenReadCompletedEventHandler((a, b) =>
                {
                    if (b.Error == null)
                    {
                        
                        Deployment.Current.Dispatcher.BeginInvoke(()=>
                        {
                            images = new BitmapImage();
                            images.SetSource(b.Result);
                            image.Source = images;
                        });
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(() => { MessageBox.Show("未找到口腔内窥镜设备"); });
                    }
                });
                Thread.Sleep(30);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) 
        {
            iscontinue = false;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            iscontinue = true;
        }
        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            iscontinue = false;
            IsolatedStorageSettings iss = IsolatedStorageSettings.ApplicationSettings;
            BitmapImage img = new BitmapImage();
            iss["ID2"] = images;
            NavigationService.GoBack();
        }
    }
}