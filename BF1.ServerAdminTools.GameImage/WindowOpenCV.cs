using BF1.ServerAdminTools.Common;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Drawing;

namespace BF1.ServerAdminTools.GameImage;

public static class WindowOpenCV
{
    private static bool FindPicFromImage(Bitmap imgSrc, Bitmap imgSub, double threshold = 0.7)
    {
        Mat srcMat = null;
        Mat dstMat = null;
        OutputArray outArray = null;
        try
        {
            srcMat = imgSrc.ToMat();
            dstMat = imgSub.ToMat();
            outArray = OutputArray.Create(srcMat);
            Cv2.MatchTemplate(srcMat, dstMat, outArray, TemplateMatchModes.CCoeffNormed);
            double minValue, maxValue;
            OpenCvSharp.Point location, point;
            Cv2.MinMaxLoc(InputArray.Create(outArray.GetMat()), out minValue, out maxValue, out location, out point);
            Console.WriteLine(maxValue);
            if (maxValue >= threshold)
                return true;
            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            if (srcMat != null)
                srcMat.Dispose();
            if (dstMat != null)
                dstMat.Dispose();
            if (outArray != null)
                outArray.Dispose();
        }
    }

    public static bool Test1()
    {
        if (!Globals.IsToolInit)
            return false;
        return FindPicFromImage(GameWindowImg.GetWindow(), Resource1.main1);
    }

    public static bool Test2()
    {
        if (!Globals.IsToolInit)
            return false;
        return FindPicFromImage(GameWindowImg.GetWindow(), Resource1.main2);
    }

    public static bool Test3()
    {
        if (!Globals.IsToolInit)
            return false;
        return FindPicFromImage(GameWindowImg.GetWindow(), Resource1.main4);
    }

    public static bool Error1()
    {
        if (!Globals.IsToolInit)
            return false;
        return FindPicFromImage(GameWindowImg.GetWindow(), Resource1.error1);
    }

    public static bool Error2()
    {
        if (!Globals.IsToolInit)
            return false;
        return FindPicFromImage(GameWindowImg.GetWindow(), Resource1.error2);
    }
    public static bool Error3()
    {
        if (!Globals.IsToolInit)
            return false;
        return FindPicFromImage(GameWindowImg.GetWindow(), Resource1.error3);
    }
    public static bool Error4()
    {
        if (!Globals.IsToolInit)
            return false;
        return FindPicFromImage(GameWindowImg.GetWindow(), Resource1.error4);
    }
    public static bool Info1()
    {
        if (!Globals.IsToolInit)
            return false;
        return FindPicFromImage(GameWindowImg.GetWindow(), Resource1.info1);
    }
}
