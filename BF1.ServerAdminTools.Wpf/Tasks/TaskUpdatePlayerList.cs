using BF1.ServerAdminTools.Common;
using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Common.Extension;
using BF1.ServerAdminTools.Common.Models;
using BF1.ServerAdminTools.Common.Utils;
using BF1.ServerAdminTools.Common.Views;

namespace BF1.ServerAdminTools.Wpf.Tasks;

internal static class TaskUpdatePlayerList
{
    public static Semaphore Semaphore = new(0, 5);

    public static void Start()
    {
        new Thread(Run)
        {
            Name = "TaskUpdatePlayerList",
            IsBackground = true
        }.Start();
    }

    /// <summary>
    /// 刷新计分板
    /// </summary>
    private static void Run()
    {
        while (Tasks.IsRun)
        {
            Semaphore.WaitOne();
            if (!Tasks.IsRun)
                return;
            //////////////////////////////// 自己数据 ////////////////////////////////

            ScoreView.PlayerOtherModel.MySelfTeamID = $"队伍ID : {Globals.LocalPlayer.TeamID}";

            if (string.IsNullOrWhiteSpace(Globals.LocalPlayer.PlayerName))
            {
                ScoreView.PlayerOtherModel.MySelfName = "玩家ID : 未知";
            }
            else
            {
                ScoreView.PlayerOtherModel.MySelfName = $"玩家ID : {Globals.LocalPlayer.PlayerName}";
            }

            ////////////////////////////////////////////////////////////////////////////////

            ScoreView.ServerInfoModel.ServerName = $"服务器名称 : {Globals.ServerHook.ServerName}  |  GameID : {Globals.ServerHook.ServerID}";

            ScoreView.ServerInfoModel.ServerTime = Globals.ServerHook.ServerTimeS = PlayerUtils.SecondsToMMSS(Globals.ServerHook.ServerTime);

            if (Globals.ServerHook.Team1Score >= 0 && Globals.ServerHook.Team1Score <= 1000 &&
                Globals.ServerHook.Team2Score >= 0 && Globals.ServerHook.Team2Score <= 1000)
            {
                ScoreView.ServerInfoModel.Team1ScoreWidth = Globals.ServerHook.Team1Score / 6.25;
                ScoreView.ServerInfoModel.Team2ScoreWidth = Globals.ServerHook.Team2Score / 6.25;

                ScoreView.ServerInfoModel.Team1Score = $"{Globals.ServerHook.Team1Score}";
                ScoreView.ServerInfoModel.Team2Score = $"{Globals.ServerHook.Team2Score}";
            }
            else if (Globals.ServerHook.Team1Score > 1000 && Globals.ServerHook.Team1Score <= 2000 ||
                Globals.ServerHook.Team2Score > 1000 && Globals.ServerHook.Team2Score <= 2000)
            {
                ScoreView.ServerInfoModel.Team1ScoreWidth = Globals.ServerHook.Team1Score / 12.5;
                ScoreView.ServerInfoModel.Team2ScoreWidth = Globals.ServerHook.Team2Score / 12.5;

                ScoreView.ServerInfoModel.Team1Score = $"{Globals.ServerHook.Team1Score}";
                ScoreView.ServerInfoModel.Team2Score = $"{Globals.ServerHook.Team2Score}";
            }
            else
            {
                ScoreView.ServerInfoModel.Team1ScoreWidth = 0;
                ScoreView.ServerInfoModel.Team2ScoreWidth = 0;

                ScoreView.ServerInfoModel.Team1Score = "0";
                ScoreView.ServerInfoModel.Team2Score = "0";
            }

            ScoreView.ServerInfoModel.Team1FromeFlag = $"从旗帜获取的得分 : {Globals.ServerHook.Team1FromeFlag}";
            ScoreView.ServerInfoModel.Team1FromeKill = $"从击杀获取的得分 : {Globals.ServerHook.Team1FromeKill}";

            ScoreView.ServerInfoModel.Team2FromeFlag = $"从旗帜获取的得分 : {Globals.ServerHook.Team2FromeFlag}";
            ScoreView.ServerInfoModel.Team2FromeKill = $"从击杀获取的得分 : {Globals.ServerHook.Team2FromeKill}";

            ScoreView.ServerInfoModel.Team1Info = $"已部署/队伍1人数 : {Globals.StatisticData_Team1.PlayerCount} / {Globals.StatisticData_Team1.MaxPlayerCount}  |  150等级人数 : {Globals.StatisticData_Team1.Rank150PlayerCount}  |  总击杀数 : {Globals.StatisticData_Team1.AllKillCount}  |  总死亡数 : {Globals.StatisticData_Team1.AllDeadCount}";
            ScoreView.ServerInfoModel.Team2Info = $"已部署/队伍2人数 : {Globals.StatisticData_Team2.PlayerCount} / {Globals.StatisticData_Team2.MaxPlayerCount}  |  150等级人数 : {Globals.StatisticData_Team2.Rank150PlayerCount}  |  总击杀数 : {Globals.StatisticData_Team2.AllKillCount}  |  总死亡数 : {Globals.StatisticData_Team2.AllDeadCount}";

            ScoreView.PlayerOtherModel.ServerPlayerCountInfo = $"服务器总人数 : {Globals.StatisticData_Team1.MaxPlayerCount + Globals.StatisticData_Team2.MaxPlayerCount}";

            ////////////////////////////////////////////////////////////////////////////////

            Application.Current?.Dispatcher.Invoke(() =>
            {
                UpdateDataGridTeam1();
                UpdateDataGridTeam2();

                ScoreView.DataGrid_PlayerList_Team1.Sort();
                ScoreView.DataGrid_PlayerList_Team2.Sort();
            });

            TaskTick.Done();
        }
    }



