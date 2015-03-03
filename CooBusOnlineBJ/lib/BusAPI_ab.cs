/// <summary>
/// 全局变量
/// </summary>
public class BusAPI_ab
{
    /// <summary>
    /// 通过路线名查询线路站点及站点坐标,utf-8编码  //f41c8afccc586de03a99c86097e98ccb //9c489bc685a5bd95d42a9f92a318218a
    /// </summary>
    public static string lineStationAPI = "http://openapi.aibang.com/bus/lines?app_key=9c489bc685a5bd95d42a9f92a318218a&city=%E5%8C%97%E4%BA%AC&with_xys=1&alt=json&q=";

    /// <summary>
    /// 通过路线lineID获取车辆实时位置，更换候点站可替换“no=1”
    /// </summary>
    public static string busAPI = "http://bjgj.aibang.com:8899/bus.php?city=%E5%8C%97%E4%BA%AC&datatype=json&no=1&encrypt=0&id=";

    /// <summary>
    /// 爱帮北京路线的汇总。 格式： 路线名,路线ID
    /// </summary>
    public static string busLinesTxtFromRaywind = "http://raywind.net/busline/line_beijing_ab.txt";

    public static string busLinesTxtFromSZIDWELL = "http://www.szidwell.com/busline/line_beijing_ab.txt";  
}

