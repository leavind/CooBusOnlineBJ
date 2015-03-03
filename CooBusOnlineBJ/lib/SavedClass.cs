
namespace SavedClass
{
    public class SavedLocation
    {
        public string savedName { get; set; }
        //public string savedAddress { get; set; }
        /// <summary>
        /// Format: lat,lng
        /// </summary>
        public string savedBaiduLocGeo { get; set; }
        public string tag { get; set; }
    }

    public class SavedLineName
    {
        public string savedName { get; set; }
        public string savedDetails { get; set; }
        public string tag { get; set; }
    }
}