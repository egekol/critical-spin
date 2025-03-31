using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardGame.Scripts.Network
{
    public interface ICardGameLevelDto
    {
        IReadOnlyDictionary<RewardRarity, List<CardGameRewardDto>> GetLevelConfigDictionary();
        IReadOnlyList<ZoneRarityCountConfig> GetZoneRarityCountConfigList();
        CardGameLevelConfig GetLevelConfig();
    }

    [CreateAssetMenu(fileName = "CardGameLevelDataTransferSo", menuName = "SO/CardGameLevelDataTransferSo", order = 0)]
    public class CardGameLevelDataTransferSo : SerializedScriptableObject, ICardGameLevelDto
    {
        public List<CardGameLevelConfigSo> LevelConfigList;

        public List<ZoneRarityCountConfig> ZoneRarityCountConfigList;
        public CardGameLevelConfig CardGameLevelConfig;

        public IReadOnlyDictionary<RewardRarity,List<CardGameRewardDto>> GetLevelConfigDictionary()
        {
            var configDict = new Dictionary<RewardRarity, List<CardGameRewardDto>>();
            foreach (var levelConfigSo in LevelConfigList)
            {
                if (configDict.ContainsKey(levelConfigSo.rewardRarity))
                {
                    configDict[levelConfigSo.rewardRarity].AddRange(levelConfigSo.RewardList);
                }
                else
                {
                    configDict.Add(levelConfigSo.rewardRarity, levelConfigSo.RewardList);
                }
            }
            
            return configDict;
            
        }

        public CardGameLevelConfig GetLevelConfig()
        {
            return CardGameLevelConfig;
        }
        
        public IReadOnlyList<ZoneRarityCountConfig> GetZoneRarityCountConfigList()
        {
            return ZoneRarityCountConfigList;
        }
    }

    [Serializable]
    public class CardGameLevelConfig
    {
        public int SafeZoneCoefficient;
        public int SuperZoneCoefficient;
        public int TotalSlotCount;
    }

    [Serializable]
    public class ZoneRarityCountConfig
    {
        public RewardRarity Rarity;
        public int MinAvailableLevel;
        public int MinAvailableCount;
        public int MaxAvailableLevel;
        public int MaxAvailableCount;
    }
}