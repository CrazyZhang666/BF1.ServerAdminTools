using BF1.ServerAdminTools.Common;
using BF1.ServerAdminTools.Common.Utils;
using BF1.ServerAdminTools.Wpf.Data;
using BF1.ServerAdminTools.Wpf.Models;
using BF1.ServerAdminTools.Wpf.TaskList;
using BF1.ServerAdminTools.Wpf.Utils;
using BF1.ServerAdminTools.Wpf.Windows;

namespace BF1.ServerAdminTools.Wpf.Views;

public partial class RuleView
{
    private void AddMapList(object sender, RoutedEventArgs e)
    {
        if (Globals.ServerInfo == null)
        {
            MsgBoxUtils.WarningMsgBox("没有服务器信息");
            return;
        }

        var map = new MapSelectWindow(SwitchMapList.Items).Set();
        if (string.IsNullOrWhiteSpace(map))
            return;

        if (!SwitchMapList.Items.Contains(map))
        {
            SwitchMapList.Items.Add(map);
        }
    }

    private void DeleteMapList(object sender, RoutedEventArgs e)
    {
        if (SwitchMapList.SelectedItem is not string item)
            return;

        SwitchMapList.Items.Remove(item);
    }

    private void OtherRule_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (OtherRule.SelectedItem is not string item)
            return;

        if (!string.IsNullOrWhiteSpace(item))
            Team2Rule.SelectedItem = "";
    }

    private void TeamRule_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (Team2Rule.SelectedItem is not string item)
            return;

        if (!string.IsNullOrWhiteSpace(item))
            OtherRule.SelectedItem = "";
    }
}

public partial class RuleView
{
    private void BreakWeaponInfoAddClick(object sender, RoutedEventArgs e)
    {
        AudioUtils.ClickSound();

        bool isContains = false;

        int index = ListWeaponInfo.SelectedIndex;
        if (index != -1)
        {
            var wi = ListWeaponInfo.SelectedItem as WeaponInfoModel;
            if (string.IsNullOrEmpty(wi.Chinese))
            {
                MainWindow.SetOperatingState(2, "请不要把分类项添加到限制武器列表");
                return;
            }

            foreach (WeaponInfoModel item in BreakWeaponInfo.Items)
            {
                if (wi.English == item.English)
                {
                    isContains = true;
                    break;
                }
            }

            foreach (var item in BreakWeaponInfo.Items)
            {
                if (ListWeaponInfo.SelectedItem == item)
                {
                    isContains = true;
                    break;
                }
            }

            if (!isContains)
            {
                BreakWeaponInfo.Items.Add(ListWeaponInfo.SelectedItem);
                (ListWeaponInfo.Items[ListWeaponInfo.SelectedIndex] as WeaponInfoModel).Mark = "✔";

                ListWeaponInfo.SelectedIndex = index;

                int count = BreakWeaponInfo.Items.Count;
                if (count != 0)
                {
                    BreakWeaponInfo.SelectedIndex = count - 1;
                }

                MainWindow.SetOperatingState(1, "添加限制武器成功");
            }
            else
            {
                MainWindow.SetOperatingState(2, "当前限制武器已存在，请不要重复添加");
            }
        }
        else
        {
            MainWindow.SetOperatingState(2, "请选择正确的内容");
        }
    }

    private void BreakWeaponInfoRemoveClick(object sender, RoutedEventArgs e)
    {
        AudioUtils.ClickSound();

        int index1 = ListWeaponInfo.SelectedIndex;
        int index2 = BreakWeaponInfo.SelectedIndex;
        if (index2 != -1)
        {
            var bwi = BreakWeaponInfo.SelectedItem as WeaponInfoModel;
            foreach (WeaponInfoModel item in ListWeaponInfo.Items)
            {
                if (item.English == bwi.English)
                {
                    item.Mark = "";
                }
            }

            BreakWeaponInfo.Items.RemoveAt(BreakWeaponInfo.SelectedIndex);

            int count = BreakWeaponInfo.Items.Count;
            if (count != 0)
            {
                BreakWeaponInfo.SelectedIndex = count - 1;
            }

            ListWeaponInfo.SelectedIndex = index1;

            MainWindow.SetOperatingState(1, "移除限制武器成功");
        }
        else
        {
            MainWindow.SetOperatingState(2, "请选择正确的内容");
        }
    }

