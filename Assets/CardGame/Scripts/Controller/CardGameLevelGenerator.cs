using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Model;
using Main.Scripts.Utilities;
using Zenject;

namespace CardGame.Controller
{
    public interface ICardGameLevelGenerator
    {
        void InitializeZones();
        void SetNextZoneModel();
    }

    public class CardGameLevelGenerator : ICardGameLevelGenerator
    {
        [Inject] private readonly CardGameModel _cardGameModel;
        [Inject] private readonly CardGameEventModel _cardGameEventModel;
        [Inject] private readonly ICardGameRarityCountCalculator _cardGameRarityCountCalculator;
        private static Random _random = new Random();

        public void InitializeZones()
        {
            _cardGameModel.ClearZoneModelList();
            _cardGameRarityCountCalculator.Initialize(_cardGameEventModel.ZoneRarityCountModel);
            var zone = CreateRandomZoneModel(0);
            _cardGameModel.AddZoneToList(zone);
            _cardGameModel.SetCurrentZoneModelFromList();

            CreateNewHundredMoreZonesToList();
        }

        public void SetNextZoneModel()
        {
            _cardGameModel.IncreaseCountIndex();
            if (_cardGameModel.ZoneModelList.Count - 1 >= _cardGameModel.CurrentZoneIndex)
            {
                CreateNewHundredMoreZonesToList();
            }
            _cardGameModel.SetCurrentZoneModelFromList();
        }

        private void CreateNewHundredMoreZonesToList()
        {
            DebugLogger.Log($"Creating new Hundred more zones");
            var index = _cardGameModel.ZoneModelList.Count - 1;
            for (int i = 0; i < 100; i++)
            {
                var zone = CreateRandomZoneModel(index);
                _cardGameModel.AddZoneToList(zone);
            }
        }

        private CardGameZoneModel CreateRandomZoneModel(int levelIndex)
        {
            var zoneType = GetZoneType(levelIndex);
            var slotCount = CardGameConstants.TotalSlotCount;
            var slotIndex = 0;
            var zoneModel = new CardGameZoneModel(zoneType, levelIndex, slotCount);
            if (zoneType == ZoneType.NormalZone)
            {
                slotCount--;
                zoneModel.AddSlotModel(new CardGameSlotModel(SlotType.Bomb, slotIndex, null));
                slotIndex++;
            }

            var rarityCountCalculatorData =
                _cardGameRarityCountCalculator.CalculateRarityCountsByLevel(levelIndex, slotCount);
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
            var modelDict = _cardGameEventModel.ZoneModelDict[rarity].RewardModelDict;
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
            var model = _cardGameEventModel;
            if (levelIndex == 0)
            {
                return ZoneType.NormalZone;
            }

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