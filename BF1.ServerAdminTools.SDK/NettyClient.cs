using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System.Net;
using System.Text;

namespace BF1.ServerAdminTools.SDK;

public record IdObj
{
    public string Remid { get; set; }
    public string Sid { get; set; }
    public string SessionId { get; set; }
    public string GameId { get; set; }
    public string ServerId { get; set; }
    public string PersistedGameId { get; set; }
}
public class KickPlayerObj
{
    /// <summary>
    /// 是否存在玩家
    /// </summary>
    public bool HavePlayer { get; set; }
    /// <summary>
    /// 是否踢出成功
    /// </summary>
    public bool Success { get; set; }
}

public class MapListObj
{
    /// <summary>
    /// 地图列表
    /// </summary>
    public Dictionary<int, string> Maps;
}
public record PlayerDataObj
{
    public bool Admin { get; set; }
    public bool VIP { get; set; }

    public byte Mark { get; set; }
    public int TeamID { get; set; }
    public byte Spectator { get; set; }
    public string Clan { get; set; }
    public string Name { get; set; }
    public long PersonaId { get; set; }
    public string SquadId { get; set; }

    public int Rank { get; set; }
    public int Kill { get; set; }
    public int Dead { get; set; }
    public int Score { get; set; }

    public float KD { get; set; }
    public float KPM { get; set; }

    public string WeaponS0CH { get; set; }
}
public record ServerInfoObj
{
    public string ServerName { get; set; }
    public long ServerID { get; set; }
    public float ServerTime { get; set; }
    public string ServerTimeS { get; set; }
    public int Team1Score { get; set; }
    public int Team2Score { get; set; }
    public int Team1FromeKill { get; set; }
    public int Team2FromeKill { get; set; }
    public int Team1FromeFlag { get; set; }
    public int Team2FromeFlag { get; set; }
    public int Team1MaxPlayerCount { get; set; }
    public int Team2MaxPlayerCount { get; set; }
    public int Team1PlayerCount { get; set; }
    public int Team2PlayerCount { get; set; }
    public int Team1Rank150PlayerCount { get; set; }
    public int Team2Rank150PlayerCount { get; set; }
    public int Team1AllKillCount { get; set; }
    public int Team2AllKillCount { get; set; }
    public int Team1AllDeadCount { get; set; }
    public int Team2AllDeadCount { get; set; }
    public string MapName { get; set; }
    public string MapUrl { get; set; }
    public string TeamOne { get; set; }
    public string TeamOneUrl { get; set; }
    public string TeamTwo { get; set; }
    public string TeamTwoUrl { get; set; }
    public string Mode { get; set; }
}
public record ScoreInfoObj
{
    /// <summary>
    /// 服务器信息
    /// </summary>
    public ServerInfoObj Info { get; set; }
    /// <summary>
    /// 队伍1
    /// </summary>
    public List<PlayerDataObj> Team1 { get; set; }
    /// <summary>
    /// 队伍2
    /// </summary>
    public List<PlayerDataObj> Team2 { get; set; }
    /// <summary>
    /// 观战
    /// </summary>
    public List<PlayerDataObj> Team3 { get; set; }
}
public record StateObj
{
    /// <summary>
    /// 游戏是否运行
    /// </summary>
    public bool IsGameRun { get; set; }
    /// <summary>
    /// 工具是否初始化
    /// </summary>
    public bool IsToolInit { get; set; }
}

internal class DecodePack
{
    public static StateObj State(IByteBuffer buff)
    {
        return new StateObj()
        {
            IsGameRun = buff.ReadByte() == 0xff,
            IsToolInit = buff.ReadByte() == 0xff
        };
    }

    public static IdObj Id(IByteBuffer buff)
    {
        return new IdObj()
        {
            Remid = buff.ReadString(buff.ReadInt(), Encoding.UTF8),
            Sid = buff.ReadString(buff.ReadInt(), Encoding.UTF8),
            SessionId = buff.ReadString(buff.ReadInt(), Encoding.UTF8),
            GameId = buff.ReadString(buff.ReadInt(), Encoding.UTF8),
            ServerId = buff.ReadString(buff.ReadInt(), Encoding.UTF8),
            PersistedGameId = buff.ReadString(buff.ReadInt(), Encoding.UTF8)
        };
    }

