using System.Text.RegularExpressions;

public class HtmlTag
{
    ///   <summary>   
    ///   移除HTML标签   
    ///   </summary>   
    ///   <param   name="HTMLStr">HTMLStr</param>   
    public static string removeHtmlTag(string HTMLStr)
    {
        return System.Text.RegularExpressions.Regex.Replace(HTMLStr, "<[^>]*>", "");
    }


    ///   <summary>   
    ///   取出文本中的图片地址   
    ///   </summary>   
    ///   <param   name="HTMLStr">HTMLStr</param>   
    public static string GetImgUrl(string HTMLStr)
    {
        string str = string.Empty;
        //string sPattern = @"^<img\s+[^>]*>";
        Regex r = new Regex(@"<img\s+[^>]*\s*src\s*=\s*([']?)(?<url>\S+)'?[^>]*>",
                RegexOptions.Compiled);
        Match m = r.Match(HTMLStr.ToLower());
        if (m.Success)
            str = m.Result("${url}");
        return str;
    }
}