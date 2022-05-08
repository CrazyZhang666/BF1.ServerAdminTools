namespace BF1.ServerAdminTools.Wpf.Utils;

public static class FileSelectUtils
{
    /// <summary>
    /// 选择txt
    /// </summary>
    /// <returns></returns>
    public static string? FileSelect()
    {
        var openFileDialog = new Microsoft.Win32.OpenFileDialog()
        {
            Filter = "名单 (*.txt)|*.txt"
        };
        var result = openFileDialog.ShowDialog();
        if (result == true)
        {
            return openFileDialog.FileName;
        }

        return null;
    }

    /// <summary>
    /// 选择图片
    /// </summary>
    /// <returns></returns>
    public static string? FileSelectPic()
    {
        var openFileDialog = new Microsoft.Win32.OpenFileDialog()
        {
            Filter = "图片 (*.jpg;*.png;*.bmp)|*.jpg;*.png;*.bmp"
        };
        var result = openFileDialog.ShowDialog();
        if (result == true)
        {
            return openFileDialog.FileName;
        }

        return null;
    }
}
