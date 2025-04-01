using System.Collections.Generic;

namespace CardGame.Model.Spin
{
    public class CardGameZoneModel
    {
        public ZoneType ZoneType { get; private set; }

        public List<CardGameSlotModel> SlotModelList { get; private set; }

        public int ZoneIndex { get; private set; }

        public CardGameZoneModel(ZoneType zoneType, int index, int slotCount)
        {
            ZoneType = zoneType;
            SlotModelList = new List<CardGameSlotModel>(slotCount);
            ZoneIndex = index;
        }


        public void AddSlotModel(CardGameSlotModel cardGameSlotModel)
        {
            SlotModelList.Add(cardGameSlotModel);
        }
    }

    public enum ZoneType
    {
        None = 0,
        NormalZone = 1,
        SafeZone = 2,
        SuperZone = 3,
    }
}