
public class aibangBusLineClass
{
    public string result_num { get; set; }
    public string web_url { get; set; }
    public string wap_url { get; set; }
    public Lines lines { get; set; }
    public class Line
    {
        public string name { get; set; }
        public string info { get; set; }
        public string stats { get; set; }
        public string stat_xys { get; set; }
        public string xys { get; set; }
    }

    public class Lines
    {
        public Line[] line { get; set; }
    }
}
