namespace BF1.ServerAdminTools.Wpf.Data;

public record ServerRuleObj
{
    /// <summary>
    /// 规则名字
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 最大击杀数
    /// </summary>
    public int MaxKill { get; set; }
    /// <summary>
    /// KD计算数
    /// </summary>
    public int KDFlag { get; set; }
    /// <summary>
    /// 最大KD
    /// </summary>
    public float MaxKD { get; set; }
    /// <summary>
    /// KPM计算数
    /// </summary>
    public int KPMFlag { get; set; }
    /// <summary>
    /// 最大KPM
    /// </summary>
    public float MaxKPM { get; set; }
    /// <summary>
    /// 最大等级
    /// </summary>
    public int MaxRank { get; set; }
    /// <summary>
    /// 最小等级
    /// </summary>
    public int MinRank { get; set; }
    /// <summary>
    /// 生涯KD
    /// </summary>
    public float LifeMaxKD { get; set; }
    /// <summary>
    /// 生涯KPM
    /// </summary>
    public float LifeMaxKPM { get; set; }
    /// <summary>
    /// 生涯武器星
    /// </summary>
    public int LifeMaxWeaponStar { get; set; }
    /// <summary>
    /// 生涯载具星
    /// </summary>
    public int LifeMaxVehicleStar { get; set; }
    /// <summary>
    /// 分差切换地图
    /// </summary>
    public int ScoreSwitchMap { get; set; }
    /// <summary>
    /// 切图计算最低分数
    /// </summary>
    public int ScoreStartSwitchMap { get; set; }
    /// <summary>
    /// 不再切图分数
    /// </summary>
    public int ScoreNotSwitchMap { get; set; }
    /// <summary>
    /// 切换地图类型
    /// </summary>
    public int SwitchMapType { get; set; }
    /// <summary>
    /// 自定义切换地图列表
    /// </summary>
    public List<string> SwitchMaps { get; set; } = new();
    /// <summary>
    /// 启用劣势方分数
    /// </summary>
    public int ScoreOtherRule { get; set; }
    /// <summary>
    /// 劣势方规则
    /// </summary>
    public string OtherRule { get; set; }
    /// <summary>
    /// 队伍2规则
    /// </summary>
    public string Team2Rule { get; set; }
    /// <summary>
    /// 保存限制武器名称列表
    /// </summary>
    public List<string> Custom_WeaponList { get; set; } = new();
    /// <summary>
    /// 自定义黑名单玩家列表
    /// </summary>
    public List<string> Custom_BlackList { get; set; } = new();
    /// <summary>
    /// 自定义白名单玩家列表
    /// </summary>
    public List<string> Custom_WhiteList { get; set; } = new();
    /// <summary>
    /// 白名单不限制击杀
    /// </summary>
    public bool WhiteListNoKill { get; set; }
    /// <summary>
    /// 白名单不限制KD
    /// </summary>
    public bool WhiteListNoKD { get; set; }
    /// <summary>
    /// 白名单不限制KPM
    /// </summary>
    public bool WhiteListNoKPM { get; set; }
    /// <summary>
    /// 白名单不限制武器
    /// </summary>
    public bool WhiteListNoW { get; set; }
    /// <summary>
    /// 白名单不限制武器数量
    /// </summary>
    public bool WhiteListNoN { get; set; }
    /// <summary>
    /// 武器数量限制
    /// </summary>
    public Dictionary<string, WeaponNumberObj> WeaponNumbers { get; set; }
}


