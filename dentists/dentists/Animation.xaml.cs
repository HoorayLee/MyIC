using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace dentists
{
    public partial class Page2 : PhoneApplicationPage
    {
        private const string loading = "/loading.gif";
        public Page2()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(MainPage_load);
            
        }

        void MainPage_load(object sender, RoutedEventArgs e)
        {
            ImageTools.IO.Decoders.AddDecoder<ImageTools.IO.Gif.GifDecoder>();
            ImageTools.ExtendedImage Img = new ImageTools.ExtendedImage();
            Img.UriSource = new Uri(loading, UriKind.Relative);
            imgGif.Source = Img;
        }
    }
}