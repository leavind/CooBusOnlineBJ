using System;
using System.Windows;
using Microsoft.Phone.Controls;
using RestSharp;
using Newtonsoft.Json;
using BusClass;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Phone.Shell;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Shapes;
using System.Text;
using System.Net;

namespace RealTimeBusBJ
{
    public partial class BusRunAiBang : PhoneApplicationPage
    {
        public BusRunAiBang()
        {
            InitializeComponent();
            this.BackKeyPress += BusRun_BackKeyPress;
            if (AppGlobalVar.ShockAlert == true)
                ((ApplicationBarMenuItem)ApplicationBar.MenuItems[3]).Text = "取消振动提醒";
            else
                ((ApplicationBarMenuItem)ApplicationBar.MenuItems[3]).Text = "开启振动提醒";
        }

        void BusRun_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isFromTile)
                e.Cancel = true;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            {
                ShellTile myTile = ShellTile.ActiveTiles.FirstOrDefault(n => n.NavigationUri.ToString().Contains("s="));
                if (myTile != null && this.NavigationService.Source.OriginalString.Contains("s="))
                {
                    isFromTile = true;
                    ((ApplicationBarIconButton)ApplicationBar.Buttons[3]).IconUri =
                        new Uri("/image/appbar/cancel.png", UriKind.RelativeOrAbsolute);
                    ((ApplicationBarIconButton)ApplicationBar.Buttons[3]).Text = "退出";
                    lineName = NavigationContext.QueryString["lineName"];
                    lineNameDetail = NavigationContext.QueryString["lineNameDetail"];
                    Sid1 = NavigationContext.QueryString["Sid"];
                }
                else
                {
                    foreach (ShellTile i in ShellTile.ActiveTiles)
                    {
                        if (i.NavigationUri.OriginalString.Contains("=" + lineName + "&"))
                        {
                            ((ApplicationBarIconButton)ApplicationBar.Buttons[3]).IsEnabled = false;
                            break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(GeoService.strBaiduGeo))
                    new ThreadStart(() => { GeoService.GetGeocoordinate(); }).Invoke();
                pageTitle.Text = lineName;
            }
        }

        public static string lineNameDetail = "";
        public static string lineName = "";
        public static string Sid1 = "";

        List<STATIONS_station> tmpStations1;

        private string SeleStaName1;
        private string SeleStaLoc1;
        private int SeleStaIndex1;

        string busDistanceTimeStr = "";

        bool isFromTile = false;
        string pageTitleTmp = "";

        DispatcherTimer t1 = new System.Windows.Threading.DispatcherTimer();
        //ImageSource garyImage = new BitmapImage(new Uri("/image/Gray.png", UriKind.RelativeOrAbsolute));
        SolidColorBrush scbTitle = (SolidColorBrush)App.Current.Resources["titleColor"];
        SolidColorBrush scbContent = (SolidColorBrush)App.Current.Resources["contentColor"];
        SolidColorBrush scbRed = new SolidColorBrush(Colors.Red);
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

            tmpStations1 = new List<STATIONS_station>();
            //new ThreadStart(() => { firstRefresh(Sid1); }).Invoke();
            pageTitleTmp = pageTitle.Text;
            QueryStation();
            t1.Tick += t1_Tick;
            //将选择的公交路线写入收藏
            if (!IsoStorage.ReadLineName().Contains(lineName))
                IsoStorage.WriteLineName(lineName + ";");
        }

        void t1_Tick(object sender, EventArgs e)
        {
            QueryLocation();
        }

        void BuildStationData(List<STATIONS_station> tmps)
        {
            tb1.Text = tmpStations1[0].name + " → " + tmpStations1[tmpStations1.Count - 1].name;
            tb2.Text = "最近车辆:";
            tb3.Text = " 无";
            tbTop.Text = "";
            List<JSONDATA_station> listJDsta = new List<JSONDATA_station>();
            for (int i = 0; i < tmps.Count; i++)
            {
                JSONDATA_station jd = new JSONDATA_station();
                jd.title = tmps[i].name.Trim() + " " + (i + 1).ToString() + "  ";
                listJDsta.Add(jd);
            }
            list1.ItemsSource = listJDsta;
        }

