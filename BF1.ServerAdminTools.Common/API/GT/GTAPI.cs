using BF1.ServerAdminTools.Common.API.GT.RespJson;
using BF1.ServerAdminTools.Common.Utils;
using RestSharp;

namespace BF1.ServerAdminTools.Common.API.GT;

public static class GTAPI
{
    private const string Host = "https://api.gametools.network";

    private static RestClient client;
    private static Dictionary<string, string> headers;

    /// <summary>
    /// 初始化RestSharp
    /// </summary>
    static GTAPI()
    {
        if (client == null)
        {
            var options = new RestClientOptions(Host)
            {
                Timeout = 10000
            };

            client = new RestClient(options);

            headers = new Dictionary<string, string>();
            headers["Accept"] = "application/json";
        }
    }

    /// <summary>
    /// 根据玩家ID获取玩家数字ID
    /// </summary>
    public static async Task<RespContent<long>> GetPersonaIdByName(string playerName)
    {
        Stopwatch sw = new();
        sw.Start();

        RespContent<long> respContent = new();

        try
        {
            respContent.IsSuccess = false;

            var request = new RestRequest("/bf1/player")
                .AddHeaders(headers)
                .AddParameter("name", playerName)
                .AddParameter("platform", "pc");

            var response = await client.ExecuteGetAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Message = response.Content;
            }
            else
            {
                respContent.Message = $"{response.StatusCode} {response.Content}";
            }
        }
        catch (Exception ex)
        {
            respContent.Message = ex.Message;
        }

        sw.Stop();
        respContent.ExecTime = sw.Elapsed.TotalSeconds;

        return respContent;
    }

    /// <summary>
    /// 获取玩家全部数据
    /// </summary>
    public static async Task<RespContent<AllPlayerData>> GetPlayerAllData(string playerName)
    {
        Stopwatch sw = new();
        sw.Start();

        RespContent<AllPlayerData> respContent = new();

        try
        {
            respContent.IsSuccess = false;

            var request = new RestRequest("/bf1/all")
                .AddHeaders(headers)
                .AddParameter("format_values", "true")
                .AddParameter("name", playerName)
                .AddParameter("platform", "pc")
                .AddParameter("lang", "zh-tw");

            var response = await client.ExecuteGetAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Message = response.Content;

                respContent.Obj = JsonUtils.JsonDese<AllPlayerData>(respContent.Message);
            }
            else
            {
                respContent.Message = $"{response.StatusCode} {response.Content}";
            }
        }
        catch (Exception ex)
        {
            respContent.Message = ex.Message;
        }

        sw.Stop();
        respContent.ExecTime = sw.Elapsed.TotalSeconds;

        return respContent;
    }

    /// <summary>
    /// 获取服务器列表
    /// </summary>
    public static async Task<RespContent<ServerInfos>> GetServersData(string serverName)
    {
        Stopwatch sw = new();
        sw.Start();

        RespContent<ServerInfos> respContent = new();

        try
        {
            respContent.IsSuccess = false;

            var request = new RestRequest("/bf1/servers")
                .AddHeaders(headers)
                .AddParameter("name", serverName)
                .AddParameter("region", "all")
                .AddParameter("platform", "pc")
                .AddParameter("limit", "30")
                .AddParameter("lang", "zh-tw");

            var response = await client.ExecuteGetAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Message = response.Content;

                respContent.Obj = JsonUtils.JsonDese<ServerInfos>(respContent.Message);
            }
            else
            {
                respContent.Message = $"{response.StatusCode} {response.Content}";
            }
        }
        catch (Exception ex)
        {
            respContent.Message = ex.Message;
        }

        sw.Stop();
        respContent.ExecTime = sw.Elapsed.TotalSeconds;

        return respContent;
    }

    /// <summary>
    /// 获取服务器详细内容
    /// </summary>
    /// <param name="gameid">服务器ID</param>
    /// <returns></returns>
    public static async Task<RespContent<ServerInfos.ServersItem>> GetServerDetailed(string gameid)
    {
        Stopwatch sw = new();
        sw.Start();

        RespContent<ServerInfos.ServersItem> respContent = new();

        try
        {
            respContent.IsSuccess = false;

            var request = new RestRequest("/bf1/detailedserver")
                .AddHeaders(headers)
                .AddParameter("gameid", gameid)
                .AddParameter("platform", "pc")
                .AddParameter("lang", "zh-tw");

            var response = await client.ExecuteGetAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Message = response.Content;

                respContent.Obj = JsonUtils.JsonDese<ServerInfos.ServersItem>(respContent.Message);
            }
            else
            {
                respContent.Message = $"{response.StatusCode} {response.Content}";
            }
        }
        catch (Exception ex)
        {
            respContent.Message = ex.Message;
        }

        sw.Stop();
        respContent.ExecTime = sw.Elapsed.TotalSeconds;

        return respContent;
    }

    /// <summary>
    /// 获取最近天数在线人数
    /// </summary>
    /// <param name="days"></param>
    /// <param name="region">ALL, EU, Asia, NAm, SAm, AU, OC</param>
    /// <returns></returns>
    public static async Task<RespContent<StatusArray>> GetStatusArray(string days, string region)
    {
        // Possible regions are: ALL, EU, Asia, NAm, SAm, AU, OC. For platform there is pc, xboxone, ps4 and all

        Stopwatch sw = new();
        sw.Start();

        RespContent<StatusArray> respContent = new();

        try
        {
            respContent.IsSuccess = false;

            var request = new RestRequest("/bf1/statusarray")
                .AddHeaders(headers)
                .AddParameter("days", days)
                .AddParameter("region", region)
                .AddParameter("platform", "pc");

            var response = await client.ExecuteGetAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Message = response.Content;

                respContent.Obj = JsonUtils.JsonDese<StatusArray>(respContent.Message);
            }
            else
            {
                respContent.Message = $"{response.StatusCode} {response.Content}";
            }
        }
        catch (Exception ex)
        {
            respContent.Message = ex.Message;
        }

        sw.Stop();
        respContent.ExecTime = sw.Elapsed.TotalSeconds;

        return respContent;
    }
}
