﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
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

namespace dentists.pages
{
    public partial class Phone_Camera : PhoneApplicationPage
    {
        IsolatedStorageSettings iss = IsolatedStorageSettings.ApplicationSettings;
        CameraCaptureTask cameraCaptureTask;
        List<BitmapImage> imagelist;
        public Phone_Camera()
        {
            InitializeComponent();
            cameraCaptureTask = new CameraCaptureTask();
            cameraCaptureTask.Completed += new EventHandler<PhotoResult>(cameraCaptureTask_Completed);
            imagelist = new List<BitmapImage>();
        }

        void cameraCaptureTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                //Code to display the photo on the page in an image control named myImage.
                System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                bmp.SetSource(e.ChosenPhoto);
                //myImage.Source = bmp;
                this.imagelist.Add(bmp);
                Image images = new Image();
                images.Source = bmp;
                images.Height = 230;
                images.Width = 440;
                images.Stretch = System.Windows.Media.Stretch.Fill;
                stack.Children.Add(images);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cameraCaptureTask.Show();
            }
            catch (System.InvalidOperationException ex)
            {
                MessageBox.Show("An error occurred.");
            }
        }

        private void send_Click(object sender, RoutedEventArgs e)
        {
            iss["ID3"] = this.imagelist;
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
            try {
                (sender as Button).IsEnabled = false;
                foreach (BitmapImage a in imagelist)
                {
                    ThreadStart start = new ThreadStart(new SendImg(new WriteableBitmap(a)).sendimagelength);
                    Thread thread = new Thread(start);
                    thread.Start();
                }
                stack.Children.Clear();
                (sender as Button).IsEnabled = true;
            }
            catch(InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }

            NavigationService.Navigate(new Uri("/Animation.xaml", UriKind.Relative));
            //this.NavigationService.GoBack(); 
        }
    }
}