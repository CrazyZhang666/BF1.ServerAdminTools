using BF1.ServerAdminTools.Common.Utils;
using BF1.ServerAdminTools.Wpf.Utils;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace BF1.ServerAdminTools.Wpf.Models;

public class QueryModel : ObservableObject
{
    private string _playerName;
    /// <summary>
    /// 玩家名称
    /// </summary>
    public string PlayerName
    {
        get { return _playerName; }
        set { _playerName = value; OnPropertyChanged(); }
    }

    private Visibility _loadingVisibility;
    /// <summary>
    /// 加载动画
    /// </summary>
    public Visibility LoadingVisibility
    {
        get { return _loadingVisibility; }
        set { _loadingVisibility = value; OnPropertyChanged(); }
    }

    //////////////////////////////////////

    private string _avatar;
    /// <summary>
    /// 玩家头像
    /// </summary>
    public string Avatar
    {
        get
        {
            if (_avatar == null)
            {
                return ImageData.NullImg;
            }
            return _avatar;
        }
        set
        {
            _avatar = value; OnPropertyChanged();
        }
    }

    private string _userName;
    /// <summary>
    /// 玩家名称
    /// </summary>
    public string UserName
    {
        get { return _userName; }
        set { _userName = value; OnPropertyChanged(); }
    }

    private string _rank;
    /// <summary>
    /// 玩家等级
    /// </summary>
    public string Rank
    {
        get { return _rank; }
        set { _rank = value; OnPropertyChanged(); }
    }

    private string _rankImg;
    /// <summary>
    /// 玩家等级图片
    /// </summary>
    public string RankImg
    {
        get
        {
            if (_rankImg == null)
            {
                return ImageData.NullImg;
            }
            return _rankImg;
        }
        set { _rankImg = value; OnPropertyChanged(); }
    }

    private string _playerTime;
    /// <summary>
    /// 玩家游玩时间
    /// </summary>
    public string PlayerTime
    {
        get { return _playerTime; }
        set { _playerTime = value; OnPropertyChanged(); }
    }
}
