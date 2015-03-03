using System;
using System.Windows;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using System.Windows.Navigation;
using BusClass;
using System.Windows.Controls;
using SavedClass;
using System.Threading;
using RouteMatrixClass;

namespace RealTimeBusBJ
{
    public partial class RoutePlan : PhoneApplicationPage
    {
        public RoutePlan()
        {
            InitializeComponent();
        }

        private int LoadCount = 0;
        string curTextBoxName;

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            //正常屏幕比例15/9，长屏16/9。下面为长屏改变布局
            double H = this.ActualHeight, W = this.ActualWidth;
            //if (Math.Round(H / W, 2) == Math.Round(16D / 9D, 2))  //if (H*9 == W*16) 
            if (Math.Round(H / W, 2) == Math.Round(16D / 9D, 2))
            {
                double offset = H * 9 / 15 - W * 15 / 16;
                list1.Height += offset;
            }   
            if (LoadCount == 0)
            {
                LoadCount += 1;
                getCurLocation();
            }
        }

        void refreshLoc(TextBox tb)
        {
            curTextBoxName = tb.Name;
            string tmp = IsoStorage.ReadLocation();
            if (tmp.Length > 0)
            {
                List<SavedLocation> listSaveloc = new List<SavedLocation>();
                string[] ss = tmp.Split(';');
                for (int i = 0; i < ss.Length; i++)
                {
                    string s = ss[i];
                    if (s.Contains(":"))
                    {
                        SavedLocation slc = new SavedLocation();
                        slc.savedName = s.Split(':')[0];
                        //slc.savedAddress = s.Split(':')[1];
                        slc.savedBaiduLocGeo = s.Split(':')[1];
                        slc.tag = i.ToString();
                        listSaveloc.Add(slc);
                    }
                }
                list2.ItemsSource = listSaveloc;
            }
        }

        void refreshLike(TextBox tb)
        {
            curTextBoxName = tb.Name;
            if (!tb.Text.Contains("当前位置") && tb.Text.Trim().Length > 0)
            {
                BaiduAPI.placeSearchLike(tb.Text.Trim(), () =>
                {
                    if (BaiduVar.placeLike.Count > 0)
                    {
                        List<SavedLineName> listLine = new List<SavedLineName>();
                        for (int i = 0; i < BaiduVar.placeLike.Count; i++)
                        {
                            SavedLineName sn = new SavedLineName();
                            sn.savedName = BaiduVar.placeLike[i].name;
                            sn.savedDetails = BaiduVar.placeLike[i].city + BaiduVar.placeLike[i].district;
                            sn.tag = i.ToString();
                            listLine.Add(sn);
                        }
                        list2.ItemsSource = listLine;
                    }
                });
            }
        }


        private void getCurLocation()
        {
            new ThreadStart(
                () =>
                {
                    //获取所在位置的名称,座标 和 详细地址
                    GeoService.GetGeocoordinate(() =>
                    {
                        BaiduAPI.geo2add(GeoService.strBaiduGeo, () =>
                        {
                            tb1.Text = "当前位置(" 
                                + BaiduVar.retGA.addressComponent.street
                                + BaiduVar.retGA.addressComponent.street_number 
                                + ")";
                            //BaiduVar.curLoc = BaiduVar.retGA.location;
                            tb2.Focus();
                        });
                    });
                }).Invoke();
        }

