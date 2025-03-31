using System.Collections.Generic;

namespace CardGame.Model
{
    public class CardGameModel
    {
        private Dictionary<CardGameRewardRarity,CardGameZoneConfig> _zoneModelList = new();

        public IReadOnlyDictionary<CardGameRewardRarity, CardGameZoneConfig> ZoneModelDict => _zoneModelList;


        public List<CardGameZoneModel> ZoneModelList { get; set; } = new();
        public CardGameZoneModel CurrentZoneModel { get; private set; } = new();
        public int CurrentZoneIndex { get; set; }

        public void AddZoneConfig(CardGameZoneConfig zoneConfig, CardGameRewardRarity rarity)
        {
            _zoneModelList.Add(rarity, zoneConfig);
        }

        public void ClearZoneConfig()
        {
            _zoneModelList.Clear();
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