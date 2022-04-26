namespace BF1.ServerAdminTools.Common.Utils;

public static class FileSelectUtils
{
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
