using BF1.ServerAdminTools.Common.API.BF1Server;
using BF1.ServerAdminTools.Common.API.BF1Server.RespJson;
using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Common.Models;
using BF1.ServerAdminTools.Common.Utils;
using BF1.ServerAdminTools.Common.Windows;

namespace BF1.ServerAdminTools.Common.Views
{
    /// <summary>
    /// DetailView.xaml 的交互逻辑
    /// </summary>
    public partial class DetailView : UserControl
    {
        public DetailModel DetailModel { get; set; }

        public class Map
        {
            public string mapPrettyName { get; set; }
            public string mapImage { get; set; }
            public string modePrettyName { get; set; }
        }

        public class ListItem
        {
            public int Index { get; set; }
            public string platform { get; set; }
            public string nucleusId { get; set; }
            public string personaId { get; set; }
            public string platformId { get; set; }
            public string displayName { get; set; }
            public string avatar { get; set; }
            public string accountId { get; set; }
        }

        private ServerDetails serverDetails = null;
        private bool isGetServerDetailsOK = false;

        private static DetailView ThisView;

        public DetailView()
        {
            InitializeComponent();
            ThisView = this;
            this.DataContext = this;

            DetailModel = new DetailModel();

            MainWindow.ClosingDisposeEvent += MainWindow_ClosingDisposeEvent;
        }

        public static Task SLoad()
        {
            return ThisView.Load();
        }

        public async Task Load()
        {
            if (string.IsNullOrEmpty(Globals.Config.SessionId))
            {
                MainWindow.SetOperatingState(2, "请先获取玩家SessionID");
                return;
            }

            if (string.IsNullOrEmpty(Globals.Config.GameId))
            {
                MainWindow.SetOperatingState(2, "请先进入服务器获取GameID");
                return;
            }

            DetailModel.ServerName = "获取中...";
            DetailModel.ServerDescription = "获取中...";
            DetailModel.ServerOwnerName = "获取中...";
            DetailModel.ServerOwnerPersonaId = "获取中...";
            DetailModel.ServerID = "获取中...";
            DetailModel.ServerGameID = "获取中...";

            DetailModel.ServerOwnerImage = ImageData.NullImg;
            DetailModel.ServerCurrentMap = ImageData.NullImg;

            ListBox_Map.Items.Clear();
            ListBox_Admin.Items.Clear();
            ListBox_VIP.Items.Clear();
            ListBox_BAN.Items.Clear();

            Globals.Server_AdminList.Clear();
            Globals.Server_Admin2List.Clear();
            Globals.Server_VIPList.Clear();

            /////////////////////////////////////////////////////////////////////////////////

            MainWindow.SetOperatingState(2, $"正在获取服务器 {Globals.Config.GameId} 详细数据中...");

            var result = await Core.InitServerInfo();

            if (result == null)
            {
                MainWindow.SetOperatingState(3, "服务器详细数据获取失败");
                return;
            }

            if (result.IsSuccess == true)
            {
                var server = result.Obj;

                DetailModel.ServerName = server.result.serverInfo.name;
                DetailModel.ServerDescription = server.result.serverInfo.description;

                DetailModel.ServerID = server.result.rspInfo.server.serverId;
                DetailModel.ServerGameID = server.result.rspInfo.server.persistedGameId;

                DetailModel.ServerOwnerName = server.result.rspInfo.owner.displayName;
                DetailModel.ServerOwnerPersonaId = server.result.rspInfo.owner.personaId;
                DetailModel.ServerOwnerImage = server.result.rspInfo.owner.avatar;
                DetailModel.ServerCurrentMap = ImageData.GetTempImagePath(server.result.serverInfo.mapImageUrl, "maps");

                // 地图列表
                foreach (var item in server.result.serverInfo.rotation)
                {
                    ListBox_Map.Items.Add(new Map()
                    {
                        mapImage = ImageData.GetTempImagePath(item.mapImage, "maps"),
                        mapPrettyName = ChsUtils.ToSimplifiedChinese(item.mapPrettyName),
                        modePrettyName = ChsUtils.ToSimplifiedChinese(item.modePrettyName)
                    });
                }

                // 服主
                int index = 0;
                ListBox_Admin.Items.Add(new ListItem()
                {
                    Index = index++,
                    avatar = server.result.rspInfo.owner.avatar,
                    displayName = server.result.rspInfo.owner.displayName,
                    personaId = server.result.rspInfo.owner.personaId
                });

                // 管理员列表
                foreach (var item in server.result.rspInfo.adminList)
                {
                    ListBox_Admin.Items.Add(new ListItem()
                    {
                        Index = index++,
                        avatar = item.avatar,
                        displayName = item.displayName,
                        personaId = item.personaId
                    });
                }

                // VIP列表
                index = 1;
                foreach (var item in server.result.rspInfo.vipList)
                {
                    ListBox_VIP.Items.Add(new ListItem()
                    {
                        Index = index++,
                        avatar = item.avatar,
                        displayName = item.displayName,
                        personaId = item.personaId
                    });
                }

                // BAN列表
                index = 1;
                foreach (var item in server.result.rspInfo.bannedList)
                {
                    ListBox_BAN.Items.Add(new ListItem()
                    {
                        Index = index++,
                        avatar = item.avatar,
                        displayName = item.displayName,
                        personaId = item.personaId
                    });
                }

                MainWindow.SetOperatingState(1, $"获取服务器 {Globals.Config.GameId} 详细数据成功  |  耗时: {result.ExecTime:0.00} 秒");
            }
            else
            {
                MainWindow.SetOperatingState(3, $"获取服务器 {Globals.Config.GameId} 详细数据失败 {result.Message}  |  耗时: {result.ExecTime:0.00} 秒");
            }
        }

