using System;
using RestSharp;
using Newtonsoft.Json;
using BaiduClass;
using RouteMatrixClass;

public class BaiduAPI
{
    public const string ak = "W6Y7nrTSCyzM3qwPyoXI7x6i";
    /// <summary>
    /// 
    /// </summary>
    public const string cityCode = "131";

    #region place
    public static void placeSearchByGeo(string geoBaidu, string KeyWord, string radiu, Action action)
    {
        try
        {
            string api = "http://api.map.baidu.com/place/v2/search?output=json&page_size=20&page_num=0&ak=" + ak;
            api += "&q=" + KeyWord + "&location=" + geoBaidu + "&radius=" + radiu;
            placeSearch(api, action);
        }
        catch (Exception ee)
        {
            throw ee;
        }
    }

    public static void placeSearchByAdd(string addr, string KeyWord, Action action)
    {
        try
        {
            string api = "http://api.map.baidu.com/place/v2/search?output=json&page_size=20&page_num=0&ak=" + ak;
            api += "&q=" + KeyWord + "&region=" + addr;
            placeSearch(api, action);
        }
        catch (Exception ee)
        {
            throw ee;
        }
    }

    private static int iii = 0;
    static void QueryRepeat5(string querystr,Action action)
    {
        string tmp = querystr.Replace("page_num=0", "page_num="+iii.ToString());
        var client = new RestClient(tmp);
        client.ExecuteAsync(new RestRequest(Method.GET), (response) =>
        {
            string jsn = response.Content.Replace(" ", "");
            if (jsn.Contains("result") && !jsn.Contains("[]"))
            {
                JSONSTR_pla jsr = (JSONSTR_pla)JsonConvert.DeserializeObject(jsn, typeof(JSONSTR_pla));
                if (jsr.status == "0")
                {
                    BaiduVar.NearLines.AddRange(jsr.results);
                    iii += 1;
                    //System.Windows.MessageBox.Show(tmp + "\n" + BaiduVar.NearLines.Count.ToString());
                    //BaiduVar.NearLines.Distinct();
                    if (iii < 5)
                        QueryRepeat5(querystr,action);
                    else
                        action.Invoke();
                }
            }
        });
    }

    private static void placeSearch(string queryStr, Action action)
    {
        try
        {
            BaiduVar.NearLines.Clear();
            iii = 0;
            QueryRepeat5(queryStr,action);
            //for (int i = 0; i < 15; i++)
            //{
            //    string tmp = queryStr.Replace("page_num=0", "page_num=1");
            //    var client = new RestClient(tmp);
            //    client.ExecuteAsync(new RestRequest(Method.GET), (response) =>
            //    {
            //        string jsn = response.Content.Replace(" ", "");
            //        if (jsn.Contains("result") && !jsn.Contains("[]"))
            //        {
            //            JSONSTR_pla jsr = (JSONSTR_pla)JsonConvert.DeserializeObject(jsn, typeof(JSONSTR_pla));
            //            if (jsr.status == "0")
            //            {
            //                BaiduVar.NearLines.AddRange(jsr.results);
            //                //System.Windows.MessageBox.Show(tmp + "\n" + BaiduVar.NearLines.Count.ToString());
            //                //BaiduVar.NearLines.Distinct();
            //                action.Invoke();
            //            }
            //        }
            //    });
            //}
        }
        catch (Exception ee)
        {
            throw ee;
        }
    }

    public static void placeSearchLike(string KeyWord, Action action)
    {
        try
        {
            BaiduVar.placeLike.Clear();
            string api = "http://api.map.baidu.com/place/v2/suggestion?region="+cityCode+"&output=json&ak=" + ak + "&q=" + KeyWord;
            var client = new RestClient(api);
            client.ExecuteAsync(new RestRequest(Method.GET), (response) =>
            {
                string jsn = response.Content;
                if (jsn.Contains("result"))
                {
                    JSONSTR_like jsr = (JSONSTR_like)JsonConvert.DeserializeObject(jsn, typeof(JSONSTR_like));
                    if (jsr.status == "0")
                    {
                        BaiduVar.placeLike.AddRange(jsr.result);
                        action.Invoke();
                    }
                }
            });
        }
        catch (Exception ee)
        {
            throw ee;
        }
    }

