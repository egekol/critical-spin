using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardGame.Scripts.Data
{
    [CreateAssetMenu(fileName = "CardGameLevelConfigSo", menuName = "SO/CardGameLevelConfigSo", order = 0)]
    public class CardGameLevelConfigSo : ScriptableObject
    {
        private const string LevelConfigPrefix = "so_config_reward_";
        [Tooltip("To make you sure having correct prefix")]
        [ReadOnly] public string Name;
        public RewardRarity rewardRarity;
        public List<CardGameRewardDto> RewardList;

        private void OnValidate()
        {
            if (!name.StartsWith(LevelConfigPrefix))
            {
                Debug.LogError($"SO name must include the prefix : {LevelConfigPrefix}");
                return;
            }

            Name = name.Substring(LevelConfigPrefix.Length);
        }
    }

    [Serializable]
    public class CardGameRewardDto
    {
        public RewardViewDataTransferSo RewardData;
        //todo this would be any transfer data file instead of So, if there is network implementation

        public ushort Amount;
        [Range(0, 100)] public int RewardProbability;
    }
}