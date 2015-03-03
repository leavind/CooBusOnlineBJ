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
    public class BusLines:INotifyPropertyChanged, INotifyPropertyChanging
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

        private string _IsOpen;
        /// <summary>
        /// 是否开通实时公交
        /// </summary>
        [Column]
        public string IsOpen
        {
            get { return this._IsOpen; }
            set
            {
                if (this._IsOpen != value)
                {
                    NotifyPropertyChanging();
                    this._IsOpen = value;
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

        private string _Dir;
        /// <summary>
        /// 行驶方向/上行 下行
        /// </summary>
        [Column]
        public string Dir
        {
            get { return this._Dir; }
            set
            {
                if (this._Dir != value)
                {
                    NotifyPropertyChanging();
                    this._Dir = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _Price;
        /// <summary>
        /// 价格
        /// </summary>
        [Column]
        public string Price
        {
            get { return this._Price; }
            set
            {
                if (this._Price != value)
                {
                    NotifyPropertyChanging();
                    this._Price = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _Begin_Time;
        /// <summary>
        /// 首班时间
        /// </summary>
        [Column]
        public string Begin_Time
        {
            get { return this._Begin_Time; }
            set
            {
                if (this._Begin_Time != value)
                {
                    NotifyPropertyChanging();
                    this._Begin_Time = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _End_Time;
        /// <summary>
        /// 末班时间
        /// </summary>
        [Column]
        public string End_Time
        {
            get { return this._End_Time; }
            set
            {
                if (this._End_Time != value)
                {
                    NotifyPropertyChanging();
                    this._End_Time = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _Start_Station;
        /// <summary>
        /// 起点站
        /// </summary>
        [Column]
        public string Start_Station
        {
            get { return this._Start_Station; }
            set
            {
                if (this._Start_Station != value)
                {
                    NotifyPropertyChanging();
                    this._Start_Station = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _End_Station;
        /// <summary>
        /// 终点站
        /// </summary>
        [Column]
        public string End_Station
        {
            get { return this._End_Station; }
            set
            {
                if (this._End_Station != value)
                {
                    NotifyPropertyChanging();
                    this._End_Station = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}

