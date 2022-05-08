namespace BF1.ServerAdminTools.Wpf.Windows;

/// <summary>
/// InputWindow.xaml 的交互逻辑
/// </summary>
public partial class InputWindow : Window
{
    public string Value { get; set; }

    public InputWindow(string title, string text, string value = "")
    {
        InitializeComponent();

        this.DataContext = this;

        Title = title;
        Text.Text = text;
        Value = value;
    }

    public string Set()
    {
        ShowDialog();
        return Value;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Close();
        return;
    }
}