        int retryTimes = 0;
        private void QueryLocation()
        {
            retryTimes += 1;
            busDistanceTimeStr = "";
            //list1.ItemsSource = null;
            //list2.ItemsSource = null;
            List<JSONDATA_Location> listJDLloc1 = new List<JSONDATA_Location>();
            progressBar.Show();
            var client = new RestClient(BusAPI_ab.busAPI + Sid1 + "&" + DateTime.Now.ToString("HHmmss"));
            client.ExecuteAsync(new RestRequest(Method.GET), (response) =>
            {
                string jsn = response.Content.Replace(" ", "");
                if (jsn.Contains("\"message\":\"success\""))
                {
                    aibangBusClass jsl = (aibangBusClass)JsonConvert.DeserializeObject(jsn, typeof(aibangBusClass));
                    foreach (var ss in jsl.root.data.bus)
                    {
                        JSONDATA_Location jl = new JSONDATA_Location();
                        jl.lat = ss.x;
                        jl.lng = ss.y;
                        jl.CarNO = ss.id;
                        jl.Station = ss.ns; //这里车应该在上一站
                        if (ss.nsd == "-1" && ss.sd == "-1")
                            jl.BusStop = "0";
                        else
                            jl.BusStop = "1";
                        listJDLloc1.Add(jl);
                    }
                    if (retryTimes < 5 && listJDLloc1.Count == 0)
                    {
                        QueryLocation();
                        return;
                    }
                    retryTimes = 0;
                }
                buildStationLocationDatas(listJDLloc1);
                progressBar.Hide();
            });
        }

        private void QueryStation()
        {
            if (tmpStations1.Count == 0)
            {
                string jsonstr = ReadStations(lineName);
                if (jsonstr == "")
                {
                    progressBar.Show();
                    string url = BusAPI_ab.lineStationAPI + HttpUtility.UrlEncode(lineName).Replace("(", "%28").Replace(")", "%29");
                    var client = new RestClient(url);
                    //MessageBox.Show(url);
                    client.ExecuteAsync(new RestRequest(Method.GET), (response) =>
                    {
                        string jsn = response.Content;
                        aibangBusLineClass jsr = (aibangBusLineClass)JsonConvert.DeserializeObject(jsn, typeof(aibangBusLineClass));
                        if (!string.IsNullOrEmpty(jsr.result_num) && int.Parse(jsr.result_num) > 0)
                        {
                            int index = lineName.IndexOf('(');
                            string line1 = lineName.Substring(0, index);
                            string tmp = lineName.Substring(index + 1, lineName.Length - index - 2);
                            string line2 = tmp.Split('-')[0];
                            string line3 = tmp.Split('-')[1];
                            //MessageBox.Show(lineName + "\r" + line1 + "\r" + line2 + "\r" + line3 + "\r");
                            //aibangBusLineClass.Line ln = jsr.lines.line.First((s) => s.name == lineName);
                            aibangBusLineClass.Line ln = jsr.lines.line[0];
                            foreach (var ttt in jsr.lines.line)
                            {
                                int indexa = ttt.name.IndexOf('(');
                                string line1a = ttt.name.Substring(0, indexa);
                                string tmpa = ttt.name.Substring(indexa + 1, ttt.name.Length - indexa - 2);
                                string line2a = tmpa.Split('-')[0];
                                string line3a = tmpa.Split('-')[1];
                                if (line1a.Contains(line1) && line2a.Contains(line2) && line3a.Contains(line3))
                                {
                                    ln = ttt;
                                    break;
                                }
                            }

                            if (ln != null)
                            {
                                string stastr = ln.stats;
                                string xys = ln.stat_xys;
                                string[] sr1 = stastr.Split(';');
                                string[] sr2 = xys.Split(';');

                                for (int i = 0; i < sr1.Length; i++)
                                {
                                    STATIONS_station ss = new STATIONS_station();
                                    ss.name = sr1[i];
                                    ss.lat = sr2[i].Split(',')[0];
                                    ss.lng = sr2[i].Split(',')[1];
                                    tmpStations1.Add(ss);
                                }

                                if (tmpStations1.Count > 0)
                                {
                                    BuildStationData(tmpStations1);
                                    getNearName();
                                    WriteStations(lineName, stastr, xys, ln.info); //写入数据库
                                    progressBar.Hide();
                                    return;
                                }
                            }
                        }
                        else
                        {
                            //if (jsn.Contains("访问数超过限制"))
                                MessageBox.Show("访问次数超过当日限制", "爱帮API错误", MessageBoxButton.OK);
                            //else
                            //    MessageBox.Show(jsn, "爱帮API错误", MessageBoxButton.OK);
                            NavigationService.GoBack();
                        }
                    });
                }
                else
                {
                    foreach (string s in jsonstr.Split(';'))
                    {
                        STATIONS_station ss = new STATIONS_station();
                        string[] st = s.Split(',');
                        ss.name = st[0];
                        ss.lat = st[1];
                        ss.lng = st[2];
                        tmpStations1.Add(ss);
                    }
                    if (tmpStations1.Count > 0)
                    {
                        BuildStationData(tmpStations1);
                        getNearName();
                        return;
                    }
                }
            }
            else
            {
                BuildStationData(tmpStations1);
                getNearName();
            }
        }


