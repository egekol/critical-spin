using System;
using System.Collections.Generic;
using CardGame.Model.Spin;
using Main.Scripts.ScriptableSingleton;

namespace CardGame.Model
{
    public class PlayerModel : ScriptableSingletonManager<PlayerModel>
    {
        public CurrencyModel CurrencyModel { get; } = new();
        public Dictionary<string, ChestModel> ChestModelDict { get; } = new();
        public Dictionary<string, GunPointModel> GunPointModelDict { get; } = new();
        public List<string> SkinInventoryList { get; } = new();

        public void UpdateModel(List<CardGameRewardModel> rewardModelList)
        {
            foreach (var rewardModel in rewardModelList) UpdateModel(rewardModel);
        }

        private void UpdateModel(CardGameRewardModel rewardModel)
        {
            switch (rewardModel.CardGameRewardType)
            {
                case CardGameRewardType.Coin:
                    UpdateCoin(rewardModel);
                    break;
                case CardGameRewardType.Chest:
                    UpdateChest(rewardModel);
                    break;
                case CardGameRewardType.GunPoint:
                    UpdateGunPoint(rewardModel);
                    break;
                case CardGameRewardType.Skin:
                    UpdateSkins(rewardModel);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateSkins(CardGameRewardModel rewardModel)
        {
            if (!SkinInventoryList.Contains(rewardModel.Value)) SkinInventoryList.Add(rewardModel.Value);
        }

        private void UpdateGunPoint(CardGameRewardModel rewardModel)
        {
            if (GunPointModelDict.ContainsKey(rewardModel.Value))
                GunPointModelDict[rewardModel.Value].Update(rewardModel.Amount);
            else
                GunPointModelDict.Add(rewardModel.Value, new GunPointModel(rewardModel.Value));
        }

        private void UpdateChest(CardGameRewardModel rewardModel)
        {
            if (ChestModelDict.ContainsKey(rewardModel.Value))
                ChestModelDict[rewardModel.Value].Update(rewardModel.Amount);
            else
                ChestModelDict.Add(rewardModel.Value, new ChestModel(rewardModel.Value));
        }

        private void UpdateCoin(CardGameRewardModel rewardModel)
        {
            CurrencyModel.AddCoin(rewardModel.Amount);
        }
    }

    public class BaseItemModel
    {
        protected BaseItemModel(string value)
        {
            Name = value;
        }

        public string Name { get; set; }
        public int Amount { get; private set; }

        public void Update(ushort amount)
        {
            Amount += amount;
        }
    }

    public class GunPointModel : BaseItemModel
    {
        public GunPointModel(string value) : base(value)
        {
        }
    }

    public class ChestModel : BaseItemModel
    {
        public ChestModel(string value) : base(value)
        {
        }
    }

    public class CurrencyModel
    {
        public ulong Coin { get; private set; }

        public void AddCoin(ulong amount)
        {
            Coin += amount;
        }
    }
    
}