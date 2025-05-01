using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardGame.Model;
using CardGame.Model.Spin;
using CardGame.Scripts.EventBus;
using CardGame.View;
using Cysharp.Threading.Tasks;
using Main.Scripts.Utilities;
using Zenject;

namespace CardGame.Controller
{
    public interface ICardGameMainController
    {
    }

    public class CardGameMainController : ICardGameMainController, IInitializable, IDisposable
    {
        [Inject] private readonly ICardGameDataTransferController _cardGameDataTransferController;
        [Inject] private readonly ICardGameLevelGenerator _cardGameLevelGenerator;
        [Inject] private readonly CardGameModel _cardGameModel;
        [Inject] private readonly ICardGameSceneController _cardGameSceneController;
        [Inject] private readonly SignalBus _signalBus;
        [Inject] private readonly PlayerModel _playerModel;

        public void Initialize()
        {
            _cardGameDataTransferController.SetGameModelFromLevelData();
            _cardGameLevelGenerator.InitializeLevel();
            _cardGameSceneController.InitializeScene();
            _signalBus.Subscribe<ExitButtonClickSignal>(OnExitButtonClicked);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<ExitButtonClickSignal>(OnExitButtonClicked);
        }

        public void OnExitButtonClicked()
        {
            SaveRewardPackToPlayerModel();
            _cardGameSceneController.SetExitButtonActive(false);
        }

        private void SaveRewardPackToPlayerModel()
        {
            DebugLogger.Log($"Saving reward to player model{string.Join(", ", _cardGameModel.RewardPack)}");
            _playerModel.UpdateModel(_cardGameModel.RewardPack);
        }

    }
}