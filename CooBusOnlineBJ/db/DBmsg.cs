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
    public class DBmsg: INotifyPropertyChanged, INotifyPropertyChanging
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
        /// ID
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

        private string _Strings;
        /// <summary>
        /// 字段名
        /// </summary>
        [Column]
        public string Strings
        {
            get { return this._Strings; }
            set
            {
                if (this._Strings != value)
                {
                    NotifyPropertyChanging();
                    this._Strings = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _Values;
        /// <summary>
        /// 值
        /// </summary>
        [Column]
        public string Values
        {
            get { return this._Values; }
            set
            {
                if (this._Values != value)
                {
                    NotifyPropertyChanging();
                    this._Values = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}

