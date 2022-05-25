using BF1.ServerAdminTools.Common.API.BF1Server.RespJson;
using BF1.ServerAdminTools.Common.Utils;
using RestSharp;

namespace BF1.ServerAdminTools.Common.API.BF1Server;

public static class ServerAPI
{
    private const string Host = "https://sparta-gw.battlelog.com/jsonrpc/pc/api";

    private static RestClient client;
    private static Dictionary<string, string> headers;

    /// <summary>
    /// 初始化RestSharp
    /// </summary>
    static ServerAPI()
    {
        if (client == null)
        {
            var options = new RestClientOptions(Host)
            {
                Timeout = 5000
            };

            client = new RestClient(options);

            headers = new Dictionary<string, string>
            {
                ["User-Agent"] = "ProtoHttp 1.3/DS 15.1.2.1.0 (Windows)",
                ["X-GatewaySession"] = Globals.Config.SessionId,
                ["X-ClientVersion"] = "release-bf1-lsu35_26385_ad7bf56a_tunguska_all_prod",
                ["X-DbId"] = "Tunguska.Shipping2PC.Win32",
                ["X-CodeCL"] = "3779779",
                ["X-DataCL"] = "3779779",
                ["X-SaveGameVersion"] = "26",
                ["X-HostingGameId"] = "tunguska",
                ["X-Sparta-Info"] = "tenancyRootEnv=unknown; tenancyBlazeEnv=unknown"
            };
        }
    }

