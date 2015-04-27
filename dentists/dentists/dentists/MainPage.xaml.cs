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
            canvaslist = new Canvas[5];
            canvaslist[0] = canvas1;
            canvaslist[1] = canvas2;
            canvaslist[2] = canvas3;
            canvaslist[3] = canvas4;
            canvaslist[4] = canvas5;
            
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

        private void Canvas_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            double change = e.DeltaManipulation.Translation.Y;

            rec1.Fill = new SolidColorBrush(Colors.Green);
            foreach (Canvas can in canvaslist)
            {
               double top = Canvas.GetTop(can);
               double res = top + change;
               if (res > 380)
               {
                   Canvas.SetTop(can,res - 450);
               }
               else if (res < -70)
               {
                   Canvas.SetTop(can,450 + res);
               }
               else 
               {
                   Canvas.SetTop(can, res);
               }
               if(res>=20&&res<110)
               {
                   Canvas.SetLeft(can, 2 * (res - 20) / 3);
               }
               else if (res >= 110 && res <= 200)
               {
                   Canvas.SetLeft(can, 60 + 2 * (110 - res) / 3);
               }
               else 
               {
                   Canvas.SetLeft(can,0);
               }
            }
        }

        private void general_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            foreach (Canvas can in canvaslist)
            {
                double top = Canvas.GetTop(can);
                double res = (Canvas.GetTop(can) + 70) % 90;
                if (res > 45)
                {
                    
                    Canvas.SetTop(can, top + 90 - res);
                    top = top + 90 - res;
                }
                else
                {
                    Canvas.SetTop(can, top - res);
                    top = top - res;
                }
                if (top >= 20 && top < 110)
                {
                    Canvas.SetLeft(can, 2 * (top - 20) / 3);
                }
                else if (top >= 110 && top <= 200)
                {
                    Canvas.SetLeft(can, 60 + 2 * (110 - top) / 3);
                }
                else
                {
                    Canvas.SetLeft(can, 0);
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/scan.xaml", UriKind.Relative));
        }

        private void Button_Click_Map(object sender, RoutedEventArgs e)
        {

        }
    }
}