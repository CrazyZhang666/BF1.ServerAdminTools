using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BF1.ServerAdminTools.Wpf.Data;
using BF1.ServerAdminTools.Common;
using BF1.ServerAdminTools.Common.Data;

namespace BF1.ServerAdminTools.Wpf.TaskList;

class TaskCheckWeapon
{
    public static bool NeedPause;
    public const int MaxKick = 5;

    public static Semaphore Semaphore = new(0, 5);

    private static Dictionary<long, int> WeaponCheckCount = new();

    private readonly static List<string> TANKERWeaponLock = new()
    {
        "U_MauserC96AutoPistol", "U_LugerArtillery", "U_PieperCarbine", "U_FrommerStopAuto", "U_C93Carbine", "U_Gewehr98_SI", "U_SawnOffShotgun", "U_M1911_Stock", "U_FN1903stock"
    };

    private readonly static List<string> TANKERWeapon1Lock = new()
    {
        "U_M1911", "U_LugerP08_VhKit"
    };

    private readonly static Dictionary<string, string[]> WeaponLock = new()
    {
        { CareerData.FLAMETHROWER.ID, new string[]{
            "U_FlameThrower", "", "", "", "", "", "U_Incendiary_Hero", "U_Club"
        } },
        { CareerData.ANTITANK.ID, new string[]{
            "U_TankGewehr", "U_SawnOffShotgun_FK", "U_TrPeriscope_Elite", "", "", "U_ATGrenade_VhKit", "U_FragGrenade", "U_Club"
        } },
        { CareerData.SENTRY.ID + "1", new string[]{
            "U_MaximMG0815", "", "", "", "", "", "U_FragGrenade", "U_Club"
        } },
        { CareerData.SENTRY.ID + "2", new string[]{
            "U_VillarPerosa", "", "", "", "", "", "U_FragGrenade", "U_Club"
        } },
        { CareerData.RUNNER.ID, new string[]{
            "U_MartiniGrenadeLauncher", "U_SawnOffShotgun_FK", "U_FlareGun_Elite", "", "", "U_SpawnBeacon", "U_SmokeGrenade", "U_ScoutKnife"
        } },
        { CareerData.RAIDER.ID, new string[]{
            "U_RoyalClub", "U_SmithWesson", "U_MedicBag", "U_GasMask", "", "U_SmokeGrenade", "U_FragGrenade", "U_RoyalClub"
        } },
        { CareerData.TANKER.ID, new string[]{
            "", "", "U_Wrench", "U_GasMask", "", "U_ATGrenade", "U_FragGrenade", "U_ScoutKnife"
        } },
        { CareerData.PILOT.ID, new string[]{
            "", "", "U_Wrench", "U_GasMask", "", "U_FlareGun", "U_FragGrenade", "U_ScoutKnife"
        } },
        { CareerData.CAVALRY.ID, new string[]{
            "U_WinchesterM1895_Horse", "U_LugerP08_VhKit", "U_AmmoPouch_Cav", "U_GasMask", "", "U_Bandages_Cav", "U_Grenade_AT_Cavalry", "U_Saber_Cav"
        } }
    };

    private readonly static Dictionary<string, string[]> WeaponLockName = new()
    {
        { CareerData.FLAMETHROWER.ID, new string[]{
            "喷火兵 Wex", "", "", "", "", "", "燃烧手榴弹", "棍棒"
        } },
        { CareerData.ANTITANK.ID, new string[]{
            "坦克猎手 Tankgewehr M1918", "短管霰弹枪", "战壕潜望镜", "", "", "反坦克手榴弹", "棒式手榴弹", "棍棒"
        } },
        { CareerData.SENTRY.ID + "1", new string[]{
            "哨兵 MG 08/15", "", "", "", "", "", "棒式手榴弹", "棍棒"
        } },
        { CareerData.SENTRY.ID + "2", new string[]{
            "哨兵 维拉·佩罗萨衝锋枪", "", "", "", "", "", "棒式手榴弹", "棍棒"
        } },
        { CareerData.RUNNER.ID, new string[]{
            "入侵者 马提尼·亨利步枪榴弹发射器", "短管霰弹枪", "信号枪 — 信号", "", "", "重生信标", "烟雾手榴弹", "U_ScoutKnife"
        } },
        { CareerData.RAIDER.ID, new string[]{
            "战壕奇兵 奇兵棒", "3 号左轮手枪", "医护箱", "防毒面具", "", "烟雾手榴弹", "U_FragGrenade", "U_RoyalClub"
        } },
        { CareerData.TANKER.ID, new string[]{
            "", "M1911", "维修工具", "防毒面具", "", "反坦克手榴弹", "棒式手榴弹", "U_ScoutKnife"
        } },
        { CareerData.PILOT.ID, new string[]{
            "", "M1911", "维修工具", "防毒面具", "", "信号枪（侦察）", "棒式手榴弹", "U_ScoutKnife"
        } },
        { CareerData.CAVALRY.ID, new string[]{
            "Russian 1895（骑兵）", "P08 手枪", "弹药包", "防毒面具", "", "绷带包", "轻型反坦克手榴弹", "U_Saber_Cav"
        } }
    };

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

