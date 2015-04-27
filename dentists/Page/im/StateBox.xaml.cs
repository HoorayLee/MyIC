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

namespace dentists.Page.im
{
    public partial class StateBox : UserControl
    {
        public StateBox()
        {
            InitializeComponent();
        }
        public void ChangeImage(int a)
        {
            switch (a)
            {
                case 0:
                    this.state.Source = new BitmapImage(new Uri("/asset/warning.png",UriKind.Relative));
                break;

            }
            
        }
    }
}
