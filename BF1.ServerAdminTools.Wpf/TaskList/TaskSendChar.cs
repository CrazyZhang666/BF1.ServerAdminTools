using BF1.ServerAdminTools.Common;
using BF1.ServerAdminTools.Wpf.Views;

namespace BF1.ServerAdminTools.Wpf.TaskList;

internal class TaskSendChar
{
    public static readonly List<string> queueMsg = new();

    private static int queueMsgSleep = 1;
    private static bool IsSend;
    private static bool IsTab;

    private static bool SendRun;
    private static bool AFKRun;

    private static int NowTime;
    private static int InitTime;

    private static int AFK;

    public static void Start()
    {
        new Thread(() =>
        {
            while (Tasks.IsRun)
            {
                Thread.Sleep(1000);
                if (SendRun)
                {
                    AutoSendMsgRun();
                }
            }
        })
        {
            Name = "TaskAutoSendMsg"
        }.Start();

        new Thread(() =>
        {
            while (Tasks.IsRun)
            {
                Thread.Sleep(1000);
                if (AFKRun)
                {
                    if (AFK > 0)
                    {
                        AFK--;
                        continue;
                    }
                    NoAFKRun();
                    AFK = 30;
                }
            }
        })
        {
            Name = "TaskNoAFK"
        }.Start();
    }

    public static void SetStart(int time, int sleep)
    {
        queueMsgSleep = sleep;
        NowTime = InitTime = time * 60;
        SendRun = true;
    }

    public static void SetStop()
    {
        SendRun = false;
    }

    public static void SetAFKStart()
    {
        AFK = 30;
        AFKRun = true;
    }

    public static void SetAFKStop()
    {
        AFKRun = false;
    }

    public static void SetIMEState()
    {
        // 设置输入法为英文
        Application.Current.Dispatcher.Invoke(() =>
        {
            InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("en-US");
        });
    }

    private static void AutoSendMsgRun()
    {
        if (!Globals.IsGameRun || !Globals.IsToolInit)
            return;

        if (NowTime > 0)
        {
            ChatView.SetTime?.Invoke(NowTime);
            NowTime--;
            return;
        }

        IsSend = true;

        while (IsTab)
        {
            Thread.Sleep(100);
        }

        SetIMEState();
        Thread.Sleep(50);

        foreach (var item in queueMsg)
        {
            Core.SendText(item);
            Thread.Sleep(queueMsgSleep * 1000);
        }

        NowTime = InitTime;

        IsSend = false;
    }

    private static void NoAFKRun()
    {
        if (IsSend)
            return;

        IsTab = true;
        SetIMEState();
        Thread.Sleep(50);

        Core.SetForegroundWindow();
        Thread.Sleep(50);

        Core.KeyTab();
        IsTab = false;
    }
}
