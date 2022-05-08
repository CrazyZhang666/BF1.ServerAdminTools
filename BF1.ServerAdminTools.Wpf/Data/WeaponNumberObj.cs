using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF1.ServerAdminTools.Wpf.Data;

public record WeaponNumberObj
{
    public string Name { get; set; }
    public List<string> Weapons { get; set; }
    public int Count { get; set; }
}