        private void MainWindow_ClosingDisposeEvent()
        {

        }

        private async void Button_GetFullServerDetails_Click(object sender, RoutedEventArgs e)
        {
            AudioUtils.ClickSound();

            await Load();
        }

        private async void ListBox_Map_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListBox_Map.SelectedIndex;
            if (index != -1)
            {
                Map currMap = ListBox_Map.SelectedItem as Map;

                if (!string.IsNullOrEmpty(Globals.Config.PersistedGameId))
                {
                    string mapInfo = currMap.modePrettyName + " - " + currMap.mapPrettyName;

                    var changeMapWindow = new ChangeMapWindow(mapInfo, currMap.mapImage)
                    {
                        Owner = MainWindow.ThisMainWindow
                    };

                    if (changeMapWindow.ShowDialog() == true)
                    {
                        MainWindow.SetOperatingState(2, $"正在更换服务器 {Globals.Config.GameId} 地图为 {currMap.mapPrettyName} 中...");

                        var result = await ServerAPI.ChangeServerMap(Globals.Config.PersistedGameId, index.ToString());

                        if (result.IsSuccess)
                        {
                            MainWindow.SetOperatingState(1, $"更换服务器 {Globals.Config.GameId} 地图为 {currMap.mapPrettyName} 成功  |  耗时: {result.ExecTime:0.00} 秒");
                            Button_GetFullServerDetails_Click(sender, e);
                        }
                        else
                        {
                            MainWindow.SetOperatingState(3, $"更换服务器 {Globals.Config.GameId} 地图为 {currMap.mapPrettyName} 失败 {result.Message}  |  耗时: {result.ExecTime:0.00} 秒");
                        }
                    }
                }
                else
                {
                    MainWindow.SetOperatingState(2, "PersistedGameId异常，请重新获取服务器详细信息");
                }
            }

            // 使ListBox能够相应重复点击
            ListBox_Map.SelectedIndex = -1;
        }

        private async void Button_RemoveSelectedAdmin_Click(object sender, RoutedEventArgs e)
        {
            AudioUtils.ClickSound();

            ListItem currListItem = ListBox_Admin.SelectedItem as ListItem;

            MainWindow.SetOperatingState(2, $"正在移除服务器管理员 {currListItem.displayName} 中...");

            var result = await ServerAPI.RemoveServerAdmin(currListItem.personaId);

            if (result.IsSuccess)
            {
                MainWindow.SetOperatingState(1, $"移除服务器管理员 {currListItem.displayName} 成功  |  耗时: {result.ExecTime:0.00} 秒");
                ListBox_Admin.Items.Remove(currListItem);
            }
            else
            {
                MainWindow.SetOperatingState(3, $"移除服务器管理员 {currListItem.displayName} 失败 {result.Message}  |  耗时: {result.ExecTime:0.00} 秒");
            }
        }