    public static ServerInfoObj ServerInfo(IByteBuffer buff)
    {
        var obj = new ServerInfoObj()
        {
            ServerName = buff.ReadString(buff.ReadInt(), Encoding.UTF8),
            ServerID = buff.ReadLong(),
            ServerTime = buff.ReadFloat(),
            ServerTimeS = buff.ReadString(buff.ReadInt(), Encoding.UTF8),
            Team1Score = buff.ReadInt(),
            Team2Score = buff.ReadInt(),
            Team1FromeKill = buff.ReadInt(),
            Team2FromeKill = buff.ReadInt(),
            Team1FromeFlag = buff.ReadInt(),
            Team2FromeFlag = buff.ReadInt(),
            Team1MaxPlayerCount = buff.ReadInt(),
            Team1PlayerCount = buff.ReadInt(),
            Team1Rank150PlayerCount = buff.ReadInt(),
            Team1AllKillCount = buff.ReadInt(),
            Team1AllDeadCount = buff.ReadInt(),
            Team2MaxPlayerCount = buff.ReadInt(),
            Team2PlayerCount = buff.ReadInt(),
            Team2Rank150PlayerCount = buff.ReadInt(),
            Team2AllKillCount = buff.ReadInt(),
            Team2AllDeadCount = buff.ReadInt()
        };
        if (buff.ReadBoolean())
        {
            obj.MapName = buff.ReadString(buff.ReadInt(), Encoding.UTF8);
            obj.MapUrl = buff.ReadString(buff.ReadInt(), Encoding.UTF8);
            obj.TeamOne = buff.ReadString(buff.ReadInt(), Encoding.UTF8);
            obj.TeamOneUrl = buff.ReadString(buff.ReadInt(), Encoding.UTF8);
            obj.TeamTwo = buff.ReadString(buff.ReadInt(), Encoding.UTF8);
            obj.TeamTwoUrl = buff.ReadString(buff.ReadInt(), Encoding.UTF8);
            obj.Mode = buff.ReadString(buff.ReadInt(), Encoding.UTF8);
        }

        return obj;
    }

    public static PlayerDataObj Player(IByteBuffer buff)
    {
        return new PlayerDataObj()
        {
            Admin = buff.ReadBoolean(),
            VIP = buff.ReadBoolean(),
            Mark = buff.ReadByte(),
            TeamID = buff.ReadInt(),
            Spectator = buff.ReadByte(),
            Clan = buff.ReadString(buff.ReadInt(), Encoding.UTF8),
            Name = buff.ReadString(buff.ReadInt(), Encoding.UTF8),
            PersonaId = buff.ReadLong(),
            SquadId = buff.ReadString(buff.ReadInt(), Encoding.UTF8),
            Rank = buff.ReadInt(),
            Dead = buff.ReadInt(),
            Score = buff.ReadInt(),
            Kill = buff.ReadInt(),
            KD = buff.ReadFloat(),
            KPM = buff.ReadFloat(),
            WeaponS0CH = buff.ReadString(buff.ReadInt(), Encoding.UTF8)
        };
    }

    public static List<PlayerDataObj> PlayerList(IByteBuffer buff)
    {
        var list = new List<PlayerDataObj>();
        int length = buff.ReadInt();
        for (int a = 0; a < length; a++)
        {
            list.Add(Player(buff));
        }
        return list;
    }

    public static ScoreInfoObj ServerScore(IByteBuffer buff)
    {
        if (!buff.ReadBoolean())
            return null;
        return new ScoreInfoObj()
        {
            Info = ServerInfo(buff),
            Team1 = buff.ReadBoolean() ? PlayerList(buff) : new(),
            Team2 = buff.ReadBoolean() ? PlayerList(buff) : new(),
            Team3 = buff.ReadBoolean() ? PlayerList(buff) : new()
        };
    }

    public static MapListObj? ServerMap(IByteBuffer buff)
    {
        if (!buff.ReadBoolean())
            return null;
        var obj = new MapListObj()
        {
            Maps = new()
        };
        if (!buff.ReadBoolean())
            return obj;

        int length = buff.ReadInt();
        for (int a = 0; a < length; a++)
        {
            obj.Maps.Add(a, buff.ReadString(buff.ReadInt(), Encoding.UTF8));
        }

        return obj;
    }

