﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF1.ServerAdminTools.SDK.Objs;

public record SoreInfoObj
{
    /// <summary>
    /// 服务器信息
    /// </summary>
    public ServerInfoObj Info { get; set; }
    /// <summary>
    /// 队伍1
    /// </summary>
    public List<PlayerDataObj> Team1 { get; set; }
    /// <summary>
    /// 队伍2
    /// </summary>
    public List<PlayerDataObj> Team2 { get; set; }
    /// <summary>
    /// 观战
    /// </summary>
    public List<PlayerDataObj> Team3 { get; set; }
}