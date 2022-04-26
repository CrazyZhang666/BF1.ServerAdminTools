using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace BF1.ServerAdminTools.Wpf.Models;

internal class MapRuleModel : ObservableObject
{
    private string _map;

    public string Map
    {
        get { return _map; }
        set { _map = value; OnPropertyChanged(); }
    }

    private string _name;

    public string Name
    {
        get { return _name; }
        set { _name = value; OnPropertyChanged(); }
    }
}