    /// <summary>
    /// 获取战地1欢迎语
    /// </summary>
    public static async Task<RespContent<WelcomeMsg>> GetWelcomeMessage()
    {
        Stopwatch sw = new();
        sw.Start();

        RespContent<WelcomeMsg> respContent = new();

        try
        {
            headers["X-GatewaySession"] = Globals.Config.SessionId;
            respContent.IsSuccess = false;

            var reqBody = new
            {
                jsonrpc = "2.0",
                method = "Onboarding.welcomeMessage",
                @params = new
                {
                    game = "tunguska",
                    minutesToUTC = "-480"
                },
                id = Guid.NewGuid()
            };

            var request = new RestRequest()
                .AddHeaders(headers)
                .AddJsonBody(reqBody);

            var response = await client.ExecutePostAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Message = response.Content;

                respContent.Obj = JsonUtils.JsonDese<WelcomeMsg>(respContent.Message);
            }
            else
            {
                var respError = JsonUtils.JsonDese<RespError>(response.Content);

                respContent.Message = $"{respError?.error.code} {respError?.error.message}";
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
    /// 设置API语言
    /// </summary>
    public static async Task<RespContent<object>> SetAPILocale()
    {
        Stopwatch sw = new();
        sw.Start();

        RespContent<object> respContent = new();

        try
        {
            headers["X-GatewaySession"] = Globals.Config.SessionId;
            respContent.IsSuccess = false;

            var reqBody = new
            {
                jsonrpc = "2.0",
                method = "CompanionSettings.setLocale",
                @params = new
                {
                    locale = "zh_TW"
                },
                id = Guid.NewGuid()
            };

            var request = new RestRequest()
                .AddHeaders(headers)
                .AddJsonBody(reqBody);

            var response = await client.ExecutePostAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Message = response.Content;
            }
            else
            {
                var respError = JsonUtils.JsonDese<RespError>(response.Content);

                respContent.Message = $"{respError.error?.code} {respError.error?.message}";
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
    /// 踢出指定玩家
    /// </summary>
    public static async Task<RespContent<object>> AdminKickPlayer(string personaId, string reason)
    {
        Stopwatch sw = new();
        sw.Start();

        //防止权限服
        if (reason == "ADMINPRIORITY")
        {
            reason = "Kick";
        }
        reason = $"BFTool:{reason}";

        RespContent<object> respContent = new();

        try
        {
            headers["X-GatewaySession"] = Globals.Config.SessionId;
            respContent.IsSuccess = false;

            var reqBody = new
            {
                jsonrpc = "2.0",
                method = "RSP.kickPlayer",
                @params = new
                {
                    game = "tunguska",
                    gameId = Globals.Config.GameId,
                    personaId,
                    reason
                },
                id = Guid.NewGuid()
            };

            var request = new RestRequest()
                .AddHeaders(headers)
                .AddJsonBody(reqBody);

            var response = await client.ExecutePostAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Message = response.Content;
            }
            else
            {
                var respError = JsonUtils.JsonDese<RespError>(response.Content);

                respContent.Message = $"{respError.error.code} {respError.error.message}";
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
    /// 更换指定玩家队伍
    /// </summary>
    public static async Task<RespContent<object>> AdminMovePlayer(string personaId, string teamId)
    {
        Stopwatch sw = new();
        sw.Start();

        RespContent<object> respContent = new();

        try
        {
            headers["X-GatewaySession"] = Globals.Config.SessionId;
            respContent.IsSuccess = false;

            var reqBody = new
            {
                jsonrpc = "2.0",
                method = "RSP.movePlayer",
                @params = new
                {
                    game = "tunguska",
                    personaId,
                    gameId = Globals.Config.GameId,
                    teamId,
                    forceKill = true,
                    moveParty = false
                },
                id = Guid.NewGuid()
            };

            var request = new RestRequest()
                .AddHeaders(headers)
                .AddJsonBody(reqBody);

            var response = await client.ExecutePostAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Message = response.Content;
            }
            else
            {
                var respError = JsonUtils.JsonDese<RespError>(response.Content);

                respContent.Message = $"{respError.error.code} {respError.error.message}";
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
    /// 更换服务器地图
    /// </summary>
    public static async Task<RespContent<object>> ChangeServerMap(string persistedGameId, string levelIndex)
    {
        Stopwatch sw = new();
        sw.Start();

        RespContent<object> respContent = new();

        try
        {
            headers["X-GatewaySession"] = Globals.Config.SessionId;
            respContent.IsSuccess = false;

            var reqBody = new
            {
                jsonrpc = "2.0",
                method = "RSP.chooseLevel",
                @params = new
                {
                    game = "tunguska",
                    persistedGameId,
                    levelIndex
                },
                id = Guid.NewGuid()
            };

            var request = new RestRequest()
                .AddHeaders(headers)
                .AddJsonBody(reqBody);

            var response = await client.ExecutePostAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Message = response.Content;
            }
            else
            {
                var respError = JsonUtils.JsonDese<RespError>(response.Content);

                respContent.Message = $"{respError.error.code} {respError.error.message}";
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
    /// 获取完整服务器详情
    /// </summary>
    public static async Task<RespContent<FullServerDetails>> GetFullServerDetails()
    {
        Stopwatch sw = new();
        sw.Start();

        RespContent<FullServerDetails> respContent = new();

        try
        {
            headers["X-GatewaySession"] = Globals.Config.SessionId;
            respContent.IsSuccess = false;

            var reqBody = new
            {
                jsonrpc = "2.0",
                method = "GameServer.getFullServerDetails",
                @params = new
                {
                    game = "tunguska",
                    gameId = Globals.Config.GameId
                },
                id = Guid.NewGuid()
            };

            var request = new RestRequest()
                .AddHeaders(headers)
                .AddJsonBody(reqBody);

            var response = await client.ExecutePostAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Message = response.Content;

                respContent.Obj = JsonUtils.JsonDese<FullServerDetails>(respContent.Message);
            }
            else
            {
                var respError = JsonUtils.JsonDese<RespError>(response.Content);

                respContent.Message = $"{respError.error.code} {respError.error.message}";
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
    /// 添加服务器管理员
    /// </summary>
    public static async Task<RespContent<object>> AddServerAdmin(string personaName)
    {
        Stopwatch sw = new();
        sw.Start();

        RespContent<object> respContent = new();

        try
        {
            headers["X-GatewaySession"] = Globals.Config.SessionId;
            respContent.IsSuccess = false;

            var reqBody = new
            {
                jsonrpc = "2.0",
                method = "RSP.addServerAdmin",
                @params = new
                {
                    game = "tunguska",
                    serverId = Globals.Config.ServerId,
                    personaName
                },
                id = Guid.NewGuid()
            };

            var request = new RestRequest()
                .AddHeaders(headers)
                .AddJsonBody(reqBody);

            var response = await client.ExecutePostAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Message = response.Content;
            }
            else
            {
                var respError = JsonUtils.JsonDese<RespError>(response.Content);

                respContent.Message = $"{respError.error.code} {respError.error.message}";
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
    /// 获取武器统计
    /// </summary>
    /// <param name="personaId"></param>
    /// <returns></returns>
    public static async Task<RespContent<GetWeaponsByPersonaId>> GetWeaponsByPersonaId(string personaId)
    {
        Stopwatch sw = new();
        sw.Start();

        RespContent<GetWeaponsByPersonaId> respContent = new();

        try
        {
            headers["X-GatewaySession"] = Globals.Config.SessionId;
            respContent.IsSuccess = false;

            var reqBody = new
            {
                jsonrpc = "2.0",
                method = "Progression.getWeaponsByPersonaId",
                @params = new
                {
                    game = "tunguska",
                    personaId
                },
                id = Guid.NewGuid()
            };

            var request = new RestRequest()
                .AddHeaders(headers)
                .AddJsonBody(reqBody);

            var response = await client.ExecutePostAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Message = response.Content;

                respContent.Obj = JsonUtils.JsonDese<GetWeaponsByPersonaId>(respContent.Message);
            }
            else
            {
                var respError = JsonUtils.JsonDese<RespError>(response.Content);

                respContent.Message = $"{respError.error.code} {respError.error.message}";
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
    /// 获取载具统计
    /// </summary>
    /// <param name="personaId"></param>
    /// <returns></returns>
    public static async Task<RespContent<GetVehiclesByPersonaId>> GetVehiclesByPersonaId(string personaId)
    {
        Stopwatch sw = new();
        sw.Start();

        RespContent<GetVehiclesByPersonaId> respContent = new();

        try
        {
            headers["X-GatewaySession"] = Globals.Config.SessionId;
            respContent.IsSuccess = false;

            var reqBody = new
            {
                jsonrpc = "2.0",
                method = "Progression.getVehiclesByPersonaId",
                @params = new
                {
                    game = "tunguska",
                    personaId
                },
                id = Guid.NewGuid()
            };

            var request = new RestRequest()
                .AddHeaders(headers)
                .AddJsonBody(reqBody);

            var response = await client.ExecutePostAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Message = response.Content;

                respContent.Obj = JsonUtils.JsonDese<GetVehiclesByPersonaId>(respContent.Message);
            }
            else
            {
                var respError = JsonUtils.JsonDese<RespError>(response.Content);

                respContent.Message = $"{respError.error.code} {respError.error.message}";
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
    /// 获取玩家统计
    /// </summary>
    /// <param name="personaId"></param>
    /// <returns></returns>
    public static async Task<RespContent<DetailedStatsByPersonaId>> DetailedStatsByPersonaId(string personaId)
    {
        Stopwatch sw = new();
        sw.Start();

        RespContent<DetailedStatsByPersonaId> respContent = new();

        try
        {
            headers["X-GatewaySession"] = Globals.Config.SessionId;
            respContent.IsSuccess = false;

            var reqBody = new
            {
                jsonrpc = "2.0",
                method = "Stats.detailedStatsByPersonaId",
                @params = new
                {
                    game = "tunguska",
                    personaId
                },
                id = Guid.NewGuid()
            };

            var request = new RestRequest()
                .AddHeaders(headers)
                .AddJsonBody(reqBody);

            var response = await client.ExecutePostAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Message = response.Content;

                respContent.Obj = JsonUtils.JsonDese<DetailedStatsByPersonaId>(respContent.Message);
            }
            else
            {
                var respError = JsonUtils.JsonDese<RespError>(response.Content);

                respContent.Message = $"{respError.error.code} {respError.error.message}";
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
    /// 移除服务器管理员
    /// </summary>
    public static async Task<RespContent<object>> RemoveServerAdmin(string personaId)
    {
        Stopwatch sw = new();
        sw.Start();

        RespContent<object> respContent = new();

        try
        {
            headers["X-GatewaySession"] = Globals.Config.SessionId;
            respContent.IsSuccess = false;

            var reqBody = new
            {
                jsonrpc = "2.0",
                method = "RSP.removeServerAdmin",
                @params = new
                {
                    game = "tunguska",
                    serverId = Globals.Config.ServerId,
                    personaId
                },
                id = Guid.NewGuid()
            };

            var request = new RestRequest()
                .AddHeaders(headers)
                .AddJsonBody(reqBody);

            var response = await client.ExecutePostAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Message = response.Content;
            }
            else
            {
                var respError = JsonUtils.JsonDese<RespError>(response.Content);

                respContent.Message = $"{respError.error.code} {respError.error.message}";
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
    /// 添加服务器VIP
    /// </summary>
    public static async Task<RespContent<object>> AddServerVip(string personaName)
    {
        Stopwatch sw = new();
        sw.Start();

        RespContent<object> respContent = new();

        try
        {
            headers["X-GatewaySession"] = Globals.Config.SessionId;
            respContent.IsSuccess = false;

            var reqBody = new
            {
                jsonrpc = "2.0",
                method = "RSP.addServerVip",
                @params = new
                {
                    game = "tunguska",
                    serverId = Globals.Config.ServerId,
                    personaName
                },
                id = Guid.NewGuid()
            };

            var request = new RestRequest()
                .AddHeaders(headers)
                .AddJsonBody(reqBody);

            var response = await client.ExecutePostAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Message = response.Content;
            }
            else
            {
                var respError = JsonUtils.JsonDese<RespError>(response.Content);

                respContent.Message = $"{respError.error.code} {respError.error.message}";
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
    /// 移除服务器VIP
    /// </summary>
    public static async Task<RespContent<object>> RemoveServerVip(string personaId)
    {
        Stopwatch sw = new();
        sw.Start();

        RespContent<object> respContent = new();

        try
        {
            headers["X-GatewaySession"] = Globals.Config.SessionId;
            respContent.IsSuccess = false;

            var reqBody = new
            {
                jsonrpc = "2.0",
                method = "RSP.removeServerVip",
                @params = new
                {
                    game = "tunguska",
                    serverId = Globals.Config.ServerId,
                    personaId
                },
                id = Guid.NewGuid()
            };

            var request = new RestRequest()
                .AddHeaders(headers)
                .AddJsonBody(reqBody);

            var response = await client.ExecutePostAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Message = response.Content;
            }
            else
            {
                var respError = JsonUtils.JsonDese<RespError>(response.Content);

                respContent.Message = $"{respError.error.code} {respError.error.message}";
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
    /// 添加服务器BAN
    /// </summary>
    public static async Task<RespContent<object>> AddServerBan(string personaName)
    {
        Stopwatch sw = new();
        sw.Start();

        RespContent<object> respContent = new();

        try
        {
            headers["X-GatewaySession"] = Globals.Config.SessionId;
            respContent.IsSuccess = false;

            var reqBody = new
            {
                jsonrpc = "2.0",
                method = "RSP.addServerBan",
                @params = new
                {
                    game = "tunguska",
                    serverId = Globals.Config.ServerId,
                    personaName
                },
                id = Guid.NewGuid()
            };

            var request = new RestRequest()
                .AddHeaders(headers)
                .AddJsonBody(reqBody);

            var response = await client.ExecutePostAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Message = response.Content;
            }
            else
            {
                var respError = JsonUtils.JsonDese<RespError>(response.Content);

                respContent.Message = $"{respError.error.code} {respError.error.message}";
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
    /// 移除服务器BAN
    /// </summary>
    public static async Task<RespContent<object>> RemoveServerBan(string personaId)
    {
        Stopwatch sw = new();
        sw.Start();

        RespContent<object> respContent = new();

        try
        {
            headers["X-GatewaySession"] = Globals.Config.SessionId;
            respContent.IsSuccess = false;

            var reqBody = new
            {
                jsonrpc = "2.0",
                method = "RSP.removeServerBan",
                @params = new
                {
                    game = "tunguska",
                    serverId = Globals.Config.ServerId,
                    personaId
                },
                id = Guid.NewGuid()
            };

            var request = new RestRequest()
                .AddHeaders(headers)
                .AddJsonBody(reqBody);

            var response = await client.ExecutePostAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Message = response.Content;
            }
            else
            {
                var respError = JsonUtils.JsonDese<RespError>(response.Content);

                respContent.Message = $"{respError.error.code} {respError.error.message}";
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
    /// 获取服务器RSP信息
    /// </summary>
    public static async Task<RespContent<ServerDetails>> GetServerDetails()
    {
        Stopwatch sw = new();
        sw.Start();

        RespContent<ServerDetails> respContent = new();

        try
        {
            headers["X-GatewaySession"] = Globals.Config.SessionId;
            respContent.IsSuccess = false;

            var reqBody = new
            {
                jsonrpc = "2.0",
                method = "RSP.getServerDetails",
                @params = new
                {
                    game = "tunguska",
                    serverId = Globals.Config.ServerId
                },
                id = Guid.NewGuid()
            };

            var request = new RestRequest()
                .AddHeaders(headers)
                .AddJsonBody(reqBody);

            var response = await client.ExecutePostAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Message = response.Content;

                respContent.Obj = JsonUtils.JsonDese<ServerDetails>(respContent.Message);
            }
            else
            {
                var respError = JsonUtils.JsonDese<RespError>(response.Content);

                respContent.Message = $"{respError.error.code} {respError.error.message}";
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
    /// 更新服务器信息
    /// </summary>
    public static async Task<RespContent<object>> UpdateServer(UpdateServerReqBody reqBody)
    {
        Stopwatch sw = new();
        sw.Start();

        RespContent<object> respContent = new();

        try
        {
            headers["X-GatewaySession"] = Globals.Config.SessionId;
            respContent.IsSuccess = false;

            var request = new RestRequest()
                .AddHeaders(headers)
                .AddJsonBody(reqBody);

            var response = await client.ExecutePostAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Message = response.Content;
            }
            else
            {
                var respError = JsonUtils.JsonDese<RespError>(response.Content);

                respContent.Message = $"{respError.error.code} {respError.error.message}";
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
    /// 获取玩家SessionID
    /// </summary>
    public static async Task<RespContent<EnvIdViaAuthCode>> GetEnvIdViaAuthCode(string authCode)
    {
        Stopwatch sw = new();
        sw.Start();

        RespContent<EnvIdViaAuthCode> respContent = new();

        try
        {
            headers["X-GatewaySession"] = Globals.Config.SessionId;
            respContent.IsSuccess = false;

            var reqBody = new
            {
                jsonrpc = "2.0",
                method = "Authentication.getEnvIdViaAuthCode",
                @params = new
                {
                    authCode,
                    locale = "zh-tw"
                },
                id = Guid.NewGuid()
            };

            var request = new RestRequest()
                .AddHeaders(headers)
                .AddJsonBody(reqBody);

            var response = await client.ExecutePostAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Message = response.Content;

                respContent.Obj = JsonUtils.JsonDese<EnvIdViaAuthCode>(respContent.Message);
            }
            else
            {
                var respError = JsonUtils.JsonDese<RespError>(response.Content);

                respContent.Message = $"{respError.error.code} {respError.error.message}";
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
    /// 获取玩家SessionID
    /// </summary>
    public static async Task<RespContent<CareerForOwnedGamesByPersonaId>> GetCareerForOwnedGamesByPersonaId(string personaId)
    {
        Stopwatch sw = new();
        sw.Start();

        RespContent<CareerForOwnedGamesByPersonaId> respContent = new();

        try
        {
            headers["X-GatewaySession"] = Globals.Config.SessionId;
            respContent.IsSuccess = false;

            var reqBody = new
            {
                jsonrpc = "2.0",
                method = "Stats.getCareerForOwnedGamesByPersonaId",
                @params = new
                {
                    game = "tunguska",
                    personaId
                },
                id = Guid.NewGuid()
            };

            var request = new RestRequest()
                .AddHeaders(headers)
                .AddJsonBody(reqBody);

            var response = await client.ExecutePostAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                respContent.IsSuccess = true;
                respContent.Message = response.Content;

                respContent.Obj = JsonUtils.JsonDese<CareerForOwnedGamesByPersonaId>(respContent.Message);
            }
            else
            {
                var respError = JsonUtils.JsonDese<RespError>(response.Content);

                respContent.Message = $"{respError.error.code} {respError.error.message}";
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
