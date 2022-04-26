using BF1.ServerAdminTools.Common.Hook;
using BF1.ServerAdminTools.Common.Utils;

namespace BF1.ServerAdminTools.Common.Chat;

internal static class ChatHelper
{
    /// <summary>
    /// 按键间隔延迟，单位：毫秒
    /// </summary>
    public static int KeyPressDelay = 50;

    public static void KeyPress(WinVK winVK, int delay)
    {
        Thread.Sleep(delay);
        WinAPI.Keybd_Event(winVK, WinAPI.MapVirtualKey(winVK, 0), 0, 0);
        Thread.Sleep(delay);
        WinAPI.Keybd_Event(winVK, WinAPI.MapVirtualKey(winVK, 0), 2, 0);
        Thread.Sleep(delay);
    }

    public static void KeyTab()
    {
        WinAPI.Keybd_Event(WinVK.TAB, WinAPI.MapVirtualKey(WinVK.TAB, 0), 0, 0);
        Thread.Sleep(3000);
        WinAPI.Keybd_Event(WinVK.TAB, WinAPI.MapVirtualKey(WinVK.TAB, 0), 2, 0);
        Thread.Sleep(50);
    }

    public static int GetStrLength(string str)
    {
        if (string.IsNullOrEmpty(str))
            return 0;

        ASCIIEncoding ascii = new();
        int tempLen = 0;
        byte[] s = ascii.GetBytes(str);
        for (int i = 0; i < s.Length; i++)
        {
            if ((int)s[i] == 63)
            {
                tempLen += 3;
            }
            else
            {
                tempLen += 1;
            }
        }

        return tempLen;
    }

    // 发送中文到战地1聊天框
    public static string SendText2Bf1Game(string msg)
    {
        // 如果内容为空，则跳过
        if (string.IsNullOrEmpty(msg))
            return null;

        if (Core.MsgGetAllocateMemoryAddress() == 0)
        {
            return "聊天功能初始化失败，请重启程序";
        }

        // 将窗口置顶
        MemoryHook.SetForegroundWindow();
        Thread.Sleep(KeyPressDelay);

        // 如果聊天框开启，让他关闭
        if (ChatMsg.GetChatIsOpen())
            KeyPress(WinVK.RETURN, KeyPressDelay);

        // 模拟按键，开启聊天框
        KeyPress(WinVK.J, KeyPressDelay);

        if (!ChatMsg.GetChatIsOpen())
        {
            return "聊天框未开启";
        }
        if (ChatMsg.ChatMessagePointer() == 0)
        {
            return "聊天框消息指针未发现";
        }
        // 挂起战地1进程
        NtProc.SuspendProcess(MemoryHook.GetProcessId());

        msg = ChsUtils.ToTraditionalChinese(ChsUtils.ToDBC(msg).Trim());
        var length = GetStrLength(msg);
        MemoryHook.WriteStringUTF8(ChatMsg.GetAllocateMemoryAddress(), null, msg);

        var startPtr = ChatMsg.ChatMessagePointer() + ChatMsg.OFFSET_CHAT_MESSAGE_START;
        var endPtr = ChatMsg.ChatMessagePointer() + ChatMsg.OFFSET_CHAT_MESSAGE_END;

        var oldStartPtr = MemoryHook.Read<long>(startPtr);
        var oldEndPtr = MemoryHook.Read<long>(endPtr);

        MemoryHook.Write<long>(startPtr, ChatMsg.GetAllocateMemoryAddress());
        MemoryHook.Write<long>(endPtr, ChatMsg.GetAllocateMemoryAddress() + length);

        // 恢复战地1进程
        NtProc.ResumeProcess(MemoryHook.GetProcessId());
        KeyPress(WinVK.RETURN, KeyPressDelay);

        // 挂起战地1进程
        NtProc.SuspendProcess(MemoryHook.GetProcessId());
        MemoryHook.Write<long>(startPtr, oldStartPtr);
        MemoryHook.Write<long>(endPtr, oldEndPtr);
        // 恢复战地1进程
        NtProc.ResumeProcess(MemoryHook.GetProcessId());

        return "发送文本到战地1聊天框成功";
    }
}