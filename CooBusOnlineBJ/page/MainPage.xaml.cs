using System;
using System.Windows;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using System.Windows.Navigation;
using BusClass;
using Microsoft.Phone.Info;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Windows.Threading;

namespace RealTimeBusBJ
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        static string tmpGeoStr = "";
        DispatcherTimer t1;
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            new ThreadStart(() =>
            {
                if (GeoService.Gpscoor == null)
                    GeoService.GetGeocoordinate(() => { freshAddress(); });
                else
                    freshAddress();
            }).Invoke();
            t1 = new DispatcherTimer();
            t1.Interval = new TimeSpan(0, 0, 0, 0, 30);
            t1.Tick += t1_Tick;
            tk1 = spQuit.Margin;
        }

        void freshAddress()
        {
            if (string.IsNullOrEmpty(tmpGeoStr))
            {
                tmpGeoStr = GeoService.strBaiduGeo;
            }
            else
            {
                if (GpsDistance.GetDistance(double.Parse(tmpGeoStr.Split(',')[0]),
                    double.Parse(tmpGeoStr.Split(',')[1]), GeoService.Gpscoor.Latitude,
                    GeoService.Gpscoor.Longitude) < 30)
                    return;
                else
                    tmpGeoStr = GeoService.strBaiduGeo;
            }
            BaiduAPI.geo2add(GeoService.strBaiduGeo, () =>
            {
                BaiduClass.AdrCm_ga gm = BaiduVar.retGA.addressComponent;
                string tmp = gm.district + gm.street + gm.street_number;
                if (tmp.Trim().Length > 0)
                    pageTitle.Text = "北京实时公交 (" + tmp + ")";
            });
        }

        private void P1_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/page/InputLine.xaml", UriKind.RelativeOrAbsolute));
        }

        private void P2_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MyNearPage.fromFavorite = false;
            NavigationService.Navigate(new Uri("/page/MyNearPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void P3_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/page/MetroMap.xaml", UriKind.RelativeOrAbsolute));
        }

        private void P4_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/page/RoutePlan.xaml", UriKind.RelativeOrAbsolute));
        }

        private void P5_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/page/Favorite.xaml", UriKind.RelativeOrAbsolute));
        }

        private void P6_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/page/Config.xaml", UriKind.RelativeOrAbsolute));
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //if (AppGlobalVar.quitConfirm && MessageBox.Show("退出应用吗？",
            //    "提示", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            //    e.Cancel = true;    
            if (AppGlobalVar.quitConfirm && spQuit.Visibility == System.Windows.Visibility.Collapsed)
            {
                spQuit.Visibility = System.Windows.Visibility.Visible;
                t1.Start();
                e.Cancel = true;
            }
        }

        Thickness tk1 = new Thickness();
        void t1_Tick(object sender, EventArgs e)
        {
            spQuit.Margin = new Thickness(0, spQuit.Margin.Top + 3, 0, 0);
            spQuit.Opacity -= 0.025;
            if (spQuit.Opacity < 0.06)
            {
                spQuit.Visibility = System.Windows.Visibility.Collapsed;
                spQuit.Margin = tk1;
                spQuit.Opacity = 1;
                t1.Stop();
            }
        }
    }
}