    private void BreakWeaponInfoClearClick(object sender, RoutedEventArgs e)
    {
        AudioUtils.ClickSound();

        int index = ListWeaponInfo.SelectedIndex;

        // 清空限制武器列表
        DataSave.NowRule.Custom_WeaponList.Clear();
        BreakWeaponInfo.Items.Clear();

        foreach (WeaponInfoModel item in ListWeaponInfo.Items)
        {
            item.Mark = "";
        }

        ListWeaponInfo.SelectedIndex = index;

        MainWindow.SetOperatingState(1, "清空限制武器列表成功");
    }
}
public partial class RuleView
{
    private void LoadWeaponNumber(string name)
    {
        if (!DataSave.NowRule.WeaponNumbers.TryGetValue(name, out var obj))
            return;
        NumberWeaponCount.Value = obj.Count;
        SelectNumberWeaponInfo.Items.Clear();

        foreach (var item in obj.Weapons)
        {
            SelectNumberWeaponInfo.Items.Add(new WeaponInfoModel()
            {
                English = item,
                Chinese = PlayerUtils.GetWeaponChsName(item)
            });
        }

        if (SelectNumberWeaponInfo.Items.Count != 0)
        {
            SelectNumberWeaponInfo.SelectedIndex = 0;
        }

        foreach (WeaponInfoModel item in ListNumberWeaponInfo.Items)
        {
            if (obj.Weapons.Contains(item.English))
            {
                item.Mark = "✔";
            }
            else
            {
                item.Mark = "";
            }
        }
    }
    private void NumberWeaponSetClick(object sender, RoutedEventArgs e)
    {
        AudioUtils.ClickSound();

        if (string.IsNullOrWhiteSpace(NumberName.Text))
            return;

        string namel = NumberName.Text.ToLower();

        if (!DataSave.NowRule.WeaponNumbers.TryGetValue(namel, out var obj))
            return;

        obj.Count = (int)NumberWeaponCount.Value;
        obj.Weapons.Clear();
        foreach (WeaponInfoModel item1 in SelectNumberWeaponInfo.Items)
        {
            obj.Weapons.Add(item1.English);
        }

        if (DataSave.NowRule.WeaponNumbers.ContainsKey(namel))
            DataSave.NowRule.WeaponNumbers[namel] = obj;
        else
            DataSave.NowRule.WeaponNumbers.Add(namel, obj);

        foreach (WeaponNumberModel item in NumberList.Items)
        {
            if (item.Name == NumberName.Text)
            {
                item.Count = obj.Count;
                break;
            }
        }

        ConfigUtils.SaveRule();
        TaskCheckNumber.Clear();

        MainWindow.SetOperatingState(1, "已设置限制组");
    }
    private void AddNumberList(object sender, RoutedEventArgs e) 
    {
        var name = new InputWindow("新建限制组名字", "输入限制组名字").Set();
        if (string.IsNullOrWhiteSpace(name))
            return;

        string namel = name.ToLower();

        if (DataSave.NowRule.WeaponNumbers.ContainsKey(namel))
        {
            MsgBoxUtils.WarningMsgBox("限制组名字已存在");
        }

        var obj = new WeaponNumberObj()
        {
            Name = name
        };
        LoadWeaponNumber(namel);
        NumberList.Items.Add(new WeaponNumberModel() 
        {
            Name = name
        });
        DataSave.NowRule.WeaponNumbers.Add(namel, obj);
        ConfigUtils.SaveRule();
    }
    private void DeleteNumberList(object sender, RoutedEventArgs e)
    {
        if (NumberList.SelectedItem is not WeaponNumberModel obj)
            return;

        string namel = obj.Name.ToLower();
        if (DataSave.NowRule.WeaponNumbers.Remove(namel))
        {
            ConfigUtils.SaveRule();
        }

        NumberList.Items.Remove(obj);
    }
    private void LoadNumberList(object sender, RoutedEventArgs e)
    {
        if (NumberList.SelectedItem is not WeaponNumberModel obj)
            return;

        NumberName.Text = obj.Name;

        LoadWeaponNumber(obj.Name.ToLower());
    }

