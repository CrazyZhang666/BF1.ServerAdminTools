using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace BF1.ServerAdminTools.Wpf.Models;

internal class SubscribeModel : ObservableObject
{
    private DateTime _lasttime;
    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime LastTime
    {
        get { return _lasttime; }
        set { _lasttime = value; OnPropertyChanged(); }
    }

    private DateTime _updatetime;
    /// <summary>
    /// 刷新时间
    /// </summary>
    public DateTime UpdateTime
    {
        get { return _updatetime; }
        set { _updatetime = value; OnPropertyChanged(); }
    }

    private string _url;
    /// <summary>
    /// 地址
    /// </summary>
    public string Url
    {
        get { return _url; }
        set { _url = value; OnPropertyChanged(); }
    }
}
