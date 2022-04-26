using BF1.ServerAdminTools.Common.Helper;

namespace BF1.ServerAdminTools.Common.Utils;

public static class FileUtils
{
    public static void WriteFile(string file, string data)
    {
        using var stream = File.Open(file, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        var temp = Encoding.UTF8.GetBytes(data);
        stream.Write(temp, 0, temp.Length);
    }

    /// <summary>
    /// 给文件名，得出当前目录完整路径，AppName带文件名后缀
    /// </summary>
    public static string GetCurrFullPath(string AppName)
    {
        return Path.Combine(ConfigLocal.CurrentDirectory_Path, AppName);
    }

    /// <summary>
    /// 文件重命名
    /// </summary>
    public static void FileReName(string OldPath, string NewPath)
    {
        FileInfo ReName = new(OldPath);
        ReName.MoveTo(NewPath);
    }
}
