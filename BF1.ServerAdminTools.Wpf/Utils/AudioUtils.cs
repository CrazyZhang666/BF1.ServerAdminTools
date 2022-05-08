using BF1.ServerAdminTools.Wpf.Data;
using System.Media;

namespace BF1.ServerAdminTools.Wpf.Utils;

public static class AudioUtils
{
    public static SoundPlayer SP_Click_01 = new(Properties.Resources.Click_01);
    public static SoundPlayer SP_Click_02 = new(Properties.Resources.Click_02);
    public static SoundPlayer SP_Click_03 = new(Properties.Resources.Click_03);
    public static SoundPlayer SP_Click_04 = new(Properties.Resources.Click_04);
    public static SoundPlayer SP_Click_05 = new(Properties.Resources.Click_05);

    public static SoundPlayer SP_DownloadOK = new(Properties.Resources.DownloadOK);

    /// <summary>
    /// 按钮点击音效
    /// </summary>
    public static void ClickSound()
    {
        switch (DataSave.Config.AudioSelect)
        {
            case 0:
                break;
            case 1:
                SP_Click_01.Play();
                break;
            case 2:
                SP_Click_02.Play();
                break;
            case 3:
                SP_Click_03.Play();
                break;
            case 4:
                SP_Click_04.Play();
                break;
            case 5:
                SP_Click_05.Play();
                break;
        }
    }
}
