using System.Collections.Generic;

namespace CardGame.Model
{
    public class CardGameModel
    {

        public IReadOnlyList<CardGameZoneModel> ZoneModelList => _zoneModelList;
        private readonly List<CardGameZoneModel> _zoneModelList = new();

        public CardGameZoneModel CurrentZoneModel { get; private set; }

        public int CurrentZoneIndex { get; private set; }

        public void SetCurrentZoneModel(CardGameZoneModel zone)
        {
            CurrentZoneModel = zone;
        }

        public void ClearZoneModelList()
        {
            _zoneModelList.Clear();
        }

        public void AddZoneToList(CardGameZoneModel zone)
        {
            _zoneModelList.Add(zone);
        }
    }


    public enum CardGameRewardRarity
    {
        Common,
        Uncommon,
        Rare,
        Legendary,
        Epic,
    }
}