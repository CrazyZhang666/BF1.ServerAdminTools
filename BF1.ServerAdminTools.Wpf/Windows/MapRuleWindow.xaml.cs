using BF1.ServerAdminTools.Common;
using BF1.ServerAdminTools.Wpf.Data;

namespace BF1.ServerAdminTools.Wpf.Windows
{
    /// <summary>
    /// MapRuleWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MapRuleWindow : Window
    {

        public MapRuleWindow(string? map = null, string? rule = null)
        {
            InitializeComponent();

            if (Globals.ServerInfo == null)
                return;
            var list = new List<string>();
            foreach (var item in Globals.ServerInfo.rotation)
            {
                if (DataSave.Config.MapRule.ContainsKey(item.mapPrettyName)
                    && item.mapPrettyName != map)
                    continue;
                list.Add(item.mapPrettyName);
            }
            MapList.ItemsSource = list;
            if (map != null)
            {
                MapList.SelectedItem = map;
            }

            var list1 = new List<ServerRuleObj>();
            foreach (var item in DataSave.Rules.Values)
            {
                list1.Add(item);
            }

            RuleList.ItemsSource = list1;
            if (rule != null && DataSave.Rules.TryGetValue(rule, out var item2))
            {
                RuleList.SelectedItem = item2;
            }
        }

        public ServerRuleObj? Set(out string? map)
        {
            ShowDialog();
            map = MapList.SelectedItem?.ToString();
            return RuleList.SelectedItem as ServerRuleObj;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
