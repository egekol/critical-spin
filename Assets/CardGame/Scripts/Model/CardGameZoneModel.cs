using System.Collections.Generic;

namespace CardGame.Model
{
    public class CardGameZoneModel
    {
        public ZoneType ZoneType { get; set; }
        public List<CardGameSlotModel> RewardModelList { get; set; } = new();
        public int ZoneIndex { get; set; }
    }

    public enum ZoneType
    {
        None = 0,
        NormalZone = 1,
        SafeZone = 2,
        SuperZone = 3,
    }
}