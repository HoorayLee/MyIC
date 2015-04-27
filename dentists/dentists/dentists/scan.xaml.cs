using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using sendImg;

namespace dentists
{
  
    public partial class scan : PhoneApplicationPage
    { 
        
        IsolatedStorageSettings iss = IsolatedStorageSettings.ApplicationSettings;
        List<BitmapImage> imagelist;
        SendImg send;
        public scan()
        {
            send = new SendImg();
            imagelist = new List<BitmapImage>();
            InitializeComponent();
        }

        
        //private void Button_Click_2(object sender, RoutedEventArgs e)
        //{
        //    WebClient webclient = new WebClient();
        //    webclient.OpenReadAsync(new Uri("http://192.168.1.1:8080/?action=snapshot"));
        //    webclient.OpenReadCompleted += new OpenReadCompletedEventHandler((a, b) =>
        //    {
        //        BitmapImage image = new BitmapImage();
        //        image.SetSource(b.Result);
        //        Image images = new Image();
        //        images.Source = image;
        //        images.Height = 300;
        //        images.Width = 300;
        //        images.Stretch = System.Windows.Media.Stretch.Fill;
        //        LayoutRoot.Children.Add(images);

        //    });
        //}

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            (sender as Button).IsEnabled = false;
            WebClient webclient = new WebClient();
            webclient.OpenReadAsync(new Uri("http://192.168.1.1:8080/?action=snapshot"));
            webclient.OpenReadCompleted += new OpenReadCompletedEventHandler((a, b) =>
            {
                if (b.Error == null)
                {
                    NavigationService.Navigate(new Uri("/Page1.xaml", UriKind.Relative));
                }
                else
                {
                    MessageBox.Show("未找到口腔内窥镜设备");
                    (sender as Button).IsEnabled = true;
                }       
            });
        }
        private void Button_Click_send(object sender, RoutedEventArgs e) 
        {
            (sender as Button).IsEnabled = false;
            foreach(BitmapImage a in imagelist)
            {
                send.SendImageToServer(a);
            }
            stack.Children.Clear();
            (sender as Button).IsEnabled = true;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            getpicture.IsEnabled = true;
            if (iss.Contains("ID2"))
            {
                BitmapImage bitmap = (BitmapImage)iss["ID2"];
                this.imagelist.Add(bitmap);
                Image images = new Image();
                images.Source = bitmap;
                images.Height = 230;
                images.Width = 440;
                images.Stretch = System.Windows.Media.Stretch.Fill;
                stack.Children.Add(images);
                
            }
        }
    }
}