namespace BF1.ServerAdminTools.Common.Utils;

public static class JsonUtils
{
    private readonly static JsonSerializerOptions Options1 = new()
    {
        IncludeFields = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
    };

    private readonly static JsonSerializerOptions Options2 = new()
    {
        WriteIndented = true,
        IncludeFields = true,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
    };

    /// <summary>
    /// 反序列化，将json字符串转换成json类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="result"></param>
    /// <returns></returns>
    public static T JsonDese<T>(string result)
    {
        try
        {
            return JsonSerializer.Deserialize<T>(result, Options1);
        }
        catch (Exception e)
        {
            Core.LogError("文件解析错误", e);
            return default;
        }
    }

    /// <summary>
    /// 序列化，将json类转换成json字符串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jsonClass"></param>
    /// <returns></returns>
    public static string JsonSeri<T>(T jsonClass)
    {
        return JsonSerializer.Serialize(jsonClass, Options2);
    }
}
