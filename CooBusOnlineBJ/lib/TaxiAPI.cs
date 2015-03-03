using System;
using RestSharp;
using RealTimeBusBJ;
using Microsoft.Phone.Maps.Controls;
using System.Windows.Media;
using System.Windows;
using System.Device.Location;

public class TaxiAPI
{

    #region Gps2BaiduGeo

    /// <summary>
    /// 在指定的地图上显示周边的Taxi
    /// </summary>
    /// <param name="map">指定的地图控件</param>
    /// <param name="lat">中心点的纬度</param>
    /// <param name="lngGps">中心点的经度</param>
    public static void ShowTaxiInMap(Map map,string latGps, string lngGps)
    {
        try
        {
            string api = "http://www.e511.com/taxi/action/taxiList.do?lon=" + lngGps + "&lat="
                + latGps + "&meter=" + AppGlobalVar.valueNearDiv;
            var client = new RestClient(api);
            client.ExecuteAsync(new RestRequest(Method.GET), (response) =>
            {
                string jsn = response.Content.Trim();
                if (jsn.Contains("$") && !string.IsNullOrEmpty(jsn.Split('$')[1]))
                {
                    string[] temps = jsn.Split('$')[1].Split('&');
                    foreach (string k in temps)
                    {
                        GeoCoordinate marsGeo = GeoConverter.GetMarsFromGps(double.Parse(k.Split(',')[3]), double.Parse(k.Split(',')[2]));
                        MyMap.AddMark(map, marsGeo, k.Split(',')[1], 14D, Colors.Green, Colors.Green, 3);
                        //GpsCoorConvert.gps2HuoXingGeoUnUpdate(double.Parse(k.Split(',')[3]), double.Parse(k.Split(',')[2]), 
                        //    (coo) =>
                        //   {
                        //       MyMap.AddMark(map, coo, k.Split(',')[1], 14D, Colors.Green, Colors.Green, 3);
                        //   });
                    }
                    //GeoCoordinate[] coo = new GeoCoordinate[] { };
                    //string[] cars = new string[] { };
                    //{
                    //暂时没有批量转换的API
                    //}
                }
            });
        }
        catch (Exception ee)
        {
            MessageBox.Show("Taxi获取失败:\n" + ee.Message,"Taxi获取失败", MessageBoxButton.OK);
            //MessageBox.Show("由于国内地图服务采用奇葩的“火星”坐标系，因此GPS获取的坐标在地图上会有500米以上的偏移。" +
            //    "本应用调用第三方的网络偏移修正接口，可能因为网络问题出错，敬请谅解！\n" + ee.Message, 
            //    "地球坐标获取失败", MessageBoxButton.OK);
        }
    }
    #endregion
}