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
    public class BusLines_ab: INotifyPropertyChanged, INotifyPropertyChanging
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
        [Column(IsPrimaryKey=true)]
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

        private string _Line_Name;
        /// <summary>
        /// 线路名
        /// </summary>
        [Column]
        public string Line_Name
        {
            get { return this._Line_Name; }
            set
            {
                if (this._Line_Name != value)
                {
                    NotifyPropertyChanging();
                    this._Line_Name = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _Info;
        /// <summary>
        /// 线路概述。 公交类型;时间;票价
        /// </summary>
        [Column]
        public string Info
        {
            get { return this._Info; }
            set
            {
                if (this._Info != value)
                {
                    NotifyPropertyChanging();
                    this._Info = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _Stations;
        /// <summary>
        /// 站点 格式--- 站台名,x,y;  
        /// </summary>
        [Column]
        public string Stations
        {
            get { return this._Stations; }
            set
            {
                if (this._Stations != value)
                {
                    NotifyPropertyChanging();
                    this._Stations = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}

