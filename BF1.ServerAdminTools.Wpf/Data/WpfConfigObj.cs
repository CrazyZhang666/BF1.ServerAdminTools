namespace BF1.ServerAdminTools.Wpf.Data;

public record WpfConfigObj
{
    public string Bg { get; set; }
    public int Bg_O { get; set; }
    public bool Window_O { get; set; }
    public bool Window_A { get; set; }
    public bool AutoRun { get; set; }
    public Dictionary<string, string> MapRule { get; set; }
}
