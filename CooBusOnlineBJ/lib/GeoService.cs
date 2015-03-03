using System;
using Windows.Devices.Geolocation;
using System.Windows;
using System.Threading;
using System.Device.Location;
public class GeoService
{
    ///// <summary>
    ///// GPS座标格式(纬度,经度)
    ///// </summary>
    //private static string strGpsGeo = "";

    /// <summary>
    /// windows.devices.Geolocation GPS create this
    /// </summary>
    public static Geocoordinate Gpscoor;

    ///// <summary>
    ///// windows.devices.Geolocation GPS create this
    ///// </summary>
    //public static Windows.Devices.Geolocation.CivicAddress curCivic;

    ///// <summary>
    ///// system.device.location.  map use this. From GPS, convert to huoXing
    ///// </summary>
    //public static GeoCoordinate HuoXingCoor;

    /// <summary>
    /// 百度地图座标格式(纬度,经度)
    /// </summary>
    public static string strBaiduGeo;

    static bool LocationMsgShown = false;
    //private static Uri uri = new Uri("/page/seleLineList.xaml", UriKind.RelativeOrAbsolute);

    public static async void GetGeocoordinate(Action action)
    {
        try
        {
            progressBar.Show();
            Geolocator locator = new Geolocator();
            locator.DesiredAccuracy = PositionAccuracy.High;
            locator.DesiredAccuracyInMeters = 50;
            locator.ReportInterval = 100;
            locator.MovementThreshold = 50;
            Geoposition position = await locator.GetGeopositionAsync();
            Gpscoor = position.Coordinate;
            //curCivic = position.CivicAddress;
            ////strGpsGeo = position.Coordinate.Latitude.ToString() + "," + position.Coordinate.Longitude.ToString();
            progressBar.Hide();
            ////BaiduAPI.gps2BaiduGeo(strGpsGeo, action);
            strBaiduGeo = GetString(GeoConverter.GetBaiduFromGps(
                            Gpscoor.Latitude, Gpscoor.Longitude));
            action.Invoke();
        }
        catch
        {
            progressBar.Hide();
            if (!LocationMsgShown)
            {
                LocationMsgShown = true;
                MessageBox.Show("定位服务是否已开启呢?\n请从这里开启:\n设置 --> 系统 --> 定位",
                    "无法获取当前位置", MessageBoxButton.OK);
                new ThreadStart(async () => { await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-location:")); }).Invoke();
            }
        }
    }

    public static async void GetGeocoordinate()
    {
        try
        {
            progressBar.Show();
            Geolocator locator = new Geolocator();
            locator.DesiredAccuracy = PositionAccuracy.High;
            locator.DesiredAccuracyInMeters = 50;
            locator.ReportInterval = 100;
            locator.MovementThreshold = 50;
            Geoposition position = await locator.GetGeopositionAsync();
            Gpscoor = position.Coordinate;
            strBaiduGeo = GetString(GeoConverter.GetBaiduFromGps(
                            Gpscoor.Latitude, Gpscoor.Longitude));
            //curCivic = position.CivicAddress;
            //strGpsGeo = position.Coordinate.Latitude.ToString() + "," + position.Coordinate.Longitude.ToString();
            progressBar.Hide();
            //BaiduAPI.gps2BaiduGeo(strGpsGeo);
        }
        catch
        {
            progressBar.Hide();
            if (!LocationMsgShown)
            {
                LocationMsgShown = true;
                MessageBox.Show("定位服务是否已开启呢?\n请从这里开启:\n设置 --> 系统 --> 定位",
                    "无法获取当前位置", MessageBoxButton.OK);
                new ThreadStart(async () => { await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-location:")); }).Invoke();
            }
        }
    }

    private static string GetString(GeoCoordinate Coor)
    {
        return Coor.Latitude.ToString() + "," + Coor.Longitude.ToString();
    }

    //public static async void GetHuoXingCoor(Action actionWithHuoXing)
    //{
    //    try
    //    {
    //        if (Gpscoor == null)
    //        {
    //            progressBar.Show();
    //            Geolocator locator = new Geolocator();
    //            locator.DesiredAccuracy = PositionAccuracy.High;
    //            locator.DesiredAccuracyInMeters = 50;
    //            locator.ReportInterval = 100;
    //            locator.MovementThreshold = 50;
    //            Geoposition position = await locator.GetGeopositionAsync();
    //            Gpscoor = position.Coordinate;
    //            //curCivic = position.CivicAddress;
    //            strGpsGeo = position.Coordinate.Latitude.ToString() + "," + position.Coordinate.Longitude.ToString();
    //            progressBar.Hide();
    //            GpsCoorConvert.gps2HuoXingGeo(position.Coordinate.Latitude, position.Coordinate.Longitude, actionWithHuoXing);
    //        }
    //        else
    //            GpsCoorConvert.gps2HuoXingGeo(Gpscoor.Latitude, Gpscoor.Longitude, actionWithHuoXing);
    //    }
    //    catch
    //    {
    //        progressBar.Hide();
    //        if (!LocationMsgShown)
    //        {
    //            LocationMsgShown = true;
    //            MessageBox.Show("定位服务是否已开启呢?\n请从这里开启:\n设置 --> 系统 --> 定位",
    //                "无法获取当前位置", MessageBoxButton.OK);
    //            new ThreadStart(async () => { await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-location:")); }).Invoke();
    //        }
    //    }
    //}
}
