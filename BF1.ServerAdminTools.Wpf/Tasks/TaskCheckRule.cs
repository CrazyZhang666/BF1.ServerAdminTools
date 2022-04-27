using BF1.ServerAdminTools.Common;
using BF1.ServerAdminTools.Common.API.BF1Server;
using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Common.Utils;
using BF1.ServerAdminTools.Wpf.Data;
using BF1.ServerAdminTools.Wpf.Utils;

namespace BF1.ServerAdminTools.Wpf.Tasks;

internal class TaskCheckRule
{
    public static bool NeedPause;
    public static Semaphore Semaphore = new(0, 5);

    private static bool IsSwitching;
    private static readonly Random Random = new();

    public static void Start()
    {
        new Thread(Run)
        {
            Name = "TaskCheckRule",
            IsBackground = true
        }.Start();
    }

    private static void Run()
    {
        while (true)
        {
            Semaphore.WaitOne();
            if (DataSave.AutoKickBreakPlayer)
            {
                if (!NeedPause)
                    StartCheck();
                AutoSwitchMap();
            }
            TaskTick.Done();
        }
    }

    public static void StartCheck()
    {
        bool other = false;
        ServerRuleObj rule = DataSave.NowRule;
        if (DataSave.NowRule.ScoreOtherRule != 0 &&
            Math.Abs(Globals.ServerHook.Team1Score - Globals.ServerHook.Team2Score)
                > DataSave.NowRule.ScoreOtherRule)
        {
            other = DataSave.Rules.TryGetValue(DataSave.NowRule.OtherRule.ToLower(), out rule);
        }

        if (other && Globals.ServerHook.Team1Score < Globals.ServerHook.Team2Score)
        {
            foreach (var item in Globals.PlayerDatas_Team1.Values)
            {
                if (NeedPause)
                    return;
                CheckPlayerIsBreakRule(item, rule);
            }
        }
        else
        {
            foreach (var item in Globals.PlayerDatas_Team1.Values)
            {
                if (NeedPause)
                    return;
                CheckPlayerIsBreakRule(item, DataSave.NowRule);
            }
        }

        if (other && Globals.ServerHook.Team1Score > Globals.ServerHook.Team2Score)
        {
            foreach (var item in Globals.PlayerDatas_Team2.Values)
            {
                if (NeedPause)
                    return;
                CheckPlayerIsBreakRule(item, rule);
            }
        }
        else
        {
            foreach (var item in Globals.PlayerDatas_Team2.Values)
            {
                if (NeedPause)
                    return;
                CheckPlayerIsBreakRule(item, DataSave.NowRule);
            }
        }
    }

    public static void CheckPlayerIsBreakRule(PlayerData playerData, ServerRuleObj rule)
    {
        if (rule == null || DataSave.NowRule == null)
            return;

        if (TaskKick.IsHave(playerData.PersonaId))
            return;

        //白名单
        bool WhiteList = DataSave.NowRule.Custom_WhiteList.Contains(playerData.Name);

        //管理员
        if (Globals.RspInfo != null)
        {
            if (Globals.RspInfo.owner?.personaId == playerData.PersonaId.ToString())
                return;
            if (Globals.RspInfo.adminList != null)
                if (Globals.RspInfo.adminList.FindIndex(a => a.personaId == playerData.PersonaId.ToString()) != -1)
                    return;
        }

        //黑名单
        if (DataSave.NowRule.Custom_BlackList.Contains(playerData.Name))
        {
            TaskKick.AddKick(new BreakRuleInfo
            {
                Name = playerData.Name,
                PersonaId = playerData.PersonaId,
                Reason = $"You have been ban on this server",
                Type = BreakType.Server_Black_List
            });

            return;
        }

        //订阅黑名单
        if (!WhiteList && SubscribeUtils.Check(playerData.PersonaId, playerData.Name))
        {
            TaskKick.AddKick(new BreakRuleInfo
            {
                Name = playerData.Name,
                PersonaId = playerData.PersonaId,
                Reason = $"You have been ban on this server",
                Type = BreakType.Server_Black_List
            });

            return;
        }

        // 限制玩家击杀
        if (playerData.Kill > rule.MaxKill && rule.MaxKill != 0)
        {
            if (WhiteList && DataSave.NowRule.WhiteListNoKill)
                return;
            TaskKick.AddKick(new BreakRuleInfo
            {
                Name = playerData.Name,
                PersonaId = playerData.PersonaId,
                Reason = $"Kill Limit {rule.MaxKill:0}",
                Type = BreakType.Kill_Limit
            });

            return;
        }

        // 计算玩家KD最低击杀数
        if (playerData.Kill > rule.KDFlag && rule.KDFlag != 0)
        {
            // 限制玩家KD
            if (playerData.KD > rule.MaxKD && rule.MaxKD != 0.00f)
            {
                if (WhiteList && DataSave.NowRule.WhiteListNoKD)
                    return;
                TaskKick.AddKick(new BreakRuleInfo
                {
                    Name = playerData.Name,
                    PersonaId = playerData.PersonaId,
                    Reason = $"KD Limit {rule.MaxKD:0.00}",
                    Type = BreakType.KD_Limit
                });
            }

            return;
        }

        // 计算玩家KPM比条件
        if (playerData.Kill > rule.KPMFlag && rule.KPMFlag != 0)
        {
            // 限制玩家KPM
            if (playerData.KPM > rule.MaxKPM && rule.MaxKPM != 0.00f)
            {
                if (WhiteList && DataSave.NowRule.WhiteListNoKPM)
                    return;
                TaskKick.AddKick(new BreakRuleInfo
                {
                    Name = playerData.Name,
                    PersonaId = playerData.PersonaId,
                    Reason = $"KPM Limit {rule.MaxKPM:0.00}",
                    Type = BreakType.KPM_Limit
                });
            }

            return;
        }

        // 从武器规则里遍历限制武器名称
        for (int i = 0; i < DataSave.NowRule.Custom_WeaponList.Count; i++)
        {
            if (WhiteList && DataSave.NowRule.WhiteListNoW)
                return;

            var item = DataSave.NowRule.Custom_WeaponList[i];

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
                    TaskKick.AddKick(new BreakRuleInfo
                    {
                        Name = playerData.Name,
                        PersonaId = playerData.PersonaId,
                        Reason = $"Weapon Limit K Bullet",
                        Type = BreakType.Weapon_Limit
                    });

                    return;
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
                    TaskKick.AddKick(new BreakRuleInfo
                    {
                        Name = playerData.Name,
                        PersonaId = playerData.PersonaId,
                        Reason = $"Weapon Limit RGL Frag",
                        Type = BreakType.Weapon_Limit
                    });

                    return;
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
                    TaskKick.AddKick(new BreakRuleInfo
                    {
                        Name = playerData.Name,
                        PersonaId = playerData.PersonaId,
                        Reason = $"Weapon Limit RGL Smoke",
                        Type = BreakType.Weapon_Limit
                    });

                    return;
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
                    TaskKick.AddKick(new BreakRuleInfo
                    {
                        Name = playerData.Name,
                        PersonaId = playerData.PersonaId,
                        Reason = $"Weapon Limit RGL HE",
                        Type = BreakType.Weapon_Limit
                    });

                    return;
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
                TaskKick.AddKick(new BreakRuleInfo
                {
                    Name = playerData.Name,
                    PersonaId = playerData.PersonaId,
                    Reason = $"Weapon Limit {PlayerUtils.GetWeaponShortTxt(item)}",
                    Type = BreakType.Weapon_Limit
                });

                return;
            }
        }