        void getNearName()
        {
            new ThreadStart(() =>
            {
                if (string.IsNullOrEmpty(GeoService.strBaiduGeo))
                {
                    GeoService.GetGeocoordinate(() =>
                    {
                        getNearStationName();
                    });
                }
                else
                    getNearStationName();
            }).Invoke();
        }

        private void getNearStationName()
        {
            string tmpCoor=GeoConverter.GetString(GeoConverter.GetMarsFromGps(GeoService.Gpscoor.Latitude,GeoService.Gpscoor.Longitude));
            string lat1 = tmpCoor.Split(',')[0];
            string lng1 = tmpCoor.Split(',')[1];

            List<double> tmpDistance = new List<double>();
            for (int i = 0; i < tmpStations1.Count; i++)
            {
                try
                {
                    tmpDistance.Add(GpsDistance.GetDistance(double.Parse(lat1), double.Parse(lng1),
                        double.Parse(tmpStations1[i].lat), double.Parse(tmpStations1[i].lng)));
                }
                catch
                {
                    if (i > 0)
                        tmpDistance.Add(double.Parse(tmpDistance[i - 1].ToString()) + 100D);
                    else
                        tmpDistance.Add(0D);
                }
            }
            int stationIndex = tmpDistance.FindIndex((i) => i == tmpDistance.Min());
            SeleStaName1 = tmpStations1[stationIndex].name;
            SeleStaIndex1 = tmpStations1.FindIndex((s) => s.name == SeleStaName1);
            SeleStaLoc1 = tmpStations1[SeleStaIndex1].lat + "," + tmpStations1[SeleStaIndex1].lng;
            QueryLocation();
        }

