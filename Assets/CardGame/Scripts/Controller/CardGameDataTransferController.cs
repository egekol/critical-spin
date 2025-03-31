using CardGame.Model;
using CardGame.Scripts.Network;
using Main.Scripts.Utilities;
using Zenject;

namespace CardGame.Controller
{
    public interface ICardGameDataTransferController
    {
        void SetModelFromLevelData(CardGameModel gameModel);
    }

    public class CardGameDataTransferController : ICardGameDataTransferController
    {
        [Inject] private readonly ICardGameLevelDto CardGameLevelDto;

        public void SetModelFromLevelData(CardGameModel gameModel)
        {
            gameModel.ClearZoneConfig();
            foreach (var keyValuePair in CardGameLevelDto.GetLevelConfigDictionary())
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
                        Value = rewardDto.RewardData.Value,
                    };
                    zoneConfig.AddRewardModel(rewardModel,rewardDto.RewardProbability);
                }

                gameModel.AddZoneConfig(zoneConfig, rarity);
                DebugLogger.Log($"[SetModelFromLevelData] Added zone config to game model: {zoneConfig}");
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
                _ => HandleException(),
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
                _ => HandleException(),
            };

            CardGameRewardType HandleException()
            {
                DebugLogger.LogError($"Unhandled reward type: {rewardType}");
                return CardGameRewardType.Coin;
            }
        }
    }
}