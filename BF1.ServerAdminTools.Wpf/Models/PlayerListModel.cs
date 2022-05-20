using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace BF1.ServerAdminTools.Wpf.Models;

public class PlayerListModel : ObservableObject, IComparable<PlayerListModel>
{
    public long PersionID;

    private int _index;
    /// <summary>
    /// 序号
    /// </summary>
    public int Index
    {
        get { return _index; }
        set { _index = value; OnPropertyChanged(); }
    }

    ///////////////////////////////////////////////////////////////////////

    private string _clan;
    /// <summary>
    /// 玩家战队
    /// </summary>
    public string Clan
    {
        get { return _clan; }
        set { _clan = value; OnPropertyChanged(); }
    }

    private string _name;
    /// <summary>
    /// 玩家ID
    /// </summary>
    public string Name
    {
        get { return _name; }
        set { _name = value; OnPropertyChanged(); }
    }

    private long _personaId;
    /// <summary>
    /// 玩家数字ID
    /// </summary>
    public long PersonaId
    {
        get { return _personaId; }
        set { _personaId = value; OnPropertyChanged(); }
    }

    private string _squadId;
    /// <summary>
    /// 玩家小队ID
    /// </summary>
    public string SquadId
    {
        get { return _squadId; }
        set { _squadId = value; OnPropertyChanged(); }
    }

    ///////////////////////////////////////////////////////////////////////

    private string _admin;
    /// <summary>
    /// 管理员
    /// </summary>
    public string Admin
    {
        get { return _admin; }
        set { _admin = value; OnPropertyChanged(); }
    }

    private string _vip;
    /// <summary>
    /// VIP
    /// </summary>
    public string VIP
    {
        get { return _vip; }
        set { _vip = value; OnPropertyChanged(); }
    }

    ///////////////////////////////////////////////////////////////////////

    private int _rank;
    /// <summary>
    /// 等级
    /// </summary>
    public int Rank
    {
        get { return _rank; }
        set { _rank = value; OnPropertyChanged(); }
    }

    private int _kill;
    /// <summary>
    /// 击杀
    /// </summary>
    public int Kill
    {
        get { return _kill; }
        set { _kill = value; OnPropertyChanged(); }
    }

    private int _dead;
    /// <summary>
    /// 死亡
    /// </summary>
    public int Dead
    {
        get { return _dead; }
        set { _dead = value; OnPropertyChanged(); }
    }

    private string _kd;
    /// <summary>
    /// KD
    /// </summary>
    public string KD
    {
        get { return _kd; }
        set { _kd = value; OnPropertyChanged(); }
    }

    private string _kpm;
    /// <summary>
    /// KPM
    /// </summary>
    public string KPM
    {
        get { return _kpm; }
        set { _kpm = value; OnPropertyChanged(); }
    }

    private int _score;
    /// <summary>
    /// 得分
    /// </summary>
    public int Score
    {
        get { return _score; }
        set { _score = value; OnPropertyChanged(); }
    }

    ///////////////////////////////////////////////////////////////////////

    private string _weaponS0;
    /// <summary>
    /// 武器槽0
    /// </summary>
    public string WeaponS0
    {
        get { return _weaponS0; }
        set { _weaponS0 = value; OnPropertyChanged(); }
    }

    private string _weaponS1;
    /// <summary>
    /// 武器槽1
    /// </summary>
    public string WeaponS1
    {
        get { return _weaponS1; }
        set { _weaponS1 = value; OnPropertyChanged(); }
    }

    private string _weaponS2;
    /// <summary>
    /// 武器槽2
    /// </summary>
    public string WeaponS2
    {
        get { return _weaponS2; }
        set { _weaponS2 = value; OnPropertyChanged(); }
    }

    private string _weaponS3;
    /// <summary>
    /// 武器槽3
    /// </summary>
    public string WeaponS3
    {
        get { return _weaponS3; }
        set { _weaponS3 = value; OnPropertyChanged(); }
    }

    private string _weaponS4;
    /// <summary>
    /// 武器槽4
    /// </summary>
    public string WeaponS4
    {
        get { return _weaponS4; }
        set { _weaponS4 = value; OnPropertyChanged(); }
    }

    private string _weaponS5;
    /// <summary>
    /// 武器槽5
    /// </summary>
    public string WeaponS5
    {
        get { return _weaponS5; }
        set { _weaponS5 = value; OnPropertyChanged(); }
    }

    private string _weaponS6;
    /// <summary>
    /// 武器槽6
    /// </summary>
    public string WeaponS6
    {
        get { return _weaponS6; }
        set { _weaponS6 = value; OnPropertyChanged(); }
    }

    private string _weaponS7;
    /// <summary>
    /// 武器槽0
    /// </summary>
    public string WeaponS7
    {
        get { return _weaponS7; }
        set { _weaponS7 = value; OnPropertyChanged(); }
    }

    private string _career;
    /// <summary>
    /// 职业
    /// </summary>
    public string Career
    {
        get { return _career; }
        set { _career = value; OnPropertyChanged(); }
    }

    public int CompareTo(PlayerListModel other)
    {
        return other.Score.CompareTo(this.Score);
    }
}
