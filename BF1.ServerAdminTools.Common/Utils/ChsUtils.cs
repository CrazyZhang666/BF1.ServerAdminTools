using Chinese;

namespace BF1.ServerAdminTools.Common.Utils;

public class ChsUtils
{
    /// <summary>
    /// 字符串简体转繁体
    /// </summary>
    /// <param name="strSimple"></param>
    /// <returns></returns>
    public static string ToTraditionalChinese(string strSimple)
    {
        return ChineseConverter.ToTraditional(strSimple);
    }

    /// <summary>
    /// 字符串繁体转简体
    /// </summary>
    /// <param name="strTraditional"></param>
    /// <returns></returns>
    public static string ToSimplifiedChinese(string strTraditional)
    {
        return ChineseConverter.ToSimplified(strTraditional);
    }

    public static string ToDBC(string input)
    {
        char[] c = input.ToCharArray();

        for (int i = 0; i < c.Length; i++)
        {
            if (c[i] == 12288)
            {
                c[i] = (char)32;
                continue;
            }

            if (c[i] > 65280 && c[i] < 65375)
            {
                c[i] = (char)(c[i] - 65248);
            }
        }

        return new string(c);
    }
}
