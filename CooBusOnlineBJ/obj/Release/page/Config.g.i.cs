﻿#pragma checksum "D:\Documents\Visual Studio 2013\Projects\CooBusOnlineBJ\CooBusOnlineBJ\page\Config.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3D985B413EE70DE4DD336F45B0864AF7"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.34014
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
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


namespace RealTimeBusBJ {
    
    
    public partial class Config : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock pageTitle;
        
        internal System.Windows.Controls.Grid G1;
        
        internal Microsoft.Phone.Controls.ToggleSwitch ts1;
        
        internal System.Windows.Controls.TextBox tb1;
        
        internal System.Windows.Controls.Slider sl1;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/RealTimeBusBJ;component/page/Config.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.pageTitle = ((System.Windows.Controls.TextBlock)(this.FindName("pageTitle")));
            this.G1 = ((System.Windows.Controls.Grid)(this.FindName("G1")));
            this.ts1 = ((Microsoft.Phone.Controls.ToggleSwitch)(this.FindName("ts1")));
            this.tb1 = ((System.Windows.Controls.TextBox)(this.FindName("tb1")));
            this.sl1 = ((System.Windows.Controls.Slider)(this.FindName("sl1")));
        }
    }
}
