using BF1.ServerAdminTools.Common;
using BF1.ServerAdminTools.Common.API.BF1Server;
using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.GameImage;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System.Collections.Concurrent;
using System.Drawing.Imaging;

namespace BF1.ServerAdminTools.Netty;

public class NettyCore
{
    /// <summary>
    /// 初始化配置文件
    /// </summary>
    public static void InitConfig()
        => ConfigUtils.Init();
    /// <summary>
    /// 加载配置文件
    /// </summary>
    public static void LoadConfig()
        => ConfigUtils.Load();
    /// <summary>
    /// 获取配置文件
    /// </summary>
    /// <returns></returns>
    public static ConfigNettyObj GetConfig()
        => ConfigUtils.Config;
    /// <summary>
    /// 保存配置文件
    /// </summary>
    /// <param name="obj"></param>
    public static void SetConfig(ConfigNettyObj obj)
        => ConfigUtils.Save(obj);
    /// <summary>
    /// 开启Netty服务器
    /// </summary>
    /// <returns></returns>
    public static Task StartServer()
        => NettyServer.Start();
    /// <summary>
    /// 停止Netty服务器
    /// </summary>
    /// <returns></returns>
    public static Task StopServer()
         => NettyServer.Stop();
    /// <summary>
    /// 发送数据给所有客户端
    /// </summary>
    /// <param name="buffer">数据</param>
    public static void SendData(IByteBuffer buffer)
        => NettyServer.SendPackToAll(buffer);
    /// <summary>
    /// 设置顶部回调
    /// </summary>
    /// <param name="call"></param>
    public static void SendTopCall(Func<IByteBuffer, IByteBuffer> call)
        => NettyServer.SetCallBack(call);
    /// <summary>
    /// 获取Netty服务器状态
    /// </summary>
    public static bool State
    {
        get
        {
            return NettyServer.State;
        }
    }
}

