using BF1.ServerAdminTools.Common;
using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Common.Helper;
using BF1.ServerAdminTools.Common.Utils;
using Downloader;

namespace BF1.ServerAdminTools.Wpf.Windows;

/// <summary>
/// UpdateWindow.xaml 的交互逻辑
/// </summary>
public partial class UpdateWindow : Window
{
    private DownloadService downloader;

    private UpdateInfo _updateInfo;

    public UpdateWindow(UpdateInfo updateInfo)
    {
        InitializeComponent();

        _updateInfo = updateInfo;
    }

    private void Window_Update_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            downloader = new DownloadService();

            foreach (var item in _updateInfo.Download)
            {
                ListBox_DownloadAddress.Items.Add(item.Name);
            }
            ListBox_DownloadAddress.SelectedIndex = 0;

            File.Delete(FileUtils.GetCurrFullPath("未下载完的更新.exe"));
        }
        catch (Exception ex)
        {
            MsgBoxUtils.ExceptionMsgBox(ex);
        }
    }

    private void Window_Update_Closing(object sender, CancelEventArgs e)
    {
        downloader.CancelAsync();
        downloader.Clear();
        downloader.Dispose();
    }

    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        ProcessUtils.OpenLink(e.Uri.OriginalString);
        e.Handled = true;
    }

    private void Button_Update_Click(object sender, RoutedEventArgs e)
    {
        AudioUtils.ClickSound();

        Button_Update.IsEnabled = false;
        Button_CancelUpdate.IsEnabled = true;

        TextBlock_Info.Text = "下载开始";
        TextBlock_Percentage.Text = "0KB / 0MB";

        int index = ListBox_DownloadAddress.SelectedIndex;
        if (index != -1)
        {
            CoreUtils.Update_Address = _updateInfo.Download[index].Url;
        }
        else
        {
            CoreUtils.Update_Address = "https://175.178.108.122:8082/BF1.ServerAdminTools.Wpf.exe";
        }

        // 下载临时文件完整路径
        string OldPath = FileUtils.GetCurrFullPath(CoreUtils.HalfwayAppName);

        downloader.DownloadProgressChanged += DownloadProgressChanged;
        downloader.DownloadFileCompleted += DownloadFileCompleted;

        downloader.DownloadFileTaskAsync(CoreUtils.Update_Address, OldPath);
    }

    private void Button_CancelUpdate_Click(object sender, RoutedEventArgs e)
    {
        AudioUtils.ClickSound();

        downloader.CancelAsync();
        downloader.Clear();

        Button_Update.IsEnabled = true;
        Button_CancelUpdate.IsEnabled = false;

        ProgressBar_Update.Minimum = 0;
        ProgressBar_Update.Maximum = 1024;
        ProgressBar_Update.Value = 0;

        TaskbarItemInfo.ProgressValue = 0;

        TextBlock_Info.Text = "下载取消";
        TextBlock_Percentage.Text = "0KB / 0MB";
    }

    private void DownloadProgressChanged(object? sender, Downloader.DownloadProgressChangedEventArgs e)
    {
        Dispatcher.BeginInvoke(new Action(delegate
        {
            ProgressBar_Update.Minimum = 0;
            ProgressBar_Update.Maximum = e.TotalBytesToReceive;
            ProgressBar_Update.Value = e.ReceivedBytesSize;

            TextBlock_Info.Text = $"下载开始 文件大小 {e.TotalBytesToReceive / 1024.0f / 1024:0.0}MB";

            TextBlock_Percentage.Text = $"{LongToString(e.ReceivedBytesSize)}/{LongToString(e.TotalBytesToReceive)}";

            TaskbarItemInfo.ProgressValue = ProgressBar_Update.Value / ProgressBar_Update.Maximum;
        }));
    }

    private void DownloadFileCompleted(object? sender, AsyncCompletedEventArgs e)
    {
        Dispatcher.BeginInvoke(new Action(delegate
        {
            if (e.Error != null)
            {
                ProgressBar_Update.Minimum = 0;
                ProgressBar_Update.Maximum = 1024;
                ProgressBar_Update.Value = 0;

                TaskbarItemInfo.ProgressValue = 0;

                TextBlock_Info.Text = $"下载失败 {e.Error.Message}";
                TextBlock_Percentage.Text = "0KB / 0MB";
            }
            else
            {
                try
                {
                    AudioUtils.SP_DownloadOK.Play();

                    // 下载临时文件完整路径
                    string OldPath = FileUtils.GetCurrFullPath(CoreUtils.HalfwayAppName);
                    // 下载完成后文件真正路径
                    string NewPath = FileUtils.GetCurrFullPath(CoreUtils.FinalAppName());
                    if (File.Exists(NewPath))
                    {
                        FileUtils.FileReName(NewPath, NewPath + ".old");
                    }
                    // 下载完成后新文件重命名
                    FileUtils.FileReName(OldPath, NewPath);

                    Thread.Sleep(100);

                    // 下载完成后旧文件重命名
                    string oldFileName = $"[旧版本服管工具请手动删除] {Guid.NewGuid()}.exe";
                    // 旧版本小助手重命名
                    FileUtils.FileReName(ConfigLocal.Current_Path, FileUtils.GetCurrFullPath(oldFileName));

                    TextBlock_Info.Text = "更新下载完成，程序将在3秒内重新启动";

                    App.AppMainMutex.Dispose();
                    Thread.Sleep(1000);
                    ProcessUtils.OpenLink(NewPath);
                    Application.Current.Shutdown();
                }
                catch (Exception ex)
                {
                    Core.LogError("下载错误", ex);
                    MsgBoxUtils.ExceptionMsgBox(ex);
                }
            }
        }));
    }

    private string LongToString(long num)
    {
        float kb = num / 1024.0f;

        if (kb > 1024)
        {
            return $"{kb / 1024:0.0}MB";
        }
        else
        {
            return $"{kb:0.0}KB";
        }
    }
}
