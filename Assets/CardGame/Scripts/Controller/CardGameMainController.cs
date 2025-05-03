using CardGame.Model;
using CardGame.Model.Spin;
using Main.Scripts.ScriptableSingleton;
using Main.Scripts.Utilities;

namespace CardGame.Controller
{
    public class CardGameMainController : ScriptableSingletonManager<CardGameMainController>
    {
        private ICardGameDataTransferController _cardGameDataTransferController;
        private CardGameModel _cardGameModel;
        private ICardGameSceneController _cardGameSceneController;
        private PlayerModel _playerModel;

        public override void Initialize()
        {
            // _signalBus.Subscribe<ExitButtonClickSignal>(OnExitButtonClicked);//todo bus
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

        public override void Destroy()
        {
            base.Destroy();
            // _signalBus.TryUnsubscribe<ExitButtonClickSignal>(OnExitButtonClicked); todo
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