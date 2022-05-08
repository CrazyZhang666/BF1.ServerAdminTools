using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace BF1.ServerAdminTools.Wpf.Models;

public class WeaponNumberModel : ObservableObject
{
    private string _name;
    /// <summary>
    /// 规则名字
    /// </summary>
    public string Name
    {
        get { return _name; }
        set { _name = value; OnPropertyChanged(); }
    }
    private int _count;
    /// <summary>
    /// 限制数量
    /// </summary>
    public int Count
    {
        get { return _count; }
        set { _count = value; OnPropertyChanged(); }
    }
}
