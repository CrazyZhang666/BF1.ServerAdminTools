using BF1.ServerAdminTools.Common.Data;

namespace BF1.ServerAdminTools.Common.Utils;

public static class PlayerUtils
{
    /// <summary>
    /// 小数类型的时间秒，转为mm:ss格式
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static string SecondsToMMSS(float time)
    {
        try
        {
            if (time >= 0 && time <= 36000)
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds(time);

                DateTime dateTime = DateTime.Parse(timeSpan.ToString());

                return $"{dateTime:mm:ss}";
            }
            else
            {
                return $"00:00";
            }
        }
        catch (Exception) { throw; }
    }

    /// <summary>
    /// 小数类型的时间秒，转为mm格式
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static int SecondsToMM(float time)
    {
        try
        {
            if (time >= 0 && time <= 36000)
            {
                int a = (int)(time / 60);

                if (a != 0)
                {
                    return a;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        catch (Exception) { throw; }
    }

    /// <summary>
    /// 计算玩家KD比
    /// </summary>
    /// <param name="kill">玩家击杀数</param>
    /// <param name="dead">玩家死亡数</param>
    /// <returns>返回玩家KD比（小数float）<returns>
    public static float GetPlayerKD(int kill, int dead)
    {
        if (kill == 0 && dead >= 0)
        {
            return 0.0f;
        }
        else if (kill > 0 && dead == 0)
        {
            return kill;
        }
        else if (kill > 0 && dead > 0)
        {
            return (float)kill / dead;
        }
        else
        {
            return (float)kill / dead;
        }
    }

    /// <summary>
    /// 计算玩家KPM比
    /// </summary>
    /// <param name="kill"></param>
    /// <param name="minute"></param>
    /// <returns></returns>
    public static float GetPlayerKPM(int kill, float minute)
    {
        if (minute != 0.0f)
        {
            return kill / minute;
        }
        else
        {
            return 0.0f;
        }
    }

    /// <summary>
    /// 判断战地1输入框字符串长度，中文3，英文1
    /// </summary>
    /// <param name="str">需要判断的字符串</param>
    /// <returns></returns>
    public static int GetStrLength(string str)
    {
        if (string.IsNullOrEmpty(str))
            return 0;

        ASCIIEncoding ascii = new();
        int tempLen = 0;
        byte[] s = ascii.GetBytes(str);
        for (int i = 0; i < s.Length; i++)
        {
            if ((int)s[i] == 63)
            {
                tempLen += 3;
            }
            else
            {
                tempLen += 1;
            }
        }

        return tempLen;
    }

    /// <summary>
    /// 获取击杀星数
    /// </summary>
    /// <param name="kills"></param>
    /// <returns></returns>
    public static string GetKillStar(int kills)
    {
        if (kills < 100)
        {
            return "";
        }
        else
        {
            int count = kills / 100;
            if (count > 100)
                count = 100;

            return $"{count}";
        }
    }

    /// <summary>
    /// 获取玩家ID或队标
    /// </summary>
    public static string GetPlayerTargetName(string originalName, out string clan)
    {
        clan = "";
        if (string.IsNullOrEmpty(originalName))
            return "";

        int index = originalName.IndexOf("]");
        string name;
        if (index != -1)
        {
            clan = originalName.Substring(1, index - 1);
            name = originalName.Substring(index + 1);
        }
        else
        {
            clan = "";
            name = originalName;
        }

        return name;
    }

    /// <summary>
    /// 获取武器对应的中文名称
    /// </summary>
    /// <param name="originWeaponName"></param>
    /// <returns></returns>
    public static string GetWeaponChsName(string originWeaponName)
    {
        if (string.IsNullOrEmpty(originWeaponName))
            return "";

        if (originWeaponName.Contains("_KBullet"))
            return "K 弹";

        if (originWeaponName.Contains("_RGL_Frag"))
            return "步枪手榴弹（破片）";

        if (originWeaponName.Contains("_RGL_Smoke"))
            return "步枪手榴弹（烟雾）";

        if (originWeaponName.Contains("_RGL_HE"))
            return "步枪手榴弹（高爆）";

        int index = WeaponData.AllWeaponInfo.FindIndex(var => var.English == originWeaponName);
        if (index != -1)
            return WeaponData.AllWeaponInfo[index].Chinese;
        else
            return originWeaponName;
    }

    /// <summary>
    /// 检查玩是否是管理员或者VIP
    /// </summary>
    /// <param name="personaId"></param>
    /// <param name="list"></param>
    /// <returns></returns>
    public static bool CheckAdminVIP(long personaId, List<long> list)
    {
        return list.Contains(personaId);
    }

    /// <summary>
    /// 获取玩家游玩时间，返回分钟数或小时数
    /// </summary>
    /// <param name="second"></param>
    /// <returns></returns>
    public static string GetPlayTime(double second)
    {
        var ts = TimeSpan.FromSeconds(second);

        if (ts.TotalHours < 1)
        {
            return ts.TotalMinutes.ToString("0") + " 分钟";
        }

        return ts.TotalHours.ToString("0") + " 小时";
    }

    /// <summary>
    /// 获取武器简短名称，用于踢人理由
    /// </summary>
    /// <param name="weaponName"></param>
    /// <returns></returns>
    public static string GetWeaponShortTxt(string weaponName)
    {
        int index = WeaponData.AllWeaponInfo.FindIndex(var => var.English.Equals(weaponName));
        if (index != -1)
        {
            return WeaponData.AllWeaponInfo[index].ShortTxt;
        }

        return weaponName;
    }

    /// <summary>
    /// 获取小队的中文名称
    /// </summary>
    /// <param name="squadID"></param>
    /// <returns></returns>
    public static string GetSquadChsName(int squadID)
    {
        switch (squadID)
        {
            case 0:
                return "无";
            case 1:
                return "苹果";
            case 2:
                return "奶油";
            case 3:
                return "查理";
            case 4:
                return "达夫";
            case 5:
                return "爱德华";
            case 6:
                return "弗莱迪";
            case 7:
                return "乔治";
            case 8:
                return "哈利";
            case 9:
                return "墨水";
            case 10:
                return "强尼";
            case 11:
                return "国王";
            case 12:
                return "伦敦";
            case 13:
                return "猿猴";
            case 14:
                return "疯子";
            case 15:
                return "橘子";
            default:
                return squadID.ToString();
        }
    }
}
