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
using Coding4Fun.Toolkit.Controls;


namespace dentists
{
  
    public partial class scan : PhoneApplicationPage
    {
        int[] sort;
        int childnumber;
        IsolatedStorageSettings iss = IsolatedStorageSettings.ApplicationSettings;
        List<BitmapImage> imagelist;
        
        public scan()
        {
            imagelist = new List<BitmapImage>();
            InitializeComponent();
            sort = new int[10];
            childnumber = 0;
        }
       

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            NavigationService.Navigate(new Uri("/ScanNow/choise_page.xaml", UriKind.Relative));
        }
        private void Button_Click_send(object sender, RoutedEventArgs e) 
        {
            (sender as Button).IsEnabled = false;
            if (imagelist.LongCount() != 0)
            {
                foreach (BitmapImage a in imagelist)
                {
                    ThreadStart start = new ThreadStart(new SendImg(new WriteableBitmap(a)).sendimagelength);
                    Thread thread = new Thread(start);
                    thread.Start();
                }
                stack.Children.Clear();
                imagelist.Clear();
                (sender as Button).IsEnabled = true;
            }
            else 
            {
                var toast = new ToastPrompt { Message = "发送图片不能为空" };
                toast.Visibility = System.Windows.Visibility.Visible;
                toast.Show();
                (sender as Button).IsEnabled = true;
            }
            iss["ID3"] = null;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            getpicture.IsEnabled = true;
            
            List<BitmapImage> images = (List<BitmapImage>)iss["ID3"];
            if(iss.Contains("haspic"))
            {
                bool haspic = (bool)iss["haspic"];
                while (haspic && images == null)
                {
                    Thread.Sleep(100);
                    images = (List<BitmapImage>)iss["ID3"];
                } 
            }
            iss.Remove("haspic");
            if(images.Count>0)
            {
                foreach (BitmapImage a in images) 
                {
                    if (childnumber < 10)
                    {
                        this.imagelist.Add(a);
                        Image image = new Image();
                        image.Source = a;
                        image.Width = 440;
                        image.Height = 520;
                        image.Margin = new Thickness(20);
                        image.Stretch = System.Windows.Media.Stretch.Uniform;
                        ContextMenu menu = new ContextMenu();
                        MenuItem menuItem1 = new MenuItem();
                        menuItem1.Header = "delete it";
                        menuItem1.Tag = childnumber;
                        sort[childnumber] = childnumber;
                        menuItem1.Click += new RoutedEventHandler(MenuItem_Click);
                        menu.Items.Add(menuItem1);
                        ContextMenuService.SetContextMenu(image, menu);
                        stack.Children.Add(image);
                        //a.SetSource(null);
                        //a.UriSource = null;
                        childnumber++;
                    }
                }
            }
            iss["ID3"] = new List<BitmapImage>();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuitem = (MenuItem)sender;
            int index = (int)menuitem.Tag;
            stack.Children.RemoveAt(sort[index]);
            imagelist.RemoveAt(sort[index]);
            int i = index;
            while (i <= childnumber) 
            {
                sort[i]--;
                i++;
            }
            childnumber--;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) 
        {
            if (e.NavigationMode == NavigationMode.Back) 
            {
                 MessageBoxResult msgRst = MessageBox.Show("要放弃本次编辑么？", "提示", MessageBoxButton.OKCancel);
                 if (msgRst == MessageBoxResult.OK)
                 {
                     this.imagelist.Clear();
                     this.stack.Children.Clear();
                 }
                 else 
                 {
                     List<BitmapImage> a = iss["ID3"] as List<BitmapImage>;
                     iss["ID3"] = this.imagelist;
                 }
            }
            else
            {
                iss["ID3"] = new List<BitmapImage>();
            }
        }
    }
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