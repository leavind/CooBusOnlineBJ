using System;
using System.Windows;
using Microsoft.Phone.Controls;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows.Navigation;
using System.Linq;
using BusClass;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Phone.Shell;
using System.ComponentModel;
using Microsoft.Phone.Tasks;
using System.Net;
using SharpGIS;

namespace RealTimeBusBJ
{
    public partial class InputLine : PhoneApplicationPage
    {
        public InputLine()
        {
            InitializeComponent();
        }

        private int LoadCount = 0;
        private List<DATA_lineAndAB> tmpLines;
        private int dbRowCount = 0;
        
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (LoadCount == 0)
            {
                tb1.Focus();
                LoadCount += 1;
                dbRowCount = GetDBRowCount();
                if (dbRowCount == 0)
                    FreshDB(false);
            }
        }

        //void QueryClick()
        //{
        //    if (tb1.Text == "请输入公交路线" || tb1.Text == "")
        //    {
        //        tb1.Focus();
        //        return;
        //    }

        //    list1.ItemsSource = null;
        //    List<JSONDATA_lineAndAB> listJDline = new List<JSONDATA_lineAndAB>();
        //    try
        //    {
        //        string busName = tb1.Text.Trim().ToUpper();
        //        string busStarter = busName.Substring(0, 1);
        //        QueryLineName(busName, listJDline);
        //        QueryLineName(busName + "A", listJDline);
        //        QueryLineName(busName + "B", listJDline);
        //        int starter = 0;
        //        if (int.TryParse(busStarter, out starter))
        //        {
        //            QueryLineName("M" + busName, listJDline);
        //            QueryLineName("M" + busName + "A", listJDline);
        //            QueryLineName("M" + busName + "B", listJDline);

        //            QueryLineName("E" + busName, listJDline);
        //            QueryLineName("E" + busName + "A", listJDline);
        //            QueryLineName("E" + busName + "B", listJDline);

        //            QueryLineName("K" + busName, listJDline);
        //            QueryLineName("K" + busName + "A", listJDline);
        //            QueryLineName("K" + busName + "B", listJDline);

        //            QueryLineName("B" + busName, listJDline);
        //            QueryLineName("B" + busName + "A", listJDline);
        //            QueryLineName("B" + busName + "B", listJDline);

        //            QueryLineName("机场" + busName, listJDline);
        //            QueryLineName("机场" + busName + "A", listJDline);
        //            QueryLineName("机场" + busName + "B", listJDline);
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        MessageBox.Show(ee.ToString());
        //    }
        //}

        //private void QueryLineName(string lineName, List<JSONDATA_lineAndAB> listJDlines)
        //{
        //    progressBar.Show();
        //    var client = new RestClient(BusAPI.lineNameAPI + lineName.ToUpper().Replace("-M", "-").Replace("-K", "-").Replace("-B", "-"));
        //    client.ExecuteAsync(new RestRequest(Method.GET), (response) =>
        //    {
        //        string jsn = response.Content;
        //        if (jsn.Contains("data") && !jsn.Contains("[]"))
        //        {
        //            JSONSTR_line jsr = (JSONSTR_line)JsonConvert.DeserializeObject(jsn, typeof(JSONSTR_line));
        //            if (jsr.success == "true")
        //            {
        //                List<DATA_lineAndAB> tempLines = new List<DATA_lineAndAB>();
        //                tempLines.AddRange(jsr.data);
        //                if (tempLines.Count > 0)
        //                {
        //                    List<string> lineNameStr = tempLines.Select((st) => st.line_name).Distinct().ToList();
        //                    foreach (var ln in lineNameStr)
        //                    {
        //                        var tmp = tempLines.Where((t) => t.line_name == ln).OrderBy((k) => k.line_name).ToList();
        //                        if (tmp != null)
        //                        {
        //                            JSONDATA_lineAndAB jd = new JSONDATA_lineAndAB();
        //                            jd.title = (tmp[0].line_name + "路\n" + tmp[0].start_station + "→"
        //                                + tmp[0].end_station).Replace("路路", "路").Replace("线路", "路");
        //                            jd.isopen = tmp[0].isopen;
        //                            if (jd.isopen == "0")
        //                                jd.title += "\n***暂未开通实时公交***";
        //                            jd.detail = "发车时间:" + tmp[0].begin_time + "→"
        //                                + tmp[0].end_time + "; 票价:" + (tmp[0].price == "0" ? " " : tmp[0].price) + "元";
        //                            jd.sid = tmp[0].id;
        //                            jd.sid2 = "";
        //                            if (tmp.Count == 2)
        //                            {
        //                                jd.title = jd.title.Replace("→", "⇆");
        //                                jd.detail = jd.detail.Replace("→", "⇆");
        //                                jd.sid2 = tmp[1].id;
        //                            }

