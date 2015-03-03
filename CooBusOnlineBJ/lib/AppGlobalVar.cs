using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System;

namespace RealTimeBusBJ
{
    public class AppGlobalVar
    {
        public static string keyQuitConfirm = "QuitConfirm"; //退出确认 string
        public static string keyNearDiv = "NearDiv"; //周边距离 string 
        public static string keyOpacity = "Opacity"; //透明度 string 
        public static string keyRewindStation = "RewindStation"; //公交显示前面的几站 
        public static string keyShockAlert = "ShockAlert"; //是否振动提醒 
        //public static string keyInvTime = "InvTime"; //公交显示自动刷新的时间

        public static bool quitConfirm = true;
        public static bool ShockAlert = true;

        public static string valueShockAlert = "1"; //退出确认 string
        public static string valueQuitConfirm = "1"; //退出确认 string
        public static string valueNearDiv = "500"; //周边距离 string 
        public static string valueOpacity = "15"; //透明度 string 
        public static string valueRewindStation = "5"; //公交显示前面的几站 
        //public static string valueInvTime = "10"; //自动刷新的时间 
        public static BitmapImage bgImage; //背景图 string 

        public static void readAllKey()
        {
            try
            {
                string str = IsoStorageSetting.Read(keyQuitConfirm);
                if (!string.IsNullOrEmpty(str))
                    valueQuitConfirm = str;
                if (valueQuitConfirm == "0")
                    quitConfirm = false;

                str = IsoStorageSetting.Read(keyShockAlert);
                if (!string.IsNullOrEmpty(str))
                    valueShockAlert = str;
                if (valueShockAlert == "0")
                    ShockAlert = false;

                str = IsoStorageSetting.Read(keyOpacity);
                if (!string.IsNullOrEmpty(str))
                    valueOpacity = str;

                str = IsoStorageSetting.Read(keyRewindStation);
                if (!string.IsNullOrEmpty(str))
                    valueRewindStation = str;

                str = IsoStorageSetting.Read(keyNearDiv);
                if (!string.IsNullOrEmpty(str))
                    valueNearDiv = str;

                ((ImageBrush)App.Current.Resources["appBgImage"]).Opacity = double.Parse(valueOpacity) / 100;
                bgImage = IsoStorage.ReadbgImg();
                if (bgImage != null)
                    ((ImageBrush)App.Current.Resources["appBgImage"]).ImageSource = IsoStorage.ReadbgImg();
            }
            catch (Exception ee)
            {
                MessageBox.Show("加载配置文件异常！\n" + ee.Message);
            }
        }
    }
}