using System.Collections.Generic;
using System.Text;

namespace CardGame.Model
{
    public class CardGameZoneConfig
    {
        public CardGameRewardRarity RewardRarity { get; set; }
        public Dictionary<CardGameRewardModel, int> RewardModelDict { get; set; } = new();

        public void SetRarity(CardGameRewardRarity rarity)
        {
            RewardRarity = rarity;
        }

        public void AddRewardModel(CardGameRewardModel rewardModel, int rewardProbability)
        {
            RewardModelDict.Add(rewardModel, rewardProbability);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var i in RewardModelDict)
            {
                sb.AppendLine($"{i.Key} probabilityRate : {i.Value}");
            }

            return $"RewardRarity: {RewardRarity}, RewardModel: {sb} ";
        }
    }
}