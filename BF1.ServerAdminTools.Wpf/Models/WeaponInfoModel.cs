using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace BF1.ServerAdminTools.Wpf.Models;

public class WeaponInfoModel : ObservableObject
{
    private string english;
    /// <summary>
    /// 英文名
    /// </summary>
    public string English
    {
        get
        {
            return english;
        }
        set
        {
            english = value;
            OnPropertyChanged();
        }
    }
    private string chinese;
    /// <summary>
    /// 中文名
    /// </summary>
    public string Chinese
    {
        get
        {
            return chinese;
        }
        set
        {
            chinese = value;
            OnPropertyChanged();
        }
    }
    private string mark;
    /// <summary>
    /// 标记名
    /// </summary>
    public string Mark
    {
        get
        {
            return mark;
        }
        set
        {
            mark = value;
            OnPropertyChanged();
        }
    }
}