    public static void busDirectionQuery(string startLocOrName,string endLocOrName, Action action)
    {
        try
        {
            BaiduVar.busRoutList.Clear();
            string api = "http://api.map.baidu.com/direction/v1?mode=transit&region="+cityCode+"&output=json&ak="
                + ak + "&origin=" + startLocOrName + "&destination=" + endLocOrName;
            var client = new RestClient(api);
            client.ExecuteAsync(new RestRequest(Method.GET), (response) =>
            {
                string jsn = response.Content;
                if (jsn.Contains("result"))
                {
                    Json_rout jsr = (Json_rout)JsonConvert.DeserializeObject(jsn, typeof(Json_rout));
                    if (jsr.status == "0")
                    {
                        if (jsr.result.routes != null)
                        {
                            BaiduVar.busRoutList.AddRange(jsr.result.routes);
                            if (BaiduVar.busRoutList.Count > 0)
                                action.Invoke();
                        }
                    }
                }
            });
        }
        catch (Exception ee)
        {
            throw ee;
        }
    }
    #endregion

    #region convert

    /// <summary>
    /// baidu坐标转地址
    /// </summary>
    /// <param name="geo"></param>
    /// <param name="action"></param>
    public static void geo2add(string geo, Action action)
    {
        try
        {
            string api = "http://api.map.baidu.com/geocoder/v2/?output=json&ak=" + ak + "&location=" + geo;

            addressConvert(api, action);
        }
        catch (Exception ee)
        {
            throw ee;
        }
    }

    /// <summary>
    /// 地址转坐标
    /// </summary>
    /// <param name="addr"></param>
    /// <param name="KeyWord"></param>
    /// <param name="action"></param>
    public static void add2geo(string addr, string KeyWord, Action action)
    {
        try
        {
            string api = "http://api.map.baidu.com/geocoder/v2/?output=json&ak=" + ak + "&address=" + addr;
            addressConvert(api, action);
        }
        catch (Exception ee)
        {
            throw ee;
        }
    }

    private static void addressConvert(string queryStr, Action action)
    {
        try
        {
            var client = new RestClient(queryStr);
            client.ExecuteAsync(new RestRequest(Method.GET), (response) =>
            {
                string jsn = response.Content;
                if (jsn.Contains("result"))
                {
                    if (jsn.Contains("confidence"))
                    {
                        JSONSTR_ag jsr = (JSONSTR_ag)JsonConvert.DeserializeObject(jsn, typeof(JSONSTR_ag));
                        if (jsr.status == "0")
                        {
                            BaiduVar.retAG = jsr.results;
                            action.Invoke();
                        }
                    }
                    else
                        if (jsn.Contains("formatted_address"))
                        {
                            JSONSTR_ga jsr = (JSONSTR_ga)JsonConvert.DeserializeObject(jsn, typeof(JSONSTR_ga));
                            if (jsr.status == "0")
                            {
                                BaiduVar.retGA = jsr.result;
                                action.Invoke();
                            }
                        }
                }
            });
        }
        catch (Exception ee)
        {
            throw ee;
        }
    }
    #endregion

    //#region Gps2BaiduGeo

    ///// <summary>
    ///// GPS坐标转换成百度地图坐标
    ///// </summary>
    ///// <param name="GpsGeo">GPS字符串(纬度,经度)</param>
    ///// <param name="action">在转换完成后执行的动作</param>
    //public static void gps2BaiduGeo(string GpsGeo, Action action)
    //{
    //    try
    //    {
    //        string[] geos = GpsGeo.Split(',');
    //        string api = "http://api.map.baidu.com/geoconv/v1/?from=1&to=5&ak=" + ak + "&coords=" + geos[1] + "," + geos[0];

