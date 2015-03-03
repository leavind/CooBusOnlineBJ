using System;
using System.Windows;
using Microsoft.Phone.Controls;
using System.Windows.Controls;
using Microsoft.Phone.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RealTimeBusBJ
{
    public partial class Config : PhoneApplicationPage
    {
        public Config()
        {
            InitializeComponent();
            ts1.IsChecked = AppGlobalVar.quitConfirm;
            sl1.Value = double.Parse(AppGlobalVar.valueOpacity);
            tb1.Text = AppGlobalVar.valueNearDiv;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask mrTask = new MarketplaceReviewTask();
            mrTask.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();
            emailComposeTask.Subject = "对WP8应用\"北京实时公交\"的建议";
            emailComposeTask.Body = "请输入你的建议：\n";
            emailComposeTask.To = "chei@outlook.com";
            emailComposeTask.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ShareLinkTask slTask = new ShareLinkTask();
            slTask.LinkUri = new Uri("http://www.windowsphone.com/s?appid=f92d67fb-3f84-4183-b7bd-efa83eec19b4");
            slTask.Message = "这个应用很不错哦，我正在用呢，推荐小伙伴们一起来用！";
            slTask.Title = "推荐WP8应用\"北京实时公交\"";
            slTask.Show();
        }

        private void ts1_Checked(object sender, RoutedEventArgs e)
        {
            IsoStorageSetting.Write(AppGlobalVar.keyQuitConfirm, "1");
            AppGlobalVar.quitConfirm = true;
        }

        private void ts1_Unchecked(object sender, RoutedEventArgs e)
        {
            IsoStorageSetting.Write(AppGlobalVar.keyQuitConfirm, "0");
            AppGlobalVar.quitConfirm = false;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            PhotoChooserTask pct = new PhotoChooserTask();
            pct.PixelHeight = (int)this.ActualHeight;
            pct.PixelWidth = (int)this.ActualWidth;
            pct.ShowCamera = true;
            pct.Completed += pct_Completed;
            pct.Show();
        }

        void pct_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                Dispatcher.BeginInvoke(() =>
                {                
                    BitmapImage bi = new BitmapImage();
                    bi.SetSource(e.ChosenPhoto);  
                    ((ImageBrush)App.Current.Resources["appBgImage"]).ImageSource = bi;
                    IsoStorage.SavebgImg(bi);
                }); 
            }
        }

        private void sl1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                IsoStorageSetting.Write(AppGlobalVar.keyOpacity, e.NewValue.ToString());
                ((ImageBrush)App.Current.Resources["appBgImage"]).Opacity = e.NewValue / 100;
                AppGlobalVar.valueOpacity = e.NewValue.ToString();
            }); 
        }

        //private void ListPickerItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        //{
        //    IsoStorageSetting.Write(AppGlobalVar.keyNearDiv, ((ListPickerItem)sender).Content.ToString());
        //}

        private void tb1_TextChanged(object sender, TextChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                int near = 0;
                if (int.TryParse(tb1.Text, out near) && near > 0)
                {
                    IsoStorageSetting.Write(AppGlobalVar.keyNearDiv, near.ToString());
                    AppGlobalVar.valueNearDiv = tb1.Text;
                }
            }); 
        }

        private void tb1_GotFocus(object sender, RoutedEventArgs e)
        {
            tb1.SelectAll();
        }
    }
}