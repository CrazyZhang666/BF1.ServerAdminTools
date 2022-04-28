using System.Net.Http;

namespace BF1.ServerAdminTools.Common.Helper;

public static class HttpUtil
{
    private static readonly HttpClient client = new();
    /// <summary>
    /// 获取网页
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>

    public static async Task<string> HttpClientGET(string url)
    {
        try
        {
            var response = await client.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return string.Empty;
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="url"></param>
    /// <param name="saveDirectory"></param>
    /// <returns></returns>

    public static async Task<bool> DownloadFile(string url, string saveDirectory)
    {
        try
        {
            var response = await client.GetAsync(url);

            if (response?.RequestMessage?.RequestUri == null)
                return false;
            using var stream = await response.Content.ReadAsStreamAsync();
            string extension = Path.GetFileName(response.RequestMessage.RequestUri.ToString());
            using var fileStream = new FileStream(saveDirectory + extension, FileMode.CreateNew);
            byte[] buffer = new byte[1024];
            int readLength = 0;
            int length;
            while ((length = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
            {
                readLength += length;
                fileStream.Write(buffer, 0, length);
            }

            return true;
        }
        catch (IOException)
        {
            return false;
        }
    }
}
