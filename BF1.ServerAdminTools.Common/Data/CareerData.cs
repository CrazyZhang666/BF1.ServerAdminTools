namespace BF1.ServerAdminTools.Common.Data;

public class CareerData
{
    public record CareerName
    {
        public string ID;
        public string Chinese;
        public string English;
    }

    public static CareerName ASSAULT = new()
    {
        ID = "ID_M_ASSAULT",
        Chinese = "突击兵",
        English = "Assault"
    };

    public static CareerName MEDIC = new()
    {
        ID = "ID_M_MEDIC",
        Chinese = "医疗兵",
        English = "Medic"
    };

    public static CareerName SUPPORT = new()
    {
        ID = "ID_M_SUPPORT",
        Chinese = "支援兵",
        English = "Support"
    };

    public static CareerName SCOUT = new()
    {
        ID = "ID_M_SCOUT",
        Chinese = "侦察兵",
        English = "Scout"
    };

    public static CareerName TANKER = new()
    {
        ID = "ID_M_TANKER",
        Chinese = "坦克手",
        English = "Tanker"
    };

    public static CareerName PILOT = new()
    {
        ID = "ID_M_PILOT",
        Chinese = "飞行员",
        English = "Pilot"
    };

    public static CareerName CAVALRY = new()
    {
        ID = "ID_M_CAVALRY",
        Chinese = "骑兵",
        English = "Cavalry"
    };

    public static CareerName FLAMETHROWER = new()
    {
        ID = "ID_M_FLAMETHROWER",
        Chinese = "喷火兵",
        English = "FlameThrower"
    };

    public static CareerName ANTITANK = new()
    {
        ID = "ID_M_ANTITANK",
        Chinese = "坦克猎手",
        English = "AntiTank"
    };

    public static CareerName SENTRY = new()
    {
        ID = "ID_M_SENTRY",
        Chinese = "哨兵",
        English = "Sentry"
    };

    public static CareerName RUNNER = new()
    {
        ID = "ID_M_RUNNER",
        Chinese = "入侵者",
        English = "Runner"
    };

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
