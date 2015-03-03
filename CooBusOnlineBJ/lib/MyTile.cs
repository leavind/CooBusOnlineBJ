using System;
using Microsoft.Phone.Shell;
public class MyTile
{
    //http://wp.eoe.cn/thread-4577-1-1.html
    //http://blog.csdn.net/tcjiaan/article/details/7313866
    //http://msdn.microsoft.com/zh-cn/library/hh202979
    //http://blog.csdn.net/tcjiaan/article/details/7644802
    //
    /// <summary>
    /// 创建或刷新一个磁贴,不存在则创建,存在则刷新.
    /// </summary>
    /// <param name="pageUrlPath">页面地址</param>
    /// <param name="title">标题</param>
    /// <param name="count">计数</param>
    /// <param name="backtitle">翻转的标题</param>
    /// <param name="backcontent">翻转的内容</param>
    /// <param name="forceImagePath">前景图</param>
    /// <param name="bgImagePath">背景图</param>
    public static void CreateOrUpdate(string pageUrlPath,
        string title, string count, string backtitle, string backcontent, string forceImagePath, string bgImagePath)
    {
        bool have = false;
        ShellTile myTile = null; // = ShellTile.ActiveTiles.FirstOrDefault(m => m.NavigationUri.ToString().Contains("s="));
        foreach (ShellTile i in ShellTile.ActiveTiles)
        {
            if (i.NavigationUri.OriginalString.Contains("=" + title + "&"))
            {
                have = true;
                myTile = i;
                break;
            }
        }

        if (have)
        {
            myTile.Update(getTileData(title, count, backtitle, backcontent, forceImagePath, bgImagePath));
        }
        else
        {
            ShellTile.Create(new Uri(pageUrlPath, UriKind.Relative),
                getTileData(title, count, backtitle, backcontent, forceImagePath, bgImagePath));
        }
    }

    private static StandardTileData getTileData(string title, string count, string backtitle,
        string backcontent, string forceImagePath, string bgImagePath)
    {
        int Counter = 0;
        StandardTileData myData = new StandardTileData()
        {
            Title = string.IsNullOrEmpty(title) == true ? string.Empty : title,
            Count = int.TryParse(count, out Counter) == true ? Counter : 0,
            BackTitle = string.IsNullOrEmpty(backtitle) == true ? string.Empty : backtitle,
            BackContent = string.IsNullOrEmpty(backcontent) == true ? string.Empty : backcontent,
            BackgroundImage = new Uri(forceImagePath, UriKind.Relative),
            BackBackgroundImage = new Uri(bgImagePath, UriKind.Relative)
        };
        return myData;
    }
}