        //                            bool have = false;
        //                            foreach (var j in listJDlines)
        //                            {
        //                                if (jd.title == j.title)
        //                                {
        //                                    have = true;
        //                                    break;
        //                                }
        //                            }

        //                            if (!have)
        //                                listJDlines.Add(jd);
        //                            if (listJDlines.Count > 0)
        //                            {
        //                                var query = from a in listJDlines
        //                                            orderby a.isopen descending, a.title ascending
        //                                            select a;
        //                                list1.ItemsSource = query.ToList<JSONDATA_lineAndAB>();
        //                                pageTitle.Text = "公交在线 (匹配路线: " + query.Count().ToString() + " 条)";
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        progressBar.Hide();
        //    });
        //}

        private void QueryFromDB()
        {
            if (tb1.Text == "请输入公交路线" || tb1.Text == "")
            {
                pageTitle.Text = "公交在线";
                tb1.Focus();
                return;
            }
            SelectByLineName(tb1.Text.Trim());
            if (tmpLines.Count > 0)
            {
                List<string> lineNameStr = tmpLines.Select((st) => st.line_name).Distinct().ToList();
                List<JSONDATA_line> jds = new List<JSONDATA_line>();
                foreach (var ln in lineNameStr)
                {
                    var tmp = tmpLines.Where((t) => t.line_name == ln).OrderBy((k) => k.line_name).ToList();
                    if (tmp != null)
                    {
                        JSONDATA_line jd = new JSONDATA_line();
                        if (tmp[0].aibanginfo == "-1")
                        {
                            jd.title = (tmp[0].line_name + "路\n" + tmp[0].start_station + "→"
                                + tmp[0].end_station).Replace("路路", "路").Replace("线路", "路");
                            jd.isopen = tmp[0].isopen;
                            if (jd.isopen == "0")
                                jd.title += "\n***暂未开通实时公交***";
                            jd.detail = "发车时间:" + tmp[0].begin_time + "→"
                                + tmp[0].end_time + "; 票价:" + (tmp[0].price == "0" ? " " : tmp[0].price) + "元";
                            jd.sid = tmp[0].id;
                            jd.sid2 = "";
                            if (tmp.Count == 2)
                            {
                                jd.title = jd.title.Replace("→", "⇆");
                                jd.detail = jd.detail.Replace("→", "⇆");
                                jd.sid2 = tmp[1].id;
                            }
                        }
                        else
                        {
                            jd.title = tmp[0].line_name;
                            jd.sid = tmp[0].id;
                            jd.detail = tmp[0].aibanginfo;
                        }

                        bool have = false;
                        foreach (var j in jds)
                        {
                            if (jd.title == j.title)
                            {
                                have = true;
                                break;
                            }
                        }

                        if (!have)
                            jds.Add(jd);
                        if (jds.Count > 0)
                        {
                            var query = from a in jds
                                        orderby a.isopen descending, a.title ascending
                                        select a;
                            list1.ItemsSource = query.ToList<JSONDATA_line>();
                            pageTitle.Text = "公交在线 (匹配路线: " + query.Count().ToString() + " 条)";
                        }
                    }
                }
            }
        }

