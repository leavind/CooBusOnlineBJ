
namespace RouteMatrixClass
{
    #region Global

    /// <summary>
    /// 百度地图坐标
    /// </summary>
    public class LocaRoute
    {
        /// <summary>
        /// latitude 纬度
        /// </summary>
        public string lat { get; set; }
        /// <summary>
        /// Longitude 经度
        /// </summary>
        public string lng { get; set; }
    }

    #endregion
    public class Json_rout
    {
        public string status { get; set; }
        public string message { get; set; }
        public string type { get; set; }
        public Info info { get; set; }
        public Result result { get; set; }
    }

    public class Info
    {
        public Copyright copyright { get; set; }
    }

    public class Copyright
    {
        public string text { get; set; }
        public string imageUrl { get; set; }
    }


    public class Result
    {
        /// <summary>
        /// 规划的路线
        /// </summary>
        public Route[] routes { get; set; }
        /// <summary>
        /// 起点经纬度
        /// </summary>
        public Origin origin { get; set; }
        /// <summary>
        /// 终点经纬度
        /// </summary>
        public Destination destination { get; set; }
        /// <summary>
        /// 的士
        /// </summary>
        public Taxi taxi { get; set; }
    }

    /// <summary>
    /// 规划的路线
    /// </summary>
    public class Route
    {
        /// <summary>
        /// 数据构造
        /// </summary>
        public Scheme[] scheme { get; set; }
    }

    /// <summary>
    /// 数据构造
    /// </summary>
    public class Scheme
    {
        /// <summary>
        /// 路段距离	单位：米
        /// </summary>
        public string distance { get; set; }
        /// <summary>
        /// 路段耗时	单位：秒
        /// </summary>
        public string duration { get; set; }
        /// <summary>
        /// 路径规划
        /// </summary>
        public Steps[][] steps { get; set; }
        /// <summary>
        /// 起点经纬度坐标
        /// </summary>
        public LocaRoute originLocation { get; set; }
        /// <summary>
        /// 终点经纬度坐标	
        /// </summary>
        public LocaRoute destinationLocation { get; set; }
    }

    /// <summary>
    /// 路段
    /// </summary>
    public class Steps
    {
        /// <summary>
        /// 路段距离	单位：米
        /// </summary>
        public string distance { get; set; }
        /// <summary>
        /// 路段耗时	单位：秒
        /// </summary>
        public string duration { get; set; }
        /// <summary>
        /// 路段位置坐标描述	
        /// </summary>
        public string path { get; set; }
        /// <summary>
        /// 路段类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 路段起点坐标
        /// </summary>
        public LocaRoute stepOriginLocation { get; set; }
        /// <summary>
        /// 路段终点坐标
        /// </summary>
        public LocaRoute stepDestinationLocation { get; set; }
        /// <summary>
        /// 路段说明
        /// </summary>
        public string stepInstruction { get; set; }
        /// <summary>
        /// 车辆
        /// </summary>
        public Vehicle vehicle { get; set; }
        /// <summary>
        /// 起点经纬度坐标
        /// </summary>
        public LocaRoute originLocation { get; set; }
        /// <summary>
        /// 终点经纬度坐标
        /// </summary>
        public LocaRoute destinationLocation { get; set; }
    }

    /// <summary>
    /// 车辆
    /// </summary>
    public class Vehicle
    {
        /// <summary>
        /// 公交线路终点名称
        /// </summary>
        public string end_name { get; set; }
        /// <summary>
        /// 公交线路的末班车时间
        /// </summary>
        public string end_time { get; set; }
        /// <summary>
        /// 公交线路终点id
        /// </summary>
        public string end_uid { get; set; }
        /// <summary>
        /// 公交线路名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 公交线路起点名称
        /// </summary>
        public string start_name { get; set; }
        /// <summary>
        /// 公交线路首班车时间
        /// </summary>
        public string start_time { get; set; }
        /// <summary>
        /// 公交线路起点id
        /// </summary>
        public string start_uid { get; set; }
        /// <summary>
        /// 路段经过的站点数量
        /// </summary>
        public string stop_num { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public string total_price { get; set; }
        /// <summary>
        /// 公交线路类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 公交线路id
        /// </summary>
        public string uid { get; set; }
        /// <summary>
        /// 区间价
        /// </summary>
        public string zone_price { get; set; }
    }

    /// <summary>
    /// 起点经纬度
    /// </summary>
    public class Origin
    {
        /// <summary>
        /// 起点经纬度
        /// </summary>
        public LocaRoute originPt { get; set; }
    }

    /// <summary>
    /// 终点经纬度坐标
    /// </summary>
    public class Destination
    {
        /// <summary>
        /// 终点经纬度坐标
        /// </summary>
        public LocaRoute destinationPt { get; set; }
    }

    /// <summary>
    /// 地士详情
    /// </summary>
    public class Detail
    {
        /// <summary>
        /// 描述
        /// </summary>
        public string desc { get; set; }
        /// <summary>
        /// 每公里价格
        /// </summary>
        public string kmPrice { get; set; }
        /// <summary>
        /// 起步价
        /// </summary>
        public string startPrice { get; set; }
        /// <summary>
        /// 总价格
        /// </summary>
        public string totalPrice { get; set; }
    }

    /// <summary>
    /// 的士
    /// </summary>
    public class Taxi
    {
        /// <summary>
        /// 地士详情
        /// </summary>
        public Detail[] detail { get; set; }
        /// <summary>
        /// 起终点之间距离
        /// </summary>
        public string distance { get; set; }
        /// <summary>
        /// 起终点方案耗时
        /// </summary>
        public string duration { get; set; }
        /// <summary>
        /// 详细信息
        /// </summary>
        public string remark { get; set; }
    }

    /// <summary>
    /// 规划的结果
    /// </summary>
    public class Plan
    {
        /// <summary>
        /// 方案标头
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 方案详情
        /// </summary>
        public string detail { get; set; }
        /// <summary>
        /// 方案包启的路线,用分号";"分隔
        /// </summary>
        public string line { get; set; }
        /// <summary>
        /// 标记
        /// </summary>
        public string tag { get; set; }
    }
}