        if (WhiteList)
            return;

        // 限制玩家最低等级
        if (playerData.Rank < DataSave.NowRule.MinRank
            && DataSave.NowRule.MinRank != 0 && playerData.Rank != 0)
        {
            TaskKick.AddKick(new BreakRuleInfo
            {
                Name = playerData.Name,
                PersonaId = playerData.PersonaId,
                Reason = $"Min Rank Limit {rule.MinRank:0}",
                Type = BreakType.Min_Rank_Limit
            });

            return;
        }

        // 限制玩家最高等级
        if (playerData.Rank > DataSave.NowRule.MaxRank
            && DataSave.NowRule.MaxRank != 0 && playerData.Rank != 0)
        {
            TaskKick.AddKick(new BreakRuleInfo
            {
                Name = playerData.Name,
                PersonaId = playerData.PersonaId,
                Reason = $"Max Rank Limit {rule.MaxRank:0}",
                Type = BreakType.Max_Rank_Limit
            });

            return;
        }
    }

    private static void AutoSwitchMap()
    {
        if (!DataSave.AutoKickBreakPlayer)
            return;
        if (IsSwitching)
            return;
        if (DataSave.NowRule.ScoreSwitchMap == 0)
            return;
        if (DataSave.NowRule.ScoreStartSwitchMap != 0 &&
            Math.Min(Globals.ServerHook.Team1Score, Globals.ServerHook.Team2Score) > DataSave.NowRule.ScoreStartSwitchMap)
            return;
        if (DataSave.NowRule.ScoreSwitchMap != 0 &&
            Math.Abs(Globals.ServerHook.Team1Score - Globals.ServerHook.Team2Score) <= DataSave.NowRule.ScoreSwitchMap)
            return;
        if (Globals.ServerHook.Team1Score == 0 || Globals.ServerHook.Team2Score == 0)
            return;

        if (DataSave.NowRule.ScoreNotSwitchMap != 0)
        {
            if (Globals.ServerHook.Team1Score > DataSave.NowRule.ScoreNotSwitchMap
                || Globals.ServerHook.Team2Score > DataSave.NowRule.ScoreSwitchMap)
                return;
        }

        StartSwitchMap();
    }

    private static void StartSwitchMap()
    {
        IsSwitching = true;
        Task.Run(async () =>
        {
            if (Globals.ServerInfo == null)
            {
                await Core.InitServerInfo();
            }
            if (Globals.ServerInfo == null)
            {
                IsSwitching = false;
                return;
            }
            var nowMap = Globals.ServerInfo.mapNamePretty;
            var list = Globals.ServerInfo.rotation;

            int index = list.FindIndex(item => item.mapPrettyName == nowMap);
            int a;
            switch (DataSave.NowRule.SwitchMapType)
            {
                case 0:
                    a = index + 1;
                    if (a >= list.Count || a < 0)
                    {
                        a = 0;
                    }
                    break;
                case 1:
                    do
                    {
                        a = Random.Next(0, list.Count - 1);
                    }
                    while (a != index);
                    break;
                case 2:
                    a = index;
                    break;
                default:
                    a = 0;
                    break;
            }
            await ServerAPI.ChangeServerMap(Globals.Config.PersistedGameId, a.ToString());
            await Task.Delay(30000);
            IsSwitching = false;
        });
    }
}