        private async void Button_AddNewAdmin_Click(object sender, RoutedEventArgs e)
        {
            AudioUtils.ClickSound();

            MainWindow.SetOperatingState(2, $"正在增加服务器管理员 {TextBox_NewAdminName.Text} 中...");

            var result = await ServerAPI.AddServerAdmin(TextBox_NewAdminName.Text);

            if (result.IsSuccess)
            {
                MainWindow.SetOperatingState(1, $"增加服务器管理员 {TextBox_NewAdminName.Text} 成功  |  耗时: {result.ExecTime:0.00} 秒");
                Button_GetFullServerDetails_Click(sender, e);
            }
            else
            {
                MainWindow.SetOperatingState(3, $"增加服务器管理员 {TextBox_NewAdminName.Text} 失败 {result.Message}  |  耗时: {result.ExecTime:0.00} 秒");
            }
        }

        private async void Button_RemoveSelectedVIP_Click(object sender, RoutedEventArgs e)
        {
            AudioUtils.ClickSound();

            ListItem currListItem = ListBox_VIP.SelectedItem as ListItem;

            MainWindow.SetOperatingState(2, $"正在移除服务器VIP {currListItem.displayName} 中...");

            var result = await ServerAPI.RemoveServerVip(currListItem.personaId);

            if (result.IsSuccess)
            {
                MainWindow.SetOperatingState(1, $"移除服务器VIP {currListItem.displayName} 成功  |  耗时: {result.ExecTime:0.00} 秒");
                ListBox_VIP.Items.Remove(currListItem);
            }
            else
            {
                MainWindow.SetOperatingState(3, $"移除服务器VIP {currListItem.displayName} 失败 {result.Message}  |  耗时: {result.ExecTime:0.00} 秒");
            }
        }

        private async void Button_AddNewVIP_Click(object sender, RoutedEventArgs e)
        {
            AudioUtils.ClickSound();

            MainWindow.SetOperatingState(2, $"正在增加服务器VIP {TextBox_NewVIPName.Text} 中...");

            var result = await ServerAPI.AddServerVip(TextBox_NewVIPName.Text);

            if (result.IsSuccess)
            {
                MainWindow.SetOperatingState(1, $"增加服务器VIP {TextBox_NewVIPName.Text} 成功  |  耗时: {result.ExecTime:0.00} 秒");
                Button_GetFullServerDetails_Click(sender, e);
            }
            else
            {
                MainWindow.SetOperatingState(3, $"增加服务器VIP {TextBox_NewVIPName.Text} 失败 {result.Message}  |  耗时: {result.ExecTime:0.00} 秒");
            }
        }

        private async void Button_RemoveSelectedBAN_Click(object sender, RoutedEventArgs e)
        {
            AudioUtils.ClickSound();

            ListItem currListItem = ListBox_BAN.SelectedItem as ListItem;

            MainWindow.SetOperatingState(2, $"正在移除服务器BAN {currListItem.displayName} 中...");

            var result = await ServerAPI.RemoveServerBan(currListItem.personaId);

            if (result.IsSuccess)
            {
                MainWindow.SetOperatingState(1, $"移除服务器BAN {currListItem.displayName} 成功  |  耗时: {result.ExecTime:0.00} 秒");
                ListBox_BAN.Items.Remove(currListItem);
            }
            else
            {
                MainWindow.SetOperatingState(3, $"移除服务器BAN {currListItem.displayName} 失败 {result.Message}  |  耗时: {result.ExecTime:0.00} 秒");
            }
        }

