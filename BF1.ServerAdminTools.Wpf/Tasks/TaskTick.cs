using BF1.ServerAdminTools.Common;
using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Common.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF1.ServerAdminTools.Wpf.Tasks;

internal class TaskTick
{
    private static object Lock = new object();
    private static int Semaphore = 0;
    public static void Start() 
    {
        new Thread(Run)
        {
            Name = "CoreTick",
            IsBackground = true
        }.Start();
    }

    public static void Done() 
    {
        lock (Lock) 
        {
            Semaphore++;
        }
    }

    private static void Run() 
    {
        while (true)
        {
            Thread.Sleep(100);
            if (!Globals.IsGameRun || !Globals.IsToolInit)
            {
                DataSave.AutoKickBreakPlayer = false;
                continue;
            }

            Core.Tick();
            ScoreView.Semaphore.Release();
            LogView.Semaphore.Release();
            TaskCheckRule.Semaphore.Release();

            while (Semaphore != 3)
            {
                Thread.Sleep(100);
            }
            Semaphore = 0;
        }
    }
}
