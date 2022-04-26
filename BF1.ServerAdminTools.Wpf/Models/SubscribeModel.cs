using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF1.ServerAdminTools.Wpf.Models;

internal class SubscribeModel : ObservableObject
{
    private string _name;
    public string Name
    {
        get { return _name; }
        set { _name = value; OnPropertyChanged(); }
    }

    private DateTime _lasttime;
    public DateTime LastTime
    {
        get { return _lasttime; }
        set { _lasttime = value; OnPropertyChanged(); }
    }

    private DateTime _updatetime;
    public DateTime UpdateTime
    {
        get { return _updatetime; }
        set { _updatetime = value; OnPropertyChanged(); }
    }

    private string _url;
    public string Url
    {
        get { return _url; }
        set { _url = value; OnPropertyChanged(); }
    }
}
