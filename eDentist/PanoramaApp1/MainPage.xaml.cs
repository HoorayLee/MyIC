using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PanoramaApp1.ViewModels;

namespace PanoramaApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
        
        // 构造函数
        public MainPage()
        {
            InitializeComponent();
        
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

        private void ScanClick(object sender, RoutedEventArgs e)
        {
            
            NavigationService.Navigate(new Uri("/Animation.xaml", UriKind.Relative));
        }

        private void LongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/RecommandInfo.xaml", UriKind.Relative));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            NavigationService.Navigate(new Uri("/RecommandInfo.xaml", UriKind.Relative));
            
        }

        private void Button_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
          
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/RecommandInfo.xaml", UriKind.Relative));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            NavigationService.Navigate(new Uri("/RecommandInfo.xaml", UriKind.Relative));
        }

        private void Panorama_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}