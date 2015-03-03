public class aibangBusClass
{
    public Root root { get; set; }
    public class Bus
    {
        public string gt { get; set; }
        public string id { get; set; }
        public string t { get; set; }
        public string ns { get; set; }
        public string nsn { get; set; }
        public string nsd { get; set; }
        public string nsrt { get; set; }
        public string nst { get; set; }
        public string sd { get; set; }
        public string srt { get; set; }
        public string st { get; set; }
        public string x { get; set; }
        public string y { get; set; }
        public string ut { get; set; }
    }

    public class Data
    {
        public Bus[] bus { get; set; }
    }

    public class Root
    {
        public string status { get; set; }
        public string message { get; set; }
        public string encrypt { get; set; }
        public string num { get; set; }
        public string lid { get; set; }
        public Data data { get; set; }
    }
}
