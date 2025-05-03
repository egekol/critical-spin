using System.Collections.Generic;
using CardGame.Model.Spin;
using Main.Scripts.ScriptableSingleton;

namespace CardGame.Model
{
    public class CardGameEventModel : ScriptableSingletonManager<CardGameEventModel>
    {
        private readonly Dictionary<CardGameRewardRarity, CardGameZoneConfig> _zoneModelDict = new();
        public IReadOnlyDictionary<CardGameRewardRarity, CardGameZoneConfig> ZoneModelDict => _zoneModelDict;

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