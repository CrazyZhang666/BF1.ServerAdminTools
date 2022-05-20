using BF1.ServerAdminTools.Common;
using BF1.ServerAdminTools.Common.API.BF1Server;
using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Netty;
using BF1.ServerAdminTools.Wpf.Data;
using BF1.ServerAdminTools.Wpf.Utils;
using BF1.ServerAdminTools.Wpf.Views;
using DotNetty.Buffers;
using System.Collections.Concurrent;

namespace BF1.ServerAdminTools.Wpf.TaskList;

internal class TaskKick
{
    /// <summary>
    /// 保存违规玩家列表信息
    /// </summary>
    public static ConcurrentDictionary<long, BreakRuleInfo> NeedKick { get; } = new();
    /// <summary>
    /// 已经发送踢出的ID
    /// </summary>
    private static ConcurrentDictionary<long, BreakRuleInfo> NowKick { get; } = new();

    public static void Start()
    {
        new Thread(Run)
        {
            Name = "TaskKick",
            IsBackground = true
        }.Start();
    }

    public static void AddKick(BreakRuleInfo info)
    {
        if (NeedKick.ContainsKey(info.PersonaId))
            return;

        NeedKick.TryAdd(info.PersonaId, info);
    }

    public static bool IsHave(long id)
    {
        return NeedKick.ContainsKey(id) || NowKick.ContainsKey(id);
    }

    public static void Kick()
    {
        Parallel.ForEachAsync(NeedKick, KickItem).Wait();
        NeedKick.Clear();
    }
    /// <summary>
    /// T人
    /// </summary>

    private static void Run()
    {
        while (Tasks.IsRun)
        {
            Thread.Sleep(100);
            if (DataSave.AutoKickBreakPlayer)
            {
                Kick();
            }

            List<long> remove = new();

            foreach (var item in NowKick)
            {
                if (!Globals.PlayerDatas_Team1.ContainsKey(item.Key)
                    && !Globals.PlayerDatas_Team2.ContainsKey(item.Key))
                {
                    //已经不在服务器了
                    remove.Add(item.Key);
                    continue;
                }
                // 如果超过15秒还在服务器
                if (CoreUtils.DiffSeconds(item.Value.Time, DateTime.Now) > 30)
                {
                    remove.Add(item.Key);
                }
            }

            foreach (var item in remove)
            {
                NowKick.TryRemove(item, out var _);
            }
        }
    }

    private static async ValueTask KickItem(KeyValuePair<long, BreakRuleInfo> item,
        CancellationToken state)
    {
        if (state.IsCancellationRequested)
            return;
        if (NowKick.ContainsKey(item.Key))
            return;

        if (Globals.Server_AdminList.Contains(item.Key) ||
            DataSave.NowRule.Custom_WhiteList.Contains(item.Value.Name))
            return;

        if (DataSave.Config.NettyBQ1)
        {
            IByteBuffer buff = Unpooled.Buffer();
            buff.WriteByte(127)
                .WriteByte(60)
                .WriteString(item.Value.Reason1);
            NettyCore.SendData(buff);
        }

        item.Value.Time = DateTime.Now;
        await KickPlayer(item.Value);
        NowKick.TryAdd(item.Key, item.Value);
    }

    private static async Task KickPlayer(BreakRuleInfo info)
    {
        var result = await ServerAPI.AdminKickPlayer(info.PersonaId.ToString(), info.Reason);

        if (result.IsSuccess)
        {
            info.Status = "踢出成功";
            info.Time = DateTime.Now;
            LogView.AddKickOKLog?.Invoke(info);
        }
        else
        {
            info.Status = "踢出失败 " + result.Message;
            info.Time = DateTime.Now;
            LogView.AddKickNOLog?.Invoke(info);
            if (DataSave.Config.NettyBQ3)
            {
                IByteBuffer buff = Unpooled.Buffer();
                buff.WriteByte(127)
                    .WriteByte(60)
                    .WriteString($"玩家：{info.Name}踢出失败\n{result.Message}");
                NettyCore.SendData(buff);
            }
        }
    }
}
