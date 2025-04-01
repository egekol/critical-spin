using System.Collections.Generic;

namespace CardGame.Model
{
    public class CardGameEventModel
    {
        public IReadOnlyDictionary<CardGameRewardRarity, CardGameZoneConfig> ZoneModelDict => _zoneModelDict;
        private Dictionary<CardGameRewardRarity, CardGameZoneConfig> _zoneModelDict = new();

        public ZoneRarityCountModel ZoneRarityCountModel { get; private set; } = new();
        public int SafeZoneCoefficient { get; private set; }

        public int SuperZoneCoefficient { get; private set; }


        
        public void AddZoneConfig(CardGameZoneConfig zoneConfig, CardGameRewardRarity rarity)
        {
            _zoneModelDict.Add(rarity, zoneConfig);
        }

        public void ClearZoneConfig()
        {
            _zoneModelDict.Clear();
        }
        public void SetLevelConfig(int safeZoneCoefficient, int superZoneCoefficient)
        {
            SafeZoneCoefficient = safeZoneCoefficient;
            SuperZoneCoefficient = superZoneCoefficient;
        }
    }
}