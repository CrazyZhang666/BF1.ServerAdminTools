namespace BF1.ServerAdminTools.GameImage;

public static class GameWindow
{
    private static bool IsOut = true;
    private static bool NeedRun = false;
    private static bool IsRun = false;
    private static Thread thread;

    /// <summary>
    /// 0 1280x720 1 1024x768
    /// </summary>
    public static int XY = 0;
    /// <summary>
    /// 不判断游戏是否在服务器
    /// </summary>
    public static void Pause()
    {
        NeedRun = false;
    }
    /// <summary>
    /// 停止运行
    /// </summary>
    public static void Stop()
    {
        IsRun = false;
    }
    /// <summary>
    /// 延迟函数
    /// </summary>
    /// <param name="a"></param>
    private static void Delay(int a)
    {
        while (a > 0)
        {
            a--;
            Thread.Sleep(1000);
            if (!NeedRun || !IsRun)
                return;
        }
    }
    /// <summary>
    /// 启动判断
    /// </summary>
    public static void Start()
    {
        if (thread == null)
        {
            IsRun = true;
            thread = new Thread(() =>
            {
                while (IsRun)
                {
                    Delay(10);
                    Run();
                }
            })
            {
                Name = "GameImageThread"
            };
            thread.Start();
        }
        NeedRun = true;
    }
    /// <summary>
    /// 进行加入服务器
    /// </summary>
    public static void Join()
    {
        //打开多人
        WindowMessage.ToM();
        Delay(5);
        int a = 0;
        do
        {
            if (WindowOpenCV.Test1())
                break;
            a++;
            Delay(5);
            if (!NeedRun || !IsRun)
                return;
        } while (a < 5);
        if (a >= 5)
        {
            return;
        }

        //打开服务器列表
        WindowMessage.ToServerList();
        Delay(5);
        if (!NeedRun || !IsRun)
            return;
        //打开最爱服务器
        WindowMessage.ToServerList1();
        Delay(5);
        if (!NeedRun || !IsRun)
            return;
        a = 0;
        do
        {
            if (WindowOpenCV.Test2())
                break;
            a++;
            Delay(2);
            if (!NeedRun || !IsRun)
                return;
        } while (a < 5);
        if (a >= 5)
        {
            return;
        }
        //打开服务器详情
        WindowMessage.ToServer();
        Delay(5);
        if (!NeedRun || !IsRun)
            return;
        a = 0;
        do
        {
            if (WindowOpenCV.Test3())
                break;
            a++;
            Delay(5);
            if (!NeedRun || !IsRun)
                return;
        } while (a < 5);
        if (a >= 5)
        {
            return;
        }
        //加入服务器
        WindowMessage.JoinServer();
        Delay(5);
        if (!NeedRun || !IsRun)
            return;
        //点两次防止没进入
        WindowMessage.JoinServer();
        IsOut = false;
    }

    private static void Run()
    {
        if (!NeedRun || !IsRun)
            return;
        if (WindowOpenCV.Error1())
        {
            if (!NeedRun || !IsRun)
                return;
            WindowMessage.Ok();
            IsOut = true;
        }
        else if (WindowOpenCV.Error2())
        {
            if (!NeedRun || !IsRun)
                return;
            WindowMessage.Online();
            IsOut = true;
        }
        else if (WindowOpenCV.Error3())
        {
            if (!NeedRun || !IsRun)
                return;
            WindowMessage.Ok();
            IsOut = true;
        }
        else if (WindowOpenCV.Error4())
        {
            if (!NeedRun || !IsRun)
                return;
            WindowMessage.Online();
            IsOut = true;
        }
        else if (IsOut)
        {
            if (!NeedRun || !IsRun)
                return;
            if (WindowOpenCV.Info1())
                IsOut = true;
            else
                Join();
        }
    }
}
