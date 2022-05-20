namespace BF1.ServerAdminTools.Common.Data;

public class TempData
{
    public struct ClientPlayer
    {
        public long BaseAddress;

        public byte Mark;
        public int TeamID;
        public byte Spectator;
        public string Name;
        public long PersonaId;
        public int PartyId;

        public string[] WeaponSlot;
        public string Career; //0x0038
    }

    public struct ClientSoldierEntity
    {
        public long pClientVehicleEntity;
        public long pVehicleEntityData;

        public long pClientSoldierEntity;
        public long pClientSoldierWeaponComponent;
        public long m_handler;
        public long pClientSoldierWeapon;
        public long pWeaponEntityData;
    }

    public struct ClientPlayerScore
    {
        public long BaseAddress;

        public long Offset;
        public long Offset0;

        public byte Mark;
        public int Rank;
        public int Kill;
        public int Dead;
        public int Score;
    }

    public struct ClientSoldierWeapon 
    {
        public long m_pSoldierWeaponData;
        public long m_pSoldierWeaponUnlockAsset;
    }

    public struct SoldierWeaponUnlockAsset
    {
        public string weaponKit; //0x0010
        public string waeponName; //0x0020
        public string career; //0x0038
    }
}