    // 更新 DataGrid 队伍1
    private static void UpdateDataGridTeam1()
    {
        if (Globals.PlayerDatas_Team1.Count == 0 && ScoreView.DataGrid_PlayerList_Team1.Count != 0)
        {
            ScoreView.DataGrid_PlayerList_Team1.Clear();
        }

        if (Globals.PlayerDatas_Team1.Count != 0)
        {
            // 更新DataGrid中现有的玩家数据，并把DataGrid中已经不在服务器的玩家清除
            List<PlayerListModel> list = new();
            foreach (var item in ScoreView.DataGrid_PlayerList_Team1)
            {
                if (Globals.PlayerDatas_Team1.ContainsKey(item.PersonaId))
                {
                    item.Rank = Globals.PlayerDatas_Team1[item.PersonaId].Rank;
                    item.Clan = Globals.PlayerDatas_Team1[item.PersonaId].Clan;
                    item.Admin = Globals.PlayerDatas_Team1[item.PersonaId].Admin ? "✔" : "";
                    item.VIP = Globals.PlayerDatas_Team1[item.PersonaId].VIP ? "✔" : "";
                    item.SquadId = Globals.PlayerDatas_Team1[item.PersonaId].SquadId;
                    item.Kill = Globals.PlayerDatas_Team1[item.PersonaId].Kill;
                    item.Dead = Globals.PlayerDatas_Team1[item.PersonaId].Dead;
                    item.KD = Globals.PlayerDatas_Team1[item.PersonaId].KD.ToString("0.00");
                    item.KPM = Globals.PlayerDatas_Team1[item.PersonaId].KPM.ToString("0.00");
                    item.Score = Globals.PlayerDatas_Team1[item.PersonaId].Score;
                    item.WeaponS0 = DataSave.IsShowCHSWeaponName ?
                       Globals.PlayerDatas_Team1[item.PersonaId].WeaponS0CH :
                       Globals.PlayerDatas_Team1[item.PersonaId].WeaponS0;
                    item.WeaponS1 = DataSave.IsShowCHSWeaponName ?
                       Globals.PlayerDatas_Team1[item.PersonaId].WeaponS1CH :
                       Globals.PlayerDatas_Team1[item.PersonaId].WeaponS1;
                    item.WeaponS2 = DataSave.IsShowCHSWeaponName ?
                       Globals.PlayerDatas_Team1[item.PersonaId].WeaponS2CH :
                       Globals.PlayerDatas_Team1[item.PersonaId].WeaponS2;
                    item.WeaponS3 = DataSave.IsShowCHSWeaponName ?
                       Globals.PlayerDatas_Team1[item.PersonaId].WeaponS3CH :
                       Globals.PlayerDatas_Team1[item.PersonaId].WeaponS3;
                    item.WeaponS4 = DataSave.IsShowCHSWeaponName ?
                       Globals.PlayerDatas_Team1[item.PersonaId].WeaponS4CH :
                       Globals.PlayerDatas_Team1[item.PersonaId].WeaponS4;
                    item.WeaponS5 = DataSave.IsShowCHSWeaponName ?
                       Globals.PlayerDatas_Team1[item.PersonaId].WeaponS5CH :
                       Globals.PlayerDatas_Team1[item.PersonaId].WeaponS5;
                    item.WeaponS6 = DataSave.IsShowCHSWeaponName ?
                       Globals.PlayerDatas_Team1[item.PersonaId].WeaponS6CH :
                       Globals.PlayerDatas_Team1[item.PersonaId].WeaponS6;
                    item.WeaponS7 = DataSave.IsShowCHSWeaponName ?
                       Globals.PlayerDatas_Team1[item.PersonaId].WeaponS7CH :
                       Globals.PlayerDatas_Team1[item.PersonaId].WeaponS7;
                }
                else
                {
                    list.Add(item);
                }
            }

            list.ForEach(item => ScoreView.DataGrid_PlayerList_Team1.Remove(item));

            // 增加DataGrid没有的玩家数据
            foreach (var item in Globals.PlayerDatas_Team1.Values)
            {
                if (!ScoreView.DataGrid_PlayerList_Team1.Where(val => val.Name == item.Name).Any())
                {
                    ScoreView.DataGrid_PlayerList_Team1.Add(new PlayerListModel()
                    {
                        Rank = item.Rank,
                        Clan = item.Clan,
                        Name = item.Name,
                        PersonaId = item.PersonaId,
                        Admin = item.Admin ? "✔" : "",
                        VIP = item.VIP ? "✔" : "",
                        SquadId = item.SquadId,
                        Kill = item.Kill,
                        Dead = item.Dead,
                        KD = item.KD.ToString("0.00"),
                        KPM = item.KPM.ToString("0.00"),
                        Score = item.Score,
                        WeaponS0 = DataSave.IsShowCHSWeaponName ? item.WeaponS0CH : item.WeaponS0,
                        WeaponS1 = DataSave.IsShowCHSWeaponName ? item.WeaponS1CH : item.WeaponS1,
                        WeaponS2 = DataSave.IsShowCHSWeaponName ? item.WeaponS2CH : item.WeaponS2,
                        WeaponS3 = DataSave.IsShowCHSWeaponName ? item.WeaponS3CH : item.WeaponS3,
                        WeaponS4 = DataSave.IsShowCHSWeaponName ? item.WeaponS4CH : item.WeaponS4,
                        WeaponS5 = DataSave.IsShowCHSWeaponName ? item.WeaponS5CH : item.WeaponS5,
                        WeaponS6 = DataSave.IsShowCHSWeaponName ? item.WeaponS6CH : item.WeaponS6,
                        WeaponS7 = DataSave.IsShowCHSWeaponName ? item.WeaponS7CH : item.WeaponS7,
                    });
                }
            }

            // 修正序号
            for (int i = 0; i < ScoreView.DataGrid_PlayerList_Team1.Count; i++)
            {
                ScoreView.DataGrid_PlayerList_Team1[i].Index = i + 1;
            }
        }
    }

