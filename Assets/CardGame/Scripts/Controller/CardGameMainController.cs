using System.Collections.Generic;
using System.Threading.Tasks;
using CardGame.Model;
using CardGame.Model.Spin;
using CardGame.View;
using Cysharp.Threading.Tasks;
using Main.Scripts.Utilities;
using Zenject;

namespace CardGame.Controller
{
    public interface ICardGameMainController
    {
    }

    public class CardGameMainController : ICardGameMainController, IInitializable, ICardGameViewDelegate
    {
        private const float WaitDurationAfterSuccess = 1.2f;
        private const float FailWaitDuration = .5f;
        [Inject] private readonly ICardGameDataTransferController _cardGameDataTransferController;
        [Inject] private readonly ICardGameLevelGenerator _cardGameLevelGenerator;
        [Inject] private readonly CardGameModel _cardGameModel;
        [Inject] private readonly ICardGameSceneController _cardGameSceneController;
        [Inject] private readonly PlayerModel _playerModel;

        public async Task OnSpinButtonClicked()
        {
            var slotModelList = _cardGameModel.CurrentZoneModel.SlotModelList;
            var slotIndex = ChooseRandomSlot(slotModelList);
            var slotModel = slotModelList[slotIndex];
            DebugLogger.Log(
                $"Spin started! Number{_cardGameModel.CurrentZoneIndex} - index: {slotIndex}, reward {slotModel.CardGameRewardModel}");
            var isFailed = slotModel.SlotType == SlotType.Bomb;

            if (!isFailed) SaveRewardToRewardPack(slotModel.CardGameRewardModel);

            await _cardGameSceneController.StartSpin(slotIndex);
            if (isFailed)
            {
                await _cardGameSceneController.PlayFailAnimation();
                await UniTask.WaitForSeconds(FailWaitDuration);
                FailGame();
                return;
            }

            _cardGameLevelGenerator.SetNextZoneModel();
            await UniTask.WaitForSeconds(WaitDurationAfterSuccess);
            _cardGameSceneController.UpdateSpinSlotView();
        }

        public void OnGiveUpButtonClicked()
        {
            _cardGameSceneController.SetFailPopupActive(false);
            RestartSpin();
        }

        public void OnReviveButtonClick()
        {
            _cardGameSceneController.SetFailPopupActive(false);
            _cardGameLevelGenerator.SetNextZoneModel();
            _cardGameSceneController.SetSpinningAvailable(true);
        }

        public void OnExitButtonClicked()
        {
            SaveRewardPackToPlayerModel();
            _cardGameSceneController.SetExitButtonActive(false);
        }

        public void Initialize()
        {
            _cardGameDataTransferController.SetGameModelFromLevelData();
            _cardGameLevelGenerator.InitializeLevel();
            _cardGameSceneController.InitializeScene(this);
        }

        private void SaveRewardToRewardPack(CardGameRewardModel cardGameRewardModel)
        {
            _cardGameModel.AddRewardToPack(cardGameRewardModel);
        }

        private void SaveRewardPackToPlayerModel()
        {
            DebugLogger.Log($"Saving reward to player model{string.Join(", ", _cardGameModel.RewardPack)}");
            _playerModel.UpdateModel(_cardGameModel.RewardPack);
        }

        private void RestartSpin()
        {
            _cardGameLevelGenerator.ResetLevel();
            _cardGameSceneController.UpdateSpinSlotView();
        }

        private void FailGame()
        {
            DebugLogger.Log("Fail game!");
            _cardGameSceneController.SetFailPopupActive(true);
        }

        private int ChooseRandomSlot(List<CardGameSlotModel> slotModelList)
        {
            var randomIndex = MathHelper.GetRandomIndex(slotModelList);
            return randomIndex;
        }
    }
}