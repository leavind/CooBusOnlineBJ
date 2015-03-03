using System.Windows;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using System;
using RestSharp;
using Newtonsoft.Json;
using System.Windows.Navigation;
using System.Linq;
using BusClass;
using System.Windows.Controls;

namespace RealTimeBusBJ
{
    public partial class seleLineList : PhoneApplicationPage
    {
        public seleLineList()
        {
            InitializeComponent();
        }
        private int LoadCount = 0;
        string[] lines;

        //是否从路线规划打开页面的
        public static bool isFromPlan = false;
        public static string stationName;
        public static string linespit;

        List<JSONDATA_line> listJDline = new List<JSONDATA_line>();
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (LoadCount == 0)
            {
                if (!isFromPlan)
                {
                    //将选择的站台位置写入收藏
                    string tmpLoc = BaiduVar.selePlace.name + ":" + BaiduVar.selePlace.location.lat + "," 
                        + BaiduVar.selePlace.location.lng;
                    if (!IsoStorage.ReadLocation().Contains(tmpLoc))
                        IsoStorage.WriteLocation(tmpLoc + ";");
                    tb_LocName.Text = "站台: " + BaiduVar.selePlace.name;
                    lines = BaiduVar.selePlace.address.Replace("路", "").Replace("线", "").Split(';');                    
                }
                else
                { 
                    tb_LocName.Text = "站台: " + stationName;
                    lines = linespit.Replace("路", "").Replace("线", "").Split(';');
                }
                Query();
                LoadCount += 1;
            }
        }

        void Query()
        {
            list1.ItemsSource = null;
            listJDline.Clear();
            try
            {
                foreach(var busName in lines)
                    QueryLineName(busName);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void QueryLineName(string lineName)
        {
            progressBar.Show();
            var client = new RestClient(BusAPI.lineNameAPI + lineName.ToUpper().Replace("-M", "-").Replace("-K", "-").Replace("-B", "-"));
            client.ExecuteAsync(new RestRequest(Method.GET), (response) =>
            {
                string jsn = response.Content;
                if (jsn.Contains("data") && !jsn.Contains("[]"))
                {
                    JSONSTR_line jsr = (JSONSTR_line)JsonConvert.DeserializeObject(jsn, typeof(JSONSTR_line));
                    if (jsr.success == "true")
                    {        
                        JSONDATA_line jd = new JSONDATA_line();
                        jd.title = (jsr.data[0].line_name + "路\n" + jsr.data[0].start_station + "→"
                            + jsr.data[0].end_station).Replace("路路", "路").Replace("线路", "路");
                        jd.isopen = jsr.data[0].isopen;
                        if (jd.isopen == "0")
                            jd.title += "\n***暂未开通实时公交***";
                        jd.detail = "发车时间:" + jsr.data[0].begin_time + "→"
                            + jsr.data[0].end_time + "; 票价:" + (jsr.data[0].price == "0" ? " " : jsr.data[0].price) + "元";
                        jd.sid = jsr.data[0].id;
                        jd.sid2 = "";
                        if (jsr.data.Length == 2)
                        {
                            jd.title = jd.title.Replace("→", "⇆");
                            jd.detail = jd.detail.Replace("→", "⇆");
                            jd.sid2 = jsr.data[1].id;
                        }

                        bool have = false;
                        foreach (var j in listJDline)
                        {
                            if (jd.title == j.title)
                            {
                                have = true;
                                break;
                            }
                        }
                        if (!have)
                            listJDline.Add(jd);

                        if (listJDline.Count > 0)
                        {
                            //list1.ItemsSource = null;
                            var query = from a in listJDline
                                        orderby a.isopen descending, a.title ascending
                                        select a;
                            list1.ItemsSource = query.ToList<JSONDATA_line>();
                        }                       
                    }
                }
                progressBar.Hide();
            });
        }

        private void QueryEid(string isOpen)
        {
            if (isOpen=="1")
            {
                var client = new RestClient(BusAPI.lineSidAPI + BusRun.lineName.Replace("路", ""));
                client.ExecuteAsync(new RestRequest(Method.GET), (response) =>
                {
                    string jsn = response.Content;
                    if (jsn.Contains("data") && !jsn.Contains("[]"))
                    {
                        JSONSTR_Eid jsr = (JSONSTR_Eid)JsonConvert.DeserializeObject(jsn, typeof(JSONSTR_Eid));
                        BusRun.Eid = jsr.data[0].eid;
                        NavigationService.Navigate(new Uri("/page/BusRuns.xaml", UriKind.RelativeOrAbsolute));
                    }
                });
            }
            else
            {
                BusRun.Eid = "";
                NavigationService.Navigate(new Uri("/page/BusRuns.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        private void reFresh()
        {
            try
            {
                JSONDATA_line JL = ((JSONDATA_line)list1.SelectedItem);
                string QuerySID = JL.sid;
                if (QuerySID.Length > 1)
                {
                    BusRun.Sid1 = QuerySID;
                    BusRun.Sid2 = JL.sid2;
                    BusRun.lineNameDetail = JL.title;
                    BusRun.lineName = JL.title.Split('\n')[0].Trim();
                    BusRun.isOpen = JL.isopen;
                    //NavigationService.Navigate(new Uri("/BusRuns.xaml", UriKind.RelativeOrAbsolute));
                    QueryEid(JL.isopen);
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

        private void list1_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(TextBlock))
            {
                reFresh();
            }
        }

        private void abRefresh_Click(object sender, EventArgs e)
        {
            Query();
        }

        private void abBack_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}