    // 更新 DataGrid 队伍2
    private static void UpdateDataGridTeam2()
    {
        if (Globals.PlayerDatas_Team2.Count == 0 && ScoreView.DataGrid_PlayerList_Team2.Count != 0)
        {
            ScoreView.DataGrid_PlayerList_Team2.Clear();
        }

        if (Globals.PlayerDatas_Team2.Count != 0)
        {
            // 更新DataGrid中现有的玩家数据，并把DataGrid中已经不在服务器的玩家清除
            List<PlayerListModel> list = new();
            foreach (var item in ScoreView.DataGrid_PlayerList_Team2)
            {
                if (Globals.PlayerDatas_Team2.ContainsKey(item.PersonaId))
                {
                    item.Rank = Globals.PlayerDatas_Team2[item.PersonaId].Rank;
                    item.Clan = Globals.PlayerDatas_Team2[item.PersonaId].Clan;
                    item.Admin = Globals.PlayerDatas_Team2[item.PersonaId].Admin ? "✔" : "";
                    item.VIP = Globals.PlayerDatas_Team2[item.PersonaId].VIP ? "✔" : "";
                    item.SquadId = Globals.PlayerDatas_Team2[item.PersonaId].SquadId;
                    item.Kill = Globals.PlayerDatas_Team2[item.PersonaId].Kill;
                    item.Dead = Globals.PlayerDatas_Team2[item.PersonaId].Dead;
                    item.KD = Globals.PlayerDatas_Team2[item.PersonaId].KD.ToString("0.00");
                    item.KPM = Globals.PlayerDatas_Team2[item.PersonaId].KPM.ToString("0.00");
                    item.Score = Globals.PlayerDatas_Team2[item.PersonaId].Score;
                    item.WeaponS0 = DataSave.IsShowCHSWeaponName ?
                       Globals.PlayerDatas_Team2[item.PersonaId].WeaponS0CH :
                       Globals.PlayerDatas_Team2[item.PersonaId].WeaponS0;
                    item.WeaponS1 = DataSave.IsShowCHSWeaponName ?
                       Globals.PlayerDatas_Team2[item.PersonaId].WeaponS1CH :
                       Globals.PlayerDatas_Team2[item.PersonaId].WeaponS1;
                    item.WeaponS2 = DataSave.IsShowCHSWeaponName ?
                       Globals.PlayerDatas_Team2[item.PersonaId].WeaponS2CH :
                       Globals.PlayerDatas_Team2[item.PersonaId].WeaponS2;
                    item.WeaponS3 = DataSave.IsShowCHSWeaponName ?
                       Globals.PlayerDatas_Team2[item.PersonaId].WeaponS3CH :
                       Globals.PlayerDatas_Team2[item.PersonaId].WeaponS3;
                    item.WeaponS4 = DataSave.IsShowCHSWeaponName ?
                       Globals.PlayerDatas_Team2[item.PersonaId].WeaponS4CH :
                       Globals.PlayerDatas_Team2[item.PersonaId].WeaponS4;
                    item.WeaponS5 = DataSave.IsShowCHSWeaponName ?
                       Globals.PlayerDatas_Team2[item.PersonaId].WeaponS5CH :
                       Globals.PlayerDatas_Team2[item.PersonaId].WeaponS5;
                    item.WeaponS6 = DataSave.IsShowCHSWeaponName ?
                       Globals.PlayerDatas_Team2[item.PersonaId].WeaponS6CH :
                       Globals.PlayerDatas_Team2[item.PersonaId].WeaponS6;
                    item.WeaponS7 = DataSave.IsShowCHSWeaponName ?
                       Globals.PlayerDatas_Team2[item.PersonaId].WeaponS7CH :
                       Globals.PlayerDatas_Team2[item.PersonaId].WeaponS7;
                }
                else
                {
                    list.Add(item);
                }
            }

            list.ForEach(item => ScoreView.DataGrid_PlayerList_Team2.Remove(item));

            // 增加DataGrid没有的玩家数据
            foreach (var item in Globals.PlayerDatas_Team2.Values)
            {
                if (!ScoreView.DataGrid_PlayerList_Team2.Where(val => val.Name == item.Name).Any())
                {
                    ScoreView.DataGrid_PlayerList_Team2.Add(new PlayerListModel()
                    {
                        Rank = item.Rank,
                        Clan = item.Clan,
                        Name = item.Name,
                        PersonaId = item.PersonaId,
                        Admin = item.Admin ? "✔" : "",
                        VIP = item.VIP ? "✔" : "",
                        SquadId = item.SquadId,
                        Kill = item.Kill,
                        Dead = item.Dead,
                        KD = item.KD.ToString("0.00"),
                        KPM = item.KPM.ToString("0.00"),
                        Score = item.Score,
                        WeaponS0 = DataSave.IsShowCHSWeaponName ? item.WeaponS0CH : item.WeaponS0,
                        WeaponS1 = DataSave.IsShowCHSWeaponName ? item.WeaponS1CH : item.WeaponS1,
                        WeaponS2 = DataSave.IsShowCHSWeaponName ? item.WeaponS2CH : item.WeaponS2,
                        WeaponS3 = DataSave.IsShowCHSWeaponName ? item.WeaponS3CH : item.WeaponS3,
                        WeaponS4 = DataSave.IsShowCHSWeaponName ? item.WeaponS4CH : item.WeaponS4,
                        WeaponS5 = DataSave.IsShowCHSWeaponName ? item.WeaponS5CH : item.WeaponS5,
                        WeaponS6 = DataSave.IsShowCHSWeaponName ? item.WeaponS6CH : item.WeaponS6,
                        WeaponS7 = DataSave.IsShowCHSWeaponName ? item.WeaponS7CH : item.WeaponS7,
                    });
                }
            }

            // 修正序号
            for (int i = 0; i < ScoreView.DataGrid_PlayerList_Team2.Count; i++)
            {
                ScoreView.DataGrid_PlayerList_Team2[i].Index = i + 1;
            }
        }
    }
}
