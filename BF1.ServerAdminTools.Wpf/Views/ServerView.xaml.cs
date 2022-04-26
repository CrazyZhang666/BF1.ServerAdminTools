using BF1.ServerAdminTools.Common.API.GT;
using BF1.ServerAdminTools.Common.API.GT.RespJson;
using BF1.ServerAdminTools.Common.Models;
using BF1.ServerAdminTools.Common.Utils;
using Microsoft.Toolkit.Mvvm.Input;

namespace BF1.ServerAdminTools.Common.Views
{
    /// <summary>
    /// ServerView.xaml 的交互逻辑
    /// </summary>
    public partial class ServerView : UserControl
    {
        public ServerModel ServerModel { get; set; }
        public ObservableCollection<ServerInfos.ServersItem> ServersItems { get; set; }

        public RelayCommand QueryServerCommand { get; private set; }
        public RelayCommand<string> ServerInfoCommand { get; private set; }

        public ServerView()
        {
            InitializeComponent();

            this.DataContext = this;

            ServerModel = new ServerModel();
            ServersItems = new ObservableCollection<ServerInfos.ServersItem>();

            QueryServerCommand = new RelayCommand(QueryServer);
            ServerInfoCommand = new RelayCommand<string>(ServerInfo);

            ServerModel.LoadingVisibility = Visibility.Collapsed;

            ServerModel.ServerName = "QQ";
        }

        private async void QueryServer()
        {
            AudioUtils.ClickSound();

            if (!string.IsNullOrEmpty(ServerModel.ServerName))
            {
                ServersItems.Clear();
                ServerModel.LoadingVisibility = Visibility.Visible;

                ServerModel.ServerName = ServerModel.ServerName.Trim();

                MainWindow._SetOperatingState(2, $"正在查询服务器 {ServerModel.ServerName} 数据中...");

                var result = await GTAPI.GetServersData(ServerModel.ServerName);

                ServerModel.LoadingVisibility = Visibility.Collapsed;

                if (result.IsSuccess)
                {
                    var servers = result.Obj;

                    foreach (var item in servers.servers)
                    {
                        item.mode = ChsUtils.ToSimplifiedChinese(item.mode);
                        item.currentMap = ChsUtils.ToSimplifiedChinese(item.currentMap);
                        item.url = ImageData.GetTempImagePath(item.url, "maps");
                        item.platform = new Random().Next(25, 45).ToString();

                        Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                        {
                            ServersItems.Add(item);
                        }));
                    }

                    MainWindow._SetOperatingState(1, $"服务器 {ServerModel.ServerName} 数据查询成功  |  耗时: {result.ExecTime:0.00} 秒");
                }
                else
                {
                    MainWindow._SetOperatingState(3, $"服务器 {ServerModel.ServerName} 数据查询失败  |  耗时: {result.ExecTime:0.00} 秒");
                }
            }
            else
            {
                MainWindow._SetOperatingState(2, $"请输入正确的服务器名称");
            }
        }

        private void ServerInfo(string gameid)
        {

        }
    }
}