    public static KickPlayerObj KickPlayer(IByteBuffer buff)
    {
        return new KickPlayerObj()
        {
            HavePlayer = buff.ReadBoolean(),
            Success = buff.ReadBoolean()
        };
    }

    public static List<string>? List(IByteBuffer buff)
    {
        if (!buff.ReadBoolean())
            return null;
        if (!buff.ReadBoolean())
            return new();
        var list = new List<string>();
        for (int a = 0; a < buff.ReadInt(); a++)
        {
            list.Add(buff.ReadString(buff.ReadInt(), Encoding.UTF8));
        }
        return list;
    }
}


public static class BuildPack
{
    public static IByteBuffer WriteString(this IByteBuffer buff, string data)
    {
        byte[] temp = Encoding.UTF8.GetBytes(data);
        buff.WriteInt(temp.Length);
        buff.WriteBytes(temp);
        return buff;
    }

    public static IByteBuffer SwitchMap(this IByteBuffer buff, int index)
    {
        buff.WriteInt(index);
        return buff;
    }
}


public class NettyClient
{
    private static MultithreadEventLoopGroup group = new();
    private static Dictionary<int, Semaphore> CallBack = new();
    private static Dictionary<int, object?> ResBack = new();

    public static IChannel ClientChannel { get; private set; }
    public static bool IsConnect { get; private set; }
    public static long Key { get; set; }

