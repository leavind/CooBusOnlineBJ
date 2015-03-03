using System.Data.Linq;

namespace RealTimeBusBJ.db
{
    public class DBContext : DataContext
    {
        public const string CONNET_STR = "Data Source='isostore:/db.sdf';Password='!@#$%^&*()'";

        public DBContext()
            : base(CONNET_STR)
        {
        }

        public Table<DBmsg> DBmsg;

        public Table<BusLines> BusLines;
        public Table<BusStations> BusStations;
        public Table<BusLines_ab> BusLines_ab;
    }
}
