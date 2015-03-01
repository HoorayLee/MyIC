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
using System.Threading;

namespace dentists
{
  
    public partial class scan : PhoneApplicationPage
    { 
        
        IsolatedStorageSettings iss = IsolatedStorageSettings.ApplicationSettings;
        List<BitmapImage> imagelist;
        SendImg send;
        public scan()
        {
            if (iss.Contains("ID3"))
            {
                iss.Remove("ID3");
            }
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
            //(sender as Button).IsEnabled = false;
            //WebClient webclient = new WebClient();
            //webclient.OpenReadAsync(new Uri("http://192.168.1.1:8080/?action=snapshot"));
            //webclient.OpenReadCompleted += new OpenReadCompletedEventHandler((a, b) =>
            //{
            //    if (b.Error == null)
            //    {
            //        NavigationService.Navigate(new Uri("/Page1.xaml", UriKind.Relative));
            //    }
            //    else
            //    {
            //        MessageBox.Show("未找到口腔内窥镜设备");
            //        (sender as Button).IsEnabled = true;
            //    }       
            //});
            NavigationService.Navigate(new Uri("/choise_page.xaml", UriKind.Relative));
        }
        private void Button_Click_send(object sender, RoutedEventArgs e) 
        {
            (sender as Button).IsEnabled = false;
            foreach(BitmapImage a in imagelist)
            {
                ThreadStart start = new ThreadStart(new SendImg(new WriteableBitmap(a)).sendimagelength);
                Thread thread = new Thread(start);
                thread.Start();
            }
            stack.Children.Clear();
            (sender as Button).IsEnabled = true;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            getpicture.IsEnabled = true;
            if (iss.Contains("ID3"))
            {
                List<BitmapImage> images = (List<BitmapImage>)iss["ID3"];
                foreach (BitmapImage a in images) 
                {
                    a.DecodePixelHeight = 300;
                    a.DecodePixelWidth = 300;
                    this.imagelist.Add(a);
                    Image image = new Image();
                    image.Source = a;
                    image.Height = 230;
                    image.Width = 440;
                    image.Stretch = System.Windows.Media.Stretch.Fill;
                    stack.Children.Add(image);
                }
                iss["ID3"] = new List<BitmapImage>();
            }
        }
    }
}