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
namespace dentists
{
    public partial class MainPage : PhoneApplicationPage
    {
        Canvas[] canvaslist;
        AMap amap;
        // 构造函数
        public MainPage()
        {
            InitializeComponent();
            amap = new AMap();
            amap.Height = 130;
            amap.Width = 335;
            UiSettings setter= amap.GetUiSettings();
            setter.ZoomControlsEnabled = false;
            setter.RotateGesturesEnabled = false;
            setter.ScaleControlEnabled = false;
            setter.ScrollGesturesEnabled = false;
            map.Children.Add(amap);
            // 将 listbox 控件的数据上下文设置为示例数据
            DataContext = App.ViewModel;

            
        }

        // 为 ViewModel 项加载数据
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void Panorama_Loaded(object sender, RoutedEventArgs e)
        {

        }
        
        private void Panorama_Loaded_1(object sender, RoutedEventArgs e)
        {
            
        }


       
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/choise_page.xaml", UriKind.Relative));
            //NavigationService.Navigate(new Uri("/scan.xaml", UriKind.Relative));
        }

        private void Button_Click_Map(object sender, RoutedEventArgs e)
        {

        }
    }
}