    //        var client = new RestClient(api);
    //        client.ExecuteAsync(new RestRequest(Method.GET), (response) =>
    //        {
    //            string jsn = response.Content;
    //            if (jsn.Contains("result"))
    //            {
    //                JSONSTR_gps2bd jsr = (JSONSTR_gps2bd)JsonConvert.DeserializeObject(jsn, typeof(JSONSTR_gps2bd));
    //                if (jsr.status == "0" && jsr.result.Length == 1)
    //                {
    //                    GeoService.strBaiduGeo = jsr.result[0].y + "," + jsr.result[0].x;
    //                    //System.Windows.MessageBox.Show(GeoService.strBaiduGeo+"\n"+geo);
    //                    action.Invoke();
    //                }
    //            }
    //        });
    //    }
    //    catch (Exception ee)
    //    {
    //        throw ee;
    //    }
    //}

    ///// <summary>
    ///// GPS坐标转换成百度地图坐标
    ///// </summary>
    ///// <param name="GpsGeo">GPS字符串(纬度,经度)</param>
    //public static void gps2BaiduGeo(string GpsGeo)
    //{
    //    try
    //    {
    //        string[] geos = GpsGeo.Split(',');
    //        string api = "http://api.map.baidu.com/geoconv/v1/?from=1&to=5&ak=" + ak + "&coords=" + geos[1] + "," + geos[0];

    //        var client = new RestClient(api);
    //        client.ExecuteAsync(new RestRequest(Method.GET), (response) =>
    //        {
    //            string jsn = response.Content;
    //            if (jsn.Contains("result"))
    //            {
    //                JSONSTR_gps2bd jsr = (JSONSTR_gps2bd)JsonConvert.DeserializeObject(jsn, typeof(JSONSTR_gps2bd));
    //                if (jsr.status == "0" && jsr.result.Length == 1)
    //                {
    //                    GeoService.strBaiduGeo = jsr.result[0].y + "," + jsr.result[0].x;
    //                    //System.Windows.MessageBox.Show(GeoService.strBaiduGeo+"\n"+geo);
    //                }
    //            }
    //        });
    //    }
    //    catch (Exception ee)
    //    {
    //        throw ee;
    //    }
    //}
    //#endregion

    //#region BaiduGeo2Gps

    ///// <summary>
    ///// 百度地图坐标转换成GPS坐标
    ///// </summary>
    ///// <param name="geo">GPS字符串(纬度,经度)</param>
    ///// <param name="action">在转换完成后执行的动作</param>
    //public static void gps2BaiduGeo(string BaiduGeo, Action action)
    //{
    //    try
    //    {
    //        string[] geos = BaiduGeo.Split(',');
    //        string api = "http://api.map.baidu.com/geoconv/v1/?from=5&to=1&ak=" + ak + "&coords=" + geos[1] + "," + geos[0];

    //        var client = new RestClient(api);
    //        client.ExecuteAsync(new RestRequest(Method.GET), (response) =>
    //        {
    //            string jsn = response.Content;
    //            if (jsn.Contains("result"))
    //            {
    //                JSONSTR_gps2bd jsr = (JSONSTR_gps2bd)JsonConvert.DeserializeObject(jsn, typeof(JSONSTR_gps2bd));
    //                if (jsr.status == "0" && jsr.result.Length == 1)
    //                {
    //                    GeoService.strBaiduGeo = jsr.result[0].y + "," + jsr.result[0].x;
    //                    //System.Windows.MessageBox.Show(GeoService.strBaiduGeo+"\n"+geo);
    //                    action.Invoke();
    //                }
    //            }
    //        });
    //    }
    //    catch (Exception ee)
    //    {
    //        throw ee;
    //    }
    //}
    //#endregion
}