using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Scripts.Network
{
    public interface ICardGameLevelDto
    {
        IReadOnlyDictionary<RewardRarity, List<CardGameRewardDto>> GetLevelConfigDictionary();
    }

    [CreateAssetMenu(fileName = "CardGameLevelDataTransferSo", menuName = "SO/CardGameLevelDataTransferSo", order = 0)]
    public class CardGameLevelDataTransferSo : ScriptableObject, ICardGameLevelDto
    {
        public List<CardGameLevelConfigSo> LevelConfigList;

        public IReadOnlyDictionary<RewardRarity,List<CardGameRewardDto>> GetLevelConfigDictionary()
        {
            var configDict = new Dictionary<RewardRarity, List<CardGameRewardDto>>();
            foreach (var levelConfigSo in LevelConfigList)
            {
                configDict.Add(levelConfigSo.rewardRarity,levelConfigSo.RewardList);
            }
            
            return configDict;
        }
    }

}