        void QueryClick()
        {
            if (tb1.Text.Trim().Length == 0 || tb1.Text.Trim().Length == 0 || tb1.Text == tb2.Text)
                return;
            panel2.Visibility = System.Windows.Visibility.Collapsed;
            list1.Visibility = System.Windows.Visibility.Visible;
            list1.ItemsSource = null;
            try
            {
                string start = tb1.Text;
                string end = tb2.Text;
                if (start.Contains("当前位置"))
                {
                    if (string.IsNullOrEmpty( GeoService.strBaiduGeo))
                        return;
                    else 
                    {
                        start = GeoService.strBaiduGeo;
                        if (!string.IsNullOrEmpty(tapedLoc))
                            end = tapedLoc;
                    }
                }
                else if (end.Contains("当前位置"))
                {
                    if (string.IsNullOrEmpty(GeoService.strBaiduGeo))
                        return;
                    else
                    {
                        end = GeoService.strBaiduGeo;
                        if (!string.IsNullOrEmpty(tapedLoc))
                            start = tapedLoc;
                    }
                }
                BaiduAPI.busDirectionQuery(start, end, () =>
                {

                    //string msg = "";
                    BaiduVar.busPlanList.Clear();
                    for (int i = 0; i < BaiduVar.busRoutList.Count; i++)
                    {
                        string lineName = "";
                        string title = "方案" + (i + 1).ToString() + ":  ";
                        string tmp = "";

                        foreach (var r in BaiduVar.busRoutList[i].scheme)
                        {
                            foreach (var j in r.steps)
                            {
                                foreach (var k in j)
                                {
                                    if (!string.IsNullOrEmpty(k.stepInstruction))
                                    {
                                        if (k.vehicle != null)
                                        {
                                            lineName += k.vehicle.name + ";";
                                        }
                                        tmp += HtmlTag.removeHtmlTag(k.stepInstruction);                                
                                    }
                                }
                            }
                            if (lineName.Length > 0)
                                title = title + lineName.Substring(0, lineName.Length - 1).Replace(";", " → ") + "\n";
                            title += "全程" + m2Km(r.distance) + ",耗时" + s2H(r.duration);
                        }
                        Plan pl = new Plan();
                        pl.title = title;
                        pl.line = lineName.Substring(0, lineName.Length - 1);
                        pl.detail = tmp.Replace("步行", "\n步行").Replace("经过", "\n经过").Replace("乘坐", "\n乘坐").Trim();
                        pl.tag = i.ToString();
                        BaiduVar.busPlanList.Add(pl);
                        //msg += title + tmp + "\n";
                    }
                    if (BaiduVar.busPlanList.Count > 0)
                        list1.ItemsSource = BaiduVar.busPlanList;
                    //MessageBox.Show(msg);
                });
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        /// <summary>
        /// 米转成公里
        /// </summary>
        /// <param name="m">单位米</param>
        /// <returns>公里,不足1公里返回米</returns>
        private string m2Km(string m)
        {
            float i = float.Parse(m);
            if (i > 1000)
                return Math.Round(i / 1000F, 1).ToString() + "公里";
            else
                return m + "米";
        }

        /// <summary>
        /// 秒转成小时
        /// </summary>
        /// <param name="s">单位秒</param>
        /// <returns>小时,不足60返回秒,不足3600返回分钟</returns>
        private string s2H(string s)
        {
            float i = float.Parse(s);
            if (i < 60)
                return s + "秒";
            else if (i >= 60 && i < 3600)
                return Math.Round(i / 60).ToString() + "分钟";
            else
                return (int.Parse(s) / 3600).ToString() + "小时"
                    + (i % 3600 < 60 ? "" : Math.Round(((i % 3600) / 60)).ToString() + "分");
        }

        private void tb1_GotFocus(object sender, RoutedEventArgs e)
        {
            //((App)Application.Current).RootVisual.RenderTransform = new CompositeTransform();
            ((TextBox)sender).SelectAll();
            refreshLoc((TextBox)sender);

            list1.Visibility = System.Windows.Visibility.Collapsed;
            panel2.Visibility = System.Windows.Visibility.Visible;
        }

        private void tb1_LostFocus(object sender, RoutedEventArgs e)
        {
            list1.Visibility = System.Windows.Visibility.Visible;
            panel2.Visibility = System.Windows.Visibility.Collapsed;
        }

        string tapedLoc;
        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (curTextBoxName == "tb1")
                tb1.Text = ((System.Windows.Controls.TextBlock)sender).Text;
            else if (curTextBoxName == "tb2")
                tb2.Text = ((System.Windows.Controls.TextBlock)sender).Text;
            if (list2.ItemsSource.GetType() == typeof(List<SavedLocation>))
                tapedLoc = ((List<SavedLocation>)list2.ItemsSource)[int.Parse(((TextBlock)sender).Tag.ToString())].savedBaiduLocGeo;
            list1.Visibility = System.Windows.Visibility.Visible;
            panel2.Visibility = System.Windows.Visibility.Collapsed;
            QueryClick();
        }

        private void B2_Click(object sender, RoutedEventArgs e)
        {
            QueryClick();
        }

        private void B1_Click(object sender, RoutedEventArgs e)
        {
            string tmp = tb1.Text;
            tb1.Text = tb2.Text;
            tb2.Text = tmp;
        }

        private void tb1_TextChanged(object sender, TextChangedEventArgs e)
        {
            refreshLike((TextBox)sender);
        }

        private void Plan_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string lineName = ((List<Plan>)list1.ItemsSource)[int.Parse(((TextBlock)sender).Tag.ToString())].line;
            seleLineList.isFromPlan = true;
            seleLineList.linespit = lineName;
            seleLineList.stationName = tb1.Text.Replace("当前位置", "").Replace("(", "").Replace(")", "");
            NavigationService.Navigate(new Uri("/page/seleLineList.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}