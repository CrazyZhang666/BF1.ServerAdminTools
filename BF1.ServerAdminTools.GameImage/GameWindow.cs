using BF1.ServerAdminTools.Common;

namespace BF1.ServerAdminTools.GameImage;

public static class GameWindow
{
    private static bool IsOut = true;
    private static bool NeedRun = false;
    private static Thread thread;

    /// <summary>
    /// 0 1280x720 1 1024x768
    /// </summary>
    public static int XY = 0;

    public static void Pause()
    {
        NeedRun = false;
    }

    public static void Start()
    {
        if (thread == null)
        {
            thread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(10000);
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

    public static void Join()
    {
        WindowMessage.ToM();
        Thread.Sleep(5000);
        int a = 0;
        do
        {
            if (WindowOpenCV.Test1())
                break;
            a++;
            Thread.Sleep(5000);
            if (!NeedRun)
                return;
        } while (a < 5);
        if (a >= 5)
        {
            return;
        }

        WindowMessage.ToServerList();
        Thread.Sleep(5000);
        if (!NeedRun)
            return;
        WindowMessage.ToServerList1();
        Thread.Sleep(5000);
        if (!NeedRun)
            return;
        a = 0;
        do
        {
            if (WindowOpenCV.Test2())
                break;
            a++;
            Thread.Sleep(2000);
            if (!NeedRun)
                return;
        } while (a < 5);
        if (a >= 5)
        {
            return;
        }

        WindowMessage.ToServer();
        Thread.Sleep(5000);
        if (!NeedRun)
            return;
        a = 0;
        do
        {
            if (WindowOpenCV.Test3())
                break;
            a++;
            Thread.Sleep(5000);
            if (!NeedRun)
                return;
        } while (a < 5);
        if (a >= 5)
        {
            return;
        }
        WindowMessage.JoinServer();
        Thread.Sleep(5000);
        if (!NeedRun)
            return;
        WindowMessage.JoinServer();
        IsOut = false;
    }

    private static void Run()
    {
        if (!NeedRun)
            return;
        if (WindowOpenCV.Error1())
        {
            WindowMessage.Ok();
            IsOut = true;
        }
        else if (WindowOpenCV.Error2())
        {
            WindowMessage.Online();
            IsOut = true;
        }
        else if (WindowOpenCV.Error3())
        {
            WindowMessage.Ok();
            IsOut = true;
        }
        else if (WindowOpenCV.Error4())
        {
            WindowMessage.Online();
            IsOut = true;
        }
        else if (IsOut)
        {
            if (WindowOpenCV.Info1())
                IsOut = true;
            else
                Join();
        }
    }
}
