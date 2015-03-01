using System;
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
using Windows.Storage.Pickers;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Windows.Resources;
using System.IO;

namespace dentists
{
    public partial class choise_page : PhoneApplicationPage
    {
        IsolatedStorageSettings iss = IsolatedStorageSettings.ApplicationSettings;
        PhotoChooserTask photoChooserTask;
        List<BitmapImage> imagelist;
        FileOpenPicker openPicker;
        public choise_page()
        {
            InitializeComponent();
            imagelist = new List<BitmapImage>();
            //photoChooserTask = new PhotoChooserTask();
            //photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);
            openPicker = new FileOpenPicker();
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.ContinuationData["Operation"] = "Image";
        }

        private void media_lib(object sender, RoutedEventArgs e)
        {
            openPicker.PickMultipleFilesAndContinue();
            //try
            //{
            //    photoChooserTask.Show();
            //}
            //catch (System.InvalidOperationException ex)
            //{
            //    MessageBox.Show("An error occurred.");
            //}

        }

        //void photoChooserTask_Completed(object sender, PhotoResult e)
        //{
        //    if (e.TaskResult == TaskResult.OK)
        //    {
        //        //MessageBox.Show(e.ChosenPhoto.Length.ToString());
        //        //Code to display the photo on the page in an image control named myImage.
        //        BitmapImage bmp = new BitmapImage();
        //        bmp.SetSource(e.ChosenPhoto);
        //        imagelist.Add(bmp);
        //    }
        //}
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back && PickFiles.PickedFiles.Count > 0)
            {
                foreach (StorageFile a in PickFiles.PickedFiles)
                {
                    getpictrue(a);
                }
            }
        }

        private async void getpictrue(StorageFile a) 
        {
            IRandomAccessStream fileStream = await a.OpenAsync(Windows.Storage.FileAccessMode.Read);
            Stream stream = fileStream.AsStream();
            BitmapImage bitmap = new BitmapImage();
            bitmap.SetSource(stream);
            imagelist.Add(bitmap);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) 
        {
            iss["ID3"] = imagelist;
        }

        private void camera_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Phone_Camera.xaml", UriKind.Relative));
        }

        private void endoscope_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/zero_camera.xaml", UriKind.Relative));
        }
    }
}