    private void NumberWeaponAddClick(object sender, RoutedEventArgs e)
    {
        AudioUtils.ClickSound();

        bool isContains = false;

        int index = ListNumberWeaponInfo.SelectedIndex;
        if (index != -1)
        {
            var wi = ListNumberWeaponInfo.SelectedItem as WeaponInfoModel;
            if (string.IsNullOrEmpty(wi.Chinese))
            {
                MainWindow.SetOperatingState(2, "请不要把分类项添加到列表");
                return;
            }

            foreach (WeaponInfoModel item in SelectNumberWeaponInfo.Items)
            {
                if (wi.English == item.English)
                {
                    isContains = true;
                    break;
                }
            }

            foreach (var item in SelectNumberWeaponInfo.Items)
            {
                if (ListNumberWeaponInfo.SelectedItem == item)
                {
                    isContains = true;
                    break;
                }
            }

            if (!isContains)
            {
                SelectNumberWeaponInfo.Items.Add(ListNumberWeaponInfo.SelectedItem);
                (ListNumberWeaponInfo.Items[ListNumberWeaponInfo.SelectedIndex] as WeaponInfoModel).Mark = "✔";

                ListNumberWeaponInfo.SelectedIndex = index;

                int count = SelectNumberWeaponInfo.Items.Count;
                if (count != 0)
                {
                    SelectNumberWeaponInfo.SelectedIndex = count - 1;
                }

                MainWindow.SetOperatingState(1, "添加限制武器成功");
            }
            else
            {
                MainWindow.SetOperatingState(2, "当前武器已存在，请不要重复添加");
            }
        }
        else
        {
            MainWindow.SetOperatingState(2, "请选择正确的内容");
        }
    }

    private void NumberWeaponRemoveClick(object sender, RoutedEventArgs e)
    {
        AudioUtils.ClickSound();

        int index1 = ListNumberWeaponInfo.SelectedIndex;
        int index2 = SelectNumberWeaponInfo.SelectedIndex;
        if (index2 != -1)
        {
            var bwi = SelectNumberWeaponInfo.SelectedItem as WeaponInfoModel;
            foreach (WeaponInfoModel item in ListNumberWeaponInfo.Items)
            {
                if (item.English == bwi.English)
                {
                    item.Mark = "";
                }
            }

            SelectNumberWeaponInfo.Items.RemoveAt(SelectNumberWeaponInfo.SelectedIndex);

            int count = SelectNumberWeaponInfo.Items.Count;
            if (count != 0)
            {
                SelectNumberWeaponInfo.SelectedIndex = count - 1;
            }

            ListNumberWeaponInfo.SelectedIndex = index1;

            MainWindow.SetOperatingState(1, "移除限制武器成功");
        }
        else
        {
            MainWindow.SetOperatingState(2, "请选择正确的内容");
        }
    }

    private void NumberWeaponClearClick(object sender, RoutedEventArgs e)
    {
        AudioUtils.ClickSound();

        int index = ListNumberWeaponInfo.SelectedIndex;

        // 清空限制武器列表
        DataSave.NowRule.Custom_WeaponList.Clear();
        SelectNumberWeaponInfo.Items.Clear();

        foreach (WeaponInfoModel item in ListNumberWeaponInfo.Items)
        {
            item.Mark = "";
        }

        ListNumberWeaponInfo.SelectedIndex = index;

        MainWindow.SetOperatingState(1, "清空限制武器列表成功");
    }
}

public partial class RuleView
{
    private void AddBlackListClick(object sender, RoutedEventArgs e)
    {
        AudioUtils.ClickSound();

        if (TextBox_BlackList_PlayerName.Text != "")
        {
            bool isContains = false;

            foreach (var item in BlackList.Items)
            {
                if ((item as string) == TextBox_BlackList_PlayerName.Text)
                {
                    isContains = true;
                }
            }

            if (!isContains)
            {
                BlackList.Items.Add(TextBox_BlackList_PlayerName.Text);

                MainWindow.SetOperatingState(1, $"添加 {TextBox_BlackList_PlayerName.Text} 到黑名单列表成功");
                TextBox_BlackList_PlayerName.Text = "";
            }
            else
            {
                MainWindow.SetOperatingState(2, $"该项 {TextBox_BlackList_PlayerName.Text} 已经存在了，请不要重复添加");
                TextBox_BlackList_PlayerName.Text = "";
            }
        }
        else
        {
            MainWindow.SetOperatingState(2, $"待添加黑名单玩家ID为空，添加操作取消");
        }
    }

    private void RemoveBlackListClick(object sender, RoutedEventArgs e)
    {
        AudioUtils.ClickSound();

        if (BlackList.SelectedIndex != -1)
        {
            MainWindow.SetOperatingState(1, $"从黑名单列表删除（{BlackList.SelectedItem}）成功");
            BlackList.Items.Remove(BlackList.SelectedItem);
        }
        else
        {
            MainWindow.SetOperatingState(2, $"请正确选中你要删除的玩家ID或自定义黑名单列表为空，删除操作取消");
        }
    }

