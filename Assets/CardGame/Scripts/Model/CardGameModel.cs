using System.Collections.Generic;
using CardGame.Scripts.Network;
using Main.Scripts.Utilities;

namespace CardGame.Model
{
    public class CardGameModel
    {
        public IReadOnlyDictionary<CardGameRewardRarity, CardGameZoneConfig> ZoneModelDict => _zoneModelDict;

        public List<CardGameZoneModel> ZoneModelList { get; private set; } = new();

        public ZoneRarityCountModel ZoneRarityCountModel { get; private set; } = new();

        public CardGameZoneModel CurrentZoneModel { get; private set; }

        public int CurrentZoneIndex { get; private set; }

        public int SafeZoneCoefficient { get; private set; }

        public int SuperZoneCoefficient { get; private set; }

        public int TotalSlotCount { get; private set; }


        private Dictionary<CardGameRewardRarity, CardGameZoneConfig> _zoneModelDict = new();

        public void AddZoneConfig(CardGameZoneConfig zoneConfig, CardGameRewardRarity rarity)
        {
            _zoneModelDict.Add(rarity, zoneConfig);
        }

        public void ClearZoneConfig()
        {
            _zoneModelDict.Clear();
        }

        public void SetCurrentZoneModel(CardGameZoneModel zone)
        {
            CurrentZoneModel = zone;
        }

        public void SetLevelConfig(int safeZoneCoefficient, int superZoneCoefficient, int totalSlotCount)
        {
            SafeZoneCoefficient = safeZoneCoefficient;
            SuperZoneCoefficient = superZoneCoefficient;
            TotalSlotCount = totalSlotCount;
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