using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Common.Utils;
using BF1.ServerAdminTools.Wpf.Data;
using RestSharp;

namespace BF1.ServerAdminTools.Wpf.Utils;

internal static class SubscribeUtils
{
    private static RestClient client;
    private static bool IsEdit = false;
    static SubscribeUtils()
    {
        if (client == null)
        {
            var options = new RestClientOptions()
            {
                Timeout = 10000
            };

            client = new RestClient(options);
        }
    }

    public static async Task<RespSubscribe> Add(string url)
    {
        RespSubscribe res = new();

        foreach (var item in DataSave.Subscribes.UrlList)
        {
            if (item == url)
            {
                return res;
            }
        }

        try
        {
            var req = new RestRequest(url);
            var res1 = await client.ExecuteGetAsync(req);
            var obj = JsonUtils.JsonDese<HttpSubscribe>(res1.Content);

            if (obj == null)
            {
                return res;
            }

            if (obj.Players == null)
            {
                res.http = obj;
                return res;
            }

            var obj1 = new SubscribeObj()
            {
                Url = url,
                Players = obj.Players,
                LastTime = obj.Time,
                UpdateTime = DateTime.Now.ToString()
            };

            DataSave.Subscribes.UrlList.Add(url);
            ConfigUtils.SaveSubscribe();
            DataSave.SubscribeCache.Cache.Add(obj1);
            ConfigUtils.SaveSubscribeCache();
            res.obj = obj1;
            res.OK = true;
        }
        catch
        { }

        return res;
    }

    public static async Task UpdateAll()
    {
        IsEdit = true;
        DataSave.SubscribeCache.Cache.Clear();
        foreach (var url in DataSave.Subscribes.UrlList)
        {
            try
            {
                var req = new RestRequest(url);
                var res1 = await client.ExecuteGetAsync(req);
                var obj = JsonUtils.JsonDese<HttpSubscribe>(res1.Content);

                if (obj == null || obj.Players == null)
                {
                    continue;
                }

                var obj1 = new SubscribeObj()
                {
                    Url = url,
                    Players = obj.Players,
                    LastTime = obj.Time,
                    UpdateTime = DateTime.Now.ToString()
                };

                DataSave.SubscribeCache.Cache.Add(obj1);

            }
            catch
            { }
        }
        IsEdit = false;
        ConfigUtils.SaveSubscribeCache();
    }

    public static void Delete(string url)
    {
        DataSave.Subscribes.UrlList.Remove(url);
        ConfigUtils.SaveSubscribe();
        SubscribeObj obj = null;
        foreach (var item in DataSave.SubscribeCache.Cache)
        {
            if (item.Url == url)
            {
                obj = item;
            }
        }
        if (obj != null)
        {
            DataSave.SubscribeCache.Cache.Remove(obj);
            ConfigUtils.SaveSubscribeCache();
        }
    }

    public static bool Check(long pid, string name)
    {
        if (IsEdit)
            return false;

        foreach (var item in DataSave.SubscribeCache.Cache)
        {
            if (IsEdit)
                return false;

            foreach (var item1 in item.Players)
            {
                //全部匹配
                if (item1.ID != 0 && !string.IsNullOrWhiteSpace(item1.Name))
                {
                    if (item1.ID == pid && item1.Name == name)
                        return true;
                }
                else if (item1.ID != 0)
                {
                    if (item1.ID == pid)
                        return true;
                }
                else if (!string.IsNullOrWhiteSpace(item1.Name))
                {
                    if (item1.Name == name)
                        return true;
                }
            }
        }

        return false;
    }
}

public record HttpSubscribe
{
    public List<PlayerItem> Players { get; set; }
    public string Time { get; set; }
}

public record RespSubscribe
{
    public HttpSubscribe http;
    public SubscribeObj obj;
    public bool OK;
}