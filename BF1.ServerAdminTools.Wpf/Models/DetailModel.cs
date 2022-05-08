using BF1.ServerAdminTools.Wpf.Utils;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace BF1.ServerAdminTools.Wpf.Models;

public class DetailModel : ObservableObject
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

    private string _serverDescription;
    /// <summary>
    /// 服务器描述
    /// </summary>
    public string ServerDescription
    {
        get { return _serverDescription; }
        set { _serverDescription = value; OnPropertyChanged(); }
    }

    private string _serverID;
    /// <summary>
    /// 服务器ServerID
    /// </summary>
    public string ServerID
    {
        get { return _serverID; }
        set { _serverID = value; OnPropertyChanged(); }
    }

    private string _serverGameID;
    /// <summary>
    /// 服务器GameID
    /// </summary>
    public string ServerGameID
    {
        get { return _serverGameID; }
        set { _serverGameID = value; OnPropertyChanged(); }
    }

    private string _serverOwnerName;
    /// <summary>
    /// 服主ID
    /// </summary>
    public string ServerOwnerName
    {
        get { return _serverOwnerName; }
        set { _serverOwnerName = value; OnPropertyChanged(); }
    }

    private string _serverOwnerPersonaId;
    /// <summary>
    /// 服主数字ID
    /// </summary>
    public string ServerOwnerPersonaId
    {
        get { return _serverOwnerPersonaId; }
        set { _serverOwnerPersonaId = value; OnPropertyChanged(); }
    }

    private string _serverOwnerImage;
    /// <summary>
    /// 服主头像
    /// </summary>
    public string ServerOwnerImage
    {
        get
        {
            if (_serverOwnerImage == null)
            {
                return ImageData.NullImg;
            }
            return _serverOwnerImage;
        }
        set { _serverOwnerImage = value; OnPropertyChanged(); }
    }

    private string _serverCurrentMap;
    /// <summary>
    /// 服务器当前地图
    /// </summary>
    public string ServerCurrentMap
    {
        get
        {
            if (_serverCurrentMap == null)
            {
                return ImageData.NullImg;
            }
            return _serverCurrentMap;
        }
        set { _serverCurrentMap = value; OnPropertyChanged(); }
    }
}
