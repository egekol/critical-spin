using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Model;
using CardGame.Model.Spin;
using Main.Scripts.Utilities;
using Zenject;

namespace CardGame.Controller
{
    public interface ICardGameLevelGenerator
    {
        void InitializeLevel();
        void SetNextZoneModel();
        void ResetLevel();
    }

    public class CardGameLevelGenerator : ICardGameLevelGenerator
    {
        private static readonly Random _random = new();
        [Inject] private readonly CardGameEventModel _cardGameEventModel;
        [Inject] private readonly CardGameModel _cardGameModel;
        [Inject] private readonly ICardGameRarityCountCalculator _cardGameRarityCountCalculator;

        public void InitializeLevel()
        {
            _cardGameRarityCountCalculator.Initialize(_cardGameEventModel.ZoneRarityCountModel);
            InitializeZones();
        }

        public void SetNextZoneModel()
        {
            _cardGameModel.IncreaseCountIndex();
            if (_cardGameModel.ZoneModelList.Count - 1 <= _cardGameModel.CurrentZoneIndex) CreateTenMoreZonesToList();
            _cardGameModel.SetCurrentZoneModelFromList();
        }

        public void ResetLevel()
        {
            _cardGameModel.Reset();
            InitializeZones();
        }

        private void InitializeZones()
        {
            var zone = CreateRandomZoneModel(0);
            _cardGameModel.AddZoneToList(zone);
            _cardGameModel.SetCurrentZoneModelFromList();

            CreateTenMoreZonesToList();
        }

        private void CreateTenMoreZonesToList()
        {
            DebugLogger.Log("Creating new Ten more zones");
            var index = _cardGameModel.ZoneModelList.Count - 1;
            for (var i = 0; i < 10; i++)
            {
                index++;
                var zone = CreateRandomZoneModel(index);
                _cardGameModel.AddZoneToList(zone);
            }
        }

        private CardGameZoneModel CreateRandomZoneModel(int levelIndex)
        {
            var zoneType = GetZoneType(levelIndex + 1);
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
            for (var i = 0; i < slotCount; i++)
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
            var totalWeight = rewards.Values.Sum();
            var randomValue = _random.Next(totalWeight);

            var cumulativeWeight = 0;
            foreach (var kvp in rewards)
            {
                cumulativeWeight += kvp.Value;
                if (randomValue < cumulativeWeight)
                    return kvp.Key;
            }

            return rewards.Keys.First();
        }

        private ZoneType GetZoneType(int levelCount)
        {
            var model = _cardGameEventModel;
            if (levelCount == 0) return ZoneType.NormalZone;

            if (levelCount % model.SuperZoneCoefficient == 0) return ZoneType.SuperZone;

            if (levelCount % model.SafeZoneCoefficient == 0) return ZoneType.SafeZone;

            return ZoneType.NormalZone;
        }
    }
}