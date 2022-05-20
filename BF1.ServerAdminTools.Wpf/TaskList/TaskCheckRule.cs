using BF1.ServerAdminTools.Common;
using BF1.ServerAdminTools.Common.API.BF1Server;
using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Common.Utils;
using BF1.ServerAdminTools.Wpf.Data;
using BF1.ServerAdminTools.Wpf.Utils;

namespace BF1.ServerAdminTools.Wpf.TaskList;

internal static class TaskCheckRule
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

    /// <summary>
    /// 检查服务器玩家是否违规
    /// </summary>
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
                    AutoSwitchMap();
                }
            }
            TaskTick.Done();
        }
    }

    public static void StartCheck()
    {
        bool other = false;
        ServerRuleObj rule = DataSave.NowRule;
        if (DataSave.NowRule.ScoreOtherRule != 0
            && Math.Abs(Globals.ServerHook.Team1Score - Globals.ServerHook.Team2Score)
                > DataSave.NowRule.ScoreOtherRule
                && !string.IsNullOrWhiteSpace(DataSave.NowRule.OtherRule))
        {
            other = DataSave.Rules.TryGetValue(DataSave.NowRule.OtherRule.ToLower(), out rule);
        }

        if (other && Globals.ServerHook.Team1Score < Globals.ServerHook.Team2Score)
        {
            foreach (var item in Globals.PlayerDatas_Team1.Values)
            {
                CheckPlayerIsBreakRule(item, rule);
            }
        }
        else
        {
            foreach (var item in Globals.PlayerDatas_Team1.Values)
            {
                CheckPlayerIsBreakRule(item, DataSave.NowRule);
            }
        }


        if (!string.IsNullOrWhiteSpace(DataSave.NowRule.Team2Rule)
            && DataSave.Rules.TryGetValue(DataSave.NowRule.Team2Rule.ToLower(), out var rule1))
        {
            foreach (var item in Globals.PlayerDatas_Team2.Values)
            {
                CheckPlayerIsBreakRule(item, rule1);
            }
        }
        else if (other && Globals.ServerHook.Team1Score > Globals.ServerHook.Team2Score)
        {
            foreach (var item in Globals.PlayerDatas_Team2.Values)
            {
                CheckPlayerIsBreakRule(item, rule);
            }
        }
        else
        {
            foreach (var item in Globals.PlayerDatas_Team2.Values)
            {
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

        if (Globals.LocalPlayer.PersonaId == playerData.PersonaId)
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
                Reason1 = $"玩家{playerData.Name}在黑名单中，被踢出",
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
                Reason1 = $"玩家{playerData.Name}在订阅黑名单中，被踢出",
                Reason = $"You have been ban on this server",
                Type = BreakType.Server_Black_List
            });

            return;
        }

        // 限制玩家击杀
        if (rule.MaxKill != 0 && playerData.Kill > rule.MaxKill
                && !(WhiteList && DataSave.NowRule.WhiteListNoKill))
        {
            TaskKick.AddKick(new BreakRuleInfo
            {
                Name = playerData.Name,
                PersonaId = playerData.PersonaId,
                Reason1 = $"玩家{playerData.Name}击杀数超出，被踢出",
                Reason = $"Kill limit {rule.MaxKill:0}",
                Type = BreakType.Kill_Limit
            });

            return;
        }

        // 计算玩家KD最低击杀数
        if (rule.KDFlag != 0 && playerData.Kill > rule.KDFlag
            && playerData.KD > rule.MaxKD && rule.MaxKD != 0.00f
            && !(WhiteList && DataSave.NowRule.WhiteListNoKD))
        {
            // 限制玩家KD
            TaskKick.AddKick(new BreakRuleInfo
            {
                Name = playerData.Name,
                PersonaId = playerData.PersonaId,
                Reason1 = $"玩家{playerData.Name}KD超出，被踢出",
                Reason = $"KD limit {rule.MaxKD:0.00}",
                Type = BreakType.KD_Limit
            });

            return;
        }

        // 计算玩家KPM比条件
        if (rule.KPMFlag != 0 && playerData.Kill > rule.KPMFlag
            && playerData.KPM > rule.MaxKPM && rule.MaxKPM != 0.00f
            && !(WhiteList && DataSave.NowRule.WhiteListNoKPM))
        {
            // 限制玩家KPM
            TaskKick.AddKick(new BreakRuleInfo
            {
                Name = playerData.Name,
                PersonaId = playerData.PersonaId,
                Reason1 = $"玩家{playerData.Name}KPM超出，被踢出",
                Reason = $"KPM limit {rule.MaxKPM:0.00}",
                Type = BreakType.KPM_Limit
            });

            return;
        }

        // 从武器规则里遍历限制武器名称
        if (!(WhiteList && DataSave.NowRule.WhiteListNoW))
            for (int i = 0; i < rule.Custom_WeaponList.Count; i++)
            {
                var item = rule.Custom_WeaponList[i];

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
                            Reason1 = $"玩家{playerData.Name}使用限制武器[K 弹]，被踢出",
                            Reason = $"Weapon limit K_Bullet",
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
                            Reason1 = $"玩家{playerData.Name}使用限制武器[步枪手榴弹（破片）]，被踢出",
                            Reason = $"Weapon limit RGL_Frag",
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
                            Reason1 = $"玩家{playerData.Name}使用限制武器[步枪手榴弹（烟雾）]，被踢出",
                            Reason = $"Weapon limit RGL_Smoke",
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
                            Reason1 = $"玩家{playerData.Name}使用限制武器[步枪手榴弹（高爆）]，被踢出",
                            Reason = $"Weapon limit RGL_HE",
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
                        Reason1 = $"玩家{playerData.Name}使用限制武器[{InfoUtils.GetWeaponChsName(item)}]，被踢出",
                        Reason = $"Weapon limit {InfoUtils.GetWeaponShortTxt(item)}",
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
                Reason1 = $"玩家{playerData.Name}最小等级[{rule.MinRank:0}]超出，被踢出",
                Reason = $"Min rank limit {rule.MinRank:0}",
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
                Reason1 = $"玩家{playerData.Name}最大等级[{rule.MaxRank:0}]超出，被踢出",
                Reason = $"Max rank limit {rule.MaxRank:0}",
                Type = BreakType.Max_Rank_Limit
            });

            return;
        }
    }

    private static void AutoSwitchMap()
    {
        //没有开启自动T人
        if (!DataSave.AutoKickBreakPlayer)
            return;
        //正在切图
        else if (IsSwitching)
            return;
        //分数为0
        else if (DataSave.NowRule.ScoreSwitchMap == 0 || Globals.ServerHook.Team1Score == 0 || Globals.ServerHook.Team2Score == 0)
            return;
        //没有到达最小切图分数
        else if (DataSave.NowRule.ScoreStartSwitchMap != 0
            && (Globals.ServerHook.Team1Score <= DataSave.NowRule.ScoreStartSwitchMap
                || Globals.ServerHook.Team2Score <= DataSave.NowRule.ScoreStartSwitchMap))
            return;
        //没有到达切图分差
        else if (Math.Abs(Globals.ServerHook.Team1Score - Globals.ServerHook.Team2Score) <= DataSave.NowRule.ScoreSwitchMap)
            return;
        //超过不切图分数
        else if (DataSave.NowRule.ScoreNotSwitchMap != 0
            && (Globals.ServerHook.Team1Score > DataSave.NowRule.ScoreNotSwitchMap
                || Globals.ServerHook.Team2Score > DataSave.NowRule.ScoreSwitchMap))
            return;

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
                case 3:
                    if (DataSave.NowRule.SwitchMaps?.Count != 0)
                    {
                        List<int> indexs = new();
                        foreach (var item1 in DataSave.NowRule.SwitchMaps)
                        {
                            index = list.FindIndex(item => item.mapPrettyName == item1);
                            indexs.Add(index);
                        }
                        a = indexs[Random.Next(0, indexs.Count - 1)];
                    }
                    else
                    {
                        do
                        {
                            a = Random.Next(0, list.Count - 1);
                        }
                        while (a != index);
                    }
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
