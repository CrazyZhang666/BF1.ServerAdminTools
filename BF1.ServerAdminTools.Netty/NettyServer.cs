using BF1.ServerAdminTools.Common;
using BF1.ServerAdminTools.Common.API.BF1Server;
using BF1.ServerAdminTools.Common.Data;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System.Text;

namespace BF1.ServerAdminTools.Netty;

internal class NettyServer
{
    private static IEventLoopGroup bossGroup;
    private static IEventLoopGroup workerGroup;
    private static IChannel boundChannel;
    public static bool State { get; private set; }
    public static async Task Start()
    {
        bossGroup = new MultithreadEventLoopGroup(1);
        workerGroup = new MultithreadEventLoopGroup();
        var bootstrap = new ServerBootstrap();

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

    public static async Task Stop()
    {
        if (boundChannel == null)
            return;
        boundChannel.Flush();
        await boundChannel.CloseAsync();
        await boundChannel.DisconnectAsync();
        State = false;
    }

    class ServerHandler : ChannelHandlerAdapter
    {
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
                            name = buffer.ReadString(buffer.ReadInt(), Encoding.UTF8);
                            if (Globals.ServerInfo == null)
                            {
                                buff.WriteBoolean(false);
                                break;
                            }
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
        }
    }
}