        private void buildStationLocationDatas(List<JSONDATA_Location> listLocs)
        {
            int seleIndex = SeleStaIndex1;
            string seleName = SeleStaName1;
            string seleLoc = SeleStaLoc1;
            List<JSONDATA_station> listJDsta = new List<JSONDATA_station>();
            for (int i = 0; i < tmpStations1.Count; i++)
            {
                JSONDATA_station jd = new JSONDATA_station();
                string StaName = tmpStations1[i].name.Trim();
                jd.title = StaName + " " + (i + 1).ToString() + "  ";
                jd.lat = tmpStations1[i].lat;
                jd.lng = tmpStations1[i].lng;

                int stop = 0, run = 0;
                //因为爱帮的数据是下一站。 而车辆行在当前站。
                string nextStationName = StaName;
                if (i < tmpStations1.Count - 1)
                    nextStationName = tmpStations1[i + 1].name.Trim();
                foreach (var v in listLocs)
                {
                    if (nextStationName == v.Station)
                    {
                        if (v.BusStop == "1")
                            stop += 1;
                        else
                            run += 1;
                    }
                }
                string stopimgPath = "/image/stop.png";
                if (i > seleIndex)
                    stopimgPath = "/image/Gray.png";
                StopIMG si = new StopIMG();
                if (stop > 0) si.Fir = stopimgPath;
                if (stop > 1) si.Sec = stopimgPath;
                if (stop > 2) si.Thi = stopimgPath;
                if (stop > 3) si.Fou = stopimgPath;
                if (stop > 4) si.Fif = stopimgPath;
                jd.StopImg = si;
                string runimgPath = "/image/run.png";
                if (i >= seleIndex)
                    runimgPath = "/image/Gray.png";
                RunIMG ri = new RunIMG();
                if (run > 0) ri.Fir = runimgPath;
                if (run > 1) ri.Sec = runimgPath;
                if (run > 2) ri.Thi = runimgPath;
                if (run > 3) ri.Fou = runimgPath;
                if (run > 4) ri.Fif = runimgPath;
                jd.RunImg = ri;

                listJDsta.Add(jd);
            }
            list1.ItemsSource = listJDsta;
            if (showBusDetail)
                spTop.Visibility = System.Windows.Visibility.Visible;
            else
                spTop.Visibility = System.Windows.Visibility.Collapsed;
            progressBar.Hide();
            tb1.Text = seleName + " → " + tmpStations1[tmpStations1.Count - 1].name;
            tb2.Text = "最近车辆:";
            tb3.Text = " 无";
            tbTop.Text = "";
            if (list1.ItemsSource.Count > 0)
            {
                int rewind = int.Parse(AppGlobalVar.valueRewindStation);
                if (listJDsta.Count > 0)
                {
                    for (int i = 0; i < listJDsta.Count; i++)
                    {
                        if (!(string.IsNullOrEmpty(seleName)) && listJDsta[i].title.Contains(seleName + " "))
                        {
                            //tb1.Text = seleName + " → " + tmpStations1[tmpStations1.Count - 1].name;
                            ////tb2.Text = SeleStaName1 + " (点击站名切换)";
                            int tmpIndex = (i - rewind) < 0 ? 0 : (i - rewind);
                            tmpIndex = seleIndex < 0 ? tmpIndex : (seleIndex - rewind < 0 ? 0 : seleIndex - rewind);
                            if (!string.IsNullOrEmpty(seleLoc))
                            {
                                List<string> listRewindStaNear = new List<string>();
                                //j=0 or tmepIndex取得多少站车辆
                                for (int j = 0; j <= i; j++)
                                    listRewindStaNear.Add(((JSONDATA_station)list1.ItemsSource[j]).title.Split(string.Empty.ToCharArray())[0]);
                                List<NearBus> nearBus = new List<NearBus>();
                                var tmp = listLocs
                                    .Where((t) => listRewindStaNear.Contains(t.Station))
                                    .ToList<JSONDATA_Location>();
                                foreach (var t in tmp)
                                {
                                    //System.Diagnostics.Debug.WriteLine("当前坐标：" + seleLoc + "; 站名："+t.Station+"; 站点坐标：" + t.lat + "," + t.lng);
                                    NearBus nb = new NearBus();
                                    nb.BusStop = t.BusStop;
                                    nb.CarNO = t.CarNO;
                                    nb.Station = t.Station;
                                    nb.staIndex = tmpStations1.FindIndex((s) => s.name == nb.Station);
                                    nb.Distance = ((int)GpsDistance.GetDistance(double.Parse(t.lat), double.Parse(t.lng),
                                        double.Parse(seleLoc.Split(',')[0]), double.Parse(seleLoc.Split(',')[1]))).ToString();
                                    if (!nearBus.Contains(nb) && !(nb.BusStop == "0" & nb.staIndex == seleIndex))
                                        nearBus.Add(nb);
                                }
                                nearBus = nearBus.OrderBy((m) => double.Parse(m.Distance)).ToList<NearBus>();
                                tb2.Text = "最近车辆:";
                                tb3.Text = " 无";
                                tbTop.Text = "";
                                if (nearBus.Count > 0)
                                {
                                    int ZeroValue = Math.Abs(seleIndex - nearBus[0].staIndex);
                                    if (ZeroValue == 0 && nearBus[0].BusStop == "1")
                                        tb3.Text = nearBus[0].CarNO + " 已到站！";
                                    else
                                        tb3.Text = nearBus[0].Station + ", " + ZeroValue.ToString() + "站, "
                                        + distance2time(nearBus[0].Distance, ZeroValue);
                                    if (ZeroValue <= 2 && AppGlobalVar.ShockAlert)
                                        new ThreadStart(() => Microsoft.Devices.VibrateController.Default.Start(new TimeSpan(0, 0, 1))).Invoke();
                                    //System.Diagnostics.Debug.WriteLine("seleIndex:" + seleIndex.ToString() 
                                    //    + "\tnearBus:" + nearBus[0].staIndex.ToString()+isSid1.ToString());
                                    busDistanceTimeStr = "";
                                    foreach (var t in nearBus)
                                    {
                                        int Zeros = Math.Abs(seleIndex - t.staIndex);
                                        if (Zeros == 0 && t.BusStop == "1")
                                            busDistanceTimeStr += t.CarNO + "\n已到站！\n\n";
                                        else
                                            busDistanceTimeStr += "NO：" + t.CarNO + "\n"
                                             + t.Station + "\n"
                                            + m2Km(t.Distance) + ", "
                                            + distance2time(t.Distance, Zeros) + "\n\n";
                                    }
                                    if (busDistanceTimeStr != "")
                                        tbTop.Text = busDistanceTimeStr;
                                    else
                                        tbTop.Text = "";
                                }
                            }
                            //this.Dispatcher.BeginInvoke(() => list1.ScrollTo(list1.ItemsSource[tmpIndex]));
                            list1.ScrollTo(list1.ItemsSource[tmpIndex]);
                            break;
                        }
                    }
                }
                else
                    list1.ScrollTo(list1.ItemsSource[seleIndex]);
            }            
        }


