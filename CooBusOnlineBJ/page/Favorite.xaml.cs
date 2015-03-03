using System;
using System.Windows;
using Microsoft.Phone.Controls;
using RestSharp;
using Newtonsoft.Json;
using BusClass;
using System.Collections.Generic;
using System.Windows.Controls;
using SavedClass;

namespace RealTimeBusBJ
{
    public partial class Favorite : PhoneApplicationPage
    {
        public Favorite()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            refreshLineName();
            refreshLoc();
        }

        void refreshLineName()
        {
            string tmp = IsoStorage.ReadLineName();
            if (tmp.Length > 0)
            {
                List<SavedLineName> listSaveline = new List<SavedLineName>();
                string[] ss = tmp.Split(';');
                for (int i = 0; i < ss.Length; i++)
                {
                    string s = ss[i];
                    if (s.Contains("\n"))
                    {
                        SavedLineName sln = new SavedLineName();
                        sln.savedName = s.Split('\n')[0];
                        sln.savedDetails = s.Split('\n')[1];
                        if (s.Split('\n').Length > 2)
                            sln.savedDetails += "\n" + s.Split('\n')[2];
                        if (s.Split('\n').Length > 3)
                            sln.savedDetails += "\n" + s.Split('\n')[3];
                        sln.tag = i.ToString();
                        listSaveline.Add(sln);
                    }
                }
                list1.ItemsSource = listSaveline;
            }
            else
                list1.ItemsSource = null;
        }

        void refreshLoc()
        {
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
            else
                list2.ItemsSource = null;
        }

        private void QueryLineName(string lineName)
        {
            progressBar.Show();
            var client = new RestClient(BusAPI.lineNameAPI + lineName.ToUpper().Replace("路", ""));
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

                        string QuerySID = jd.sid;
                        if (QuerySID.Length > 1)
                        {
                            BusRun.Sid1 = QuerySID;
                            BusRun.Sid2 = jd.sid2;
                            BusRun.lineNameDetail = jd.title;
                            BusRun.lineName = jd.title.Split('\n')[0].Trim();
                            BusRun.isOpen = jd.isopen;
                        }

                        if (jd.isopen == "1")
                        {
                            var client2 = new RestClient(BusAPI.lineSidAPI + BusRun.lineName.ToUpper().Replace("路", ""));
                            client2.ExecuteAsync(new RestRequest(Method.GET), (response2) =>
                            {
                                string jsn2 = response2.Content;
                                if (jsn2.Contains("data") && !jsn2.Contains("[]"))
                                {
                                    JSONSTR_Eid jsr2 = (JSONSTR_Eid)JsonConvert.DeserializeObject(jsn2, typeof(JSONSTR_Eid));
                                    BusRun.Eid = jsr2.data[0].eid;
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
                }
                progressBar.Hide();
            });
        }

        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(Image))
            {                
                Image img=(Image)e.OriginalSource;
                foreach (SavedLineName i in (List<SavedLineName>)list1.ItemsSource)
                {
                    if (img.Tag.ToString() == i.tag)
                    {
                        IsoStorage.RemoveLineName(i.savedName +"\n"+ i.savedDetails + ";");
                        refreshLineName();
                        break;
                    }
                }
            }
        }

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(TextBlock))
            {
                TextBlock tb = (TextBlock)e.OriginalSource;
                foreach (SavedLineName i in (List<SavedLineName>)list1.ItemsSource)
                {
                    if (tb.Text == i.savedName || tb.Text == i.savedDetails)
                    {
                        QueryLineName(i.savedName);
                        break;
                    }
                }
            }
        }

        private void Image2_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(Image))
            {
                Image img = (Image)e.OriginalSource;
                foreach (SavedLocation i in (List<SavedLocation>)list2.ItemsSource)
                {
                    if (img.Tag.ToString() == i.tag)
                    {
                        IsoStorage.RemoveLocation(i.savedName + ":"+ i.savedBaiduLocGeo + ";");
                        refreshLoc();
                        break;
                    }
                }
            }
        }

        private void TextBlock2_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(TextBlock))
            {
                TextBlock tb = (TextBlock)e.OriginalSource;
                foreach (SavedLocation i in (List<SavedLocation>)list2.ItemsSource)
                {
                    if (tb.Text == i.savedName)
                    {
                        MyNearPage.fromFavorite = true;
                        MyNearPage.favoriteLocName = i.savedName;
                        //MyNearPage.favoriteLocAddress = i.savedAddress;
                        MyNearPage.favoriteBaiduLocGeo = i.savedBaiduLocGeo;          
                        NavigationService.Navigate(new Uri("/page/MyNearPage.xaml", UriKind.RelativeOrAbsolute));
                        break;
                    }
                }
            }
        }

        private void abBack_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}