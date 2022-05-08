using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace BF1.ServerAdminTools.Wpf.Models;

public class PlayerOtherModel : ObservableObject
{
    private string _mySelfName;
    /// <summary>
    /// 玩家自己ID
    /// </summary>
    public string MySelfName
    {
        get { return _mySelfName; }
        set { _mySelfName = value; OnPropertyChanged(); }
    }

    private string _mySelfTeamID;
    /// <summary>
    /// 玩家自己队伍ID
    /// </summary>
    public string MySelfTeamID
    {
        get { return _mySelfTeamID; }
        set { _mySelfTeamID = value; OnPropertyChanged(); }
    }

    private string _serverPlayerCountInfo;
    /// <summary>
    /// 服务器人数信息
    /// </summary>
    public string ServerPlayerCountInfo
    {
        get { return _serverPlayerCountInfo; }
        set { _serverPlayerCountInfo = value; OnPropertyChanged(); }
    }
}
