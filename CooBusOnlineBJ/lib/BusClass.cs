
namespace BusClass
{
    #region line
    public class DATA_line
    {
        public string begin_time { get; set; }
        public string dir { get; set; }
        public string end_station { get; set; }
        public string end_time { get; set; }
        public string id { get; set; }
        public string isopen { get; set; }
        public string line_name { get; set; }
        public string price { get; set; }
        public string start_station { get; set; }
    }

    public class DATA_lineAndAB
    {
        public string begin_time { get; set; }
        public string dir { get; set; }
        public string end_station { get; set; }
        public string end_time { get; set; }
        public string id { get; set; }
        public string isopen { get; set; }
        public string line_name { get; set; }
        public string price { get; set; }
        public string start_station { get; set; }
        public string aibanginfo { get; set; }
    }

    public class JSONSTR_line
    {
        public DATA_line[] data { get; set; }
        public string msg { get; set; }
        public string success { get; set; }
    }

    public class JSONDATA_line
    {
        public string title { get; set; }
        public string detail { get; set; }
        public string sid { get; set; }
        public string sid2 { get; set; }
        public string isopen { get; set; }
    }

    #endregion

    #region stations
    public class DATA_station
    {
        public BASIC_station basic { get; set; }
        public STATIONS_station[] stations { get; set; }
        public TRACK_station[] track { get; set; }
    }

    public class BASIC_station
    {
        public string begin_time { get; set; }
        public string end_station { get; set; }
        public string end_time { get; set; }
        public string id { get; set; }
        public string price { get; set; }
        public string start_station { get; set; }
    }

    public class STATIONS_station
    {
        public string code { get; set; }
        public string id { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string name { get; set; }
    }

    public class TRACK_station
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }
    public class JSONSTR_station
    {
        public DATA_station data { get; set; }
        public string msg { get; set; }
        public string success { get; set; }
    }


    /// <summary>
    /// 这里使用GPS坐标,不能搞混了.
    /// </summary>
    public class JSONDATA_station
    {
        public string title { get; set; }
        //public string stop { get; set; }
        //public string run { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public StopIMG StopImg { get; set; }
        public RunIMG RunImg { get; set; }
        //public string detail { get; set; }
        //public string id { get; set; }
    }

    public class StopIMG
    {
        public string Fir { get; set; }
        public string Sec { get; set; }
        public string Thi { get; set; }
        public string Fou { get; set; }
        public string Fif { get; set; }
    }

    public class RunIMG
    {
        public string Fir { get; set; }
        public string Sec { get; set; }
        public string Thi { get; set; }
        public string Fou { get; set; }
        public string Fif { get; set; }
    }

    #endregion


    #region Eid

    public class JSONSTR_Eid
    {
        public DATA_Eid[] data { get; set; }
        public string msg { get; set; }
        public string success { get; set; }
    }

    public class DATA_Eid
    {
        public string eid { get; set; }
        public string isopen { get; set; }
    }
    #endregion

    #region Location
    public class JSONSTR_Location
    {
        public KEY_Location key { get; set; }
        public string seed { get; set; }
        public string[][] records { get; set; }
    }

    public class KEY_Location
    {
        public string sys_time { get; set; }
        public string user_name { get; set; }
        public string jingdu { get; set; }
        public string weidu { get; set; }
        public string datetime { get; set; }
        public string heart_time { get; set; }
        public string su { get; set; }
        public string status { get; set; }
        public string hangxiang { get; set; }
        public string sim_id { get; set; }
        public string user_id { get; set; }
        public string sale_type { get; set; }
        public string iconType { get; set; }
        public string server_time { get; set; }
        public string product_type { get; set; }
        public string expire_date { get; set; }
        public string group_id { get; set; }
        public string next_station { get; set; }
        public string cur_station { get; set; }
        public string cur_station_state { get; set; }
        public string subline_id { get; set; }
    }

    public class JSONDATA_Location
    {
        public string CarNO { get; set; }
        public string Station { get; set; }
        public string BusStop { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
    }

    public class NearBus
    {
        public string CarNO { get; set; }
        public string Station { get; set; }
        public int staIndex { get; set; }
        public string Distance { get; set; }
        public string BusStop { get; set; }
    }
    #endregion


    #region LineHistory
    public class LineHistory
    {
        public string lineName { get; set; }
        public string btnContent = "删除";
    }

    #endregion
}
