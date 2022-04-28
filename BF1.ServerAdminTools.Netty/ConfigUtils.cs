using BF1.ServerAdminTools.Common.Helper;
using BF1.ServerAdminTools.Common.Utils;

namespace BF1.ServerAdminTools.Netty;

internal class ConfigUtils
{
    private static string FileLocal = $"{ConfigLocal.Base}/Netty/config.json";
    public static ConfigNettyObj Config { get; private set; }
    /// <summary>
    /// 初始化配置文件
    /// </summary>
    public static void Init()
        => Directory.CreateDirectory($"{ConfigLocal.Base}/Netty");
    /// <summary>
    /// 加载配置文件
    /// </summary>
    public static void Load()
    {
        if (File.Exists(FileLocal))
            Config = JsonUtils.JsonDese<ConfigNettyObj>(File.ReadAllText(FileLocal));
        else
        {
            Save(Config = new()
            {
                Port = 23232,
                ServerKey = new Random().NextInt64()
            });
        }
    }
    /// <summary>
    /// 保存配置文件
    /// </summary>
    /// <param name="obj"></param>
    public static void Save(ConfigNettyObj obj)
    {
        Config = obj;
        FileUtils.WriteFile(FileLocal, JsonUtils.JsonSeri(Config));
    }
}
