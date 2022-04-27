namespace BF1.ServerAdminTools.Common.Data;

public record ConfigObj
{
    public string Remid { get; set; } = "";
    public string Sid { get; set; } = "";

    public string SessionId { get; set; } = "";
    public string GameId { get; set; } = "";
    public string ServerId { get; set; } = "";
    public string PersistedGameId { get; set; } = "";
}
