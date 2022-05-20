namespace BF1.ServerAdminTools.Common.Data;

public record PlayerData
{
    public bool Admin { get; set; }
    public bool VIP { get; set; }

    public bool IsSurvival { get; set; }
    public byte Mark { get; set; }
    public int TeamID { get; set; }
    public byte Spectator { get; set; }
    public string Clan { get; set; }
    public string Name { get; set; }
    public long PersonaId { get; set; }
    public string SquadId { get; set; }
    public string Career { get; set; }
    public string CareerCH { get; set; }

    public int Rank { get; set; }
    public int Kill { get; set; }
    public int Dead { get; set; }
    public int Score { get; set; }

    public float KD { get; set; }
    public float KPM { get; set; }

    public string WeaponS0 { get; set; }
    public string WeaponS1 { get; set; }
    public string WeaponS2 { get; set; }
    public string WeaponS3 { get; set; }
    public string WeaponS4 { get; set; }
    public string WeaponS5 { get; set; }
    public string WeaponS6 { get; set; }
    public string WeaponS7 { get; set; }

    public string WeaponS0CH { get; set; }
    public string WeaponS1CH { get; set; }
    public string WeaponS2CH { get; set; }
    public string WeaponS3CH { get; set; }
    public string WeaponS4CH { get; set; }
    public string WeaponS5CH { get; set; }
    public string WeaponS6CH { get; set; }
    public string WeaponS7CH { get; set; }
}
