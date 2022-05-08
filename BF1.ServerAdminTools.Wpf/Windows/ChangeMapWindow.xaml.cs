using BF1.ServerAdminTools.Common.Utils;

namespace BF1.ServerAdminTools.Wpf.Windows;

/// <summary>
/// ChangeMapWindow.xaml 的交互逻辑
/// </summary>
public partial class ChangeMapWindow : Window
{
    public string MapName { get; set; }
    public string MapImage { get; set; }

    public ChangeMapWindow(string mapName, string mapImage)
    {
        InitializeComponent();

        this.DataContext = this;

        MapName = mapName;
        MapImage = mapImage;
    }

    private void Window_ChangeMap_Loaded(object sender, RoutedEventArgs e)
    {

    }

    private void Button_OK_Click(object sender, RoutedEventArgs e)
    {
        AudioUtils.ClickSound();

        this.DialogResult = true;
        this.Close();
    }

    private void Button_Cancel_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = false;
        this.Close();
    }
}
