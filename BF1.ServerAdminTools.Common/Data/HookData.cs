namespace BF1.ServerAdminTools.Common.Data;

public struct ClientPlayer
{
    public long BaseAddress;

    public int TeamID;
    public byte Spectator;

    public long PersonaId;
    public string PlayerName;
}

public struct ServerHook
{
    public long Offset0;

    public string ServerName;
    public string ServerMap;
    public string ServerMapCH;
    public long ServerID;
    public float ServerTime;
    public string ServerTimeS;

    public int Team1Score;
    public int Team2Score;

    public int Team1FromeKill;
    public int Team2FromeKill;

    public int Team1FromeFlag;
    public int Team2FromeFlag;
}

public struct StatisticData
{
    public int MaxPlayerCount;
    public int PlayerCount;
    public int Rank150PlayerCount;

    public int AllKillCount;
    public int AllDeadCount;
}
