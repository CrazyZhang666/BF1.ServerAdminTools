using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace BF1.ServerAdminTools.Wpf.Models;

internal class MapRuleModel : ObservableObject
{
    private string _map;
    /// <summary>
    /// 地图名
    /// </summary>
    public string Map
    {
        get { return _map; }
        set { _map = value; OnPropertyChanged(); }
    }

    private string _name;
    /// <summary>
    /// 规则名
    /// </summary>
    public string Name
    {
        get { return _name; }
        set { _name = value; OnPropertyChanged(); }
    }
}
