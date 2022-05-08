namespace BF1.ServerAdminTools.Wpf.Data;

public record WeaponNumberObj
{
    public string Name { get; set; }
    public List<string> Weapons { get; set; } = new();
    public int Count { get; set; }
}
