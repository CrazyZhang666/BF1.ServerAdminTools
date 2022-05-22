namespace BF1.ServerAdminTools.Netty;

public record ConfigNettyObj
{
    /// <summary>
    /// 端口
    /// </summary>
    public int Port { get; set; }
    /// <summary>
    /// 服务器密钥
    /// </summary>
    public long ServerKey { get; set; }
}