    private void InputBlackListClick(object sender, RoutedEventArgs e)
    {
        AudioUtils.ClickSound();

        var res = FileSelectUtils.FileSelect();
        if (res == null)
        {
            return;
        }

        try
        {
            var data = File.ReadAllText(res);

            // 清空黑名单列表
            DataSave.NowRule.Custom_BlackList.Clear();
            BlackList.Items.Clear();

            var list = data.Split("\n");
            foreach (var item in list)
            {
                var name = item.Trim();
                if (string.IsNullOrWhiteSpace(name))
                    continue;

                DataSave.NowRule.Custom_BlackList.Add(name);
                BlackList.Items.Add(name);
            }

            MainWindow.SetOperatingState(1, "导入黑名单列表成功");
        }
        catch (Exception ex)
        {
            MsgBoxUtils.ErrorMsgBox("导入黑名单时发生错误");
            Core.LogError("导入黑名单发生错误", ex);
            MainWindow.SetOperatingState(1, "导入黑名单列表发生错误");
            return;
        }
    }

    private void ClearBlackListClick(object sender, RoutedEventArgs e)
    {
        AudioUtils.ClickSound();

        // 清空黑名单列表
        DataSave.NowRule.Custom_BlackList.Clear();
        BlackList.Items.Clear();

        MainWindow.SetOperatingState(1, $"清空黑名单列表成功");
    }

    private void AddWhiteListClick(object sender, RoutedEventArgs e)
    {
        AudioUtils.ClickSound();

        if (TextBox_WhiteList_PlayerName.Text != "")
        {
            bool isContains = false;

            foreach (var item in WhiteList.Items)
            {
                if ((item as string) == TextBox_WhiteList_PlayerName.Text)
                {
                    isContains = true;
                }
            }

            if (!isContains)
            {
                WhiteList.Items.Add(TextBox_WhiteList_PlayerName.Text);

                MainWindow.SetOperatingState(1, $"添加 {TextBox_WhiteList_PlayerName.Text} 到白名单列表成功");

                TextBox_WhiteList_PlayerName.Text = "";
            }
            else
            {
                MainWindow.SetOperatingState(2, $"该项 {TextBox_WhiteList_PlayerName.Text} 已经存在了，请不要重复添加");
                TextBox_WhiteList_PlayerName.Text = "";
            }
        }
        else
        {
            MainWindow.SetOperatingState(2, $"待添加白名单玩家ID为空，添加操作取消");
        }
    }

    private void RemoveWhiteListClick(object sender, RoutedEventArgs e)
    {
        AudioUtils.ClickSound();

        if (WhiteList.SelectedIndex != -1)
        {
            MainWindow.SetOperatingState(1, $"从白名单列表删除（{WhiteList.SelectedItem}）成功");
            WhiteList.Items.Remove(WhiteList.SelectedItem);
        }
        else
        {
            MainWindow.SetOperatingState(2, $"请正确选中你要删除的玩家ID或自定义白名单列表为空，删除操作取消");
        }
    }

    private void InputWhiteListClick(object sender, RoutedEventArgs e)
    {
        AudioUtils.ClickSound();

        var res = FileSelectUtils.FileSelect();
        if (res == null)
        {
            return;
        }

        try
        {
            var data = File.ReadAllText(res);

            // 清空黑名单列表
            DataSave.NowRule.Custom_WhiteList.Clear();
            WhiteList.Items.Clear();

            var list = data.Split("\n");
            foreach (var item in list)
            {
                var name = item.Trim();
                if (string.IsNullOrWhiteSpace(name))
                    continue;

                DataSave.NowRule.Custom_WhiteList.Add(name);
                WhiteList.Items.Add(name);
            }

            MainWindow.SetOperatingState(1, "导入白名单列表成功");
        }
        catch (Exception ex)
        {
            MsgBoxUtils.ErrorMsgBox("导入白名单时发生错误");
            Core.LogError("导入白名单发生错误", ex);
            MainWindow.SetOperatingState(1, "导入白名单列表发生错误");
            return;
        }
    }

    private void ClearWhiteListClick(object sender, RoutedEventArgs e)
    {
        AudioUtils.ClickSound();

        // 清空白名单列表
        DataSave.NowRule.Custom_WhiteList.Clear();
        WhiteList.Items.Clear();

        MainWindow.SetOperatingState(1, $"清空白名单列表成功");
    }
}