using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace BF1.ServerAdminTools.Wpf.Models;

public class MainModel : ObservableObject
{
    private string _appRunTime;
    /// <summary>
    /// 程序运行时间
    /// </summary>
    public string AppRunTime
    {
        get { return _appRunTime; }
        set { _appRunTime = value; OnPropertyChanged(); }
    }
}