        private async void Button_AddNewBAN_Click(object sender, RoutedEventArgs e)
        {
            AudioUtils.ClickSound();

            MainWindow.SetOperatingState(2, $"正在增加服务器BAN {TextBox_NewBANName.Text} 中...");

            var result = await ServerAPI.AddServerBan(TextBox_NewBANName.Text);

            if (result.IsSuccess)
            {
                MainWindow.SetOperatingState(1, $"增加服务器BAN {TextBox_NewBANName.Text} 成功  |  耗时: {result.ExecTime:0.00} 秒");
                Button_GetFullServerDetails_Click(sender, e);
            }
            else
            {
                MainWindow.SetOperatingState(3, $"增加服务器BAN {TextBox_NewBANName.Text} 失败 {result.Message}  |  耗时: {result.ExecTime:0.00} 秒");
            }
        }

        private async void Button_KickSelectedSpectator_Click(object sender, RoutedEventArgs e)
        {
            AudioUtils.ClickSound();

            if (!string.IsNullOrEmpty(Globals.Config.SessionId))
            {
                SpectatorInfo info = ListBox_Spectator.SelectedItem as SpectatorInfo;

                MainWindow.SetOperatingState(2, $"正在踢出玩家 {info.Name} 中...");

                var reason = ChsUtils.ToTraditionalChinese(TextBox_KickSelectedSpectatorReason.Text);

                if (reason == "@Kick")
                {
                    reason = "ADMINPRIORITY";
                }

                var result = await ServerAPI.AdminKickPlayer(info.PersonaId.ToString(), reason);

                if (result.IsSuccess)
                {
                    MainWindow.SetOperatingState(1, $"踢出玩家 {info.Name} 成功  |  耗时: {result.ExecTime:0.00} 秒");
                    ListBox_Spectator.Items.Remove(info);
                }
                else
                {
                    MainWindow.SetOperatingState(3, $"踢出玩家 {info.Name} 失败 {result.Message}  |  耗时: {result.ExecTime:0.00} 秒");
                }
            }
            else
            {
                MainWindow.SetOperatingState(2, "请先获取玩家SessionID");
            }
        }

        private void Button_RefreshSpectatorList_Click(object sender, RoutedEventArgs e)
        {
            AudioUtils.ClickSound();

            ListBox_Spectator.Items.Clear();

            int index = 1;
            foreach (var item in Globals.PlayerDatas_Team3.Values)
            {
                ListBox_Spectator.Items.Add(new SpectatorInfo()
                {
                    Index = index++,
                    Avatar = @"\Assets\Images\Other\Avatar.jpg",
                    Name = item.Name,
                    PersonaId = item.PersonaId.ToString()
                });
            }
        }

        private async void Button_GetServerDetails_Click(object sender, RoutedEventArgs e)
        {
            AudioUtils.ClickSound();

            if (!string.IsNullOrEmpty(Globals.Config.SessionId))
            {
                if (!string.IsNullOrEmpty(Globals.Config.ServerId))
                {
                    MainWindow.SetOperatingState(2, $"正在获取服务器 {Globals.Config.ServerId} 数据中...");

                    var result = await ServerAPI.GetServerDetails();

                    if (result.IsSuccess)
                    {
                        serverDetails = result.Obj;

                        TextBox_ServerName.Text = serverDetails.result.serverSettings.name;
                        TextBox_ServerDescription.Text = serverDetails.result.serverSettings.description;

                        isGetServerDetailsOK = true;

                        MainWindow.SetOperatingState(1, $"获取服务器 {Globals.Config.ServerId} 数据成功  |  耗时: {result.ExecTime:0.00} 秒");
                    }
                    else
                    {
                        MainWindow.SetOperatingState(3, $"获取服务器 {Globals.Config.ServerId} 数据失败 {result.Message}  |  耗时: {result.ExecTime:0.00} 秒");
                    }
                }
                else
                {
                    MainWindow.SetOperatingState(2, "请先进入服务器获取ServerID");
                }
            }
            else
            {
                MainWindow.SetOperatingState(2, "请先获取玩家SessionID");
            }
        }