    private static void WeaponCheck(PlayerData data)
    {
        if (TaskKick.IsHave(data.PersonaId))
        {
            WeaponCheckCount.Remove(data.PersonaId);
            return;
        }

        if (IsEmpty(data))
        {
            return;
        }

        if (data.Career == CareerData.SENTRY.ID)
        {
            if (data.InCar)
            {
                return;
            }
            string[] temp = WeaponLock[CareerData.SENTRY.ID + "1"];
            string[] temp1 = WeaponLock[CareerData.SENTRY.ID + "2"];
            string[] temp2 = WeaponLockName[CareerData.SENTRY.ID + "1"];
            string[] temp3 = WeaponLockName[CareerData.SENTRY.ID + "1"];
            if (data.WeaponS0 != temp[0] && data.WeaponS0 != temp1[0])
            {
                if (RunKick(data, $"玩家武器错误\n" +
                    $"兵种：{data.CareerCH}\n" +
                    $"武器栏：默认 -> 玩家\n" +
                    $"{temp2[0]}/{temp3[0]} -> {data.WeaponS0CH}"))
                {
                    return;
                }
            }
            if (data.WeaponS1 != temp[1]
                || data.WeaponS2 != temp[2]
                || data.WeaponS3 != temp[3]
                || data.WeaponS4 != temp[4]
                || data.WeaponS5 != temp[5]
                || data.WeaponS6 != temp[6]
                || data.WeaponS7 != temp[7])
            {
                if (RunKick(data, $"玩家武器错误\n" +
                    $"兵种：{data.CareerCH}\n" +
                    $"武器栏：默认 -> 玩家\n" +
                    $"{temp2[1]} -> {data.WeaponS1CH}\n" +
                    $"{temp2[2]} -> {data.WeaponS2CH}\n" +
                    $"{temp2[3]} -> {data.WeaponS3CH}\n" +
                    $"{temp2[4]} -> {data.WeaponS4CH}\n" +
                    $"{temp2[5]} -> {data.WeaponS5CH}\n" +
                    $"{temp2[6]} -> {data.WeaponS6CH}\n" +
                    $"{temp2[7]} -> {data.WeaponS7CH}"))
                {
                    return;
                }
            }
        }
        else if (data.Career == CareerData.TANKER.ID)
        {
            if (data.InCar)
            {
                return;
            }
            string[] temp = WeaponLock[CareerData.TANKER.ID];
            string[] temp1 = WeaponLockName[CareerData.TANKER.ID];
            if (data.WeaponS2 != temp[2]
                || data.WeaponS3 != temp[3]
                || data.WeaponS4 != temp[4]
                || data.WeaponS5 != temp[5]
                || data.WeaponS6 != temp[6]
                || data.WeaponS7 != temp[7])
            {
                if (RunKick(data, $"玩家武器错误\n" +
                    $"兵种：{data.CareerCH}\n" +
                    $"武器栏：默认 -> 玩家\n" +
                    $"{temp1[2]} -> {data.WeaponS2CH}\n" +
                    $"{temp1[3]} -> {data.WeaponS3CH}\n" +
                    $"{temp1[4]} -> {data.WeaponS4CH}\n" +
                    $"{temp1[5]} -> {data.WeaponS5CH}\n" +
                    $"{temp1[6]} -> {data.WeaponS6CH}\n" +
                    $"{temp1[7]} -> {data.WeaponS7CH}"))
                {
                    return;
                }
            }

            if (!TANKERWeaponLock.Contains(data.WeaponS0))
            {
                if (RunKick(data, $"玩家武器错误\n" +
                        $"兵种：{data.CareerCH}\n" +
                        $"主武器栏：{data.WeaponS0CH}"))
                {
                    return;
                }
            }

            if (!TANKERWeapon1Lock.Contains(data.WeaponS1))
            {
                if (RunKick(data, $"玩家武器错误\n" +
                        $"兵种：{data.CareerCH}\n" +
                        $"副武器栏：{data.WeaponS0CH}"))
                {
                    return;
                }
            }
        }
        else if (data.Career == CareerData.PILOT.ID)
        {
            if (data.InCar)
            {
                return;
            }
            string[] temp = WeaponLock[CareerData.PILOT.ID];
            string[] temp1 = WeaponLockName[CareerData.PILOT.ID];
            if (data.WeaponS2 != temp[2]
                || data.WeaponS3 != temp[3]
                || data.WeaponS4 != temp[4]
                || data.WeaponS5 != temp[5]
                || data.WeaponS6 != temp[6]
                || data.WeaponS7 != temp[7])
            {
                if (RunKick(data, $"玩家武器错误\n" +
                    $"兵种：{data.CareerCH}\n" +
                    $"武器栏：默认 -> 玩家\n" +
                    $"{temp1[2]} -> {data.WeaponS2CH}\n" +
                    $"{temp1[3]} -> {data.WeaponS3CH}\n" +
                    $"{temp1[4]} -> {data.WeaponS4CH}\n" +
                    $"{temp1[5]} -> {data.WeaponS5CH}\n" +
                    $"{temp1[6]} -> {data.WeaponS6CH}\n" +
                    $"{temp1[7]} -> {data.WeaponS7CH}"))
                {
                    return;
                }
            }

            if (!TANKERWeaponLock.Contains(data.WeaponS0))
            {
                if (RunKick(data, $"玩家武器错误\n" +
                        $"兵种：{data.CareerCH}\n" +
                        $"主武器栏：{data.WeaponS0CH}"))
                {
                    return;
                }
            }

            if (!TANKERWeapon1Lock.Contains(data.WeaponS1))
            {
                if (RunKick(data, $"玩家武器错误\n" +
                        $"兵种：{data.CareerCH}\n" +
                        $"副武器栏：{data.WeaponS0CH}"))
                {
                    return;
                }
            }
        }
        else if (data.Career == CareerData.CAVALRY.ID)
        {
            if (data.InCar)
            {
                return;
            }
            string[] temp = WeaponLock[CareerData.CAVALRY.ID];
            string[] temp1 = WeaponLockName[CareerData.CAVALRY.ID];
            if (data.WeaponS0 != temp[0]
                || data.WeaponS2 != temp[2]
                || data.WeaponS3 != temp[3]
                || data.WeaponS4 != temp[4]
                || data.WeaponS5 != temp[5]
                || data.WeaponS6 != temp[6]
                || data.WeaponS7 != temp[7])
            {
                if (RunKick(data, $"玩家武器错误\n" +
                    $"兵种：{data.CareerCH}\n" +
                    $"武器栏：默认 -> 玩家\n" +
                    $"{temp1[0]} -> {data.WeaponS0CH}\n" +
                    $"{temp1[2]} -> {data.WeaponS2CH}\n" +
                    $"{temp1[3]} -> {data.WeaponS3CH}\n" +
                    $"{temp1[4]} -> {data.WeaponS4CH}\n" +
                    $"{temp1[5]} -> {data.WeaponS5CH}\n" +
                    $"{temp1[6]} -> {data.WeaponS6CH}\n" +
                    $"{temp1[7]} -> {data.WeaponS7CH}"))
                {
                    return;
                }
            }

            if (!TANKERWeapon1Lock.Contains(data.WeaponS1))
            {
                if (RunKick(data, $"玩家武器错误\n" +
                        $"兵种：{data.CareerCH}\n" +
                        $"副武器栏：{data.WeaponS0CH}"))
                {
                    return;
                }
            }
        }
        else if (data.Career == CareerData.FLAMETHROWER.ID)
        {
            if (data.InCar)
            {
                return;
            }
            string[] temp = WeaponLock[CareerData.FLAMETHROWER.ID];
            string[] temp1 = WeaponLockName[CareerData.FLAMETHROWER.ID];
            if (data.WeaponS0 != temp[0]
                || data.WeaponS1 != temp[1]
                || data.WeaponS2 != temp[2]
                || data.WeaponS3 != temp[3]
                || data.WeaponS4 != temp[4]
                || data.WeaponS5 != temp[5]
                || data.WeaponS6 != temp[6]
                || data.WeaponS7 != temp[7])
            {
                if (RunKick(data, $"玩家武器错误\n" +
                    $"兵种：{data.CareerCH}\n" +
                    $"武器栏：默认 -> 玩家\n" +
                    $"{temp1[0]} -> {data.WeaponS0CH}\n" +
                    $"{temp1[1]} -> {data.WeaponS1CH}\n" +
                    $"{temp1[2]} -> {data.WeaponS2CH}\n" +
                    $"{temp1[3]} -> {data.WeaponS3CH}\n" +
                    $"{temp1[4]} -> {data.WeaponS4CH}\n" +
                    $"{temp1[5]} -> {data.WeaponS5CH}\n" +
                    $"{temp1[6]} -> {data.WeaponS6CH}\n" +
                    $"{temp1[7]} -> {data.WeaponS7CH}"))
                {
                    return;
                }
            }
        }
        else if (data.Career == CareerData.ANTITANK.ID)
        {
            if (data.InCar)
            {
                return;
            }
            string[] temp = WeaponLock[CareerData.ANTITANK.ID];
            string[] temp1 = WeaponLockName[CareerData.ANTITANK.ID];
            if (data.WeaponS0 != temp[0]
                || data.WeaponS1 != temp[1]
                || data.WeaponS2 != temp[2]
                || data.WeaponS3 != temp[3]
                || data.WeaponS4 != temp[4]
                || data.WeaponS5 != temp[5]
                || data.WeaponS6 != temp[6]
                || data.WeaponS7 != temp[7])
            {
                if (RunKick(data, $"玩家武器错误\n" +
                    $"兵种：{data.CareerCH}\n" +
                    $"武器栏：默认 -> 玩家\n" +
                    $"{temp1[0]} -> {data.WeaponS0CH}\n" +
                    $"{temp1[1]} -> {data.WeaponS1CH}\n" +
                    $"{temp1[2]} -> {data.WeaponS2CH}\n" +
                    $"{temp1[3]} -> {data.WeaponS3CH}\n" +
                    $"{temp1[4]} -> {data.WeaponS4CH}\n" +
                    $"{temp1[5]} -> {data.WeaponS5CH}\n" +
                    $"{temp1[6]} -> {data.WeaponS6CH}\n" +
                    $"{temp1[7]} -> {data.WeaponS7CH}"))
                {
                    return;
                }
            }
        }
        else if (data.Career == CareerData.RUNNER.ID)
        {
            if (data.InCar)
            {
                return;
            }
            string[] temp = WeaponLock[CareerData.RUNNER.ID];
            string[] temp1 = WeaponLockName[CareerData.RUNNER.ID];
            if (data.WeaponS0 != temp[0]
                || data.WeaponS1 != temp[1]
                || data.WeaponS2 != temp[2]
                || data.WeaponS3 != temp[3]
                || data.WeaponS4 != temp[4]
                || data.WeaponS5 != temp[5]
                || data.WeaponS6 != temp[6]
                || data.WeaponS7 != temp[7])
            {
                if (RunKick(data, $"玩家武器错误\n" +
                    $"兵种：{data.CareerCH}\n" +
                    $"武器栏：默认 -> 玩家\n" +
                    $"{temp1[0]} -> {data.WeaponS0CH}\n" +
                    $"{temp1[1]} -> {data.WeaponS1CH}\n" +
                    $"{temp1[2]} -> {data.WeaponS2CH}\n" +
                    $"{temp1[3]} -> {data.WeaponS3CH}\n" +
                    $"{temp1[4]} -> {data.WeaponS4CH}\n" +
                    $"{temp1[5]} -> {data.WeaponS5CH}\n" +
                    $"{temp1[6]} -> {data.WeaponS6CH}\n" +
                    $"{temp1[7]} -> {data.WeaponS7CH}"))
                {
                    return;
                }
            }
        }
        else if (data.Career == CareerData.RAIDER.ID)
        {
            if (data.InCar)
            {
                return;
            }
            string[] temp = WeaponLock[CareerData.RAIDER.ID];
            string[] temp1 = WeaponLockName[CareerData.RAIDER.ID];
            if (data.WeaponS0 != temp[0]
                || data.WeaponS1 != temp[1]
                || data.WeaponS2 != temp[2]
                || data.WeaponS3 != temp[3]
                || data.WeaponS4 != temp[4]
                || data.WeaponS5 != temp[5]
                || data.WeaponS6 != temp[6]
                || data.WeaponS7 != temp[7])
            {
                if (RunKick(data, $"玩家武器错误\n" +
                    $"兵种：{data.CareerCH}\n" +
                    $"武器栏：默认 -> 玩家\n" +
                    $"{temp1[0]} -> {data.WeaponS0CH}\n" +
                    $"{temp1[1]} -> {data.WeaponS1CH}\n" +
                    $"{temp1[2]} -> {data.WeaponS2CH}\n" +
                    $"{temp1[3]} -> {data.WeaponS3CH}\n" +
                    $"{temp1[4]} -> {data.WeaponS4CH}\n" +
                    $"{temp1[5]} -> {data.WeaponS5CH}\n" +
                    $"{temp1[6]} -> {data.WeaponS6CH}\n" +
                    $"{temp1[7]} -> {data.WeaponS7CH}"))
                {
                    return;
                }
            }
        }
        else if (data.Career == CareerData.ASSAULT.ID)
        {
            if (data.InCar)
            {
                return;
            }
            string[] temp = WeaponLock[CareerData.RAIDER.ID];
            string[] temp1 = WeaponLockName[CareerData.RAIDER.ID];
            if (data.WeaponS0 != temp[0]
                || data.WeaponS1 != temp[1]
                || data.WeaponS2 != temp[2]
                || data.WeaponS3 != temp[3]
                || data.WeaponS4 != temp[4]
                || data.WeaponS5 != temp[5]
                || data.WeaponS6 != temp[6]
                || data.WeaponS7 != temp[7])
            {
                if (RunKick(data, $"玩家武器错误\n" +
                    $"兵种：{data.CareerCH}\n" +
                    $"武器栏：默认 -> 玩家\n" +
                    $"{temp1[0]} -> {data.WeaponS0CH}\n" +
                    $"{temp1[1]} -> {data.WeaponS1CH}\n" +
                    $"{temp1[2]} -> {data.WeaponS2CH}\n" +
                    $"{temp1[3]} -> {data.WeaponS3CH}\n" +
                    $"{temp1[4]} -> {data.WeaponS4CH}\n" +
                    $"{temp1[5]} -> {data.WeaponS5CH}\n" +
                    $"{temp1[6]} -> {data.WeaponS6CH}\n" +
                    $"{temp1[7]} -> {data.WeaponS7CH}"))
                {
                    return;
                }
            }
        }
    }

    private static bool RunKick(PlayerData data, string reason)
    {
        if (WeaponCheckCount.ContainsKey(data.PersonaId))
        {
            WeaponCheckCount[data.PersonaId]++;
        }
        else
        {
            WeaponCheckCount.Add(data.PersonaId, 1);
        }
        if (WeaponCheckCount[data.PersonaId] >= 5)
        {
            WeaponCheckCount.Remove(data.PersonaId);
            TaskKick.AddKick(new()
            {
                Name = data.Name,
                PersonaId = data.PersonaId,
                Type = BreakType.Error_Weapon,
                Time = DateTime.Now,
                Reason = "Weapon Error",
                Reason1 = reason
            });
            return true;
        }
        return false;
    }

    private static void StartCheck()
    {
        foreach (var item in Globals.PlayerDatas_Team2.Values)
        {
            WeaponCheck(item);
        }
        foreach (var item in Globals.PlayerDatas_Team1.Values)
        {
            WeaponCheck(item);
        }
    }
}
