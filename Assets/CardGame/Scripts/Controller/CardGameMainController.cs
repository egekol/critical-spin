using CardGame.EventBus;
using CardGame.Model;
using CardGame.Model.Spin;
using Main.Scripts.ScriptableSingleton;
using Main.Scripts.Utilities;
using UniRx;
using UnityEngine;

namespace CardGame.Controller
{
    [CreateAssetMenu(fileName = "CardGameMainController", menuName = "SO/Manager/CardGameMainController", order = 0)]
    public class CardGameMainController : ScriptableSingletonManager<CardGameMainController>
    {
        private ICardGameDataTransferController _cardGameDataTransferController;
        private CardGameModel _cardGameModel;
        private ICardGameSceneController _cardGameSceneController;
        private PlayerModel _playerModel;

        public override void Initialize()
        {
            base.Initialize();
            MessageBroker.Default.Receive<ExitButtonClickSignal>().Subscribe(OnExitButtonClicked).AddTo(_compositeDisposable);
        }

        public override void LateAwake()
        {
            _cardGameDataTransferController = CardGameDataTransferController.Instance;
            _cardGameModel = CardGameModel.Instance;
            _cardGameSceneController = CardGameSceneController.Instance;
            _playerModel = PlayerModel.Instance;
            base.LateAwake();
        }

        public override void Start()
        {
            base.Start();
            _cardGameDataTransferController.SetGameModelFromLevelData();
            _cardGameSceneController.InitializeScene();
        }

        private void OnExitButtonClicked(ExitButtonClickSignal obj)
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