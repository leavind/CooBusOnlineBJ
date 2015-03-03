//
// Copyright (C) 1000 - 9999 Somebody Anonymous
// NO WARRANTY OR GUARANTEE
//

using System;
using System.Device.Location;

class GeoConverter
{
    const double pi = 3.14159265358979324;
    const double x_pi = pi * 3000.0 / 180.0;
    // Krasovsky 1940
    // a = 6378245.0, 1/f = 298.3
    // b = a * (1 - f)
    // ee = (a^2 - b^2) / a^2;
    const double a = 6378245.0;
    const double ee = 0.00669342162296594323;
    /// <summary>
    /// GPS(WGS-84)转火星坐标(GCJ-02)
    /// </summary>
    /// <param name="gpsGeocoordinate"></param>
    /// <returns></returns>
    public static GeoCoordinate GetMarsFromGps(double gpsLat,double gpsLng)
    {
        double marsLat = 0;
        double marsLng = 0;
        transform(gpsLat, gpsLng, out marsLat, out marsLng);
        return new GeoCoordinate(marsLat, marsLng);
    }

    /// <summary>
    /// 百度(BD-09)转火星坐标(GCJ-02)
    /// </summary>
    /// <param name="baiduLat"></param>
    /// <param name="baiduLng"></param>
    /// <returns></returns>
    public static GeoCoordinate GetMarsFromBaidu(double baiduLat, double baiduLng)
    {
        double marsLat = 0;
        double marsLng = 0;
        bd_decrypt(baiduLat, baiduLng,out marsLat,out marsLng);
        return new GeoCoordinate(marsLat, marsLng);
    }

    /// <summary>
    /// 火星坐标(GCJ-02)转百度(BD-09)
    /// </summary>
    /// <param name="marsLat"></param>
    /// <param name="marsLng"></param>
    /// <returns></returns>
    public static GeoCoordinate GetBaiduFromMars(double marsLat, double marsLng)
    {
        double baiduLat = 0;
        double baiduLng = 0;
        bd_encrypt(marsLat, marsLng, out baiduLat, out baiduLng);
        return new GeoCoordinate(baiduLat, baiduLng);
    }

    /// <summary>
    /// GPS(WGS-84)转百度(BD-09)
    /// </summary>
    /// <param name="gpsLat"></param>
    /// <param name="gpsLng"></param>
    /// <returns></returns>
    public static GeoCoordinate GetBaiduFromGps(double gpsLat, double gpsLng)
    {
        GeoCoordinate marsGeo = GetMarsFromGps(gpsLat, gpsLng);
        double baiduLat = 0;
        double baiduLng = 0;
        bd_encrypt(marsGeo.Latitude, marsGeo.Longitude, out baiduLat, out baiduLng);
        return new GeoCoordinate(baiduLat, baiduLng);
    }

    /// <summary>
    /// 格式化字符串(维度lat,经度lng)
    /// </summary>
    /// <param name="Coor"></param>
    /// <returns></returns>
    public static string GetString(GeoCoordinate Coor)
    {
        return Coor.Latitude.ToString() + "," + Coor.Longitude.ToString();
    }

    // World Geodetic System ==> Mars Geodetic System
    private static void transform(double wgLat, double wgLon, out double mgLat, out double mgLon)
    {
        if (outOfChina(wgLat, wgLon))
        {
            mgLat = wgLat;
            mgLon = wgLon;
            return;
        }
        double dLat = transformLat(wgLon - 105.0, wgLat - 35.0);
        double dLon = transformLon(wgLon - 105.0, wgLat - 35.0);
        double radLat = wgLat / 180.0 * pi;
        double magic = Math.Sin(radLat);
        magic = 1 - ee * magic * magic;
        double sqrtMagic = Math.Sqrt(magic);
        dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * pi);
        dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * pi);
        mgLat = wgLat + dLat;
        mgLon = wgLon + dLon;
    }

    static bool outOfChina(double lat, double lon)
    {
        if (lon < 72.004 || lon > 137.8347)
            return true;
        if (lat < 0.8293 || lat > 55.8271)
            return true;
        return false;
    }

    static double transformLat(double x, double y)
    {
        double ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y + 0.2 * Math.Sqrt(Math.Abs(x));
        ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
        ret += (20.0 * Math.Sin(y * pi) + 40.0 * Math.Sin(y / 3.0 * pi)) * 2.0 / 3.0;
        ret += (160.0 * Math.Sin(y / 12.0 * pi) + 320 * Math.Sin(y * pi / 30.0)) * 2.0 / 3.0;
        return ret;
    }

    static double transformLon(double x, double y)
    {
        double ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1 * Math.Sqrt(Math.Abs(x));
        ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
        ret += (20.0 * Math.Sin(x * pi) + 40.0 * Math.Sin(x / 3.0 * pi)) * 2.0 / 3.0;
        ret += (150.0 * Math.Sin(x / 12.0 * pi) + 300.0 * Math.Sin(x / 30.0 * pi)) * 2.0 / 3.0;
        return ret;
    }

    static void bd_encrypt(double gg_lat, double gg_lon, out double bd_lat, out double bd_lon)
    {
        double x = gg_lon, y = gg_lat;
        double z = Math.Sqrt(x * x + y * y) + 0.00002 * Math.Sin(y * x_pi);
        double theta = Math.Atan2(y, x) + 0.000003 * Math.Cos(x * x_pi);
        bd_lon = z * Math.Cos(theta) + 0.0065;
        bd_lat = z * Math.Sin(theta) + 0.006;
    }

    static void bd_decrypt(double bd_lat, double bd_lon, out double gg_lat, out double gg_lon)
    {
        double x = bd_lon - 0.0065, y = bd_lat - 0.006;
        double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * x_pi);
        double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * x_pi);
        gg_lon = z * Math.Cos(theta);
        gg_lat = z * Math.Sin(theta);
    }
}