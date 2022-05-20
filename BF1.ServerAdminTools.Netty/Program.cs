using BF1.ServerAdminTools.Common;
using DotNetty.Buffers;

namespace BF1.ServerAdminTools.Netty;

public class NettyMain
{
    public static void Main()
    {
        Console.WriteLine("BF1.ServerAdminTools 启动中");

        Core.Init(new Log());
        Core.ConfigInit();
        Core.SQLInit();

        if (!Core.IsGameRun())
        {
            Console.WriteLine("检测到游戏未启动");
        }
        Core.HookInit();
        new Thread(() =>
        {
            while (true)
            {
                Core.Tick();
                Thread.Sleep(100);
            }
        }).Start();

        Console.WriteLine("BF1.ServerAdminTools 正在读取配置文件");
        NettyCore.InitConfig();
        NettyCore.LoadConfig();
        NettyCore.StartServer();
        Console.WriteLine("BF1.ServerAdminTools Netty服务器已启动");
        while (true)
        {
            string input = Console.ReadLine();
            if (input == null)
                return;
            string[] arg = input.Split(" ");
            if (arg[0] == "stop")
            {
                Console.WriteLine("BF1.ServerAdminTools Netty服务器正在关闭");
                NettyCore.StopServer();
                Core.MsgFreeMemory();
                Core.SQLClose();

                return;
            }
        }
    }


}

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

internal class Log : IMsgCall
{
    public void Info(string data)
    {
        Console.WriteLine(data);
    }

    public void Error(string data, Exception e)
    {
        Console.WriteLine(data + "\n" + e.ToString());
    }
}