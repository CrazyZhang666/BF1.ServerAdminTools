namespace BF1.ServerAdminTools.Common.Data;

public class WeaponData
{
    public record WeaponName
    {
        public string ID;
        public string Chinese;
        public string English;
        public string Career;
    }

    /// <summary>
    /// 全部武器信息，ShortTxt不超过16个字符
    /// </summary>
    public static List<WeaponName> AllWeaponInfo { get; } = new()
    {
        // 配枪
        new(){ ID="======== 公用配枪 ========", Chinese="", English="" },
        ////
        new(){ ID="U_M1911", Chinese="M1911", English="M1911" },
        new(){ ID="U_LugerP08", Chinese="P08 手枪", English="P08" },
        new(){ ID="U_FN1903", Chinese="Mle 1903", English="M1903" },
        new(){ ID="U_BorchardtC93", Chinese="C93", English="C93" },
        new(){ ID="U_SmithWesson", Chinese="3 号左轮手枪", English="No3 Rev" },
        new(){ ID="U_Kolibri", Chinese="Kolibri", English="Kolibri" },
        new(){ ID="U_NagantM1895", Chinese="纳甘左轮手枪", English="Nagant Rev" },
        new(){ ID="U_Obrez", Chinese="Obrez 手枪", English="Obrez" },
        new(){ ID="U_Webley_Mk6", Chinese="Mk VI 左轮手枪", English="Mk VI" },

        new(){ ID="U_M1911_Preorder_Hellfighter", Chinese="地狱战士 M1911", English="M1911 HF" },
        new(){ ID="U_LugerP08_Wep_Preorder", Chinese="红男爵的 P08", English="P08 HNJ" },
        new(){ ID="U_M1911_Suppressed", Chinese="M1911（消音器）", English="M1911 XYQ" },

        new(){ ID="U_SingleActionArmy", Chinese="维和左轮 Peacekeeper", English="Peacekeeper" },

        // 手榴弹
        new(){ ID="======== 手榴弹 ========", Chinese="", English="" },
        ////
        new(){ ID="U_FragGrenade", Chinese="棒式手榴弹", English="Frag Grenade" },
        new(){ ID="U_GermanStick", Chinese="破片手榴弹", English="German Stick" },
        new(){ ID="U_GasGrenade", Chinese="毒气手榴弹", English="Gas Grenade" },
        new(){ ID="U_ImpactGrenade", Chinese="衝击手榴弹", English="Impact Grenade" },
        new(){ ID="U_Incendiary", Chinese="燃烧手榴弹", English="Incendiary" },
        new(){ ID="U_MiniGrenade", Chinese="小型手榴弹", English="Mini Grenade" },
        new(){ ID="U_SmokeGrenade", Chinese="烟雾手榴弹", English="Smoke Grenade" },
        new(){ ID="U_Grenade_AT", Chinese="轻型反坦克手榴弹", English="Grenade AT" },

        new(){ ID="U_ImprovisedGrenade", Chinese="土制手榴弹", English="Imsp Grenade" },
        new(){ ID="U_RussianBox", Chinese="俄罗斯标准手榴弹", English="Russian Box" },

        ////////////////////////////////// 突击兵 Assault //////////////////////////////////

        // 主要武器
        new(){ ID="======== 突击兵 主要武器 ========", Chinese="", English="" },
        ////
        new(){ ID="U_RemingtonM10_Wep_Slug", Chinese="Model 10-A（霰弹块）", English="10A XDK",
            Career=CareerData.ASSAULT.ID },
        new(){ ID="U_RemingtonM10_Wep_Choke", Chinese="Model 10-A（猎人）", English="10A LR",
            Career=CareerData.ASSAULT.ID},
        new(){ ID="U_RemingtonM10", Chinese="Model 10-A（原厂）", English="10A YC",
            Career=CareerData.ASSAULT.ID},
        new(){ ID="U_Winchester1897_Wep_Sweeper", Chinese="M97 战壕枪（扫荡）", English="M97 SD",
            Career=CareerData.ASSAULT.ID},
        new(){ ID="U_Winchester1897_Wep_LowRecoil", Chinese="M97 战壕枪（Back-Bored）",
            English="M97 BB", Career=CareerData.ASSAULT.ID },
        new(){ ID="U_Winchester1897_Wep_Choke", Chinese="M97 战壕枪（猎人）", English="M97 LR",
            Career=CareerData.ASSAULT.ID },
        new(){ ID="U_MP18_Wep_Trench", Chinese="MP 18（壕沟战）", English="MP18 HGZ",
            Career=CareerData.ASSAULT.ID},
        new(){ ID="U_MP18_Wep_Burst", Chinese="MP 18（实验）", English="MP18 SY",
            Career=CareerData.ASSAULT.ID },
        new(){ ID="U_MP18_Wep_Accuracy", Chinese="MP 18（瞄准镜）", English="MP18 MZJ",
            Career=CareerData.ASSAULT.ID},
        new(){ ID="U_BerettaM1918_Wep_Trench", Chinese="M1918 自动衝锋枪（壕沟战）",
            English="MP1918 HGZ" , Career=CareerData.ASSAULT.ID},
        new(){ ID="U_BerettaM1918_Wep_Stability", Chinese="M1918 自动衝锋枪（衝锋）",
            English="MP1918 CF" , Career=CareerData.ASSAULT.ID},
        new(){ ID="U_BerettaM1918", Chinese="M1918 自动衝锋枪（原厂）", English="MP1918 YC",
            Career=CareerData.ASSAULT.ID },
        new(){ ID="U_BrowningA5_Wep_LowRecoil", Chinese="12g 自动霰弹枪（Back-Bored）",
            English="12g BB" , Career=CareerData.ASSAULT.ID},
        new(){ ID="U_BrowningA5_Wep_Choke", Chinese="12g 自动霰弹枪（猎人）", English="12g LR",
            Career=CareerData.ASSAULT.ID },
        new(){ ID="U_BrowningA5_Wep_ExtensionTube", Chinese="12g 自动霰弹枪（加长）", English="12g JC",
            Career=CareerData.ASSAULT.ID },
        new(){ ID="U_Hellriegel1915", Chinese="Hellriegel 1915（原厂）", English="H1915 YC",
            Career=CareerData.ASSAULT.ID },
        new(){ ID="U_Hellriegel1915_Wep_Accuracy", Chinese="Hellriegel 1915（防御）", English="H1915 FY",
            Career=CareerData.ASSAULT.ID },
        new(){ ID="U_Winchester1897_Wep_Preorder", Chinese="地狱战士战壕霰弹枪", English="M97 DYZS", Career=CareerData.ASSAULT.ID },
        new(){ ID="U_SjogrenShotgun", Chinese="Sjögren Inertial（原厂）", English="RDP YC",
            Career=CareerData.ASSAULT.ID },
        new(){ ID="U_SjogrenShotgun_Wep_Slug", Chinese="Sjögren Inertial（霰弹块）", English="RDP XDK" , Career=CareerData.ASSAULT.ID},
        new(){ ID="U_Ribeyrolles", Chinese="利贝罗勒 1918（原厂）", English="L1918 YC",
            Career=CareerData.ASSAULT.ID },
        new(){ ID="U_Ribeyrolles_Wep_Optical", Chinese="Ribeyrolles 1918（瞄准镜）", English="L1918 MZJ",
            Career=CareerData.ASSAULT.ID },
        new(){ ID="U_RemingtonModel1900", Chinese="Model 1900（原厂）", English="M1900 YC",
            Career=CareerData.ASSAULT.ID},
        new(){ ID="U_RemingtonModel1900_Wep_Slug", Chinese="Model 1900（霰弹块）", English="M1900 XDK", Career=CareerData.ASSAULT.ID },
        new(){ ID="U_MaximSMG", Chinese="SMG 08/18（原厂）", English="SMG0818 YC",
            Career=CareerData.ASSAULT.ID},
        new(){ ID="U_MaximSMG_Wep_Accuracy", Chinese="SMG 08/18（瞄准镜）", English="SMG0818 MZJ",
            Career=CareerData.ASSAULT.ID },
        new(){ ID="U_SteyrM1912_P16", Chinese="M1912/P.16（衝锋）", English="M1912 P.16 CF" ,
            Career=CareerData.ASSAULT.ID},
        new(){ ID="U_SteyrM1912_P16_Wep_Burst", Chinese="Maschinenpistole M1912/P.16（实验）", English="M1912 P.16 SY",
            Career=CareerData.ASSAULT.ID},
        new(){ ID="U_Mauser1917Trench", Chinese="M1917 战壕卡宾枪", English="M1917 KBQ ZH",
            Career=CareerData.ASSAULT.ID },
        new(){ ID="U_Mauser1917Trench_Wep_Scope", Chinese="M1917 卡宾枪（巡逻）", English="M1917 KBQ XL" , Career=CareerData.ASSAULT.ID},
        new(){ ID="U_ChauchatSMG", Chinese="RSC 衝锋枪（原厂）", English="RSC YC",
            Career=CareerData.ASSAULT.ID },
        new(){ ID="U_ChauchatSMG_Wep_Optical", Chinese="RSC 衝锋枪（瞄准镜）", English="RSC MZJ",
            Career=CareerData.ASSAULT.ID},
        new(){ ID="U_M1919Thompson_Wep_Trench", Chinese="Annihilator（壕沟）", English="Annihilator HG",
            Career=CareerData.ASSAULT.ID },
        new(){ ID="U_M1919Thompson_Wep_Stability", Chinese="Annihilator（衝锋）", English="Annihilator CF",
            Career=CareerData.ASSAULT.ID},
        new(){ ID="U_FrommerStopAuto", Chinese="费罗梅尔停止手枪（自动）", English="FrommerStopAuto",
            Career=CareerData.ASSAULT.ID },
        new(){ ID="U_SawnOffShotgun", Chinese="短管霰弹枪", English="SawnOffShotgun",
            Career=CareerData.ASSAULT.ID},

        // 配枪
        new(){ ID="======== 突击兵 配枪 ========", Chinese="", English="" },
        ////
        new(){ ID="U_GasserM1870", Chinese="加塞 M1870", English="M1870" },
        new(){ ID="U_LancasterHowdah", Chinese="Howdah 手枪", English="Howdah" },
        new(){ ID="U_Hammerless", Chinese="1903 Hammerless", English="1903" },

        // 配备一二
        new(){ ID="======== 突击兵 配备一二 ========", Chinese="", English="" },
        ////
        new(){ ID="U_Dynamite", Chinese="炸药", English="Dynamite",
            Career=CareerData.ASSAULT.ID },
        new(){ ID="U_ATGrenade", Chinese="反坦克手榴弹", English="ATGrenade",
            Career=CareerData.ASSAULT.ID },
        new(){ ID="U_ATMine", Chinese="反坦克地雷", English="ATMine",
            Career=CareerData.ASSAULT.ID },
        new(){ ID="U_BreechGun", Chinese="反坦克火箭砲", English="AT",
            Career=CareerData.ASSAULT.ID },
        new(){ ID="U_BreechGun_Flak", Chinese="防空火箭砲", English="AAT",
            Career=CareerData.ASSAULT.ID },

        ////////////////////////////////// 医疗兵 Medic   //////////////////////////////////

        // 主要武器
        new(){ ID="======== 医疗兵 主要武器 ========", Chinese="", English="" },
        ////
        new(){ ID="U_CeiRigottiM1895_Wep_Trench", Chinese="Cei-Rigotti（壕沟战）", English="M1895 HGZ",
            Career=CareerData.MEDIC.ID },
        new(){ ID="U_CeiRigottiM1895_Wep_Range", Chinese="Cei-Rigotti（瞄准镜）", English="M1895 MZJ",
            Career=CareerData.MEDIC.ID },
        new(){ ID="U_CeiRigottiM1895", Chinese="Cei-Rigotti（原厂）", English="M1895 YC",
            Career=CareerData.MEDIC.ID },
        new(){ ID="U_MauserSL1916_Wep_Scope", Chinese="Selbstlader M1916（神射手）", English="M1916 SSS",
            Career=CareerData.MEDIC.ID },
        new(){ ID="U_MauserSL1916_Wep_Range", Chinese="Selbstlader M1916（瞄准镜）", English="M1916 MZJ",
            Career=CareerData.MEDIC.ID },
        new(){ ID="U_MauserSL1916", Chinese="Selbstlader M1916（原厂）", English="M1916 YC",
            Career=CareerData.MEDIC.ID },
        new(){ ID="U_WinchesterM1907_Wep_Trench", Chinese="M1907 半自动步枪（壕沟战）", English="M1907 JGZ",
            Career=CareerData.MEDIC.ID },
        new(){ ID="U_WinchesterM1907_Wep_Auto", Chinese="M1907 半自动步枪（扫荡）", English="M1907 SD",
            Career=CareerData.MEDIC.ID },
        new(){ ID="U_WinchesterM1907", Chinese="M1907 半自动步枪（原厂）", English="M1907 YC",
            Career=CareerData.MEDIC.ID },
        new(){ ID="U_Mondragon_Wep_Range", Chinese="蒙德拉贡步枪（瞄准镜）", English="Mondragon MZJ",
            Career=CareerData.MEDIC.ID },
        new(){ ID="U_Mondragon_Wep_Stability", Chinese="蒙德拉贡步枪（衝锋）", English="Mondragon CF",
            Career=CareerData.MEDIC.ID },
        new(){ ID="U_Mondragon_Wep_Bipod", Chinese="蒙德拉贡步枪（狙击手）", English="Mondragon JJS",
            Career=CareerData.MEDIC.ID },
        new(){ ID="U_RemingtonModel8", Chinese="自动装填步枪 8.35（原厂）", English="8.35 YC",
            Career=CareerData.MEDIC.ID },
        new(){ ID="U_RemingtonModel8_Wep_Scope", Chinese="自动装填步枪 8.35（神射手）", English="8.35 SSS",
            Career=CareerData.MEDIC.ID },
        new(){ ID="U_RemingtonModel8_Wep_ExtendedMag", Chinese="自动装填步枪 8.25（加长）", English="8.25 JC",
            Career=CareerData.MEDIC.ID },
        new(){ ID="U_Luger1906", Chinese="Selbstlader 1906（原厂）", English="1906 YC",
            Career=CareerData.MEDIC.ID },
        new(){ ID="U_Luger1906_Wep_Scope", Chinese="Selbstlader 1906（狙击手）", English="1906 JJS",
            Career=CareerData.MEDIC.ID },
        new(){ ID="U_RSC1917_Wep_Range", Chinese="RSC 1917（瞄准镜）", English="RSC 1917 MZJ",
            Career=CareerData.MEDIC.ID },
        new(){ ID="U_RSC1917", Chinese="RSC 1917（原厂）", English="RSC 1917 YC",
            Career=CareerData.MEDIC.ID },
        new(){ ID="U_FedorovAvtomat_Wep_Trench", Chinese="费德洛夫自动步枪（壕沟战）", English="Fedorov HGZ",
            Career=CareerData.MEDIC.ID },
        new(){ ID="U_FedorovAvtomat_Wep_Range", Chinese="费德洛夫自动步枪（瞄准镜）", English="Fedorov MZJ",
            Career=CareerData.MEDIC.ID },
        new(){ ID="U_GeneralLiuRifle", Chinese="刘将军步枪（原厂）", English="GeneralLiu YC",
            Career=CareerData.MEDIC.ID },
        new(){ ID="U_GeneralLiuRifle_Wep_Stability", Chinese="刘将军步枪（衝锋）", English="GeneralLiu CF",
            Career=CareerData.MEDIC.ID },
        new(){ ID="U_FarquharHill_Wep_Range", Chinese="Farquhar-Hill 步枪（瞄准镜）", English="Farquhar MZJ",
            Career=CareerData.MEDIC.ID },
        new(){ ID="U_FarquharHill_Wep_Stability", Chinese="Farquhar-Hill 步枪（衝锋）", English="Farquhar CF",
            Career=CareerData.MEDIC.ID },
        new(){ ID="U_BSAHowellM1916", Chinese="Howell 自动步枪（原厂）", English="Howell YC",
            Career=CareerData.MEDIC.ID },
        new(){ ID="U_BSAHowellM1916_Wep_Scope", Chinese="Howell 自动步枪（狙击手）", English="Howell JJS",
            Career=CareerData.MEDIC.ID },
        new(){ ID="U_FedorovDegtyarev", Chinese="费德洛夫 Degtyarev", English="Fedorov SL",
            Career=CareerData.MEDIC.ID },

        // 配枪
        new(){ ID="======== 医疗兵 配枪 ========", Chinese="", English="" },
        ////
        new(){ ID="U_WebFosAutoRev_455Webley", Chinese="自动左轮手枪", English="Auto Rev" },
        new(){ ID="U_MauserC96", Chinese="C96", English="C96" },
        new(){ ID="U_Mauser1914", Chinese="Taschenpistole M1914", English="M1914" },

        // 配备一二
        new(){ ID="======== 医疗兵 配备一二 ========", Chinese="", English="" },
        ////
        new(){ ID="U_Syringe", Chinese="医疗用针筒", English="Syringe",
            Career=CareerData.MEDIC.ID },
        new(){ ID="U_MedicBag", Chinese="医护箱", English="MedicBag",
            Career=CareerData.MEDIC.ID },
        new(){ ID="U_Bandages", Chinese="绷带包", English="Bandages",
            Career=CareerData.MEDIC.ID },
        new(){ ID="_RGL_Frag", Chinese="步枪手榴弹（破片）", English="RGL Frag",
            Career=CareerData.MEDIC.ID },
        new(){ ID="_RGL_Smoke", Chinese="步枪手榴弹（烟雾）", English="RGL Smoke",
            Career=CareerData.MEDIC.ID },
        new(){ ID="_RGL_HE", Chinese="步枪手榴弹（高爆）", English="RGL HE",
            Career=CareerData.MEDIC.ID },

        ////////////////////////////////// 支援兵 Support //////////////////////////////////

        // 主要武器
        new(){ ID="======== 支援兵 主要武器 ========", Chinese="", English="" },
        ////
        new(){ ID="U_LewisMG_Wep_Suppression", Chinese="路易士机枪（压制）", English="LewisMG YZ",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_LewisMG_Wep_Range", Chinese="路易士机枪（瞄准镜）", English="LewisMG MZJ",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_LewisMG", Chinese="路易士机枪（轻量化）", English="LewisMG QLH",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_HotchkissM1909_Wep_Stability", Chinese="M1909 贝内特·梅西耶机枪（衝锋）", English="M1909 CF",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_HotchkissM1909_Wep_Range", Chinese="M1909 贝内特·梅西耶机枪（瞄准镜）", English="M1909 MZJ",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_HotchkissM1909_Wep_Bipod", Chinese="M1909 贝内特·梅西耶机枪（望远瞄具）", English="M1909 WYMJ",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_MadsenMG_Wep_Trench", Chinese="麦德森机枪（壕沟战）", English="MadsenMG HGZ",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_MadsenMG_Wep_Stability", Chinese="麦德森机枪（衝锋）", English="MadsenMG CF",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_MadsenMG", Chinese="麦德森机枪（轻量化）", English="MadsenMG QLH",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_Bergmann1915MG_Wep_Suppression", Chinese="MG15 n.A.（压制）", English="MG15 YZ",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_Bergmann1915MG_Wep_Stability", Chinese="MG15 n.A.（衝锋）", English="MG15 CF",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_Bergmann1915MG", Chinese="MG15 n.A.（轻量化）", English="MG15 QLH",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_BARM1918_Wep_Trench", Chinese="M1918 白朗宁自动步枪（壕沟战）", English="M1918 HGZ",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_BARM1918_Wep_Stability", Chinese="M1918 白朗宁自动步枪（衝锋）", English="M1918 CF",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_BARM1918_Wep_Bipod", Chinese="M1918 白朗宁自动步枪（望远瞄具）", English="M1918 WYMJ",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_HuotAutoRifle", Chinese="Huot 自动步枪（轻量化）", English="Huot QLH",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_HuotAutoRifle_Wep_Range", Chinese="Huot 自动步枪（瞄准镜）", English="Huot HGZ",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_Chauchat", Chinese="绍沙轻机枪（轻量化）", English="Chauchat QLH",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_Chauchat_Wep_Bipod", Chinese="绍沙轻机枪（望远瞄具）", English="Chauchat WYMJ",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_ParabellumLMG", Chinese="Parabellum MG14/17（轻量化）", English="MG1417 QLH",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_ParabellumLMG_Wep_Suppression", Chinese="Parabellum MG14/17（压制）", English="MG1417 YZ",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_PerinoM1908", Chinese="Perino Model 1908（轻量化）", English="M1908 QLH",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_PerinoM1908_Wep_Defensive", Chinese="Perino Model 1908（防御）", English="M1908 FY",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_BrowningM1917", Chinese="M1917 机枪（轻量化）", English="M1917 QLH",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_BrowningM1917_Wep_Suppression", Chinese="M1917 机枪（望远瞄具）", English="M1917 WYMJ",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_MG0818", Chinese="轻机枪 08/18（轻量化）", English="MG0818 QLH",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_MG0818_Wep_Defensive", Chinese="轻机枪 08/18（压制）", English="MG0818 YZ",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_WinchesterBurton_Wep_Trench", Chinese="波顿 LMR（战壕）", English="Burton LMR ZH",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_WinchesterBurton_Wep_Optical", Chinese="波顿 LMR（瞄准镜）", English="Burton LMR HZJ",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_MauserC96AutoPistol", Chinese="C96（卡宾枪）", English="C96 KBQ",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_LugerArtillery", Chinese="P08 Artillerie", English="P08 Artillerie",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_PieperCarbine", Chinese="皮珀 M1893", English="M1893",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_M1911_Stock", Chinese="M1911（加长）", English="M1911 JC",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_FN1903stock", Chinese="Mle 1903（加长）", English="Mle 1903 JC",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_C93Carbine", Chinese="C93（卡宾枪）", English="C93 KBQ",
            Career=CareerData.SUPPORT.ID },

        // 配枪
        new(){ ID="======== 支援兵 配枪 ========", Chinese="", English="" },
        ////
        new(){ ID="U_SteyrM1912", Chinese="Repetierpistole M1912", English="M1912" },
        new(){ ID="U_Bulldog", Chinese="斗牛犬左轮手枪", English="Bulldog" },
        new(){ ID="U_BerettaM1915", Chinese="Modello 1915", English="Modello 1915" },

        // 配备一二
        new(){ ID="======== 支援兵 配备一二 ========", Chinese="", English="" },
        ////
        new(){ ID="U_AmmoCrate", Chinese="弹药箱", English="Ammo Crate",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_AmmoPouch", Chinese="弹药包", English="Ammo Pouch",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_Mortar", Chinese="迫击砲（空爆）", English="Mortar KB",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_Mortar_HE", Chinese="迫击砲（高爆）", English="Mortar GB",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_Wrench", Chinese="维修工具", English="Wrench",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_LimpetMine", Chinese="磁吸地雷", English="Limpet Mine",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_Crossbow", Chinese="十字弓发射器（破片）", English="Crossbow PP",
            Career=CareerData.SUPPORT.ID },
        new(){ ID="U_Crossbow_HE", Chinese="十字弓发射器（高爆）", English="Crossbow GB",
            Career=CareerData.SUPPORT.ID },

        ////////////////////////////////// 侦察兵 Scout   //////////////////////////////////

        // 主要武器
        new(){ ID="======== 侦察兵 主要武器 ========", Chinese="", English="" },
        ////
        new(){ ID="U_WinchesterM1895_Wep_Trench", Chinese="Russian 1895（壕沟战）", English="1895 HGZ",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_WinchesterM1895_Wep_Long", Chinese="Russian 1895（狙击手）", English="1895 JJS",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_WinchesterM1895", Chinese="Russian 1895（步兵）", English="1895 BB",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_Gewehr98_Wep_Scope", Chinese="Gewehr 98（神射手）", English="G98 SSS",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_Gewehr98_Wep_LongRange", Chinese="Gewehr 98（狙击手）", English="G98 JJS",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_Gewehr98", Chinese="Gewehr 98（步兵）", English="G98 BB",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_LeeEnfieldSMLE_Wep_Scope", Chinese="SMLE MKIII（神射手）", English="MKIII SSS",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_LeeEnfieldSMLE_Wep_Med", Chinese="SMLE MKIII（卡宾枪）", English="MKIII KBQ",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_LeeEnfieldSMLE", Chinese="SMLE MKIII（步兵）", English="MKIII BB",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_SteyrManM1895_Wep_Scope", Chinese="Gewehr M.95（神射手）", English="G95 SSS",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_SteyrManM1895_Wep_Med", Chinese="Gewehr M.95（卡宾枪）", English="G95 KBQ",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_SteyrManM1895", Chinese="Gewehr M.95（步兵）", English="G95 BB",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_SpringfieldM1903_Wep_Scope", Chinese="M1903（神射手）", English="M1903 SSS",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_SpringfieldM1903_Wep_LongRange", Chinese="M1903（狙击手）", English="M1903 JJS",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_SpringfieldM1903_Wep_Pedersen", Chinese="M1903（实验）", English="M1903 SY",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_MartiniHenry", Chinese="马提尼·亨利步枪（步兵）", English="MartiniHenry BB",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_MartiniHenry_Wep_LongRange", Chinese="马提尼·亨利步枪（狙击手）", English="MartiniHenry JJS",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_LeeEnfieldSMLE_Wep_Preorder", Chinese="阿拉伯的劳伦斯的 SMLE", English="SMLE LLS",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_Lebel1886_Wep_LongRange", Chinese="勒贝尔 M1886（狙击手）", English="M1886 JJS",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_Lebel1886", Chinese="勒贝尔 M1886（步兵）", English="M1886 BB",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_MosinNagant1891", Chinese="莫辛-纳甘 M91（步兵）", English="M91 BB",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_MosinNagant1891_Wep_Scope", Chinese="莫辛-纳甘 M91（神射手）", English="M91 SSS",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_VetterliVitaliM1870", Chinese="Vetterli-Vitali M1870/87（步兵）", English="M1870 BB",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_VetterliVitaliM1870_Wep_Med", Chinese="Vetterli-Vitali M1870/87（卡宾枪）", English="M1870 KBQ",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_Type38Arisaka", Chinese="三八式步枪（步兵）", English="Type38 BB",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_Type38Arisaka_Wep_Scope", Chinese="三八式步枪（巡逻）", English="Type38 XL",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_CarcanoCarbine", Chinese="卡尔卡诺 M91 卡宾枪", English="M91 KBQ",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_CarcanoCarbine_Wep_Scope", Chinese="卡尔卡诺 M91 卡宾枪（巡逻）", English="M91 KBQ XL",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_RossMkIII", Chinese="罗斯 MKIII（步兵）", English="RossMkIII BB",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_RossMkIII_Wep_Scope", Chinese="罗斯 MKIII（神射手）", English="RossMkIII SSS",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_Enfield1917", Chinese="M1917 Enfield（步兵）", English="M1917 BB",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_Enfield1917_Wep_LongRange", Chinese="M1917 Enfield（消音器）", English="M1917 XYQ",
            Career=CareerData.SCOUT.ID },

        // 配枪
        new(){ ID="======== 侦察兵 配枪 ========", Chinese="", English="" },
        ////
        new(){ ID="U_MarsAutoPistol", Chinese="Mars 自动手枪", English="MarsAutoPistol" },
        new(){ ID="U_Bodeo1889", Chinese="Bodeo 1889", English="Bodeo 1889" },
        new(){ ID="U_FrommerStop", Chinese="费罗梅尔停止手枪", English="Frommer Stop" },

        // 配备一二
        new(){ ID="======== 侦察兵 配备一二 ========", Chinese="", English="" },
        ////
        new(){ ID="U_FlareGun", Chinese="信号枪（侦察）", English="Flare Gun ZC",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_FlareGun_Flash", Chinese="信号枪（闪光）", English="Flare Gun SG",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_TrPeriscope", Chinese="战壕潜望镜", English="Tr Periscope",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_Shield", Chinese="狙击手护盾", English="Shield",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_HelmetDecoy", Chinese="狙击手诱饵", English="Helmet Decoy",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_TripWireBomb", Chinese="绊索炸弹（高爆）", English="Trip Wire Bomb",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_TripWireGas", Chinese="绊索炸弹（毒气）", English="Trip Wire Gas",
            Career=CareerData.SCOUT.ID },
        new(){ ID="U_TripWireBurn", Chinese="绊索炸弹（燃烧）", English="Trip Wire Burn",
            Career=CareerData.SCOUT.ID },
        new(){ ID="_KBullet", Chinese="K 弹", English="K Bullet",
            Career=CareerData.SCOUT.ID },

        /////////////////////////////////////////////////////////////////////////////

        // 精英兵
        new(){ ID="======== 精英兵 ========", Chinese="", English="" },
        ////
        new(){ ID="U_MaximMG0815", Chinese="哨兵 MG 08/15", English="Maxim MG0815",
            Career=CareerData.SENTRY.ID },

        new(){ ID="U_VillarPerosa", Chinese="哨兵 维拉·佩罗萨衝锋枪", English="Villar Perosa",
            Career=CareerData.SENTRY.ID },

        new(){ ID="U_FlameThrower", Chinese="喷火兵 Wex", English="Wex",
            Career=CareerData.FLAMETHROWER.ID },
        new(){ ID="U_Incendiary_Hero", Chinese="燃烧手榴弹", English="Incendiary Hero",
            Career=CareerData.FLAMETHROWER.ID },

        new(){ ID="U_RoyalClub", Chinese="战壕奇兵 奇兵棒", English="Royal Club",
            Career=CareerData.RAIDER.ID },

        new(){ ID="U_MartiniGrenadeLauncher", Chinese="入侵者 马提尼·亨利步枪榴弹发射器", English="Martini GL",
            Career=CareerData.RUNNER.ID },
        new(){ ID="U_SawnOffShotgun_FK", Chinese="短管霰弹枪", English="SawnOffShotgun",
            Career=CareerData.RUNNER.ID },
        new(){ ID="U_FlareGun_Elite", Chinese="信号枪 — 信号", English="FlareGun Elite",
            Career=CareerData.RUNNER.ID },
        new(){ ID="U_SpawnBeacon", Chinese="重生信标", English="Spawn Beacon",
            Career=CareerData.RUNNER.ID },

        new(){ ID="U_TankGewehr", Chinese="坦克猎手 Tankgewehr M1918", English="Tank Gewehr",
            Career=CareerData.ANTITANK.ID },
        new(){ ID="U_TrPeriscope_Elite", Chinese="战壕潜望镜", English="Tr Periscope",
            Career=CareerData.ANTITANK.ID },
        new(){ ID="U_ATGrenade_VhKit", Chinese="反坦克手榴弹", English="AT Grenade",
            Career=CareerData.ANTITANK.ID },

        ///////////////////////////////////////////////////////////////////////////////////

        // 载具
        new(){ ID="======== 坦克 ========", Chinese="", English="" },
        ////
        new(){ ID="ID_P_VNAME_MARKV", Chinese="载具 Mark V 巡航坦克", English="Mark V" },

        new(){ ID="ID_P_VNAME_A7V", Chinese="重型坦克 AV7 重型坦克", English="AV7" },

        new(){ ID="ID_P_VNAME_FT17", Chinese="轻型坦克 FT-17 轻型坦克", English="FT-17" },

        new(){ ID="ID_P_VNAME_ARTILLERYTRUCK", Chinese="载具 火炮装甲车", English="ARTILLERYTRUCK" },

        new(){ ID="ID_P_VNAME_STCHAMOND", Chinese="攻击坦克 圣沙蒙", English="STCHAMOND" },

        new(){ ID="ID_P_VNAME_ASSAULTTRUCK", Chinese="突袭装甲车 朴帝洛夫·加福德", English="ASSAULTTRUCK" },

        ////////////////
        
        new(){ ID="======== 飞机 ========", Chinese="", English="" },
        ////
        new(){ ID="ID_P_VNAME_HALBERSTADT", Chinese="攻击机 哈尔伯施塔特 CL.II 攻击机", English="HALBERSTADT" },
        new(){ ID="ID_P_VNAME_RUMPLER", Chinese="攻击机 Rumpler C.I 攻击机", English="RUMPLER" },
        new(){ ID="ID_P_VNAME_BRISTOL", Chinese="攻击机 布里斯托 F2.B 攻击机", English="BRISTOL" },
        new(){ ID="ID_P_VNAME_SALMSON", Chinese="攻击机 A.E.F 2-A2 攻击机", English="SALMSON" },

        new(){ ID="ID_P_VNAME_DH10", Chinese="轰炸机 Airco DH.10 轰炸机", English="DH10" },
        new(){ ID="ID_P_VNAME_HBG1", Chinese="轰炸机 汉莎·布兰登堡 G.I 轰炸机", English="HBG1" },
        new(){ ID="ID_P_VNAME_CAPRONI", Chinese="轰炸机 卡普罗尼 CA.5 轰炸机", English="CAPRONI" },
        new(){ ID="ID_P_VNAME_GOTHA", Chinese="轰炸机 戈塔 G 轰炸机", English="GOTHA" },

        new(){ ID="ID_P_VNAME_SOPWITH", Chinese="战斗机 索普维斯骆驼式战斗机", English="SOPWITH" },
        new(){ ID="ID_P_VNAME_ALBATROS", Chinese="战斗机 信天翁 D-III 战斗机", English="ALBATROS" },
        new(){ ID="ID_P_VNAME_DR1", Chinese="战斗机 DR.1 战斗机", English="DR1" },
        new(){ ID="ID_P_VNAME_SPAD", Chinese="战斗机 SPAD S XIII 战斗机", English="SPAD S XIII" },

        new(){ ID="ID_P_VNAME_ILYAMUROMETS", Chinese="重型轰炸机 伊利亚·穆罗梅茨", English="ILYAMUROMETS" },

        new(){ ID="ID_P_VNAME_ASTRATORRES", Chinese="飞船 C 级飞船", English="ASTRATORRES" },

        ////////////////

        new(){ ID="======== 船 ========", Chinese="", English="" },
        ////
        new(){ ID="ID_P_VNAME_HMS_LANCE", Chinese="船 L 级驱逐舰", English="Mark V" },

        ////////////////
        
        new(){ ID="======== 骑兵 ========", Chinese="", English="" },
        ////
        new(){ ID="ID_P_VNAME_HORSE", Chinese="骑兵 战马", English="HORSE" },

        ////////////////
        
        new(){ ID="======== 驾驶员下车 ========", Chinese="", English="" },
        ////
        new(){ ID="U_WinchesterM1895_Horse", Chinese="Russian 1895（骑兵）", English="M1895 Horse",
            Career=CareerData.CAVALRY.ID },
        new(){ ID="U_AmmoPouch_Cav", Chinese="弹药包", English="Ammo Pouch",
            Career=CareerData.CAVALRY.ID },
        new(){ ID="U_Bandages_Cav", Chinese="绷带包", English="Bandages",
            Career=CareerData.CAVALRY.ID },
        new(){ ID="U_Grenade_AT_Cavalry", Chinese="轻型反坦克手榴弹", English="Grenade AT",
            Career=CareerData.CAVALRY.ID },

        new(){ ID="U_LugerP08_VhKit", Chinese="P08 手枪", English="LugerP08 VhKit" },

        ////////////////
        
        new(){ ID="======== 特殊载具 ========", Chinese="", English="" },
        ////
        new(){ ID="ID_P_INAME_U_MORTAR", Chinese="特殊载具 空爆迫击炮", English="MORTAR" },
        new(){ ID="ID_P_INAME_MORTAR_HE", Chinese="特殊载具 高爆迫击炮", English="MORTAR HE" },

        /////////////////////////////////////////////////////////////////////////////

        // 运输载具
        new(){ ID="======== 运输载具 ========", Chinese="", English="" },
        ////
        new(){ ID="ID_P_VNAME_NSU", Chinese="运输载具 MC 3.5HP 附边车摩托车", English="NSU" },
        new(){ ID="ID_P_VNAME_MOTORCYCLE", Chinese="运输载具 MC 18J 附边车摩托车", English="MOTORCYCLE" },
        new(){ ID="ID_P_VNAME_EHRHARDT", Chinese="运输载具 EV4 装甲车", English="EHRHARDT" },
        new(){ ID="ID_P_VNAME_ROMFELL", Chinese="运输载具 Romfell 装甲车", English="ROMFELL" },
        new(){ ID="ID_P_VNAME_ROLLS", Chinese="运输载具 RNAS 装甲车", English="ROLLS" },
        new(){ ID="ID_P_VNAME_MODEL30", Chinese="运输载具 M30 侦察车", English="MODEL30" },
        new(){ ID="ID_P_VNAME_TERNI", Chinese="运输载具 F.T. 侦察车", English="TERNI" },
        new(){ ID="ID_P_VNAME_MERCEDES_37", Chinese="运输载具 37/95 侦察车", English="MERCEDES 37" },
        new(){ ID="ID_P_VNAME_BENZ_MG", Chinese="运输载具 KFT 侦察车", English="BENZ MG" },

        new(){ ID="ID_P_VNAME_MAS15", Chinese="运输载具 M.A.S 鱼雷快艇", English="MAS15" },
        new(){ ID="ID_P_VNAME_YLIGHTER", Chinese="运输载具 Y-Lighter 登陆艇", English="MAS15" },

        /////////////////////////////////////////////////////////////////////////////

        // 定点武器
        new(){ ID="======== 定点武器 ========", Chinese="", English="" },
        ////
        new(){ ID="ID_P_VNAME_BL9", Chinese="定点武器 BL 9.2 攻城炮", English="BL9" },
        new(){ ID="ID_P_VNAME_TURRET", Chinese="定点武器 堡垒火炮", English="TURRET" },
        new(){ ID="ID_P_VNAME_AASTATION", Chinese="定点武器 QF 1 防空炮", English="AASTATION" },
        new(){ ID="ID_P_VNAME_FIELDGUN", Chinese="定点武器 FK 96 野战炮", English="FIELDGUN" },
        new(){ ID="ID_P_INAME_MAXIM", Chinese="定点武器 重机枪", English="MAXIM" },
        new(){ ID="ID_P_VNAME_COASTALBATTERY", Chinese="定点武器 350/52 o 岸防炮", English="COASTALBATTERY" },
        new(){ ID="ID_P_VNAME_SK45GUN", Chinese="定点武器 SK45 岸防炮", English="SK45GUN" },

        /////////////////////////////////////////////////////////////////////////////

        // 战争巨兽
        new(){ ID="======== 战争巨兽 ========", Chinese="", English="" },
        ////
        new(){ ID="ID_P_VNAME_ARMOREDTRAIN", Chinese="战争巨兽 装甲列车", English="ARMOREDTRAIN" },
        new(){ ID="ID_P_VNAME_ZEPPELIN", Chinese="战争巨兽 飞船 l30", English="ZEPPELIN" },
        new(){ ID="ID_P_VNAME_IRONDUKE", Chinese="战争巨兽 无畏舰", English="IRONDUKE" },
        new(){ ID="ID_P_VNAME_CHAR", Chinese="战争巨兽 Char 2C", English="CHAR" },

        /////////////////////////////////////////////////////////////////////////////
        
        // 近战
        new(){ ID="======== 近战武器 ========", Chinese="", English="" },
        ////
        new(){ ID="U_GrenadeClub", Chinese="哑弹棒", English="Grenade Club" },
        new(){ ID="U_Club", Chinese="棍棒", English="Club" },

        // 其他
        new(){ ID="======== 其他 ========", Chinese="", English="" },
        ////
        new(){ ID="U_GasMask", Chinese="防毒面具", English="Gas Mask" },
    };
}