    public static async Task Start(string ip, int port)
    {
        CallBack.Clear();
        for (int a = 0; a < 20; a++)
        {
            CallBack.Add(a, new(0, 5));
        }
        ResBack.Clear();
        for (int a = 0; a < 20; a++)
        {
            ResBack.Add(a, null);
        }
        var bootstrap = new Bootstrap();
        bootstrap
            .Group(group)
            .Channel<TcpSocketChannel>()
            .Option(ChannelOption.SoBacklog, 100)
            .Handler(new LoggingHandler("BF1.Boot"))
            .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
            {
                IChannelPipeline pipeline = channel.Pipeline;
                pipeline.AddLast(new LoggingHandler("BF1.Pipe"));
                pipeline.AddLast(new LengthFieldBasedFrameDecoder(1024 * 2000, 0, 4, 0, 4));
                pipeline.AddLast(new ClientHandler());
            }));
        ClientChannel = await bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse(ip), port));
        IsConnect = true;
    }

    public static async Task<StateObj?> GetState()
    {
        if (!IsConnect)
            return null;
        var pack = Unpooled.Buffer()
            .WriteLong(Key)
            .WriteByte(0);
        await ClientChannel.WriteAndFlushAsync(pack);
        return await Task.Run(() =>
        {
            CallBack[0].WaitOne();
            return ResBack[0] as StateObj;
        });
    }

    public static async Task<StateObj?> CheckState()
    {
        if (!IsConnect)
            return null;
        var pack = Unpooled.Buffer()
            .WriteLong(Key)
            .WriteByte(1);
        await ClientChannel.WriteAndFlushAsync(pack);
        return await Task.Run(() =>
        {
            CallBack[1].WaitOne();
            return ResBack[1] as StateObj;
        });
    }

    public static async Task<IdObj?> GetId()
    {
        if (!IsConnect)
            return null;
        var pack = Unpooled.Buffer()
            .WriteLong(Key)
            .WriteByte(2);
        await ClientChannel.WriteAndFlushAsync(pack);
        return await Task.Run(() =>
        {
            CallBack[2].WaitOne();
            return ResBack[2] as IdObj;
        });
    }

    public static async Task<ServerInfoObj?> GetServerInfo()
    {
        if (!IsConnect)
            return null;
        var pack = Unpooled.Buffer()
            .WriteLong(Key)
            .WriteByte(3);
        await ClientChannel.WriteAndFlushAsync(pack);
        return await Task.Run(() =>
        {
            CallBack[3].WaitOne();
            return ResBack[3] as ServerInfoObj;
        });
    }

    public static async Task<ScoreInfoObj?> GetServerScore()
    {
        if (!IsConnect)
            return null;
        var pack = Unpooled.Buffer()
            .WriteLong(Key)
            .WriteByte(4);
        await ClientChannel.WriteAndFlushAsync(pack);
        return await Task.Run(() =>
        {
            CallBack[4].WaitOne();
            return ResBack[4] as ScoreInfoObj;
        });
    }

    public static async Task<MapListObj?> GetServerMap()
    {
        if (!IsConnect)
            return null;
        var pack = Unpooled.Buffer()
            .WriteLong(Key)
            .WriteByte(5);
        await ClientChannel.WriteAndFlushAsync(pack);
        return await Task.Run(() =>
        {
            CallBack[5].WaitOne();
            return ResBack[5] as MapListObj;
        });
    }

    public static async Task<bool?> SwitchMap(int index)
    {
        if (!IsConnect)
            return null;
        var pack = Unpooled.Buffer()
            .WriteLong(Key)
            .WriteByte(6)
            .SwitchMap(index);
        await ClientChannel.WriteAndFlushAsync(pack);
        return await Task.Run(() =>
        {
            CallBack[6].WaitOne();
            return (bool?)ResBack[6];
        });
    }

    public static async Task<KickPlayerObj?> KickPlayer(string name, string reason)
    {
        if (!IsConnect)
            return null;
        var pack = Unpooled.Buffer()
            .WriteLong(Key)
            .WriteByte(7)
            .WriteString(name)
            .WriteString(reason);
        await ClientChannel.WriteAndFlushAsync(pack);
        return await Task.Run(() =>
        {
            CallBack[7].WaitOne();
            return ResBack[7] as KickPlayerObj;
        });
    }

    public static async Task<List<string>?> GetVipList()
    {
        if (!IsConnect)
            return null;
        var pack = Unpooled.Buffer()
            .WriteLong(Key)
            .WriteByte(8);
        await ClientChannel.WriteAndFlushAsync(pack);
        return await Task.Run(() =>
        {
            CallBack[8].WaitOne();
            return ResBack[8] as List<string>;
        });
    }

    public static async Task<List<string>?> GetAdminList()
    {
        if (!IsConnect)
            return null;
        var pack = Unpooled.Buffer()
            .WriteLong(Key)
            .WriteByte(9);
        await ClientChannel.WriteAndFlushAsync(pack);
        return await Task.Run(() =>
        {
            CallBack[9].WaitOne();
            return ResBack[9] as List<string>;
        });
    }

    class ClientHandler : SimpleChannelInboundHandler<IByteBuffer>
    {
        protected override void ChannelRead0(IChannelHandlerContext ctx, IByteBuffer buff)
        {
            if (buff != null)
            {
                var res = buff.ReadByte();
                if (res == 70)
                {
                    Console.WriteLine("Server key error");
                    CallBack[0].Release();
                    return;
                }
                switch (res)
                {
                    case 0:
                        ResBack[0] = DecodePack.State(buff);
                        CallBack[0].Release();
                        break;
                    case 1:
                        ResBack[1] = DecodePack.State(buff);
                        CallBack[1].Release();
                        break;
                    case 2:
                        ResBack[2] = DecodePack.Id(buff);
                        CallBack[2].Release();
                        break;
                    case 3:
                        ResBack[3] = DecodePack.ServerInfo(buff);
                        CallBack[3].Release();
                        break;
                    case 4:
                        ResBack[4] = DecodePack.ServerScore(buff);
                        CallBack[4].Release();
                        break;
                    case 5:
                        ResBack[5] = DecodePack.ServerMap(buff);
                        CallBack[5].Release();
                        break;
                    case 6:
                        ResBack[6] = buff.ReadBoolean();
                        CallBack[6].Release();
                        break;
                    case 7:
                        ResBack[7] = DecodePack.KickPlayer(buff);
                        CallBack[7].Release();
                        break;
                    case 8:
                        ResBack[8] = DecodePack.List(buff);
                        CallBack[8].Release();
                        break;
                    case 9:
                        ResBack[9] = DecodePack.List(buff);
                        CallBack[9].Release();
                        break;
                }
            }
        }
        public override void ChannelReadComplete(IChannelHandlerContext context)
            => context.Flush();
        public override void HandlerRemoved(IChannelHandlerContext context)
        {
            base.HandlerRemoved(context);
            IsConnect = false;
        }

        public override void ChannelActive(IChannelHandlerContext context)
        {

        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            context.CloseAsync();
        }
    }
}

