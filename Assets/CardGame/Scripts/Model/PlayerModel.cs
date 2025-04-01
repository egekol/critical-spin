using System;
using System.Collections.Generic;
using CardGame.Model.Spin;

namespace CardGame.Model
{
    public class PlayerModel
    {
        public long Coin { get; set; }
        public Dictionary<string, ChestModel> ChestModelDict { get; set; } = new();
        public Dictionary<string, GunPointModel> GunPointModelDict { get; set; } = new();
        public List<string> SkinInventoryList { get; set; } = new();

        public void UpdateModel(List<CardGameRewardModel> rewardModelList)
        {
            foreach (var rewardModel in rewardModelList)
            {
                UpdateModel(rewardModel);
            }
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
            if (!SkinInventoryList.Contains(rewardModel.Value))
            {
                SkinInventoryList.Add(rewardModel.Value);
            }
        }

        private void UpdateGunPoint(CardGameRewardModel rewardModel)
        {
            if (GunPointModelDict.ContainsKey(rewardModel.Value))
            {
                GunPointModelDict[rewardModel.Value].Update(rewardModel.Amount);
            }
            else
            {
                GunPointModelDict.Add(rewardModel.Value, new GunPointModel(rewardModel.Value));
            }
        }

        private void UpdateChest(CardGameRewardModel rewardModel)
        {
            if (ChestModelDict.ContainsKey(rewardModel.Value))
            {
                ChestModelDict[rewardModel.Value].Update(rewardModel.Amount);
            }
            else
            {
                ChestModelDict.Add(rewardModel.Value, new ChestModel(rewardModel.Value));
            }
        }

        private void UpdateCoin(CardGameRewardModel rewardModel)
        {
            Coin += rewardModel.Amount;
        }
    }

    public class BaseItemModel
    {
        public string Name { get; set; }
        public int Amount { get; set; }

        protected BaseItemModel(string value)
        {
            Name = value;
        }

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
}