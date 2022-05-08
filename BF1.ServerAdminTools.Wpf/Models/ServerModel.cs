using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace BF1.ServerAdminTools.Wpf.Models;

public class ServerModel : ObservableObject
{
    private string serverName;
    /// <summary>
    /// 服务器名称
    /// </summary>
    public string ServerName
    {
        get { return serverName; }
        set { serverName = value; OnPropertyChanged(); }
    }

    private Visibility loadingVisibility;
    /// <summary>
    /// 加载动画
    /// </summary>
    public Visibility LoadingVisibility
    {
        get { return loadingVisibility; }
        set { loadingVisibility = value; OnPropertyChanged(); }
    }
}
