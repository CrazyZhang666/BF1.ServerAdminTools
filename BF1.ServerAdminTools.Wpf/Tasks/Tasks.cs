using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF1.ServerAdminTools.Wpf.Tasks;

internal static class Tasks
{
    public static void Start()
    {
        TaskCheckLife.Start();
        TaskTick.Start();
        TaskCheckRule.Start();
        TaskKick.Start();
    }
}
