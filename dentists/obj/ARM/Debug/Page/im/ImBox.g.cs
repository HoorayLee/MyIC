﻿#pragma checksum "C:\Users\乐文\Documents\Visual Studio 2013\Projects\dentists\dentists\Page\im\ImBox.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "DE625D0889B5797BDF52A5045B9DECD6"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.34011
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace dentists.Page.im {
    
    
    public partial class Imbox : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.StackPanel background;
        
        internal System.Windows.Controls.Image heard;
        
        internal System.Windows.Controls.TextBlock text;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/dentists;component/Page/im/ImBox.xaml", System.UriKind.Relative));
            this.background = ((System.Windows.Controls.StackPanel)(this.FindName("background")));
            this.heard = ((System.Windows.Controls.Image)(this.FindName("heard")));
            this.text = ((System.Windows.Controls.TextBlock)(this.FindName("text")));
        }
    }
}

