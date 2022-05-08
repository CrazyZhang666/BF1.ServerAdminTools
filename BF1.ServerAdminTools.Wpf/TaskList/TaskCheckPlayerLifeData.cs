using BF1.ServerAdminTools.Common;
using BF1.ServerAdminTools.Common.API.BF1Server;
using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Wpf.Data;

namespace BF1.ServerAdminTools.Wpf.TaskList;

internal static class TasCheckPlayerLifeData
{
    public static bool NeedPause;
    public static void Start()
    {
        new Thread(Run)
        {
            Name = "TasCheckPlayerLifeData",
            IsBackground = true
        }.Start();
    }

    /// <summary>
    /// 检查玩家生涯
    /// </summary>
    private static void Run()
    {
        List<PlayerData> players = new();
        while (Tasks.IsRun)
        {
            while (NeedPause)
            {
                Thread.Sleep(100);
            }
            Thread.Sleep(100);
            // 自动踢出违规玩家
            if (DataSave.AutoKickBreakPlayer)
            {
                if (DataSave.NowRule.LifeMaxKD == 0 && DataSave.NowRule.LifeMaxKPM == 0
                    && DataSave.NowRule.LifeMaxWeaponStar == 0 && DataSave.NowRule.LifeMaxVehicleStar == 0)
                    continue;

                lock (Globals.PlayerDatas_Team1)
                {
                    players.AddRange(Globals.PlayerDatas_Team1.Values);
                }
                lock (Globals.PlayerDatas_Team2)
                {
                    players.AddRange(Globals.PlayerDatas_Team2.Values);
                }

                try
                {
                    Parallel.ForEach(players, item =>
                    {
                        if (NeedPause)
                            return;
                        CheckBreakLifePlayer(item);
                    });
                }
                catch (Exception e)
                {
                    Core.LogError("生涯数据获取错误", e);
                }
                Thread.Sleep(20000);
            }
        }
    }
    public static void CheckBreakLifePlayer(PlayerData data)
    {
        //管理员
        if (Globals.RspInfo != null)
        {
            if (Globals.RspInfo.owner?.personaId == data.PersonaId.ToString())
                return;
            if (Globals.RspInfo.adminList != null)
                if (Globals.RspInfo.adminList.FindIndex(a => a.personaId == data.PersonaId.ToString()) != -1)
                    return;
        }

        if (Globals.LocalPlayer.PersonaId == data.PersonaId)
            return;

        // 跳过白名单玩家
        if (DataSave.NowRule.Custom_WhiteList.Contains(data.Name))
            return;

        if (TaskKick.IsHave(data.PersonaId))
            return;

        lock (Globals.PlayerDatas_Team1)
        {
            lock (Globals.PlayerDatas_Team2)
            {
                //已经不在服务器了
                if (!Globals.PlayerDatas_Team1.ContainsKey(data.PersonaId) && !Globals.PlayerDatas_Team2.ContainsKey(data.PersonaId))
                    return;
            }
        }

        var resultTemp = ServerAPI.DetailedStatsByPersonaId(data.PersonaId.ToString()).Result;
        if (!resultTemp.IsSuccess)
        {
            return;
        }

        var career = resultTemp.Obj;

        // 拿到该玩家的生涯数据
        int kills = career.result.basicStats.kills;
        int deaths = career.result.basicStats.deaths;

        float kd = (float)Math.Round((double)kills / deaths, 2);
        float kpm = career.result.basicStats.kpm;
        int weaponStar = 0;
        int vehicleStar = 0;

        var res1 = ServerAPI.GetWeaponsByPersonaId(data.PersonaId.ToString()).Result;

        if (res1.IsSuccess)
            foreach (var item in res1.Obj.result)
            {
                foreach (var item1 in item.weapons)
                {
                    if (weaponStar < (int)item1.stats.values.kills)
                        weaponStar = (int)item1.stats.values.kills;
                }
            }

        var res2 = ServerAPI.GetVehiclesByPersonaId(data.PersonaId.ToString()).Result;

        if (res2.IsSuccess)
            foreach (var item in res2.Obj.result)
            {
                foreach (var item1 in item.vehicles)
                {
                    if (weaponStar < (int)item1.stats.values.kills)
                        weaponStar = (int)item1.stats.values.kills;
                }
            }

        weaponStar /= 100;
        vehicleStar /= 100;

        // 限制玩家生涯KD
        if (DataSave.NowRule.LifeMaxKD != 0 && kd > DataSave.NowRule.LifeMaxKD)
        {
            TaskKick.AddKick(new BreakRuleInfo
            {
                Name = data.Name,
                PersonaId = data.PersonaId,
                Reason = $"Life KD Limit {DataSave.NowRule.LifeMaxKD:0.00}",
                Type = BreakType.Life_KD_Limit
            });

            return;
        }

        // 限制玩家生涯KPM
        if (DataSave.NowRule.LifeMaxKPM != 0 && kpm > DataSave.NowRule.LifeMaxKPM)
        {
            TaskKick.AddKick(new BreakRuleInfo
            {
                Name = data.Name,
                PersonaId = data.PersonaId,
                Reason = $"Life KPM Limit {DataSave.NowRule.LifeMaxKPM:0.00}",
                Type = BreakType.Life_KPM_Limit
            });

            return;
        }

        // 限制玩家武器星级
        if (DataSave.NowRule.LifeMaxWeaponStar != 0 && weaponStar > DataSave.NowRule.LifeMaxWeaponStar)
        {
            TaskKick.AddKick(new BreakRuleInfo
            {
                Name = data.Name,
                PersonaId = data.PersonaId,
                Reason = $"Life Weapon Star Limit {DataSave.NowRule.LifeMaxWeaponStar:0}",
                Type = BreakType.Life_Weapon_Star_Limit
            });

            return;
        }

        // 限制玩家载具星级
        if (DataSave.NowRule.LifeMaxVehicleStar != 0 && vehicleStar > DataSave.NowRule.LifeMaxVehicleStar)
        {
            TaskKick.AddKick(new BreakRuleInfo
            {
                Name = data.Name,
                PersonaId = data.PersonaId,
                Reason = $"Life Vehicle Star Limit {DataSave.NowRule.LifeMaxVehicleStar:0}",
                Type = BreakType.Life_Vehicle_Star_Limit
            });

            return;
        }
    }
}