        private void QueryEid(string isOpen)
        {
            if (isOpen == "1")
            {
                var client = new RestClient(BusAPI.lineSidAPI + BusRun.lineName.Replace("路", ""));
                client.ExecuteAsync(new RestRequest(Method.GET), (response) =>
                {
                    string jsn = response.Content;
                    if (jsn.Contains("data") && !jsn.Contains("[]"))
                    {
                        JSONSTR_Eid jsr = (JSONSTR_Eid)JsonConvert.DeserializeObject(jsn, typeof(JSONSTR_Eid));
                        BusRun.Eid = jsr.data[0].eid;
                        NavigationService.Navigate(new Uri("/page/BusRun.xaml", UriKind.RelativeOrAbsolute));
                    }
                });
            }
            else
            {
                BusRun.Eid = "";
                NavigationService.Navigate(new Uri("/page/BusRun.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        private void tb1_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tb1.Text == "请输入公交路线")
                tb1.Text = "";
            else
                tb1.SelectAll();
        }

        private void tb1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tb1.Text.Trim().Length == 0)
            {
                tb1.Text = "请输入公交路线";
            }
        }

        private void reFresh()
        {
            try
            {
                JSONDATA_line JL = ((JSONDATA_line)list1.SelectedItem);                
                string QuerySID = JL.sid;
                if (QuerySID.Length > 0)
                {
                    if (JL.title.Contains("→") || JL.title.Contains("⇆"))
                    {
                        BusRun.Sid1 = QuerySID;
                        BusRun.Sid2 = JL.sid2;
                        BusRun.lineNameDetail = JL.title;
                        BusRun.lineName = JL.title.Split('\n')[0].Trim();
                        BusRun.isOpen = JL.isopen;
                        //NavigationService.Navigate(new Uri("/BusRun.xaml", UriKind.RelativeOrAbsolute));
                        QueryEid(JL.isopen);
                    }
                    else
                    {
                        BusRunAiBang.Sid1 = QuerySID;
                        BusRunAiBang.lineName = JL.title;
                        BusRunAiBang.lineNameDetail = JL.detail;
                        NavigationService.Navigate(new Uri("/page/BusRunAiBang.xaml", UriKind.RelativeOrAbsolute));
                    }
                }
            }
            catch (WebBrowserNavigationException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }


        //private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        //{
        //    tb1.Text = ((System.Windows.Controls.TextBlock)sender).Text;
        //    QueryClick();
        //}

        private void list1_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(TextBlock))
            {
                reFresh();
            }
        }

