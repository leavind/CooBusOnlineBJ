using System;
using System.Text;

/// <summary>
/// unicode 16进制标识字符转中文
/// </summary>
public class Uni2Chs
{
    /// <summary>
    /// unicode 16进制标识字符转中文
    /// </summary>
    /// <param name="hexString">16进制标识字符</param>
    /// <returns>中文字符串</returns>
    public static string uni2Chs(string hexString)
    {
        StringBuilder sb = new StringBuilder();
        string[] tmp = hexString.Replace("\\u", "ÿ").Split('ÿ');
        foreach (var s in tmp)
        {
            int len = s.Length;
            if (len > 0)
            {
                sb.Append(char.ConvertFromUtf32(Int32.Parse(s.Substring(0, 4), System.Globalization.NumberStyles.HexNumber)));
                if (len > 4)
                    sb.Append(s.Substring(4, len - 4));
            }
        }
        return sb.ToString();
    }
}