namespace BF1.ServerAdminTools.Common.Data;

public class CareerData
{
    public record CareerName
    {
        public string ID;
        public string Chinese;
        public string English;
    }

    /// <summary>
    /// 突击兵
    /// </summary>
    public static CareerName ASSAULT = new()
    {
        ID = "ID_M_ASSAULT",
        Chinese = "突击兵",
        English = "Assault"
    };
    /// <summary>
    /// 医疗兵
    /// </summary>
    public static CareerName MEDIC = new()
    {
        ID = "ID_M_MEDIC",
        Chinese = "医疗兵",
        English = "Medic"
    };
    /// <summary>
    /// 支援兵
    /// </summary>
    public static CareerName SUPPORT = new()
    {
        ID = "ID_M_SUPPORT",
        Chinese = "支援兵",
        English = "Support"
    };
    /// <summary>
    /// 侦察兵
    /// </summary>
    public static CareerName SCOUT = new()
    {
        ID = "ID_M_SCOUT",
        Chinese = "侦察兵",
        English = "Scout"
    };
    /// <summary>
    /// 坦克手
    /// </summary>
    public static CareerName TANKER = new()
    {
        ID = "ID_M_TANKER",
        Chinese = "坦克手",
        English = "Tanker"
    };
    /// <summary>
    /// 飞行员
    /// </summary>
    public static CareerName PILOT = new()
    {
        ID = "ID_M_PILOT",
        Chinese = "飞行员",
        English = "Pilot"
    };
    /// <summary>
    /// 骑兵
    /// </summary>
    public static CareerName CAVALRY = new()
    {
        ID = "ID_M_CAVALRY",
        Chinese = "骑兵",
        English = "Cavalry"
    };
    /// <summary>
    /// 喷火兵
    /// </summary>
    public static CareerName FLAMETHROWER = new()
    {
        ID = "ID_M_FLAMETHROWER",
        Chinese = "喷火兵",
        English = "FlameThrower"
    };
    /// <summary>
    /// 坦克猎手
    /// </summary>
    public static CareerName ANTITANK = new()
    {
        ID = "ID_M_ANTITANK",
        Chinese = "坦克猎手",
        English = "AntiTank"
    };
    /// <summary>
    /// 哨兵
    /// </summary>
    public static CareerName SENTRY = new()
    {
        ID = "ID_M_SENTRY",
        Chinese = "哨兵",
        English = "Sentry"
    };
    /// <summary>
    /// 入侵者
    /// </summary>
    public static CareerName RUNNER = new()
    {
        ID = "ID_M_RUNNER",
        Chinese = "入侵者",
        English = "Runner"
    };
    /// <summary>
    /// 战壕奇兵
    /// </summary>
    public static CareerName RAIDER = new()
    {
        ID = "ID_M_RAIDER",
        Chinese = "战壕奇兵",
        English = "Raider"
    };

    /// <summary>
    /// 全部职业信息，ShortTxt不超过16个字符
    /// </summary>
    public static List<CareerName> AllCareerInfo { get; } = new()
    {
        ASSAULT, MEDIC, SUPPORT, SCOUT, TANKER, PILOT,CAVALRY, FLAMETHROWER, ANTITANK, SENTRY, RUNNER, RAIDER
    };
}