        private void abBack_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }


        private void abInput_Click(object sender, EventArgs e)
        {
            this.Focus();
            ApplicationBarIconButton appbtn = this.ApplicationBar.Buttons[2] as ApplicationBarIconButton;
            if (appbtn.Text == "123")
            {
                appbtn.Text = "Abc";
                InputScope ips = new InputScope();
                InputScopeName ipn = new InputScopeName();
                ipn.NameValue = InputScopeNameValue.Text;
                ips.Names.Add(ipn);
                tb1.InputScope = ips;
            }
            else
            {
                appbtn.Text = "123";
                InputScope ips = new InputScope();
                InputScopeName ipn = new InputScopeName();
                ipn.NameValue = InputScopeNameValue.Digits;
                ips.Names.Add(ipn);
                tb1.InputScope = ips;
            }
            tb1.Focus();
        }

        private void abQuery_Click(object sender, EventArgs e)
        {
            this.Focus();
            if (tb1.Text.Trim().Length > 0)
            {
                QueryFromDB();
                if (tmpLines.Count == 0)
                {
                    //查不到，是否在线查询 http://wap.8684.cn/search.php?k=pp&q=观光
                    if (MessageBox.Show("此线路未开通实时公交，是否从网站查看途经站点？", "", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        WebBrowserTask ie = new WebBrowserTask();
                        //ie.Uri = new Uri("http://wap.8684.cn/search.php?k=pp&q=" + tb1.Text, UriKind.Absolute);
                        ie.Uri = new UriBuilder(new Uri("http://wap.8684.cn/search.php?k=pp&q=" + tb1.Text, UriKind.Absolute)).Uri;
                        ie.Show();
                    }
                }
                //QueryClick();
            }
        }

        #region 数据库实体的操作，所有的线路存入本地数据库
        int totalQty = 0, OpenQty = 0;
        private void FreshDB(bool showmsg)
        {
            progressBar.Show();
            this.Focus();
            tb1.Text = "正在更新酷米客线路...";
            totalQty = 0;
            OpenQty = 0;
            var client = new RestClient(BusAPI.lineNameAPI); //酷米客路线
            client.ExecuteAsync(new RestRequest(Method.GET), (response) =>
            {
                string jsn = response.Content;
                if (jsn.Contains("data") && !jsn.Contains("[]"))
                {
                    JSONSTR_line jsr = (JSONSTR_line)JsonConvert.DeserializeObject(jsn, typeof(JSONSTR_line));
                    if (jsr.success == "true")
                    {
                        using (db.DBContext database = new db.DBContext())
                        {
                            database.BusLines.DeleteAllOnSubmit(database.BusLines.ToList());
                            database.DBmsg.DeleteAllOnSubmit(database.DBmsg.ToList());
                            database.SubmitChanges();
                            List<db.BusLines> bs = new List<db.BusLines>();
                            foreach (var d in jsr.data)
                            {
                                db.BusLines bl = new db.BusLines();
                                bl.Begin_Time = d.begin_time;
                                bl.Dir = d.dir;
                                bl.End_Station = d.end_station;
                                bl.End_Time = d.end_time;
                                bl.ID = d.id;
                                bl.IsOpen = d.isopen;
                                bl.Line_Name = d.line_name;
                                bl.Price = d.price;
                                bl.Start_Station = d.start_station;
                                bs.Add(bl);
                            }
                            database.BusLines.InsertAllOnSubmit(bs);
                            database.SubmitChanges();
                            dbRowCount = GetDBRowCount();
                            totalQty += database.BusLines.Select((t) => t.Line_Name).Distinct().Count();
                            OpenQty += database.BusLines.Where((t) => t.IsOpen == "1").Select((t) => t.Line_Name).Distinct().Count();
                        }
                    }
                }

                //var client1 = new RestClient(BusAPI_ab.busLinesTxt); //爱帮北京路线
                //client1.ExecuteAsync(new RestRequest(Method.GET), (response1) =>
                //{
                //    string tmpstr = response1.Content;
                //    if (tmpstr.Contains("【start】") && tmpstr.Contains("【end】"))
                //    {
                //        tmpstr=tmpstr.Replace("【start】", "").Replace("【end】", "").Trim();
                //        using (db.DBContext database = new db.DBContext())
                //        {
                //            database.BusLines_ab.DeleteAllOnSubmit(database.BusLines_ab.ToList());
                //            database.SubmitChanges();
                //            List<db.BusLines_ab> bs = new List<db.BusLines_ab>();
                //            foreach (var t in tmpstr.Split('\n'))
                //            {
                //                if (t.Trim().Length > 0)
                //                {
                //                    db.BusLines_ab bl = new db.BusLines_ab();
                //                    string[] sss = t.Trim().Split(',');
                //                    bl.Line_Name = sss[0];
                //                    bl.ID = sss[1];
                //                    bs.Add(bl);
                //                }
                //            }
                //            database.BusLines_ab.InsertAllOnSubmit(bs);
                //            db.DBmsg dm = new db.DBmsg();
                //            dm.ID = "1";
                //            dm.Strings = "DBTime";
                //            dm.Values = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //            database.DBmsg.InsertOnSubmit(dm);
                //            database.SubmitChanges();
                //            dbRowCount = GetDBRowCount();
                //            totalQty += bs.Count / 2;
                //            OpenQty += bs.Count / 2;
                //        }
                //        progressBar.Hide();
                //        if (showmsg)
                //            MessageBox.Show("共有线路:\t\t" + totalQty.ToString() + "\n开通实时公交:\t\t" + OpenQty.ToString(),
                //                "离线数据已更新", MessageBoxButton.OK);
                //    }
                //});
                showmsg2 = showmsg;
                tb1.Text = "正在更新北京交委线路...";
                //wc.DownloadStringAsync(new Uri(BusAPI_ab.busLinesTxtFromSZIDWELL)); 
                GZipWebClient gwc = new GZipWebClient();
                gwc.DownloadStringCompleted += gwc_DownloadStringCompleted;
                gwc.DownloadStringAsync(new Uri("http://www.szidwell.com/busline/aibangLines.txt"));
            });
        }

        bool showmsg2 = false;
        void gwc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                string result = (string)e.Result;
                if (result.Contains("【start】") && result.Contains("【end】"))
                {
                    result = result.Replace("【start】", "").Replace("【end】", "").Trim();
                    using (db.DBContext database = new db.DBContext())
                    {
                        database.BusLines_ab.DeleteAllOnSubmit(database.BusLines_ab.ToList());
                        database.SubmitChanges();
                        List<db.BusLines_ab> bs = new List<db.BusLines_ab>();
                        foreach (var t in result.Split('\n'))
                        {
                            if (t.Trim().Length > 0)
                            {
                                db.BusLines_ab bl = new db.BusLines_ab();
                                string[] sss = t.Trim().Split('ÿ');
                                bl.ID = sss[0];
                                bl.Line_Name = sss[1];
                                bl.Stations = sss[2];
                                bl.Info = sss[3];
                                bs.Add(bl);
                            }
                        }
                        //foreach (var t in result.Split('\n'))
                        //{
                        //    if (t.Trim().Length > 0)
                        //    {
                        //        db.BusLines_ab bl = new db.BusLines_ab();
                        //        string[] sss = t.Trim().Split(',');
                        //        bl.Line_Name = sss[0];
                        //        bl.ID = sss[1];
                        //        bs.Add(bl);
                        //    }
                        //}
                        database.BusLines_ab.InsertAllOnSubmit(bs);
                        db.DBmsg dm = new db.DBmsg();
                        dm.ID = "1";
                        dm.Strings = "DBTime";
                        dm.Values = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        database.DBmsg.InsertOnSubmit(dm);
                        database.SubmitChanges();
                        dbRowCount += bs.Count;
                        totalQty += bs.Count / 2;
                        OpenQty += bs.Count / 2;
                    }
                    progressBar.Hide();
                    if (showmsg2)
                        MessageBox.Show("共有线路:\t\t" + totalQty.ToString() + "\n开通实时公交:\t\t" + OpenQty.ToString(),
                            "离线数据已更新", MessageBoxButton.OK);
                    tb1.Text = "";
                    tb1.Focus();
                }
            }
        }


        private void SelectByLineName(string lineName)
        {
            db.DBContext database = new db.DBContext();
            tmpLines = new List<DATA_lineAndAB>();
            //酷米客路线
            var tmp = database.BusLines.Where((b) => b.Line_Name.Contains(lineName));
            foreach (var d in tmp)
            {
                DATA_lineAndAB dl = new DATA_lineAndAB();
                dl.begin_time = d.Begin_Time;
                dl.dir = d.Dir;
                dl.end_station = d.End_Station;
                dl.end_time = d.End_Time;
                dl.id = d.ID;
                dl.isopen = d.IsOpen;
                dl.line_name = d.Line_Name;
                dl.price = d.Price;
                dl.start_station = d.Start_Station;
                dl.aibanginfo = "-1";
                tmpLines.Add(dl);
            }

            //爱帮路线
            var tmp2 = database.BusLines_ab.Where((b) => b.Line_Name.Contains(lineName));
            foreach (var d in tmp2)
            {
                DATA_lineAndAB dl = new DATA_lineAndAB();
                dl.id = d.ID;
                dl.line_name = d.Line_Name;
                dl.aibanginfo = d.Info;
                tmpLines.Add(dl);
            }
        }

        private void SelectByLineID(string lineID)
        {
            db.DBContext database = new db.DBContext();
            tmpLines.Clear();
            var tmp = database.BusLines.Where((b) => b.Line_Name == lineID);
            foreach (var d in tmp)
            {
                DATA_lineAndAB dl = new DATA_lineAndAB();
                dl.begin_time = d.Begin_Time;
                dl.dir = d.Dir;
                dl.end_station = d.End_Station;
                dl.end_time = d.End_Time;
                dl.id = d.ID;
                dl.isopen = d.IsOpen;
                dl.line_name = d.Line_Name;
                dl.price = d.Price;
                dl.start_station = d.Start_Station;
                tmpLines.Add(dl);
            }
        }

        private int GetDBRowCount()
        {
            db.DBContext database = new db.DBContext();
            return database.BusLines.Count() + database.BusLines_ab.Count();       
        }

        void ShowDatabaseMsg(string title)
        {
            int totalQty = 0, OpenQty = 0;
            string dtime = "\t无数据";
            using (db.DBContext database = new db.DBContext())
            {
                totalQty = database.BusLines.Select((t) => t.Line_Name).Distinct().Count();
                OpenQty = database.BusLines.Where((t) => t.IsOpen == "1").Select((t) => t.Line_Name).Distinct().Count();
                totalQty += database.BusLines_ab.Count() / 2;
                OpenQty += database.BusLines_ab.Count() / 2;
                List<string> str = database.DBmsg.Select((t) => t.Values).ToList();
                if (str.Count > 0)
                    dtime = str[0];
            }
            MessageBox.Show("更新时间:\t" + dtime + "\n共有线路:\t\t" + totalQty.ToString()
                + "\n开通实时公交:\t\t" + OpenQty.ToString(),
                title, MessageBoxButton.OK);
        }

        #endregion



        private void ab2_Click(object sender, EventArgs e)
        {
            FreshDB(true);
        }

        private void ab3_Click(object sender, EventArgs e)
        {
            ShowDatabaseMsg("离线数据详情");
        }

        private void tb1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (dbRowCount > 0 && tb1.Text != "请输入公交路线" && tb1.Text.Trim().Length > 0)
            {
                QueryFromDB();
            }
            else
            {
                pageTitle.Text = "公交在线";
                //tb1.Focus();
                return;
            }
        }

        private void ab4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("酷米客数据自动从酷米客官网更新。"
                + "\n\n北京交委的数据由开发者手工整理，若有更新，请及时通知开发者(参照IOS、安卓官方版“北京实时公交”)。"
                + "\n\n共需要下载数据约300KB，GSM/2G网络可能需要1分钟，请耐心等待。"
                + "\n\n对比大兴线的酷米客数据，北京交委的数据质量极差，闪退请反馈给开发者具体的线路、站点!"
                + "\n\n诚意之作，请在“确定”后打分鼓励开发者!☺",
             ((ApplicationBarMenuItem)sender).Text, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                MarketplaceReviewTask mrTask = new MarketplaceReviewTask();
                mrTask.Show();
            }
        }

        private void abRecord_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/page/Favorite.xaml", UriKind.RelativeOrAbsolute));
            this.NavigationService.RemoveBackEntry();
        }
    }
}