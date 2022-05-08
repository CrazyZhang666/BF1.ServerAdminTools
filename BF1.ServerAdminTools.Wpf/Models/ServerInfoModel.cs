using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace BF1.ServerAdminTools.Wpf.Models;

public class ServerInfoModel : ObservableObject
{
    private string _serverName;
    /// <summary>
    /// 服务器名称
    /// </summary>
    public string ServerName
    {
        get { return _serverName; }
        set { _serverName = value; OnPropertyChanged(); }
    }

    private string _serverTime;
    /// <summary>
    /// 服务器时间
    /// </summary>
    public string ServerTime
    {
        get { return _serverTime; }
        set { _serverTime = value; OnPropertyChanged(); }
    }

    ///////////////////////////////////////////////////////////////////////

    private string _team1Score;
    /// <summary>
    /// 队伍1比分
    /// </summary>
    public string Team1Score
    {
        get { return _team1Score; }
        set { _team1Score = value; OnPropertyChanged(); }
    }

    private double _team1ScoreWidth;
    /// <summary>
    /// 队伍1比分，图形宽度
    /// </summary>
    public double Team1ScoreWidth
    {
        get { return _team1ScoreWidth; }
        set { _team1ScoreWidth = value; OnPropertyChanged(); }
    }

    private string _team1ScoreFlag;
    /// <summary>
    /// 队伍1从旗帜获取的得分
    /// </summary>
    public string Team1FromeFlag
    {
        get { return _team1ScoreFlag; }
        set { _team1ScoreFlag = value; OnPropertyChanged(); }
    }

    private string _team1ScoreKill;
    /// <summary>
    /// 队伍1从击杀获取的得分
    /// </summary>
    public string Team1FromeKill
    {
        get { return _team1ScoreKill; }
        set { _team1ScoreKill = value; OnPropertyChanged(); }
    }

    private string _team1Info;
    /// <summary>
    /// 队伍1信息
    /// </summary>
    public string Team1Info
    {
        get { return _team1Info; }
        set { _team1Info = value; OnPropertyChanged(); }
    }

    ///////////////////////////////////////////////////////////////////////

    private string _team2Score;
    /// <summary>
    /// 队伍2比分
    /// </summary>
    public string Team2Score
    {
        get { return _team2Score; }
        set { _team2Score = value; OnPropertyChanged(); }
    }

    private double _team2ScoreWidth;
    /// <summary>
    /// 队伍2比分，图形宽度
    /// </summary>
    public double Team2ScoreWidth
    {
        get { return _team2ScoreWidth; }
        set { _team2ScoreWidth = value; OnPropertyChanged(); }
    }

    private string _team2ScoreFlag;
    /// <summary>
    /// 队伍2从旗帜获取的得分
    /// </summary>
    public string Team2FromeFlag
    {
        get { return _team2ScoreFlag; }
        set { _team2ScoreFlag = value; OnPropertyChanged(); }
    }

    private string _team2ScoreKill;
    /// <summary>
    /// 队伍2从击杀获取的得分
    /// </summary>
    public string Team2FromeKill
    {
        get { return _team2ScoreKill; }
        set { _team2ScoreKill = value; OnPropertyChanged(); }
    }

    private string _team2Info;
    /// <summary>
    /// 队伍2信息
    /// </summary>
    public string Team2Info
    {
        get { return _team2Info; }
        set { _team2Info = value; OnPropertyChanged(); }
    }

    ///////////////////////////////////////////////////////////////////////
}
