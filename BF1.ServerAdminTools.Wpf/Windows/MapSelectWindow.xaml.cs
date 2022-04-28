using BF1.ServerAdminTools.Common;

namespace BF1.ServerAdminTools.Wpf.Windows
{
    /// <summary>
    /// MapSelectWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MapSelectWindow : Window
    {
        public MapSelectWindow(ItemCollection collection)
        {
            InitializeComponent();

            List<string> list = new List<string>();
            foreach (var item in Globals.ServerInfo.rotation)
            {
                if (collection.Contains(item.mapPrettyName))
                    continue;

                list.Add(item.mapPrettyName);
            }

            MapList.ItemsSource = list;
        }

        public string? Set()
        {
            ShowDialog();
            return MapList.SelectedItem as string;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
