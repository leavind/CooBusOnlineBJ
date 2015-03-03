using System;

public class GpsDistance
{
    private const double EARTH_RADIUS = 6378137;
    private static double rad(double d)
    {
        return d * Math.PI / 180.0;
    }

    public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
    {
        double radLat1 = rad(lat1);
        double radLat2 = rad(lat2);
        double a = radLat1 - radLat2;
        double b = rad(lng1) - rad(lng2);
        double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2)
            + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
        s = s * EARTH_RADIUS;
        s = Math.Round(s * 10000) / 10000;
        return s;
    }
}

///// <summary>
///// GPS座标格式(纬度,经度)
///// </summary>
//private static string strGpsCoor;
///// <summary>
///// 百度地图坐标
///// </summary>
//public class Loca
//{
//    /// <summary>
//    /// latitude 纬度
//    /// </summary>
//    public string lat { get; set; }
//    /// <summary>
//    /// Longitude 经度
//    /// </summary>
//    public string lng { get; set; }
//}