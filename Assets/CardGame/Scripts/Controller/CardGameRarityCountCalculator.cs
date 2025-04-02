using System;
using System.Linq;
using CardGame.Model;
using CardGame.Model.Spin;
using Main.Scripts.Utilities;

namespace CardGame.Controller
{
    public interface ICardGameRarityCountCalculator
    {
        RarityCountCalculatorData CalculateRarityCountsByLevel(int levelIndex, int slotCount);
        void Initialize(ZoneRarityCountModel zoneRarityCountModel);
    }

    public struct RarityCountCalculatorData
    {
        public CardGameRewardRarity[] RarityArray;

        public RarityCountCalculatorData(CardGameRewardRarity[] rarityArray)
        {
            RarityArray = rarityArray;
        }
    }

    public class CardGameRarityCountCalculator : ICardGameRarityCountCalculator
    {
        private ZoneRarityCountModel _rarityCountModel;

        public void Initialize(ZoneRarityCountModel zoneRarityCountModel)
        {
            _rarityCountModel = zoneRarityCountModel;
        }

        public RarityCountCalculatorData CalculateRarityCountsByLevel(int levelIndex, int slotCount)
        {
            var data = new RarityCountCalculatorData(new CardGameRewardRarity[slotCount]);
            var countLeft = slotCount;
            var countIndex = 0;
            for (var i = _rarityCountModel.ZoneRarityCountDict.Values.Count() - 1; i >= 0; i--)
            {
                var rarity = _rarityCountModel.ZoneRarityCountDict.Values.ElementAt(i);
                var rarityCount = 0;

                if (rarity.MinAvailableLevel <= levelIndex && levelIndex < rarity.MaxAvailableLevel)
                {
                    var count = MathHelper.Map(levelIndex, rarity.MinAvailableLevel, rarity.MaxAvailableLevel,
                        rarity.MinAvailableCount, rarity.MaxAvailableCount);
                    var ceilingCount = (int)Math.Ceiling(count);
                    rarityCount = Math.Min(ceilingCount, countLeft);
                    countLeft -= rarityCount;
                }

                for (var j = 0; j < rarityCount; j++)
                {
                    data.RarityArray[countIndex] = rarity.Rarity;
                    countIndex++;
                }

                if (countLeft <= 0) return data;
            }

            if (data.RarityArray.Length == 0)
                for (var i = 0; i < countLeft; i++)
                    data.RarityArray[i] = CardGameRewardRarity.Common;

            for (var i = 0; i < countLeft; i++)
            {
                var randomIndex = MathHelper.GetRandomIndex(data.RarityArray);
                var rarity = data.RarityArray[randomIndex];
                data.RarityArray[randomIndex] = rarity;
                countIndex++;
            }

            return data;
        }
    }
}