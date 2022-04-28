namespace BF1.ServerAdminTools.Wpf.Data;

public record WpfConfigObj
{
    /// <summary>
    /// 背景图片位置
    /// </summary>
    public string Background { get; set; }
    /// <summary>
    /// 背景透明度
    /// </summary>
    public int BackgroudOpacity { get; set; }
    /// <summary>
    /// 开启窗口虚化
    /// </summary>
    public bool WindowVacuity { get; set; }
    /// <summary>
    /// 是否自动进服
    /// </summary>
    public bool AutoJoinServer { get; set; }
    /// <summary>
    /// 自动启动netty
    /// </summary>
    public bool AutoRunNetty { get; set; }
    /// <summary>
    /// 游戏窗口尺寸
    /// </summary>
    public int GameXYSelect { get; set; }
    /// <summary>
    /// 地图规则
    /// </summary>
    public Dictionary<string, string> MapRule { get; set; } = new();
    /// <summary>
    /// 播放的音频
    /// </summary>
    public int AudioSelect { get; set; }
    public string Msg0 { get; set; }
    public string Msg1 { get; set; }
    public string Msg2 { get; set; }
    public string Msg3 { get; set; }
    public string Msg4 { get; set; }
    public string Msg5 { get; set; }
    public string Msg6 { get; set; }
    public string Msg7 { get; set; }
    public string Msg8 { get; set; }
    public string Msg9 { get; set; }
}
