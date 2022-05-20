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
    /// 获取职业中文名
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string GetCareerChsName(string input)
    {
        foreach (var item in CareerData.AllCareerInfo)
        { 
            if(item.ID == input)
            {
                return item.Chinese;
            }
        }

        return input;
    }
    public static string GetCareerName(string input)
    {
        foreach (var item in CareerData.AllCareerInfo)
        {
            if (item.ID == input)
            {
                return item.English;
            }
        }

        return input;
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

        int index = WeaponData.AllWeaponInfo.FindIndex(var => var.ID == originWeaponName);
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
        int index = WeaponData.AllWeaponInfo.FindIndex(var => var.ID.Equals(weaponName));
        if (index != -1)
        {
            return WeaponData.AllWeaponInfo[index].English;
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
        return squadID switch
        {
            0 => "无",
            1 => "苹果",
            2 => "奶油",
            3 => "查理",
            4 => "达夫",
            5 => "爱德华",
            6 => "弗莱迪",
            7 => "乔治",
            8 => "哈利",
            9 => "墨水",
            10 => "强尼",
            11 => "国王",
            12 => "伦敦",
            13 => "猿猴",
            14 => "疯子",
            15 => "橘子",
            _ => squadID.ToString(),
        };
    }
}
