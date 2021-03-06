﻿using System;
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
    public partial class zero_camera : PhoneApplicationPage
    {
        private bool iscontinue = true;
        BitmapImage images;
        List<BitmapImage> imagelist = null;
        IsolatedStorageSettings iss = IsolatedStorageSettings.ApplicationSettings;
        WebClient webclient;
        public zero_camera()
        {
            InitializeComponent();
            imagelist = new List<BitmapImage>();
            image.Stretch = System.Windows.Media.Stretch.Fill;
            ThreadStart start = new ThreadStart(getpicture);
            Thread thread = new Thread(start);
            thread.Start();
        }
        
        public void getpicture() 
        {
            while (iscontinue)
            {
                webclient = new WebClient(); 
                webclient.OpenReadCompleted += new OpenReadCompletedEventHandler((a, b) =>
                {
                    if (b.Error == null)
                    {
                        if (b.Result != null)
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(()=>
                            {
                                images = new BitmapImage();
                                images.SetSource(b.Result);
                                image.Source = images;
                            });
                        }
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(() => { MessageBox.Show("未找到口腔内窥镜设备"); });
                        iscontinue = false;
                        webclient.OpenReadCompleted += null;
                    }
                });
                webclient.OpenReadAsync(new Uri("http://192.168.1.1:8080/?action=snapshot"));
                Thread.Sleep(30);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) 
        {
            iscontinue = false;
            iss["ID3"] = imagelist;
            PhoneApplicationFrame myFrame = Application.Current.RootVisual as PhoneApplicationFrame;
            if (myFrame != null)
            {
                try
                {
                    myFrame.RemoveBackEntry();
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            this.NavigationService.GoBack(); 
        }
        
        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void Button_Click_Get(object sender, RoutedEventArgs e)
        {
            continueb.IsEnabled = true;
            getphoto.IsEnabled = false;
            iscontinue = false;
            imagelist.Add(images);
        }

        private void Button_Click_Continue(object sender, RoutedEventArgs e)
        {
            getphoto.IsEnabled = true;
            continueb.IsEnabled = false;
            ThreadStart start = new ThreadStart(getpicture);
            Thread thread = new Thread(start);
            thread.Start();
        }
    }
}