internal static class NettyServer
{
    private static IEventLoopGroup bossGroup;
    private static IEventLoopGroup workerGroup;
    private static IChannel boundChannel;
    private static ServerBootstrap bootstrap;
    private static Func<IByteBuffer, IByteBuffer>? TopCall;
    public static readonly ConcurrentDictionary<IChannelHandlerContext, bool> Contexts = new();
    public static bool State { get; private set; }
    /// <summary>
    /// 启动netty服务器
    /// </summary>
    /// <returns></returns>
    public static async Task Start()
    {
        if (State)
            return;
        bossGroup = new MultithreadEventLoopGroup(1);
        workerGroup = new MultithreadEventLoopGroup();
        bootstrap = new ServerBootstrap();

        bootstrap
            .Group(bossGroup, workerGroup)
            .Channel<TcpServerSocketChannel>()
            .Option(ChannelOption.SoBacklog, 100)
            .Handler(new LoggingHandler("BF1.Boot"))
            .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
            {
                IChannelPipeline pipeline = channel.Pipeline;
                pipeline.AddLast(new LoggingHandler("BF1.Pipe"));
                pipeline.AddLast(new LengthFieldPrepender(4));
                pipeline.AddLast(new ServerHandler());
            }));

        boundChannel = await bootstrap.BindAsync(ConfigUtils.Config.Port);
        State = true;
    }
    /// <summary>
    /// 停止Netty服务器
    /// </summary>
    /// <returns></returns>
    public static async Task Stop()
    {
        foreach (var item in Contexts)
        {
            await item.Key.CloseAsync();
        }
        Contexts.Clear();
        if (boundChannel == null)
            return;
        boundChannel.Flush();
        await boundChannel.CloseAsync();
        await boundChannel.CloseCompletion;
        State = false;
    }

    /// <summary>
    /// 设置顶层回调
    /// </summary>
    /// <param name="call"></param>
    public static void SetCallBack(Func<IByteBuffer, IByteBuffer> call)
    {
        TopCall = call;
    }

    /// <summary>
    /// 发送数据到所有客户端
    /// </summary>
    /// <param name="buffer">数据</param>
    public static void SendPackToAll(IByteBuffer buffer)
    {
        Task.Run(() =>
        {
            try
            {
                foreach (var item in Contexts.Keys)
                {
                    item.WriteAndFlushAsync(buffer);
                }
            }
            catch (Exception e)
            {
                Core.WriteExceptionMsg(e);
            }
        });
    }

    class ServerHandler : ChannelHandlerAdapter
    {
        public override void ChannelRegistered(IChannelHandlerContext context)
        {
            base.ChannelRegistered(context);
            if (!Contexts.ContainsKey(context))
            {
                Contexts.TryAdd(context, true);
            }
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            base.ChannelInactive(context);
            if (Contexts.ContainsKey(context))
            {
                Contexts.TryRemove(context, out var item);
            }
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if (message is IByteBuffer buffer)
            {
                Task.Run(async () =>
                {
                    IByteBuffer buff = Unpooled.Buffer();
                    var key = buffer.ReadLong();
                    if (key != ConfigUtils.Config.ServerKey)
                    {
                        buff.WriteByte(70);
                        await context.WriteAndFlushAsync(buff);
                        return;
                    }
                    var type = buffer.ReadByte();
                    switch (type)
                    {
                        //获取状态
                        case 0:
                            buff.WriteByte(0);
                            EncodePack.State(buff);
                            break;
                        //刷新状态
                        case 1:
                            buff.WriteByte(1);
                            EncodePack.Check(buff);
                            break;
                        //获取用户信息
                        case 2:
                            buff.WriteByte(2);
                            EncodePack.Id(buff);
                            break;
                        //获取服务器信息
                        case 3:
                            buff.WriteByte(3);
                            EncodePack.ServerInfo(buff);
                            break;
                        //获取服务器数据
                        case 4:
                            buff.WriteByte(4);
                            EncodePack.ServerScore(buff);
                            break;
                        //获取服务器地图
                        case 5:
                            buff.WriteByte(5);
                            EncodePack.ServerMap(buff);
                            break;
                        //切换地图
                        case 6:
                            var res = await ServerAPI.ChangeServerMap(Globals.Config.PersistedGameId,
                                buffer.ReadInt().ToString());
                            buff.WriteByte(6)
                                .WriteBoolean(res.IsSuccess);
                            break;
                        //踢出玩家
                        case 7:
                            string name = buffer.ReadString(buffer.ReadInt(), Encoding.UTF8);
                            string reason = buffer.ReadString(buffer.ReadInt(), Encoding.UTF8);
                            IEnumerable<PlayerData> list;
                            lock (Globals.PlayerList_All)
                            {
                                list = Globals.PlayerList_All.Values.Where(item => item.Name == name);
                            }
                            buff.WriteByte(7);
                            if (!list.Any())
                            {
                                buff.WriteBoolean(false);
                                buff.WriteBoolean(false);
                                break;
                            }
                            buff.WriteBoolean(true);
                            var result1 = await ServerAPI.AdminKickPlayer(list.First().PersonaId.ToString(), reason);
                            buff.WriteBoolean(result1.IsSuccess);
                            break;
                        //获取VIP列表
                        case 8:
                            buff.WriteByte(8);
                            EncodePack.VipList(buff);
                            break;
                        //获取管理列表
                        case 9:
                            buff.WriteByte(9);
                            EncodePack.AdminList(buff);
                            break;
                        //添加VIP
                        case 10:
                            buff.WriteByte(10);
                            name = buffer.ReadString(buffer.ReadInt(), Encoding.UTF8);
                            if (Globals.ServerInfo == null)
                            {
                                buff.WriteBoolean(false);
                                break;
                            }
                            buff.WriteBoolean(true);
                            var list1 = Globals.RspInfo.vipList;
                            if (list1.FindIndex(a =>
                            {
                                string temp = a.displayName;
                                if (temp.StartsWith("["))
                                {
                                    temp = temp[temp.IndexOf(']')..];
                                }
                                return temp == name;
                            }) != -1)
                            {
                                buff.WriteBoolean(false);
                                return;
                            }
                            buff.WriteBoolean(true);
                            var result = await ServerAPI.AddServerVip(name);
                            buff.WriteBoolean(result.IsSuccess);
                            break;
                        //删除VIP
                        case 11:
                            buff.WriteByte(11);
                            if (Globals.ServerInfo == null)
                            {
                                buff.WriteBoolean(false);
                                break;
                            }
                            name = buffer.ReadString(buffer.ReadInt(), Encoding.UTF8);
                            buff.WriteBoolean(true);
                            list1 = Globals.RspInfo.vipList;
                            if (list1.FindIndex(a =>
                            {
                                string temp = a.displayName;
                                if (temp.StartsWith("["))
                                {
                                    temp = temp[temp.IndexOf(']')..];
                                }
                                return temp == name;
                            }) == -1)
                            {
                                buff.WriteBoolean(false);
                                return;
                            }
                            buff.WriteBoolean(true);
                            var result2 = await ServerAPI.RemoveServerVip(name);
                            buff.WriteBoolean(result2.IsSuccess);
                            break;
                        //换边
                        case 12:
                            buff.WriteByte(12);
                            if (Globals.ServerInfo == null && !string.IsNullOrEmpty(Globals.Config.SessionId))
                            {
                                buff.WriteBoolean(false);
                                break;
                            }
                            name = buffer.ReadString(buffer.ReadInt(), Encoding.UTF8);
                            buff.WriteBoolean(true);
                            lock (Globals.PlayerList_All)
                            {
                                list = Globals.PlayerList_All.Values.Where(item => item.Name == name);
                            }
                            if (!list.Any())
                            {
                                buff.WriteBoolean(false);
                                break;
                            }
                            var player = list.First();
                            buff.WriteBoolean(true);
                            var result3 = await ServerAPI.AdminMovePlayer(player.PersonaId.ToString(), player.TeamID.ToString());
                            buff.WriteBoolean(result3.IsSuccess);
                            break;
                        //获取屏幕游戏截图
                        case 13:
                            buff.WriteByte(13);
                            if (!Globals.IsGameRun || !Globals.IsToolInit)
                            {
                                buff.WriteBoolean(false);
                                break;
                            }
                            var img = GameWindowImg.GetWindow();
                            MemoryStream stream = new();
                            img.Save(stream, ImageFormat.Png);
                            var data = stream.ToArray();
                            buff.WriteInt(data.Length);
                            buff.WriteBytes(data, 0, data.Length);
                            break;
                        //顶层回调
                        case 127:
                            if (TopCall != null)
                            {
                                buff = TopCall.Invoke(buffer);
                            }
                            break;
                    }
                    await context.WriteAndFlushAsync(buff);
                });
            }
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
            Contexts.TryRemove(context, out var temp);
        }
    }
}