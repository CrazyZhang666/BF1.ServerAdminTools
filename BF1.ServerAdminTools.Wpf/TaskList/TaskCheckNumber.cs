using BF1.ServerAdminTools.Common;
using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Wpf.Data;

namespace BF1.ServerAdminTools.Wpf.TaskList;

internal static class TaskCheckNumber
{
    public static bool NeedPause;

    public static Semaphore Semaphore = new(0, 5);

    private static Dictionary<string, List<long>> Team1Weapon = new();
    private static Dictionary<string, List<long>> Team2Weapon = new();

    public static void Start()
    {
        new Thread(Run)
        {
            Name = "TaskCheckNumber",
            IsBackground = true
        }.Start();
    }

    private static void Run()
    {
        while (Tasks.IsRun)
        {
            Semaphore.WaitOne();
            if (!Tasks.IsRun)
                return;
            if (DataSave.AutoKickBreakPlayer)
            {
                if (!NeedPause)
                {
                    StartCheck();
                }
            }
            TaskTick.Done();
        }
    }

    public static void Clear()
    {
        Team1Weapon.Clear();
    }

    private static void CheckTeam(List<long> obj, WeaponNumberObj item, ICollection<PlayerData> list)
    {
        foreach (var item1 in list)
        {
            //没有武器
            if (IsEmpty(item1))
                continue;
            //武器在限制组里面
            bool group = IsInGroup(item.Weapons, item1);
            //是否之前已经使用了
            bool have = obj.Contains(item1.PersonaId);
            bool white = WhiteList(item1);

            //在限制组，但是限制不用
            if (!group && have)
            {
                obj.Remove(item1.PersonaId);
            }

            //在限制组，且白名单
            if (group && white)
            {
                if (!have)
                    obj.Add(item1.PersonaId);
                continue;
            }

            //正在使用，且之前已经使用
            if (group && have)
            {
                continue;
            }

            //使用武器
            if (group)
            {
                //超过数量
                if (obj.Count >= item.Count)
                {
                    //白名单不T出
                    if (white && DataSave.NowRule.WhiteListNoN)
                        continue;
                    if (TaskKick.IsHave(item1.PersonaId))
                        continue;

                    TaskKick.AddKick(new BreakRuleInfo
                    {
                        Name = item1.Name,
                        PersonaId = item1.PersonaId,
                        Reason1 = $"玩家{item1.Name}武器数量限制，被踢出",
                        Reason = $"To many weapon",
                        Type = BreakType.To_Many_Weapon
                    });

                    continue;
                }
                //没有使用过记录
                else if (!have)
                {
                    obj.Add(item1.PersonaId);
                }
            }
        }
    }

    private static void StartCheck()
    {
        foreach (var item in DataSave.NowRule.WeaponNumbers.Values)
        {
            //获取储存
            if (!Team1Weapon.TryGetValue(item.Name, out List<long> obj))
            {
                obj = new();
                Team1Weapon.Add(item.Name, obj);
            }
            //清理退出的玩家
            foreach (var item1 in new List<long>(obj))
            {
                if (!Globals.PlayerDatas_Team1.Values.Where(a => a.PersonaId == item1).Any())
                {
                    obj.Remove(item1);
                }
            }
            //检查队伍
            CheckTeam(obj, item, Globals.PlayerDatas_Team1.Values);

            if (!Team2Weapon.TryGetValue(item.Name, out obj))
            {
                obj = new();
                Team2Weapon.Add(item.Name, obj);
            }
            foreach (var item1 in new List<long>(obj))
            {
                if (!Globals.PlayerDatas_Team2.Values.Where(a => a.PersonaId == item1).Any())
                {
                    obj.Remove(item1);
                }
            }
            CheckTeam(obj, item, Globals.PlayerDatas_Team2.Values);
        }
    }

    private static bool WhiteList(PlayerData playerData)
    {
        //白名单
        if (DataSave.NowRule.Custom_WhiteList.Contains(playerData.Name))
        {
            return true;
        }

        //管理员
        if (Globals.RspInfo != null)
        {
            if (Globals.RspInfo.owner?.personaId == playerData.PersonaId.ToString())
                return true;
            if (Globals.RspInfo.adminList != null)
                if (Globals.RspInfo.adminList.FindIndex(a => a.personaId == playerData.PersonaId.ToString()) != -1)
                    return true;
        }

        return false;
    }

