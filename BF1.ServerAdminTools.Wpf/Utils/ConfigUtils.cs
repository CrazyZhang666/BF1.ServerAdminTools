using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Common.Helper;
using BF1.ServerAdminTools.Netty;
using BF1.ServerAdminTools.Wpf.Data;

namespace BF1.ServerAdminTools.Common.Utils;

internal static class ConfigUtils
{
    public static string ServerRule { get; } = $"{ConfigLocal.Base}/ServerRule";
    public static string Wpf { get; } = $"{ConfigLocal.Base}/Wpf";
    public static string Subscribe { get; } = $"{ConfigLocal.Base}/Subscribe";
    public static string SubscribeList { get; } = $"{Subscribe}/list.json";
    public static string SubscribeCache { get; } = $"{Subscribe}/cache.json";
    public static string Self { get; } = $"{Wpf}/config.json";

    public static void Init()
    {
        Directory.CreateDirectory(ServerRule);
        Directory.CreateDirectory(Wpf);
        Directory.CreateDirectory(Subscribe);
        NettyCore.InitConfig();
    }

    public static void SaveAllRule()
    {
        foreach (var item in DataSave.Rules)
        {
            FileUtils.WriteFile($"{ServerRule}/{item.Key}.json", JsonUtils.JsonSeri(item.Value));
        }
    }

    public static void LoadAll()
    {
        var dir = new DirectoryInfo(ServerRule);
        foreach (var item in dir.GetFiles())
        {
            if (item.Extension is ".json")
            {
                var name = item.Name.Trim().ToLower().Replace(".json", "");
                var data = File.ReadAllText(item.FullName);
                var rule = JsonUtils.JsonDese<ServerRuleObj>(data);

                if (rule != null)
                {
                    DataSave.Rules.Add(name, rule);
                }
            }
        }

        if (!DataSave.Rules.ContainsKey("default"))
        {
            var rule = new ServerRuleObj()
            {
                Name = "Default"
            };
            DataSave.Rules.Add("default", rule);
            FileUtils.WriteFile($"{ServerRule}/default.json", JsonUtils.JsonSeri(rule));
        }

        if (File.Exists(Self))
        {
            DataSave.Config = JsonUtils.JsonDese<WpfConfigObj>(File.ReadAllText(Self));
        }
        if (File.Exists(SubscribeList))
        {
            DataSave.Subscribes = JsonUtils.JsonDese<SubscribeConfigObj>(File.ReadAllText(SubscribeList));
        }
        if (File.Exists(SubscribeCache))
        {
            DataSave.SubscribeCache = JsonUtils.JsonDese<SubscribeCacheObj>(File.ReadAllText(SubscribeCache));
        }
        if (DataSave.Config == null)
        {
            DataSave.Config = new()
            {
                MapRule = new(),
                AudioSelect = 3,
                AutoRunNetty = true,
                BackgroudOpacity = 20,
                WindowVacuity = true
            };
            FileUtils.WriteFile(Self, JsonUtils.JsonSeri(DataSave.NowRule));
        }

        if (DataSave.Subscribes == null)
        {
            DataSave.Subscribes = new()
            {
                UrlList = new()
            };
            FileUtils.WriteFile(SubscribeList, JsonUtils.JsonSeri(DataSave.Subscribes));
        }

        if (DataSave.SubscribeCache == null)
        {
            DataSave.SubscribeCache = new()
            {
                Cache = new()
            };
            FileUtils.WriteFile(SubscribeCache, JsonUtils.JsonSeri(DataSave.SubscribeCache));
        }

        if (DataSave.Config.MapRule == null)
        {
            DataSave.Config.MapRule = new();
        }
        var remove = new List<string>();
        foreach (var item in DataSave.Config.MapRule)
        {
            if (!DataSave.Rules.ContainsKey(item.Value))
            {
                remove.Add(item.Key);
            }
        }
        if (remove.Count != 0)
        {
            foreach (var item in remove)
            {
                DataSave.Config.MapRule.Remove(item);
            }
            SaveConfig();
        }

        NettyCore.LoadConfig();
    }

    public static void SaveConfig()
    {
        FileUtils.WriteFile(Self, JsonUtils.JsonSeri(DataSave.Config));
    }

    public static void DeleteRule(string name)
    {
        File.Delete($"{ServerRule}/{name}.json");
    }

    public static void SaveRule()
    {
        FileUtils.WriteFile($"{ServerRule}/{DataSave.NowRule.Name.Trim().ToLower()}.json",
            JsonUtils.JsonSeri(DataSave.NowRule));
    }

    public static void SaveRule(ServerRuleObj rule)
    {
        FileUtils.WriteFile($"{ServerRule}/{rule.Name.Trim().ToLower()}.json",
            JsonUtils.JsonSeri(rule));
    }

    public static void SaveSubscribe()
    {
        FileUtils.WriteFile(SubscribeList, JsonUtils.JsonSeri(DataSave.Subscribes));
    }
    public static void SaveSubscribeCache()
    {
        FileUtils.WriteFile(SubscribeCache, JsonUtils.JsonSeri(DataSave.SubscribeCache));
    }
}
