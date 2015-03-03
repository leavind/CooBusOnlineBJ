/// <summary>
/// 全局变量
/// </summary>
public class BusAPI
{
    /// <summary>
    /// 城市代码 深圳860515,北京860201,西安862307,惠州
    /// </summary>
    public const string city_id = "860201";
    /// <summary>
    /// 查询路线是否存在，以及往返的两条子路线概况。 可以取得sublineID
    /// </summary>
    public static string lineNameAPI = "http://busi.gpsoo.net/v1/bus/get_lines_by_city?type=handset&city_id="+city_id+"&line_name=";

    /// <summary>
    /// 查询子路线详情。站台详情
    /// </summary>
    public static string lineSubAPI = "http://busi.gpsoo.net/v1/bus/get_subline_inf?mapType=BAIDU&sid=";

    /// <summary>
    /// 查询路线eid, 是否开通实时公交
    /// </summary>
    public static string lineSidAPI = "http://busi.gpsoo.net/v1/bus/t_lineisopen?code=" + city_id + "&line=";

    /// <summary>
    /// 根据路线eid查询车车辆实况\位置。（包含两个子路线的所有车）
    /// </summary>
    public static string lineEidAPI = "http://busi.gpsoo.net/v1/bus/get_online_gps?mapType=BAIDU&school_id=";
        
    
    ///// <summary>
    ///// 保存路线查询结果集
    ///// </summary>
    //public static List<JSONDATA_line> listJDline = new List<JSONDATA_line>();

    ///// <summary>
    ///// 保存站点单一路线查询结果集（含车在站台间位置）
    ///// </summary>
    //public static List<JSONDATA_station> listJDsta = new List<JSONDATA_station>();

    ///// <summary>
    ///// 保存车辆位置查询结果集
    ///// </summary>
    //public static List<JSONDATA_Location> listJDLloc = new List<JSONDATA_Location>();
}

