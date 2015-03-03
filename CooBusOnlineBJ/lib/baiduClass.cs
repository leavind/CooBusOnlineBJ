
namespace BaiduClass
{

    #region Global

    /// <summary>
    /// 百度地图坐标
    /// </summary>
    public class Loca
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

    #region place
    public class JSONSTR_pla
    {
        public Ret_pla[] results { get; set; }
        public string status { get; set; }
        public string message { get; set; }
    }

    public class Ret_pla
    {
        public string name { get; set; }
        public Loca location { get; set; }
        public string address { get; set; }
        public string street_id { get; set; }
        public string uid { get; set; }
    }

    public class JSONSTR_like
    {
        public Ret_like[] result { get; set; }
        public string status { get; set; }
        public string message { get; set; }
    }

    public class Ret_like
    {
        public string name { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string business { get; set; }
        public string cityid { get; set; }
    }
    #endregion

    #region convert
    public class JSONSTR_ga
    {
        public Ret_ga result { get; set; }
        public string status { get; set; }
    }

    public class Ret_ga
    {
        public Loca location { get; set; }
        public string formatted_address { get; set; }
        public string business { get; set; }
        public string cityCode { get; set; }
        public AdrCm_ga addressComponent { get; set; }
    }

    public class AdrCm_ga
    {
        public string city { get; set; }
        public string district { get; set; }
        public string province { get; set; }
        public string street { get; set; }
        public string street_number { get; set; }
    }

    public class JSONSTR_ag
    {
        public Ret_ag results { get; set; }
        public string status { get; set; }
    }

    public class Ret_ag
    {
        public Loca location { get; set; }
        public string precise { get; set; }
        public string confidence { get; set; }
        public string level { get; set; }
    }
    #endregion

    #region GPS2BaiduGEO
    public class Ret_gps2bd
    {
        /// <summary>
        /// 奇葩的百度.x是经度
        /// </summary>
        public string x { get; set; }
        /// <summary>
        /// 奇葩的百度.y是纬度
        /// </summary>
        public string y { get; set; }
    }

    public class JSONSTR_gps2bd
    {
        public string status { get; set; }
        public Ret_gps2bd[] result { get; set; }
    }
    #endregion

}