using System;
using RestSharp;
using System.Device.Location;
using System.Windows;

/// <summary>
/// 火星坐标转换 http://www.zdoz.net/apiList.html
/// </summary>
public class GpsCoorConvert
{

    #region Gps2HuoXing GPS坐标转换成火星坐标

    /// <summary>
    /// GPS坐标转换成火星坐标
    /// </summary>
    /// <param name="latGps">纬度</param>
    /// <param name="lngGps">经度</param>
    /// <param name="action"></param>
    public static void gps2HuoXingGeo(double latGps, double lngGps, Action actionWithHuoXing)
    {
        try
        {
            string api = "http://api.zdoz.net/transgps.aspx?lat=" + latGps.ToString() + "&lng=" + lngGps.ToString();
            var client = new RestClient(api);
            client.ExecuteAsync(new RestRequest(Method.GET), (response) =>
            {
                string jsn = response.Content;
                if (jsn.Contains("Lng") && jsn.Contains("Lat"))
                {
                    jsn = jsn.Replace("{\"Lng\":", "").Replace("\"Lat\":", "").Replace("}", "");
                    GeoService.HuoXingCoor = new GeoCoordinate(double.Parse(jsn.Split(',')[1]), double.Parse(jsn.Split(',')[0]));
                    actionWithHuoXing.Invoke();
                }
            });
        }
        catch (Exception ee)
        {
            MessageBox.Show("由于国内地图服务采用奇葩的“火星”坐标系，因此GPS获取的坐标在地图上会有500米以上的偏移。" +
                "本应用调用第三方的网络偏移修正接口，可能因为网络问题出错，敬请谅解！\n" + ee.Message,
                "地球坐标获取失败", MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// GPS坐标转换成火星坐标
    /// </summary>
    /// <param name="latGps">纬度</param>
    /// <param name="lngGps">经度</param>
    /// <param name="action"></param>
    public static void gps2HuoXingGeoUnUpdate(double latGps, double lngGps, Action<GeoCoordinate> actionWithHuoXing)
    {
        try
        {
            string api = "http://api.zdoz.net/transgps.aspx?lat=" + latGps.ToString() + "&lng=" + lngGps.ToString();
            var client = new RestClient(api);
            client.ExecuteAsync(new RestRequest(Method.GET), (response) =>
            {
                string jsn = response.Content;
                if (jsn.Contains("Lng") && jsn.Contains("Lat") && !jsn.Contains("Error"))
                {
                    jsn = jsn.Replace("{\"Lng\":", "").Replace("\"Lat\":", "").Replace("}", "");
                    GeoCoordinate Coor = new GeoCoordinate(double.Parse(jsn.Split(',')[1]), double.Parse(jsn.Split(',')[0]));
                    actionWithHuoXing.Invoke(Coor);
                }
            });
        }
        catch (Exception ee)
        {
            MessageBox.Show("由于国内地图服务采用奇葩的“火星”坐标系，因此GPS获取的坐标在地图上会有500米以上的偏移。" +
                "本应用调用第三方的网络偏移修正接口，可能因为网络问题出错，敬请谅解！\n" + ee.Message,
                "地球坐标获取失败", MessageBoxButton.OK);
        }
    }
    #endregion


    #region Baidu2HuoXing Baidu坐标转换成火星坐标

    /// <summary>
    /// Baidu坐标转换成火星坐标
    /// </summary>
    /// <param name="latBaidu">纬度</param>
    /// <param name="lngBaidu">经度</param>
    /// <param name="action"></param>
    public static void Baidu2HuoXingGeoUnUpdate(double latBaidu, double lngBaidu, Action<GeoCoordinate> actionWithHuoXing)
    {
        try
        {
            string api = "http://api.zdoz.net/bd2wgs.aspx?lat=" + latBaidu.ToString() + "&lng=" + lngBaidu.ToString();
            var client = new RestClient(api);
            client.ExecuteAsync(new RestRequest(Method.GET), (response) =>
            {
                string jsn = response.Content;
                if (jsn.Contains("Lng") && jsn.Contains("Lat") && !jsn.Contains("Error"))
                {
                    jsn = jsn.Replace("{\"Lng\":", "").Replace("\"Lat\":", "").Replace("}", "");
                    gps2HuoXingGeoUnUpdate(double.Parse(jsn.Split(',')[1]), double.Parse(jsn.Split(',')[0]), actionWithHuoXing);
                }
            });
        }
        catch (Exception ee)
        {
            MessageBox.Show("由于国内地图服务采用奇葩的“火星”坐标系，因此GPS获取的坐标在地图上会有500米以上的偏移。" +
                "本应用调用第三方的网络偏移修正接口，可能因为网络问题出错，敬请谅解！\n" + ee.Message,
                "地球坐标获取失败", MessageBoxButton.OK);
        }
    }
    #endregion
}