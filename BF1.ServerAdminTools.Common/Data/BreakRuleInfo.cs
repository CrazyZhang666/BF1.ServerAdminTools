namespace BF1.ServerAdminTools.Common.Data;

public enum BreakType
{
    Kill_Limit, KD_Limit, KPM_Limit, Rank_Limit, Weapon_Limit, Life_KD_Limit, Life_KPM_Limit, Life_Weapon_Star_Limit, Life_Vehicle_Star_Limit, Min_Rank_Limit, Max_Rank_Limit, Server_Black_List, To_Many_Weapon, Error_Weapon
}

public record BreakRuleInfo
{
    /// <summary>
    /// 被踢出的玩家ID
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 被踢出的玩家数字ID
    /// </summary>
    public long PersonaId { get; set; }
    /// <summary>
    /// 被踢出原因
    /// </summary>
    public BreakType Type { get; set; }
    /// <summary>
    /// 被踢出的原因
    /// </summary>
    public string Reason { get; set; }
    /// <summary>
    /// 发送到群的信息
    /// </summary>
    public string Reason1 { get; set; }
    /// <summary>
    /// 执行踢人操作的状态
    /// </summary>
    public string Status { get; set; }
    /// <summary>
    /// 记录踢人请求响应时间
    /// </summary>
    public DateTime Time { get; set; }
}
