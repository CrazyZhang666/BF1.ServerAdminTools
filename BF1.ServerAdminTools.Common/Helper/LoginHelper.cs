using BF1.ServerAdminTools.Common.API.BF1Server;
using RestSharp;
namespace BF1.ServerAdminTools.Common.Helper;

internal static class LoginHelper
{
    public static async Task<string> LoginSessionID()
    {
        string temp;
        if (string.IsNullOrEmpty(Globals.Config.Remid) && string.IsNullOrEmpty(Globals.Config.Sid))
        {
            temp = "玩家Cookie为空，跳过本次刷新";
            Core.LogError(temp);
            return temp;
        }
        var str = "https://accounts.ea.com/connect/auth?response_type=code&locale=zh_CN&client_id=sparta-backend-as-user-pc";
        var options = new RestClientOptions(str)
        {
            Timeout = 5000,
            FollowRedirects = false
        };

        string cookie = "";
        if (!string.IsNullOrEmpty(Globals.Config.Remid)) cookie = $"{cookie}remid={Globals.Config.Remid};";
        if (!string.IsNullOrEmpty(Globals.Config.Sid)) cookie = $"{cookie}sid={Globals.Config.Sid};";

        var client = new RestClient(options);
        var request = new RestRequest()
            .AddHeader("Cookie", cookie);

        Core.LogInfo($"正在获取SessionId，使用Cookie: {cookie}");

        var response = await client.ExecuteGetAsync(request);
        if (response.StatusCode != HttpStatusCode.Redirect)
        {
            temp = $"刷新SessionID失败，EA连接失败";
            Core.LogError($"刷新SessionID失败，EA连接失败");
            return temp;
        }

        string location = response.Headers.ToList()
                .Find(x => x.Name == "Location")
                .Value.ToString();

        if (!location.Contains("http://127.0.0.1/success?code="))
        {
            temp = $"刷新SessionID失败，Cookie已失效";
            Core.LogError(temp);
            return temp;
        }

        string code = location.Replace("http://127.0.0.1/success?code=", "");
        Core.LogInfo($"Authcode为 {code}");

        if (response.Cookies["remid"] != null)
        {
            Globals.Config.Remid = response.Cookies["remid"].Value;
            Core.LogInfo($"Remid已变更，当前Remid为 {Globals.Config.Remid}");
        }
        if (response.Cookies["sid"] != null)
        {
            Globals.Config.Sid = response.Cookies["sid"].Value;
            Core.LogInfo($"Sid已变更，当前Sid为 {Globals.Config.Sid}");
        }

        var result = await ServerAPI.GetEnvIdViaAuthCode(code);

        if (result.IsSuccess)
        {
            var envIdViaAuthCode = result.Obj;
            Globals.Config.SessionId = envIdViaAuthCode.result.sessionId;
            temp = $"刷新SessionID成功 {Globals.Config.SessionId} |  耗时: {result.ExecTime:0.00} 秒";
            Core.LogInfo(temp);
            Core.SaveConfig();
        }
        else
        {
            temp = $"刷新SessionID失败，Code无效 {code} |  耗时: {result.ExecTime:0.00} 秒";
            Core.LogError(temp);
        }

        return temp;
    }
}
