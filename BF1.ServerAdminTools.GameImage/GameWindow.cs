namespace BF1.ServerAdminTools.GameImage;

public static class GameWindow
{
    private static bool IsOut = true;
    private static bool NeedRun = false;

    public static void Pause()
    {
        NeedRun = false;
    }

    public static void Start()
    {
        NeedRun = true;
    }

    static GameWindow()
    {
        new Thread(() =>
        {
            while (true)
            {
                Thread.Sleep(10000);
                Run();
            }
        })
        { 
            Name = "GameImageThread"
        }.Start();
    }

    public static void Join()
    {
        WindowMessage.ToM();
        int a = 0;
        do
        {
            if (WindowOpenCV.Test1())
                break;
            a++;
            Thread.Sleep(1000);
        } while (a < 5);
        if (a >= 5)
        {
            return;
        }

        WindowMessage.ToServerList();
        Thread.Sleep(500);
        WindowMessage.ToServerList1();
        Thread.Sleep(1000);
        a = 0;
        do
        {
            if (WindowOpenCV.Test2())
                break;
            a++;
            Thread.Sleep(1000);
        } while (a < 5);
        if (a >= 5)
        {
            return;
        }

        WindowMessage.ToServer();
        Thread.Sleep(1000);
        a = 0;
        do
        {
            if (WindowOpenCV.Test3())
                break;
            a++;
            Thread.Sleep(1000);
        } while (a < 5);
        if (a >= 5)
        {
            return;
        }
        WindowMessage.JoinServer();
        Thread.Sleep(1000);
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
