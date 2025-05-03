using CardGame.Data;
using CardGame.Model;
using CardGame.Model.Spin;
using Main.Scripts.ScriptableSingleton;
using Main.Scripts.Utilities;
using UnityEngine;

namespace CardGame.Controller
{
    public interface ICardGameDataTransferController
    {
        void SetGameModelFromLevelData();
    }

    public class CardGameDataTransferController : ScriptableSingletonManager<CardGameDataTransferController>, ICardGameDataTransferController
    {
        private  CardGameEventModel _cardGameEventModel;
        [SerializeField] private  CardGameLevelDataTransferSo _cardGameLevelDto;

        public override void LateAwake()
        {
            base.LateAwake();
            _cardGameEventModel = CardGameEventModel.Instance;
        }

        public void SetGameModelFromLevelData()
        {
            SetRewardModelFromLevelData(_cardGameEventModel);
            SetZoneRarityCountFromLevelData(_cardGameEventModel);
            SetLevelConfigFromLevelData(_cardGameEventModel);
        }

        private void SetLevelConfigFromLevelData(CardGameEventModel eventModel)
        {
            var config = _cardGameLevelDto.GetLevelConfig();
            eventModel.SetLevelConfig(config.SafeZoneCoefficient, config.SuperZoneCoefficient);
        }

        private void SetRewardModelFromLevelData(CardGameEventModel eventModel)
        {
            eventModel.ClearZoneConfig();
            foreach (var keyValuePair in _cardGameLevelDto.GetLevelConfigDictionary())
            {
                var zoneConfig = new CardGameZoneConfig();
                var rarity = ConvertRarityToCardGameRarity(keyValuePair.Key);
                zoneConfig.SetRarity(rarity);

                foreach (var rewardDto in keyValuePair.Value)
                {
                    var rewardModel = new CardGameRewardModel
                    {
                        CardGameRewardType = ConvertRewardDtoToCardGameRewardType(rewardDto.RewardData.Type),
                        Amount = rewardDto.Amount,
                        Value = rewardDto.RewardData.Value
                    };
                    zoneConfig.AddRewardModel(rewardModel, rewardDto.RewardProbability);
                }

                eventModel.AddZoneConfig(zoneConfig, rarity);
                DebugLogger.Log($"[SetModelFromLevelData] Added zone config to game model: {zoneConfig}");
            }
        }

        private void SetZoneRarityCountFromLevelData(CardGameEventModel eventModel)
        {
            eventModel.ZoneRarityCountModel.ClearZoneRarityCount();

            var configList = _cardGameLevelDto.GetZoneRarityCountConfigList();

            foreach (var zoneRarityCountConfig in configList)
            {
                var rarity = ConvertRarityToCardGameRarity(zoneRarityCountConfig.Rarity);

                eventModel.ZoneRarityCountModel.AddZoneRarityCountConfig(rarity,
                    zoneRarityCountConfig.MinAvailableLevel,
                    zoneRarityCountConfig.MinAvailableCount, zoneRarityCountConfig.MaxAvailableCount,
                    zoneRarityCountConfig.MaxAvailableLevel);
            }
        }

        private CardGameRewardRarity ConvertRarityToCardGameRarity(RewardRarity rewardRarity)
        {
            return rewardRarity switch
            {
                RewardRarity.Common => CardGameRewardRarity.Common,
                RewardRarity.Uncommon => CardGameRewardRarity.Uncommon,
                RewardRarity.Rare => CardGameRewardRarity.Rare,
                RewardRarity.Legendary => CardGameRewardRarity.Legendary,
                RewardRarity.Epic => CardGameRewardRarity.Epic,
                RewardRarity.None => HandleException(),
                _ => HandleException()
            };

            CardGameRewardRarity HandleException()
            {
                DebugLogger.LogError($"Unhandled reward type: {rewardRarity}");
                return CardGameRewardRarity.Common;
            }
        }

        private CardGameRewardType ConvertRewardDtoToCardGameRewardType(RewardType rewardType)
        {
            return rewardType switch
            {
                RewardType.Coin => CardGameRewardType.Coin,
                RewardType.Chest => CardGameRewardType.Chest,
                RewardType.GunPoint => CardGameRewardType.GunPoint,
                RewardType.Skin => CardGameRewardType.Skin,
                RewardType.None => HandleException(),
                _ => HandleException()
            };

            CardGameRewardType HandleException()
            {
                DebugLogger.LogError($"Unhandled reward type: {rewardType}");
                return CardGameRewardType.Coin;
            }
        }
    }
}