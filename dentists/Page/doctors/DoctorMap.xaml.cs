using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Com.AMap.Api.Maps;
using Com.AMap.Api.Maps.Model;
using Com.AMap.Api.Services;
using Windows.Devices.Geolocation;
using System.Windows.Media;
using System.Diagnostics;
using Com.AMap.Api.Services.Results;
using System.Collections;
using Coding4Fun.Toolkit.Controls;
using System.IO.IsolatedStorage;
using dentists.helper;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace dentists.Page.doctors
{
    public partial class DoctorMap : PhoneApplicationPage
    {
        bool first = true;
        AMap amap;
        bool isloading = false;
        AMapMarker marker;
        AMapCircle circle;
        float zoom = 12;
        uint radius = 3000;
        string citycode = "";
        AMapGeolocator geolocator;
        bool show = true;
        LatLng latlng = null;
        public DoctorMap()
        {
            InitializeComponent();
            amap = new AMap();
            amap.Width = 480;
            amap.Height = 800;
            this.map.Children.Clear();
            this.map.Children.Add(amap);
            amap.Loaded += (a, b) => 
            { 
                isloading = true;
                try
                {
                    //获取当前的经纬度
                    geolocator = new AMapGeolocator();
                    geolocator.PositionChanged += mylocation_PositionChanged;
                    geolocator.Start();

                    //注册地图Marker点击事件
                    //geolocator.DesiredAccuracy = PositionAccuracy.Default;
                    //var loc = await geolocator.GetGeopositionAsync();
                    //amap.AddMarker(new AMapMarkerOptions() { Anchor = new Point(10, 10), Title = loc.Coordinate.Latitude.ToString() });
                    //amap.AddMarker(new AMapMarkerOptions() { Anchor = new Point(10, 30), Title = loc.Coordinate.Longitude.ToString() });
                }
                catch (Exception ex)
                {
                    if ((uint)ex.HResult == 0x80004004)
                    {
                        var toast = new ToastPrompt { Message = "系统设置关闭了位置服务." };
                        toast.Show();
                        //StatusTextBlock.Text = "系统设置关闭了位置服务.";
                    }
                }
            };
            amap.MarkerClickListener += amap_MarkerClickListener;
        }

        private void amap_MarkerClickListener(AMapMarker sender, AMapEventArgs args)
        {
            sender.ShowInfoWindow(new hospitalinfo(sender.Title,this.citycode,this),new Point(sender.Position.longitude,sender.Position.latitude));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="centerX"></param>
        /// <param name="centerY"></param>
        /// <param name="keywords">查询关键字，多个关键字用“|”分割，“空格"表示与，“双引号”表示不可分割，keywords与types二选其一为必填</param>
        /// <param name="types"></param>
        /// <param name="radius"></param>
        private async void GetPOIAround(double centerX, double centerY, string keywords, string types)
        {
            
            AMapPOIResults poir = await AMapPOISearch.POIAround(centerX, centerY, keywords, types, radius);
            this.Dispatcher.BeginInvoke(() =>
            {
                if (poir.Erro == null && poir.POIList != null)
                {
                    if (poir.POIList.Count == 0)
                    {
                        radius = (radius + 3000) % 50000;
                        if (radius != 50000)
                        {
                            GetPOIAround(centerX, centerY, keywords, types);
                        }
                        else
                        {
                            var toast = new ToastPrompt { Message = "Didn't find POI" };
                            toast.Show();
                        }
                        //MessageBox.Show("无查询结果");
                    }
                    else 
                    {
                        this.progressbar.Visibility = System.Windows.Visibility.Collapsed;
                        IEnumerable pois = poir.POIList;
                        int i = 0;
                        foreach (AMapPOI item in pois)
                        {
                            i++;
                            try
                            {
                                amap.AddMarker(new AMapMarkerOptions()
                                {
                                    Position = new LatLng(item.Location.Lat, item.Location.Lon),//amap.Center,// 
                                    Title = item.Name,
                                    Snippet = "Snippet",
                                    IconUri = new Uri("asset/tooth.png",UriKind.RelativeOrAbsolute),
                                });
                            }
                            catch(Exception a)
                            {

                            }
                        }
                    }
                }
                else
                {
                    var toast = new ToastPrompt { Message = poir.Erro.Message };
                    toast.Show();
                    //radius = (radius + 3000) % 50000;
                    //GetPOIAround(centerX, centerY, keywords, types);
                }
            });
        }

        private async Task GeoCodeToAddress(double lon,double lat) 
        {
            //请求查询 
            AMapReGeoCodeResult rcc = await AMapReGeoCodeSearch.GeoCodeToAddress(lon,lat,20,"",Com.AMap.Api.Services.Extensions.All); 
            lock (this.citycode) 
            {
                this.Dispatcher.BeginInvoke(() =>   //处理结果 
                {
                    if (rcc.Erro == null && rcc.ReGeoCode != null) 
                    { 
                        AMapReGeoCode regeocode = rcc.ReGeoCode;
                        AMapAddressComponent addressComponent = regeocode.Address_component;
                        this.citycode = addressComponent.Province + "_" + addressComponent.City + "_" + addressComponent.Township;
                    } 
                }); 
            }
        }

        void mylocation_PositionChanged(AMapGeolocator sender, AMapPositionChangedEventArgs args)
        {
            latlng = args.LngLat;
            this.Dispatcher.BeginInvoke(() =>   //处理结果 
            {
                this.dingwei.IsEnabled = true;
            }); 
            GeoCodeToAddress(args.LngLat.longitude, args.LngLat.latitude);
                
            if (marker == null)
            {
                //添加定位精度圈，为覆盖物中的圆 
                circle = amap.AddCircle(new AMapCircleOptions()
                {
                    Center = args.LngLat,//圆点位置 
                    Radius = (float)args.Accuracy,//半径 
                    FillColor = Color.FromArgb(200, 100, 150, 255),
                    StrokeWidth = 2,//边框粗细 
                    StrokeColor = Color.FromArgb(80, 0, 0, 255),//边框颜色 
                });
                //添加我的位置图标，为覆盖物中的标注 
                marker = amap.AddMarker(new AMapMarkerOptions()
                {
                    Position = args.LngLat,//图标的位置 
                    Title = "我的位置",
                    //IconUri = new Uri("Images/marker_gps_no_sharing.png", UriKind.Relative),//图标的URL 
                    Anchor = new Point(0.5, 0.5),//图标中心点 
                });
            }
            else
            {
                try
                {
                    //更新我的位置和精度圈 
                    marker.Position = args.LngLat;
                    circle.Center = args.LngLat;
                    circle.Radius = (float)args.Accuracy;//圆半径
                }
                catch (Exception a) 
                {

                }
            }
            if (first) 
            {
                first = false;
                //设置当前地图的经纬度和缩放级别
                amap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(args.LngLat, (float)amap.Zoom));
            }
            GetPOIAround(args.LngLat.longitude, args.LngLat.latitude,"", "090202");
        }



        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (isloading) 
            {
                lock(amap)
                {
                   amap.Destory();
                   geolocator.Stop();
                   base.OnBackKeyPress(e);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            amap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(latlng, (float)amap.Zoom));
        }
    }
}
//lock (this.citycode)
//{
//    if (poir.POIList != null)
//    {
//        if (poir.POIList.Count != 0)
//        {
//            JObject jObj = new JObject();
//            JArray poi = new JArray();
//            foreach (AMapPOI a in poir.POIList)
//            {
//                JObject jO = new JObject();
//                jO.Add("name", a.Name);
//                jO.Add("citycode", this.citycode);
//                poi.Add(jO);
//            }
//            jObj.Add("count", poi.Count);
//            jObj.Add("content", poi);
//            string result = "";
//            Reqdoc.postreq("http://121.42.136.178:19710/DoctorListHandler.ashx", jObj.ToString(), result);
//        }
//    }
//}
//Debug.WriteLine(item.Adcode);
//Debug.WriteLine(item.Address);
//Debug.WriteLine(item.Citycode);
//Debug.WriteLine(item.Direction);
//Debug.WriteLine(item.Discount_num);
//Debug.WriteLine(item.Distance);
//Debug.WriteLine(item.Email);
//Debug.WriteLine(item.Entr_location);
//Debug.WriteLine(item.Exit_location);
//Debug.WriteLine(item.Gridcode);
//Debug.WriteLine(item.Groupbuy_num);
//Debug.WriteLine(item.Id);
//Debug.WriteLine(item.Location);
//Debug.WriteLine(item.Match);
//Debug.WriteLine(item.Name);
//Debug.WriteLine(item.Navi_poiid);
//Debug.WriteLine(item.Postcode);
//Debug.WriteLine(item.Recommend);
//Debug.WriteLine(item.Rich_content);
//Debug.WriteLine(item.Tel);
//Debug.WriteLine(item.Timestamp);
//Debug.WriteLine(item.Type);
//Debug.WriteLine(item.Website);
//Debug.WriteLine(item.Weight);