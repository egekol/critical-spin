using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Model;
using Zenject;

namespace CardGame.Controller
{
    public interface ICardGameLevelGenerator
    {
        void InitializeFirstZone();
    }
    
    

    public class CardGameLevelGenerator : ICardGameLevelGenerator
    {
        [Inject] private readonly CardGameModel _cardGameModel;
        [Inject] private readonly ICardGameRarityCountCalculator _cardGameRarityCountCalculator;
        private static Random _random = new Random();

        public void InitializeFirstZone()
        {
            _cardGameModel.ZoneModelList.Clear();
            _cardGameRarityCountCalculator.Initialize(_cardGameModel.ZoneRarityCountModel);
            var zone = CreateRandomZoneModel(0);
            _cardGameModel.SetCurrentZoneModel(zone);
        }

        private CardGameZoneModel CreateRandomZoneModel(int levelIndex)
        {
            var zoneType = GetZoneType(levelIndex);
            var slotCount = _cardGameModel.TotalSlotCount;
            var slotIndex = 0;
            var zoneModel = new CardGameZoneModel(zoneType, levelIndex, slotCount);
            if (zoneType == ZoneType.NormalZone)
            {
                slotCount--;
                zoneModel.AddSlotModel(new CardGameSlotModel(SlotType.Bomb, slotIndex, null));
                slotIndex++;
            }

            var rarityCountCalculatorData = _cardGameRarityCountCalculator.CalculateRarityCountsByLevel(levelIndex, slotCount);
            for (int i = 0; i < slotCount; i++)
            {
                var rarity = rarityCountCalculatorData.RarityArray[i];
                var reward = CreateRandomRewardModel(rarity);
                zoneModel.AddSlotModel(new CardGameSlotModel(SlotType.Reward, slotIndex, reward));
                slotIndex++;
            }
            
            return zoneModel;
        }

        private CardGameRewardModel CreateRandomRewardModel(CardGameRewardRarity rarity)
        {
            var modelDict = _cardGameModel.ZoneModelDict[rarity].RewardModelDict;
            var model = GetWeightedRandomReward(modelDict);
            return model;
        }
        public static CardGameRewardModel GetWeightedRandomReward(Dictionary<CardGameRewardModel, int> rewards)
        {
            int totalWeight = rewards.Values.Sum();
            int randomValue = _random.Next(totalWeight); 

            int cumulativeWeight = 0;
            foreach (var kvp in rewards)
            {
                cumulativeWeight += kvp.Value;
                if (randomValue < cumulativeWeight)
                    return kvp.Key;
            }

            return rewards.Keys.First(); 
        }

        private ZoneType GetZoneType(int levelIndex)
        {
            var model = _cardGameModel;

            if (levelIndex % model.SuperZoneCoefficient == 0)
            {
                return ZoneType.SuperZone;
            }
            
            if (levelIndex % model.SafeZoneCoefficient == 0)
            {
                return ZoneType.SafeZone;
            }

            return ZoneType.NormalZone;
        }
    }
}