using System;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Runtime.CompilerServices;

namespace RealTimeBusBJ.db
{
    /// <summary>
    /// 所有路线汇总表
    /// </summary>
    [System.Data.Linq.Mapping.Table]
    public class BusStations : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        private void NotifyPropertyChanging([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        private string _ID;
        /// <summary>
        /// lineID
        /// </summary>
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT  IDENTITY(1, 1)")]
        public string ID
        {
            get { return this._ID; }
            set
            {
                if (this._ID != value)
                {
                    NotifyPropertyChanging();
                    this._ID = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _SubLineID;
        /// <summary>
        /// 子线路ID
        /// </summary>
        [Column]
        public string SubLineID
        {
            get { return this._SubLineID; }
            set
            {
                if (this._SubLineID != value)
                {
                    NotifyPropertyChanging();
                    this._SubLineID = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _JsonString;
        /// <summary>
        /// Json字符串
        /// </summary>
        [Column(DbType = "NTEXT")]
        public string JsonString
        {
            get { return this._JsonString; }
            set
            {
                if (this._JsonString != value)
                {
                    NotifyPropertyChanging();
                    this._JsonString = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}

