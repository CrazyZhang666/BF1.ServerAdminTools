namespace BF1.ServerAdminTools.Wpf.Utils;

public class BlurUtils
{
    private readonly Window _window;

    /// <summary>
    /// 创建 <see cref="WindowAccentCompositor"/> 的一个新实例。
    /// </summary>
    /// <param name="window">要创建模糊特效的窗口实例。</param>
    public BlurUtils(Window window) => _window = window ?? throw new ArgumentNullException(nameof(window));

    public void Composite(bool open)
    {
        Window window = _window;
        var handle = new WindowInteropHelper(window).EnsureHandle();
        Composite(handle, open ? AccentState.ACCENT_ENABLE_BLURBEHIND : AccentState.ACCENT_DISABLED);
    }

    private void Composite(IntPtr handle, AccentState state)
    {
        // 创建 AccentPolicy 对象。
        var accent = new AccentPolicy
        {
            AccentState = state,
            GradientColor = 0,
        };

        // 将托管结构转换为非托管对象。
        var accentPolicySize = Marshal.SizeOf(accent);
        var accentPtr = Marshal.AllocHGlobal(accentPolicySize);
        Marshal.StructureToPtr(accent, accentPtr, false);

        // 设置窗口组合特性。
        try
        {
            // 设置模糊特效。
            var data = new WindowCompositionAttributeData
            {
                Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                SizeOfData = accentPolicySize,
                Data = accentPtr,
            };
            SetWindowCompositionAttribute(handle, ref data);
        }
        finally
        {
            // 释放非托管对象。
            Marshal.FreeHGlobal(accentPtr);
        }
    }

    [DllImport("user32.dll")]
    private static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

    private enum AccentState
    {
        ACCENT_DISABLED = 0,
        ACCENT_ENABLE_GRADIENT = 1,
        ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
        ACCENT_ENABLE_BLURBEHIND = 3,
        ACCENT_ENABLE_ACRYLICBLURBEHIND = 4,
        ACCENT_INVALID_STATE = 5,
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct AccentPolicy
    {
        public AccentState AccentState;
        public int AccentFlags;
        public int GradientColor;
        public int AnimationId;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct WindowCompositionAttributeData
    {
        public WindowCompositionAttribute Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }

    private enum WindowCompositionAttribute
    {
        // 省略其他未使用的字段
        WCA_ACCENT_POLICY = 19,
        // 省略其他未使用的字段
    }
}