    private static bool IsEmpty(PlayerData playerData)
    {
        return string.IsNullOrWhiteSpace(playerData.WeaponS0) &&
            string.IsNullOrWhiteSpace(playerData.WeaponS1) &&
            string.IsNullOrWhiteSpace(playerData.WeaponS2) &&
            string.IsNullOrWhiteSpace(playerData.WeaponS3) &&
            string.IsNullOrWhiteSpace(playerData.WeaponS4) &&
            string.IsNullOrWhiteSpace(playerData.WeaponS5) &&
            string.IsNullOrWhiteSpace(playerData.WeaponS6) &&
            string.IsNullOrWhiteSpace(playerData.WeaponS7);
    }

    private static bool IsInGroup(List<string> list, PlayerData playerData)
    {
        foreach (var item in list)
        {
            // K 弹
            if (item == "_KBullet")
            {
                if (playerData.WeaponS0.Contains("_KBullet") ||
                    playerData.WeaponS1.Contains("_KBullet") ||
                    playerData.WeaponS2.Contains("_KBullet") ||
                    playerData.WeaponS3.Contains("_KBullet") ||
                    playerData.WeaponS4.Contains("_KBullet") ||
                    playerData.WeaponS5.Contains("_KBullet") ||
                    playerData.WeaponS6.Contains("_KBullet") ||
                    playerData.WeaponS7.Contains("_KBullet"))
                {
                    return true;
                }
            }

            // 步枪手榴弹（破片）
            if (item == "_RGL_Frag")
            {
                if (playerData.WeaponS0.Contains("_RGL_Frag") ||
                    playerData.WeaponS1.Contains("_RGL_Frag") ||
                    playerData.WeaponS2.Contains("_RGL_Frag") ||
                    playerData.WeaponS3.Contains("_RGL_Frag") ||
                    playerData.WeaponS4.Contains("_RGL_Frag") ||
                    playerData.WeaponS5.Contains("_RGL_Frag") ||
                    playerData.WeaponS6.Contains("_RGL_Frag") ||
                    playerData.WeaponS7.Contains("_RGL_Frag"))
                {
                    return true;
                }
            }

            // 步枪手榴弹（烟雾）
            if (item == "_RGL_Smoke")
            {
                if (playerData.WeaponS0.Contains("_RGL_Smoke") ||
                    playerData.WeaponS1.Contains("_RGL_Smoke") ||
                    playerData.WeaponS2.Contains("_RGL_Smoke") ||
                    playerData.WeaponS3.Contains("_RGL_Smoke") ||
                    playerData.WeaponS4.Contains("_RGL_Smoke") ||
                    playerData.WeaponS5.Contains("_RGL_Smoke") ||
                    playerData.WeaponS6.Contains("_RGL_Smoke") ||
                    playerData.WeaponS7.Contains("_RGL_Smoke"))
                {
                    return true;
                }
            }

            // 步枪手榴弹（高爆）
            if (item == "_RGL_HE")
            {
                if (playerData.WeaponS0.Contains("_RGL_HE") ||
                    playerData.WeaponS1.Contains("_RGL_HE") ||
                    playerData.WeaponS2.Contains("_RGL_HE") ||
                    playerData.WeaponS3.Contains("_RGL_HE") ||
                    playerData.WeaponS4.Contains("_RGL_HE") ||
                    playerData.WeaponS5.Contains("_RGL_HE") ||
                    playerData.WeaponS6.Contains("_RGL_HE") ||
                    playerData.WeaponS7.Contains("_RGL_HE"))
                {
                    return true;
                }
            }

            if (playerData.WeaponS0 == item ||
                playerData.WeaponS1 == item ||
                playerData.WeaponS2 == item ||
                playerData.WeaponS3 == item ||
                playerData.WeaponS4 == item ||
                playerData.WeaponS5 == item ||
                playerData.WeaponS6 == item ||
                playerData.WeaponS7 == item)
            {
                return true;
            }
        }
        return false;
    }
}
