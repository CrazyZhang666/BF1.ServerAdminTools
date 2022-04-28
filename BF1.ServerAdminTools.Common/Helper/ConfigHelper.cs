using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Common.Utils;

namespace BF1.ServerAdminTools.Common.Helper;

public static class ConfigLocal
{
    /// <summary>
    /// 获取当前运行文件完整路径
    /// </summary>
    public static string Current_Path = Process.GetCurrentProcess().MainModule.FileName;

    /// <summary>
    /// 获取当前文件目录，不加文件名及后缀
    /// </summary>
    public static string CurrentDirectory_Path = AppDomain.CurrentDomain.BaseDirectory;

    /// <summary>
    /// 我的文档完整路径
    /// </summary>
    public static string MyDocuments_Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    public static string MyDocument = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    public static string Base { get; } = $"{MyDocument}/BF1 Server";
    public static string Cache { get; } = $"{Base}/Cache";
    public static string Log { get; } = $"{Base}/Log";

    public static string SettingFile { get; } = $"{Base}/config.json";
}

internal static class ConfigHelper
{
    /// <summary>
    /// 保存错误Log日志文件到本地
    /// </summary>
    /// <param name="logContent">保存内容</param>
    public static void WriteErrorLog(string logContent)
    {
        try
        {
            string path = ConfigLocal.Log + @"\ErrorLog";
            Directory.CreateDirectory(path);
            path += $@"\#ErrorLog# {DateTime.Now:yyyyMMdd_HH-mm-ss_ffff}.log";
            File.WriteAllText(path, "[Error]2.0.0运行出现错误" + Environment.NewLine + logContent);
        }
        catch (Exception) { }
    }

    /// <summary>
    /// 保存配置文件
    /// </summary>
    public static void SaveConfig()
    {
        FileUtils.WriteFile(ConfigLocal.SettingFile, JsonUtils.JsonSeri(Globals.Config));
    }

    /// <summary>
    /// 加载配置文件
    /// </summary>
    public static void LoadConfig()
    {
        Directory.CreateDirectory(ConfigLocal.Cache);
        Directory.CreateDirectory(ConfigLocal.Log);
        if (!File.Exists(ConfigLocal.SettingFile))
        {
            Globals.Config = new();
            File.WriteAllText(ConfigLocal.SettingFile, JsonUtils.JsonSeri(Globals.Config));
        }
        else
        {
            Globals.Config = JsonUtils.JsonDese<ConfigObj>(File.ReadAllText(ConfigLocal.SettingFile));
        }
    }
}
