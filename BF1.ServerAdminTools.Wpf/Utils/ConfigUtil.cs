using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Common.Helper;
using BF1.ServerAdminTools.Netty;
using BF1.ServerAdminTools.Wpf.Data;

namespace BF1.ServerAdminTools.Common.Utils;

internal static class ConfigUtil
{
    public static string ServerRule { get; } = $"{ConfigLocal.Base}/ServerRule";
    public static string Wpf { get; } = $"{ConfigLocal.Base}/Wpf";
    public static string Subscribe { get; } = $"{ConfigLocal.Base}/Subscribe";
    public static string Self { get; } = $"{Wpf}/config.json";

    public static void Init()
    {
        Directory.CreateDirectory(ServerRule);
        Directory.CreateDirectory(Wpf);
        Directory.CreateDirectory(Subscribe);
        NettyCore.InitConfig();
    }

    public static void SaveAll()
    {
        foreach (var item in DataSave.Rules)
        {
            FileUtil.WriteFile($"{ServerRule}/{item.Key}.json", JsonUtil.JsonSeri(item.Value));
        }

        foreach (var item in DataSave.Subscribes)
        {
            FileUtil.WriteFile($"{Subscribe}/{item.Key}.json", JsonUtil.JsonSeri(item.Value));
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
                var rule = JsonUtil.JsonDese<ServerRuleObj>(data);

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
            FileUtil.WriteFile($"{ServerRule}/default.json", JsonUtil.JsonSeri(rule));
        }

        dir = new DirectoryInfo(Subscribe);
        foreach (var item in dir.GetFiles())
        {
            if (item.Extension is ".json")
            {
                var data = File.ReadAllText(item.FullName);
                var rule = JsonUtil.JsonDese<SubscribeObj>(data);

                if (rule != null)
                {
                    DataSave.Subscribes.Add(rule.Name, rule);
                }
            }
        }

        if (File.Exists(Self))
        {
            DataSave.Config = JsonUtil.JsonDese<WpfConfigObj>(File.ReadAllText(Self));
        }
        if (DataSave.Config == null)
        {
            DataSave.Config = new()
            {
                AutoRun = true,
                Bg_O = 20,
                Window_O = true
            };
            FileUtil.WriteFile(Self, JsonUtil.JsonSeri(DataSave.NowRule));
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
        FileUtil.WriteFile(Self, JsonUtil.JsonSeri(DataSave.Config));
    }

    public static void DeleteRule(string name)
    {
        File.Delete($"{ServerRule}/{name}.json");
    }

    public static void SaveRule()
    {
        FileUtil.WriteFile($"{ServerRule}/{DataSave.NowRule.Name.Trim().ToLower()}.json",
            JsonUtil.JsonSeri(DataSave.NowRule));
    }

    public static void SaveRule(ServerRuleObj rule)
    {
        FileUtil.WriteFile($"{ServerRule}/{rule.Name.Trim().ToLower()}.json",
            JsonUtil.JsonSeri(rule));
    }

    public static void SaveSubscribe(SubscribeObj subscribe)
    {
        FileUtil.WriteFile($"{Subscribe}/{subscribe.Name.Trim()}.json",
            JsonUtil.JsonSeri(subscribe));
    }
}
