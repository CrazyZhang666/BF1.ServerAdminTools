//using DotNetty.Buffers;
//using DotNetty.Codecs;
//using DotNetty.Handlers.Logging;
//using DotNetty.Transport.Bootstrapping;
//using DotNetty.Transport.Channels;
//using DotNetty.Transport.Channels.Sockets;
//using NUnit.Framework;
//using System;
//using System.Net;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace BF1.ServerAdminTools.Test;

//public class NettyTests
//{
//    public static IEventLoopGroup bossGroup;
//    public static IEventLoopGroup workerGroup;
//    public static IChannel boundChannel;
//    public static Semaphore semaphore = new(0, 5);

//    public static string Data;

//    public static IChannel ClientChannel { get; private set; }
//    [SetUp]
//    public async Task Setup()
//    {
//        bossGroup = new MultithreadEventLoopGroup(1);
//        workerGroup = new MultithreadEventLoopGroup();

//        boundChannel = await new ServerBootstrap()
//            .Group(bossGroup, workerGroup)
//            .Channel<TcpServerSocketChannel>()
//            .Option(ChannelOption.SoBacklog, 100)
//            .Handler(new LoggingHandler("BF1.Boot"))
//            .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
//            {
//                IChannelPipeline pipeline = channel.Pipeline;
//                pipeline.AddLast(new LoggingHandler("BF1.Pipe"));
//                pipeline.AddLast(new LengthFieldPrepender(4));
//                pipeline.AddLast(new ServerHandler());
//            })).BindAsync(23333);

//        ClientChannel = await new Bootstrap()
//            .Group(workerGroup)
//            .Channel<TcpSocketChannel>()
//            .Option(ChannelOption.SoBacklog, 100)
//            .Handler(new LoggingHandler("BF1.Boot"))
//            .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
//            {
//                IChannelPipeline pipeline = channel.Pipeline;
//                pipeline.AddLast(new LoggingHandler("BF1.Pipe"));
//                pipeline.AddLast(new LengthFieldBasedFrameDecoder(1024 * 200, 0, 4, 0, 4));
//                pipeline.AddLast(new ClientHandler());
//            })).ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 23333));
//    }

//    [Test]
//    public async Task Test1()
//    {
//        IByteBuffer buff = Unpooled.Buffer();
//        buff.WriteByte(0);
//        await ClientChannel.WriteAndFlushAsync(buff);
//        semaphore.WaitOne();
//        Assert.AreEqual(Data, "≤‚ ‘");
//    }
//}

//class ClientHandler : SimpleChannelInboundHandler<IByteBuffer>
//{
//    protected override void ChannelRead0(IChannelHandlerContext ctx, IByteBuffer buff)
//    {
//        if (buff != null)
//        {
//            NettyTests.Data = buff.ReadString(buff.ReadInt(), Encoding.UTF8);
//            NettyTests.semaphore.Release();
//        }
//    }
//    public override void ChannelReadComplete(IChannelHandlerContext context)
//        => context.Flush();
//    public override void HandlerRemoved(IChannelHandlerContext context)
//    {
//        base.HandlerRemoved(context);
//    }

//    public override void ChannelActive(IChannelHandlerContext context)
//    {

//    }

//    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
//    {
//        context.CloseAsync();
//    }
//}

//class ServerHandler : ChannelHandlerAdapter
//{
//    public override void ChannelRead(IChannelHandlerContext context, object message)
//    {
//        var buffer = message as IByteBuffer;
//        if (buffer != null)
//        {
//            IByteBuffer buff = Unpooled.Buffer();
//            byte[] temp = Encoding.UTF8.GetBytes("≤‚ ‘");
//            buff.WriteInt(temp.Length);
//            buff.WriteBytes(temp);
//            context.WriteAndFlushAsync(buff);
//        }
//    }

//    public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

//    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
//    {
//        Console.WriteLine("Exception: " + exception);
//        context.CloseAsync();
//    }
//}
