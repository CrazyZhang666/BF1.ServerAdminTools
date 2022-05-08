using BF1.ServerAdminTools.Wpf.Data;
using BF1.ServerAdminTools.Wpf.Utils;
using BF1.ServerAdminTools.Wpf.Windows;

namespace BF1.ServerAdminTools.Wpf.Views;

public partial class RuleView
{
    private bool IsRun1 = false;
    private async void UpdateSubscribeClick(object sender, RoutedEventArgs e)
    {
        if (IsRun1)
            return;

        IsRun1 = true;
        UpdateSubscribe.IsEnabled = false;
        MainWindow.SetOperatingState(1, "开始更新订阅");
        await SubscribeUtils.UpdateAll();
        SubscribeBlackList.Items.Clear();
        foreach (var item in DataSave.SubscribeCache.Cache)
        {
            SubscribeBlackList.Items.Add(item);
        }

        MainWindow.SetOperatingState(1, "订阅更新完成");
        IsRun1 = false;
        UpdateSubscribe.IsEnabled = true;
    }

    private async void AddSubscribe(object sender, RoutedEventArgs e)
    {
        var url = new InputWindow("订阅地址", "请输入订阅黑名单的网址").Set();
        if (string.IsNullOrWhiteSpace(url))
            return;
        var res = await SubscribeUtils.Add(url);
        if (res.OK == false)
        {
            MsgBoxUtils.WarningMsgBox("订阅信息获取失败");
            return;
        }

        SubscribeBlackList.Items.Add(res.obj);
    }

    private void DeleteSubscribe(object sender, RoutedEventArgs e)
    {
        if (SubscribeBlackList.SelectedItem is not SubscribeObj item)
            return;

        SubscribeUtils.Delete(item.Url);
        SubscribeBlackList.Items.Remove(item);
    }

    private void SeeSubscribe(object sender, RoutedEventArgs e)
    {
        if (IsRun1)
            return;

        if (SubscribeBlackList.SelectedItem is not SubscribeObj item)
            return;

        BanListInfo.Items.Clear();
        foreach (var item1 in item.Players)
        {
            BanListInfo.Items.Add(item1);
        }
    }
}
