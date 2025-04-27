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
        
        [Inject] private readonly ICardGameDataTransferController _cardGameDataTransferController;
        [Inject] private readonly ICardGameLevelGenerator _cardGameLevelGenerator;
        [Inject] private readonly CardGameModel _cardGameModel;
        [Inject] private readonly ICardGameSceneController _cardGameSceneController;
        [Inject] private readonly PlayerModel _playerModel;

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


    }
}