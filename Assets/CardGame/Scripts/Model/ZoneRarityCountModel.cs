using System.Collections.Generic;
using System.Linq;
using CardGame.Model.Spin;
using Main.Scripts.Utilities;

namespace CardGame.Model
{
    public class ZoneRarityCountModel
    {
        public IReadOnlyDictionary<CardGameRewardRarity, ZoneRarityCountData> ZoneRarityCountDict =>
            _zoneRarityCountDict;

        private SortedDictionary<CardGameRewardRarity, ZoneRarityCountData> _zoneRarityCountDict = new();

        public void ClearZoneRarityCount()
        {
            _zoneRarityCountDict.Clear();
        }

        public void AddZoneRarityCountConfig(CardGameRewardRarity rarity, int minAvailableLevel,
            int minAvailableCount,
            int maxAvailableCount, int maxAvailableLevel)
        {
            var zoneRarityCountData = new ZoneRarityCountData(rarity, minAvailableLevel, minAvailableCount,
                maxAvailableLevel, maxAvailableCount);
            var isAdded = _zoneRarityCountDict.TryAdd(rarity, zoneRarityCountData);

            if (!isAdded)
            {
                DebugLogger.LogError($"Failed to add! {rarity} already exists.");
            }
            else
            {
                DebugLogger.Log(
                    $"[SetZoneRarityCountFromLevelData] Added zoneRarityCount config to zone config: {zoneRarityCountData}");
            }
        }

    }

    public struct ZoneRarityCountData
    {
        public readonly CardGameRewardRarity Rarity;
        public readonly int MinAvailableLevel;
        public readonly int MinAvailableCount;
        public readonly int MaxAvailableLevel;
        public readonly int MaxAvailableCount;

        public ZoneRarityCountData(CardGameRewardRarity rarity, int minAvailableLevel,
            int minAvailableCount,
            int maxAvailableLevel,
            int maxAvailableCount)
        {
            Rarity = rarity;
            MinAvailableLevel = minAvailableLevel;
            MinAvailableCount = minAvailableCount;
            MaxAvailableLevel = maxAvailableLevel;
            MaxAvailableCount = maxAvailableCount;
        }

        public override string ToString()
        {
            return
                $"{Rarity} : Min - {MinAvailableLevel} , {MinAvailableCount} | Max {MaxAvailableLevel} , {MaxAvailableCount}";
        }
    }
}

