using BF1.ServerAdminTools.Common;
using BF1.ServerAdminTools.Common.Hook;
using System.Runtime.InteropServices;

namespace BF1.ServerAdminTools.GameImage;

public static class WindowMessage
{
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int Left;                             //最左坐标
        public int Top;                             //最上坐标
        public int Right;                           //最右坐标
        public int Bottom;                        //最下坐标
    }

    [DllImport("user32")]
    private static extern int mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
    //移动鼠标 
    private const int MOUSEEVENTF_MOVE = 0x0001;
    //模拟鼠标左键按下 
    private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
    //模拟鼠标左键抬起 
    private const int MOUSEEVENTF_LEFTUP = 0x0004;
    //模拟鼠标右键按下 
    private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
    //模拟鼠标右键抬起 
    private const int MOUSEEVENTF_RIGHTUP = 0x0010;
    //模拟鼠标中键按下 
    private const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
    //模拟鼠标中键抬起 
    private const int MOUSEEVENTF_MIDDLEUP = 0x0040;
    //标示是否采用绝对坐标 
    private const int MOUSEEVENTF_ABSOLUTE = 0x8000;
    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(int X, int Y);

    private static void Click(int x, int y)
    {
        if (!Globals.IsToolInit)
            return;
        Core.SetForegroundWindow();
        RECT rc = new();
        GetWindowRect(Core.GetWindowHandle(), ref rc);
        SetCursorPos(x + rc.Left, y + rc.Top);
        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
    }

    public static void ToMain()
    {
        Core.SetForegroundWindow();
        for (int a = 0; a < 5; a++)
        {
            WinAPI.Keybd_Event(WinVK.ESCAPE, WinAPI.MapVirtualKey(WinVK.ESCAPE, 0), 0, 0);
            Thread.Sleep(100);
            WinAPI.Keybd_Event(WinVK.ESCAPE, WinAPI.MapVirtualKey(WinVK.ESCAPE, 0), 2, 0);
            Thread.Sleep(300);
        }
    }
    /// <summary>
    /// 打开多人
    /// </summary>
    public static void ToM()
    {
        ToMain();
        if (GameWindow.XY == 0)
            Click(160, 80);
        else if (GameWindow.XY == 1)
            Click(150, 165);
    }
    /// <summary>
    /// 预览服务器
    /// </summary>
    public static void ToServerList()
    {
        if (GameWindow.XY == 0)
            Click(650, 250);
        else if (GameWindow.XY == 1)
            Click(500, 300);
    }
    /// <summary>
    /// 最爱服务器
    /// </summary>
    public static void ToServerList1()
    {
        if (GameWindow.XY == 0)
            Click(150, 134);
        else if (GameWindow.XY == 1)
            Click(115, 200);
    }
    /// <summary>
    /// 选择服务器
    /// </summary>
    public static void ToServer()
    {
        if (GameWindow.XY == 0)
            Click(300, 200);
        else if (GameWindow.XY == 1)
            Click(250, 254);
    }
    /// <summary>
    /// 加入观战位
    /// </summary>
    public static void JoinServer()
    {
        if (GameWindow.XY == 0)
            Click(400, 290);
        else if (GameWindow.XY == 1)
            Click(350, 330);
    }
    /// <summary>
    /// 点击确认
    /// </summary>
    public static void Ok()
    {
        if (GameWindow.XY == 0)
            Click(650, 440);
        else if (GameWindow.XY == 1)
            Click(500, 350);
    }
    /// <summary>
    /// 点击上线
    /// </summary>
    public static void Online()
    {
        if (GameWindow.XY == 0)
            Click(650, 450);
        else if (GameWindow.XY == 1)
            Click(500, 360);
    }
}