        private async void Button_UpdateServer_Click(object sender, RoutedEventArgs e)
        {
            AudioUtils.ClickSound();

            if (!isGetServerDetailsOK)
            {
                MainWindow.SetOperatingState(2, $"请先获取服务器信息后，再执行本操作");
                return;
            }

            var serverName = TextBox_ServerName.Text.Trim();
            var serverDescription = TextBox_ServerDescription.Text.Trim();

            if (string.IsNullOrEmpty(serverName))
            {
                MainWindow.SetOperatingState(2, $"服务器名称不能为空");
                return;
            }

            if (!string.IsNullOrEmpty(Globals.Config.SessionId))
            {
                if (!string.IsNullOrEmpty(Globals.Config.ServerId))
                {
                    MainWindow.SetOperatingState(2, $"正在更新服务器 {Globals.Config.ServerId} 数据中...");

                    UpdateServerReqBody reqBody = new();

                    var tempParams = new UpdateServerReqBody.Params
                    {
                        deviceIdMap = new UpdateServerReqBody.Params.DeviceIdMap()
                        {
                            machash = Guid.NewGuid().ToString()
                        },
                        serverId = Globals.Config.ServerId,
                        bannerSettings = new UpdateServerReqBody.Params.BannerSettings()
                        {
                            bannerUrl = "",
                            clearBanner = true
                        }
                    };

                    var tempMapRotation = new UpdateServerReqBody.Params.MapRotation();
                    var temp = serverDetails.result.mapRotations[0];
                    var tempMaps = new List<UpdateServerReqBody.Params.MapRotation.MapsItem>();
                    foreach (var item in temp.maps)
                    {
                        tempMaps.Add(new UpdateServerReqBody.Params.MapRotation.MapsItem()
                        {
                            gameMode = item.gameMode,
                            mapName = item.mapName
                        });
                    }
                    tempMapRotation.maps = tempMaps;
                    tempMapRotation.rotationType = temp.rotationType;
                    tempMapRotation.mod = temp.mod;
                    tempMapRotation.name = temp.name;
                    tempMapRotation.description = temp.description;
                    tempMapRotation.id = "100";

                    tempParams.mapRotation = tempMapRotation;

                    tempParams.serverSettings = new UpdateServerReqBody.Params.ServerSettings()
                    {
                        name = serverName,
                        description = serverDescription,

                        message = serverDetails.result.serverSettings.message,
                        password = serverDetails.result.serverSettings.password,
                        bannerUrl = serverDetails.result.serverSettings.bannerUrl,
                        mapRotationId = serverDetails.result.serverSettings.mapRotationId,
                        customGameSettings = serverDetails.result.serverSettings.customGameSettings
                    };

                    reqBody.@params = tempParams;
                    reqBody.id = Guid.NewGuid().ToString();

                    var result = await ServerAPI.UpdateServer(reqBody);

                    if (result.IsSuccess)
                    {
                        MainWindow.SetOperatingState(1, $"更新服务器 {Globals.Config.ServerId} 数据成功  |  耗时: {result.ExecTime:0.00} 秒");
                        Button_GetFullServerDetails_Click(sender, e);
                    }
                    else
                    {
                        MainWindow.SetOperatingState(3, $"更新服务器 {Globals.Config.ServerId} 数据失败 {result.Message}  |  耗时: {result.ExecTime:0.00} 秒");
                    }
                }
                else
                {
                    MainWindow.SetOperatingState(2, "请先进入服务器获取ServerID");
                }
            }
            else
            {
                MainWindow.SetOperatingState(2, "请先获取玩家SessionID");
            }

            isGetServerDetailsOK = false;
        }

        private void Button_SetServerDetails2Traditional_Click(object sender, RoutedEventArgs e)
        {
            AudioUtils.ClickSound();

            var serverDescription = TextBox_ServerDescription.Text.Trim();

            if (string.IsNullOrEmpty(serverDescription))
            {
                MainWindow.SetOperatingState(2, $"服务器描述不能为空");
                return;
            }

            TextBox_ServerDescription.Text = ChsUtils.ToTraditionalChinese(serverDescription);

            MainWindow.SetOperatingState(1, $"转换服务器描述文本为繁体中文成功");
        }
    }
}
