using BF1.ServerAdminTools.Common;
using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Common.Utils;
using DotNetty.Buffers;
using System.Text;

namespace BF1.ServerAdminTools.Netty;

internal static class EncodePack
{
    /// <summary>
    /// 写字符串
    /// </summary>
    /// <param name="buff"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    internal static IByteBuffer WriteString(this IByteBuffer buff, string data)
    {
        byte[] temp = Encoding.UTF8.GetBytes(data);
        buff.WriteInt(temp.Length);
        buff.WriteBytes(temp);
        return buff;
    }
    /// <summary>
    /// 写工具状态
    /// </summary>
    /// <param name="buff"></param>
    internal static void State(IByteBuffer buff)
    {
        buff.WriteByte(Globals.IsGameRun ? 0xff : 0x00)
            .WriteByte(Globals.IsToolInit ? 0xff : 0x00);
    }
    /// <summary>
    /// 写检查后状态
    /// </summary>
    /// <param name="buff"></param>

    internal static void Check(IByteBuffer buff)
    {
        buff.WriteByte(Core.IsGameRun() ? 0xff : 0x00)
            .WriteByte(Core.HookInit() ? 0xff : 0x00);
    }
    /// <summary>
    /// 写登录账户信息
    /// </summary>
    /// <param name="buff"></param>
    internal static void Id(IByteBuffer buff)
    {
        buff.WriteString(Globals.Config.Remid)
            .WriteString(Globals.Config.Sid)
            .WriteString(Globals.Config.SessionId)
            .WriteString(Globals.Config.GameId)
            .WriteString(Globals.Config.ServerId)
            .WriteString(Globals.Config.PersistedGameId);
    }
    /// <summary>
    /// 写加入服务器的信息
    /// </summary>
    /// <param name="buff"></param>
    internal static void ServerInfo(IByteBuffer buff)
    {
        buff.WriteString(Globals.ServerHook.ServerName)
            .WriteLong(Globals.ServerHook.ServerID)
            .WriteFloat(Globals.ServerHook.ServerTime)
            .WriteString(Globals.ServerHook.ServerTimeS)
            .WriteInt(Globals.ServerHook.Team1Score)
            .WriteInt(Globals.ServerHook.Team2Score)
            .WriteInt(Globals.ServerHook.Team1FromeKill)
            .WriteInt(Globals.ServerHook.Team2FromeKill)
            .WriteInt(Globals.ServerHook.Team1FromeFlag)
            .WriteInt(Globals.ServerHook.Team2FromeFlag)
            .WriteInt(Globals.StatisticData_Team1.MaxPlayerCount)
            .WriteInt(Globals.StatisticData_Team1.PlayerCount)
            .WriteInt(Globals.StatisticData_Team1.Rank150PlayerCount)
            .WriteInt(Globals.StatisticData_Team1.AllKillCount)
            .WriteInt(Globals.StatisticData_Team1.AllDeadCount)
            .WriteInt(Globals.StatisticData_Team2.MaxPlayerCount)
            .WriteInt(Globals.StatisticData_Team2.PlayerCount)
            .WriteInt(Globals.StatisticData_Team2.Rank150PlayerCount)
            .WriteInt(Globals.StatisticData_Team2.AllKillCount)
            .WriteInt(Globals.StatisticData_Team2.AllDeadCount);
        if (Globals.ServerDetailed != null)
        {
            buff.WriteBoolean(true)
                .WriteString(Globals.ServerDetailed.currentMap)
                .WriteString(Globals.ServerDetailed.currentMapImage)
                .WriteString(Globals.ServerDetailed.teams.teamOne.key)
                .WriteString(Globals.ServerDetailed.teams.teamOne.image)
                .WriteString(Globals.ServerDetailed.teams.teamTwo.key)
                .WriteString(Globals.ServerDetailed.teams.teamTwo.image)
                .WriteString(ChsUtils.ToSimplifiedChinese(Globals.ServerDetailed.mode));
        }
        else
        {
            buff.WriteBoolean(false);
        }
    }
    /// <summary>
    /// 写玩家信息
    /// </summary>
    /// <param name="buff"></param>
    /// <param name="player"></param>
    internal static void Player(IByteBuffer buff, PlayerData player)
    {
        buff.WriteBoolean(player.Admin)
            .WriteBoolean(player.VIP)
            .WriteByte(player.Mark)
            .WriteInt(player.TeamID)
            .WriteByte(player.Spectator)
            .WriteString(player.Clan)
            .WriteString(player.Name)
            .WriteLong(player.PersonaId)
            .WriteString(player.SquadId)
            .WriteInt(player.Rank)
            .WriteInt(player.Dead)
            .WriteInt(player.Score)
            .WriteInt(player.Kill)
            .WriteFloat(player.KD)
            .WriteFloat(player.KPM)
            .WriteString(player.WeaponS0CH);
    }
    /// <summary>
    /// 写玩家列表
    /// </summary>
    /// <param name="buff"></param>
    /// <param name="list"></param>
    internal static void PlayerList(IByteBuffer buff, List<PlayerData> list)
    {
        buff.WriteInt(list.Count);
        foreach (var item in list)
        {
            Player(buff, item);
        }
    }
    /// <summary>
    /// 写服务器计分板数据
    /// </summary>
    /// <param name="buff"></param>
    internal static void ServerScore(IByteBuffer buff)
    {
        if (Globals.ServerInfo == null)
        {
            buff.WriteBoolean(false);
            return;
        }
        buff.WriteBoolean(true);
        ServerInfo(buff);
        List<PlayerData> list;
        lock (Globals.PlayerDatas_Team1)
        {
            list = new(Globals.PlayerDatas_Team1.Values);
        }
        if (list.Count != 0)
        {
            buff.WriteBoolean(true);
            PlayerList(buff, list);
        }
        else
        {
            buff.WriteBoolean(false);
        }
        lock (Globals.PlayerDatas_Team2)
        {
            list = new(Globals.PlayerDatas_Team2.Values);
        }
        if (list.Count != 0)
        {
            buff.WriteBoolean(true);
            PlayerList(buff, list);
        }
        else
        {
            buff.WriteBoolean(value: false);
        }
        lock (Globals.PlayerDatas_Team3)
        {
            list = new(Globals.PlayerDatas_Team3.Values);
        }
        if (list.Count != 0)
        {
            buff.WriteBoolean(true);
            PlayerList(buff, list);
        }
        else
        {
            buff.WriteBoolean(value: false);
        }
    }
    /// <summary>
    /// 写服务器地图循环列表
    /// </summary>
    /// <param name="buff"></param>
    internal static void ServerMap(IByteBuffer buff)
    {
        if (Globals.ServerInfo == null)
        {
            buff.WriteBoolean(false);
            return;
        }

        buff.WriteBoolean(true);
        if (Globals.ServerInfo.rotation.Count == 0)
        {
            buff.WriteBoolean(false);
            return;
        }

        buff.WriteBoolean(true);
        buff.WriteInt(Globals.ServerInfo.rotation.Count);
        foreach (var item in Globals.ServerInfo.rotation)
        {
            buff.WriteString(ChsUtils.ToSimplifiedChinese(item.mapPrettyName));
        }
    }
    /// <summary>
    /// 写管理员列表
    /// </summary>
    /// <param name="buff"></param>
    internal static void AdminList(IByteBuffer buff)
    {
        if (Globals.ServerInfo == null)
        {
            buff.WriteBoolean(false);
            return;
        }

        buff.WriteBoolean(true);
        var list = Globals.RspInfo.adminList;
        if (list.Count == 0)
        {
            buff.WriteBoolean(false);
            return;
        }
        buff.WriteBoolean(true)
            .WriteInt(list.Count);

        foreach (var item in list)
        {
            buff.WriteString(item.displayName);
        }
    }
    /// <summary>
    /// 写VIP列表
    /// </summary>
    /// <param name="buff"></param>
    internal static void VipList(IByteBuffer buff)
    {
        if (Globals.ServerInfo == null)
        {
            buff.WriteBoolean(false);
            return;
        }

        buff.WriteBoolean(true);
        var list = Globals.RspInfo.vipList;
        if (list.Count == 0)
        {
            buff.WriteBoolean(false);
            return;
        }
        buff.WriteBoolean(true)
            .WriteInt(list.Count);

        foreach (var item in list)
        {
            buff.WriteString(item.displayName);
        }
    }
}
