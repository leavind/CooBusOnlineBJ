﻿#pragma checksum "C:\Users\lei\Desktop\CooBusOnline\CooBusOnlineBJ\page\SeleLineList.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "633C6AE33F0B87823AB0319C4D999AF5"
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
using Microsoft.Phone.Shell;
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
    
    
    public partial class seleLineList : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock pageTitle;
        
        internal System.Windows.Controls.TextBlock tb_LocName;
        
        internal Microsoft.Phone.Controls.LongListSelector list1;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton abBack;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton abRefresh;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/RealTimeBusBJ;component/page/SeleLineList.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.pageTitle = ((System.Windows.Controls.TextBlock)(this.FindName("pageTitle")));
            this.tb_LocName = ((System.Windows.Controls.TextBlock)(this.FindName("tb_LocName")));
            this.list1 = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("list1")));
            this.abBack = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("abBack")));
            this.abRefresh = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("abRefresh")));
        }
    }
}

