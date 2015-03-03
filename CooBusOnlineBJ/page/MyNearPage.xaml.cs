using System.Windows;
using Microsoft.Phone.Controls;
using System.Threading;
using BaiduClass;
using System.Windows.Navigation;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Phone.Tasks;
using System.Device.Location;

namespace RealTimeBusBJ
{
    public partial class MyNearPage : PhoneApplicationPage
    {
        public MyNearPage()
        {
            InitializeComponent();
        }

        private int LoadCount = 0;
        public static bool fromFavorite = false;
        public static string favoriteLocName = "";
        public static string favoriteLocAddress = "";
        /// <summary>
        /// Format: lat,lng
        /// </summary>
        public static string favoriteBaiduLocGeo = "";
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (LoadCount == 0)
            {
                pgRefresh();                 
                LoadCount += 1;
            }
        }

        private void pgRefresh()
        {
            if (fromFavorite)
            {
                pv2Header.Text = "查看地图 ";
                tb_LocName.Text = "你在: " + (string.IsNullOrEmpty(favoriteLocName) ? "" : (favoriteLocName + " 附近"));
                tb_LocAddr.Text = favoriteLocAddress;
                //获取所在位置周边的公交
                BaiduAPI.placeSearchByGeo(favoriteBaiduLocGeo, "公交", AppGlobalVar.valueNearDiv, () =>
                {
                    list1.ItemsSource = BaiduVar.NearLines;
                });
                GeoCoordinate marsGeo = GeoConverter.GetMarsFromBaidu(double.Parse(favoriteBaiduLocGeo.Split(',')[0]),
                   double.Parse(favoriteBaiduLocGeo.Split(',')[1]));
                map2.Layers.Clear();
                map2.SetView(marsGeo, 16D, 0D, 55D);
                MyMap.AddMark(map2, marsGeo, (string.IsNullOrEmpty(favoriteLocName) ? "保存的位置"
                    : favoriteLocName), 26D, Colors.Red, Colors.Red, 2);
            }
            else
            {
                GeoService.GetGeocoordinate(() =>
                {
                    //获取所在位置的名称,座标 和 详细地址
                    BaiduAPI.geo2add(GeoService.strBaiduGeo, () =>
                    {
                        tb_LocName.Text = "你在: " + (string.IsNullOrEmpty(BaiduVar.retGA.business) ? ""
                            : (BaiduVar.retGA.business + " 附近"));
                        tb_LocAddr.Text = BaiduVar.retGA.formatted_address;
                    });
                    //获取所在位置周边的公交
                    BaiduAPI.placeSearchByGeo(GeoService.strBaiduGeo, "公交", AppGlobalVar.valueNearDiv, () =>
                    {
                        list1.ItemsSource = BaiduVar.NearLines;
                    });
                    GeoCoordinate marsGeo = GeoConverter.GetMarsFromGps(GeoService.Gpscoor.Latitude, GeoService.Gpscoor.Longitude);
                    map2.Layers.Clear();
                    map2.SetView(marsGeo, 16D, 0D, 0D);
                    TaxiAPI.ShowTaxiInMap(map2, GeoService.Gpscoor.Latitude.ToString(),
                        GeoService.Gpscoor.Longitude.ToString());
                    MyMap.AddMark(map2, marsGeo, "你在这", 26D, Colors.Blue, Colors.Blue, 2);
                });
            }
        }
    
        private void list1_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(TextBlock))
            {
                BaiduVar.selePlace = (Ret_pla)list1.SelectedItem;
                //BaiduVar.selLoc = BaiduVar.selePlace.location;
                NavigationService.Navigate(new Uri("/page/seleLineList.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        private void abBack_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void abRefresh_Click(object sender, EventArgs e)
        {
            pgRefresh(); 
        }

        private void about_Click(object sender, EventArgs e)
        {
            string msg = "目前最多能查到周边的士（空车）的位置和车牌号，司机的电话现在还查不到，需要走几步路哦！";
            if (fromFavorite)
                msg = "显示从收藏中选定地点在地图上的位置。";
            msg += "\n为了节省数据流量，请点击“确定”下载离线地图（系统共用）。";
            if (MessageBox.Show(msg, "说明",
              MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                MapDownloaderTask t = new MapDownloaderTask();
                t.Show();
            }    
        }

        private void map2_Loaded(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = "7c075e17-4e01-4117-832a-07f9c010f4d2";
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = "su34I69276CJYF9Y69ZBtQ";

        }
    }
}