using BaiduClass;
using RouteMatrixClass;
using System.Collections.Generic;
/// <summary>
/// 全局变量
/// </summary>
public class BaiduVar
{
    /// <summary>
    /// 查询到的gps坐标转换成地址
    /// </summary>
    public static Ret_ga retGA = new Ret_ga();

    /// <summary>
    /// 查询到的地址转换成gps坐标
    /// </summary>
    public static Ret_ag retAG = new Ret_ag();
    
    /// <summary>
    /// 从周围的地点列表中选择的地点
    /// </summary>
    public static Ret_pla selePlace;

    ///// <summary>
    ///// 当前位置的 百度坐标
    ///// </summary>
    //public static Loca curLoc;

    /// <summary>
    /// 选择位置的 百度坐标
    /// </summary>
    public static Loca savedLoc;

    ///// <summary>
    ///// 目的地的 百并坐标
    ///// </summary>
    //public static Loca endLoc;

    ///// <summary>
    ///// 当前位置的 百度坐标地名
    ///// </summary>
    //public static string curLocName;

    ///// <summary>
    ///// 选择位置的 百度坐标地名
    ///// </summary>
    //public static string selLocName;

    ///// <summary>
    ///// 目的地的 百并坐标地名
    ///// </summary>
    //public static string endLocName;

    /// <summary>
    /// 查询到的附近地点集合
    /// </summary>
    public static List<Ret_pla> NearLines = new List<Ret_pla>();

    /// <summary>
    /// 模糊匹配地点的的百度返回值的集合
    /// </summary>
    public static List<Ret_like> placeLike = new List<Ret_like>();

    /// <summary>
    /// 模糊匹配地点的的百度返回值的集合
    /// </summary>
    public static List<Route> busRoutList = new List<Route>();

    /// <summary>
    /// 模糊匹配地点的的百度返回值的集合
    /// </summary>
    public static List<Plan> busPlanList = new List<Plan>();
}