        void LongListSelectorTap(TextBlock tb)
        {
            for (int i = 0; i < list1.ItemsSource.Count; i++)
            {
                if (((JSONDATA_station)list1.ItemsSource[i]).title.Contains(tb.Text.Split(string.Empty.ToCharArray())[0]))
                {
                    SeleStaIndex1 = i;
                    SeleStaLoc1 = ((JSONDATA_station)list1.ItemsSource[i]).lat + "," + ((JSONDATA_station)list1.ItemsSource[i]).lng;
                    SeleStaName1 = ((JSONDATA_station)list1.ItemsSource[i]).title.Split(string.Empty.ToCharArray())[0];
                    break; ;
                }
            }
            QueryLocation();
        }

        private void list1_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(TextBlock))
            {
                LongListSelectorTap((TextBlock)e.OriginalSource);
            }
        }

        int getTileQty()
        {
            return ShellTile.ActiveTiles.Count(n => n.NavigationUri.ToString().Contains("s=")) + 1;
        }

        private void abPin_Click(object sender, EventArgs e)
        {
            if (isFromTile)
                App.Current.Terminate();
            else
            {
                MyTile.CreateOrUpdate("/page/BusRunAiBang.xaml?s=" + getTileQty() + "&lineNameDetail="
                                    + lineNameDetail
                                    + "&lineName=" + lineName
                                    + "&Sid=" + Sid1,
                                    lineName, "0", lineName, lineNameDetail, "/image/ico.png", "/image/100.png");
                ((ApplicationBarIconButton)ApplicationBar.Buttons[3]).IsEnabled = false;
            }
        }

        private void abBack_Click(object sender, EventArgs e)
        {
            if (isFromTile)
            {
                NavigationService.Navigate(new Uri("/page/MainPage.xaml", UriKind.RelativeOrAbsolute));
                this.NavigationService.RemoveBackEntry();
            }
            else
                NavigationService.GoBack();
        }

        private void abHuancheng_Click(object sender, EventArgs e)
        {
            QueryLocation();
        }

        private void abRefresh_Click(object sender, EventArgs e)
        {
            //BackgroundAudioPlayer.Instance.Play();
            QueryLocation();
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("自动定位到最近的站台。点击站台名称, 切换候车站台！☺",
                ((ApplicationBarMenuItem)sender).Text, MessageBoxButton.OK);
        }

        /// <summary>
        /// 用距离(米)估算时间,深圳公交的速度每秒6米差不多了,不能再快了.
        /// </summary>
        /// <param name="m">单位米</param>
        /// <returns>小时,不足60返回秒,不足3600返回分钟</returns>
        private string distance2time(string m, int betweenStaQty)
        {
            try
            {
                //System.Diagnostics.Debug.WriteLine(m + "米," + betweenStaQty.ToString());
                int addStart = 0;
                if (betweenStaQty > 0)
                    addStart = (betweenStaQty - 1) * 60;
                float i = float.Parse(m) / 6F + addStart;
                if (i < 60)
                    return ((int)(i)).ToString() + "秒";
                else if (i >= 60 && i < 3600)
                    return ((int)Math.Round(i / 60)).ToString() + "分钟";
                else
                    return ((int)(i) / 3600).ToString() + "小时"
                        + (i % 3600 < 60 ? "" : ((int)Math.Round(((i % 3600) / 60))).ToString() + "分");
            }
            catch
            {
                return " 分钟";
            }
        }

        /// <summary>
        /// 米转成公里
        /// </summary>
        /// <param name="m">单位米</param>
        /// <returns>公里,不足1公里返回米</returns>
        private string m2Km(string m)
        {
            try
            {
                float i = float.Parse(m);
                if (i > 1000)
                    return ((int)Math.Round(i / 1000F)).ToString() + "公里";
                else
                    return ((int)(decimal.Parse(m))).ToString() + "米";
            }
            catch
            {
                return " 米";
            }
        }


        private void ApplicationBarMenuItem10_Click(object sender, EventArgs e)
        {
            if (((ApplicationBarMenuItem)sender).Text == "每10秒自动刷新车辆")
            {
                ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = false;
                ((ApplicationBarMenuItem)ApplicationBar.MenuItems[1]).IsEnabled = false;
                t1.Interval = new TimeSpan(0, 0, 10);
                t1.Start();
                ((ApplicationBarMenuItem)sender).Text = "停止自动刷新";
            }
            else
            {
                ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = true;
                ((ApplicationBarMenuItem)ApplicationBar.MenuItems[1]).IsEnabled = true;
                t1.Interval = new TimeSpan(0, 0, 9999);
                t1.Stop();
                ((ApplicationBarMenuItem)sender).Text = "每10秒自动刷新车辆";
            }
        }

        bool showBusDetail = true;
        private void ApplicationBarMenuItem60_Click(object sender, EventArgs e)
        {
            showBusDetail = !showBusDetail;
            if (showBusDetail)
                spTop.Visibility = System.Windows.Visibility.Visible;
            else
                spTop.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void list_ItemRealized(object sender, ItemRealizationEventArgs e)
        {
            int seleIndex = SeleStaIndex1;

            List<TextBlock> lbs1 = VisTreeHelp.GetChildObjects<TextBlock>(e.Container.GetVisualParent());
            List<Ellipse> Ellipse1 = VisTreeHelp.GetChildObjects<Ellipse>(e.Container.GetVisualParent());
            //List<Rectangle> Rectangle1 = VisTreeHelp.GetChildObjects<Rectangle>(e.Container.GetVisualParent());
            for (int i = 0; i < lbs1.Count; i++)
            {
                int index = int.Parse(lbs1[i].Text.Split(string.Empty.ToCharArray())[1].Trim()) - 1;
                if (index < seleIndex)
                {
                    lbs1[i].Foreground = scbContent;
                    lbs1[i].FontWeight = FontWeights.Normal;
                    Ellipse1[i].Stroke = scbContent;
                }
                else if (index == seleIndex)
                {
                    lbs1[i].Foreground = scbRed;
                    lbs1[i].FontWeight = FontWeights.ExtraBlack;
                    Ellipse1[i].Stroke = scbRed;
                }
                else if (index > seleIndex)
                {
                    lbs1[i].Foreground = scbTitle;
                    lbs1[i].FontWeight = FontWeights.Normal;
                    Ellipse1[i].Stroke = scbTitle;
                }
                //Rectangle1[i].Fill = lbs[i].Foreground;
            }
            lbs1.Clear();
            Ellipse1.Clear(); //Rectangle1.Clear();
        }

        #region 数据库实体的操作，路线站点的本地数据库缓存

        private string ReadStations(string SID)
        {
            string ret = "";
            using (db.DBContext database = new db.DBContext())
            {
                if (database.BusLines_ab.Count() > 0)
                {
                    var tmp = database.BusLines_ab.Where((s) => s.ID == SID);
                    if (tmp.Count() == 1)
                        ret = tmp.ToList()[0].Stations;
                }
            }
            return ret;
        }

        private void WriteStations(string linename, string stats, string stat_xys,string info)
        {
            using (db.DBContext database = new db.DBContext())
            {
                db.BusLines_ab sta = database.BusLines_ab.First((s) => s.Line_Name == linename);
                StringBuilder sb = new StringBuilder();
                string[] stastr = stats.Split(';');
                string[] xys = stat_xys.Split(';');
                for (int i = 0; i < stastr.Length; i++)
                    sb.Append(stastr[i] + "," + xys[i] + ";");
                sta.Stations = sb.ToString().Substring(0, sb.Length - 1);
                sta.Info = info;
                database.SubmitChanges();
            }
        }

        private void ApplicationBarMenuItem11_Click(object sender, EventArgs e)
        {
            ApplicationBarMenuItem mi = (ApplicationBarMenuItem)sender;
            if (mi.Text == "取消振动提醒")
            {
                IsoStorageSetting.Write(AppGlobalVar.keyShockAlert, "0");
                AppGlobalVar.ShockAlert = false;
                ((ApplicationBarMenuItem)sender).Text = "开启振动提醒";
            }
            else
            {
                IsoStorageSetting.Write(AppGlobalVar.keyShockAlert, "1");
                AppGlobalVar.ShockAlert = true;
                ((ApplicationBarMenuItem)sender).Text = "取消振动提醒";
            }
        }
        //public event PropertyChangedEventHandler PropertyChanged;
        //// Used to notify the app that a property has changed.
        //private void NotifyPropertyChanged(string propertyName)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}

        #endregion
    }
}