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
using Coding4Fun.Toolkit.Controls;

namespace dentists
{
    public partial class choise_page : PhoneApplicationPage
    {
        IsolatedStorageSettings iss = IsolatedStorageSettings.ApplicationSettings;
        List<BitmapImage> imagelist;
        FileOpenPicker openPicker;
        public choise_page()
        {
            InitializeComponent();
            imagelist = new List<BitmapImage>();
            openPicker = new FileOpenPicker();
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.ContinuationData["Operation"] = "Image";
        }

        private void media_lib(object sender, RoutedEventArgs e)
        {
            openPicker.PickMultipleFilesAndContinue();
        }
       
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back && PickFiles.PickedFiles.Count > 0)
            {
                foreach (StorageFile a in PickFiles.PickedFiles)
                {
                    getpictrue(a);
                }
            }
            else if (e.NavigationMode == NavigationMode.Back)
            {
                List<BitmapImage> images = (List<BitmapImage>)iss["ID3"];
                foreach (BitmapImage a in images)
                {
                    imagelist.Add(a);
                }
                iss["ID3"] = new List<BitmapImage>();
            }
        }

        private async void getpictrue(StorageFile a) 
        {
            IRandomAccessStream fileStream = await a.OpenAsync(Windows.Storage.FileAccessMode.Read);
            Stream stream = fileStream.AsStream();
            BitmapImage bitmap = new BitmapImage();
            bitmap.DecodePixelWidth = 400;
            bitmap.SetSource(stream);
            if (imagelist.Count <= 10)
            {
                imagelist.Add(bitmap);
            }
            stream.Flush();
            stream.Close();
            stream.Dispose();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) 
        {

            if (e.NavigationMode == NavigationMode.Back)
            {
                if(this.imagelist.Count!=0)
                {
                    bool haspic = true;
                    iss["haspic"] = haspic;
                }
                else
                {
                    bool haspic = false;
                    iss["haspic"] = haspic;
                }
                iss["ID3"] = this.imagelist;
                    //Deployment.Current.Dispatcher.BeginInvoke(() =>
                    //{
                    //});
            }
            else
            {   
                iss["ID3"] = new List<BitmapImage>();
            }
        }

        private void camera_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ScanNow/Phone_Camera.xaml", UriKind.Relative));
        }

        private void endoscope_Click(object sender, RoutedEventArgs e)
        {
            Button c = sender as Button;
            c.IsEnabled = false;
            WebClient webclient = new WebClient();
            webclient.OpenReadAsync(new Uri("http://192.168.1.1:8080/?action=snapshot"));
            webclient.OpenReadCompleted += new OpenReadCompletedEventHandler((a, b) =>
            {
                if (b.Error == null)
                {
                    c.IsEnabled = true;
                    NavigationService.Navigate(new Uri("/ScanNow/zero_camera.xaml", UriKind.Relative));
                }
                else
                {
                    c.IsEnabled = true;
                    var toast = new ToastPrompt { Message = "未找到口腔内窥镜设备" };
                    toast.Visibility = System.Windows.Visibility.Visible;
                    toast.Show();
                }
            });
        }
    }
}
//photoChooserTask = new PhotoChooserTask();
//photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);
//try
//{
//    photoChooserTask.Show();
//}
//catch (System.InvalidOperationException ex)
//{
//    MessageBox.Show("An error occurred.");
//}

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