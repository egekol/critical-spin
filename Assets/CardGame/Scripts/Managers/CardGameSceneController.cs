using System.Collections.Generic;
using System.Threading.Tasks;
using CardGame.Controller;
using CardGame.EventBus;
using CardGame.EventBus.Spin;
using CardGame.Managers.Spin;
using CardGame.Model.Spin;
using CardGame.View;
using Cysharp.Threading.Tasks;
using Main.Scripts.ScriptableSingleton;
using Main.Scripts.ScriptableSingleton.PrefabManager;
using Main.Scripts.Utilities;
using UniRx;
using UnityEngine;

namespace CardGame.Managers
{
    public interface ICardGameSceneController
    {
        void InitializeScene();
        void SetExitButtonActive(bool isActive);
    }

    [CreateAssetMenu(fileName = "CardGameSceneController", menuName = "SO/Manager/CardGameSceneController", order = 0)]
    public class CardGameSceneController : ScriptableSingletonManager<CardGameSceneController>, ICardGameSceneController
    {
        [SerializeField] private RewardViewIconSpriteAtlasSo _cache;
        [SerializeField] private CardGameSceneView _sceneViewPrefab;
        private ICardGameLevelGenerator _cardGameLevelGenerator;
        private ICardGameSceneView _cardGameSceneView;
        private CardGameModel _cardGameModel;
        private const float WaitDurationAfterSuccess = 1.2f;
        private const float FailWaitDuration = .5f;

        public override void Initialize()
        {
            base.Initialize();
            MessageBroker.Default.Receive<SpinButtonClickSignal>().Subscribe(OnSpinButtonClicked).AddTo(_compositeDisposable);
            MessageBroker.Default.Receive<OnGiveUpButtonClickSignal>().Subscribe(OnGiveUpButtonClicked).AddTo(_compositeDisposable);
            MessageBroker.Default.Receive<OnReviveButtonClickSignal>().Subscribe(OnReviveButtonClicked).AddTo(_compositeDisposable);
        }

        public override void LateAwake()
        {
            _cardGameLevelGenerator = new CardGameLevelGenerator();
            _cardGameModel = CardGameModel.Instance;
            _cardGameSceneView = PrefabInitializerManager.Instance.InstantiatePrefabInScene(_sceneViewPrefab);
            base.LateAwake();
        }


        private void OnReviveButtonClicked(OnReviveButtonClickSignal obj)
        {
            SetFailPopupActive(false);
            _cardGameLevelGenerator.SetNextZoneModel();
            SetSpinningAvailable(true);
            var model = _cardGameModel.CurrentZoneModel;
            MessageBroker.Default.Publish(new SpinRestartSignal(model));
        }
        
        private void OnGiveUpButtonClicked(OnGiveUpButtonClickSignal obj)
        {
            SetFailPopupActive(false);
            RestartSpin();
        }

        private void OnSpinButtonClicked(SpinButtonClickSignal obj)
        {
            var slotModelList = _cardGameModel.CurrentZoneModel.SlotModelList;
            var slotIndex = ChooseRandomSlot(slotModelList);
            var slotModel = slotModelList[slotIndex];
            DebugLogger.Log($"Spin started! Number{_cardGameModel.CurrentZoneIndex} - index: {slotIndex}, reward {slotModel.CardGameRewardModel}");
            var isFailed = slotModel.SlotType == SlotType.Bomb;

            if (!isFailed) SaveRewardToRewardPack(slotModel.CardGameRewardModel);

            StartSpinFlow(slotIndex, isFailed).Forget();
        }

        private async UniTask StartSpinFlow(int slotIndex, bool isFailed)
        {
            await StartSpin(slotIndex);
            if (isFailed)
            {
                await PlayFailAnimation();
                await UniTask.WaitForSeconds(FailWaitDuration);
                FailGame();
                return;
            }

            _cardGameLevelGenerator.SetNextZoneModel();
            await UniTask.WaitForSeconds(WaitDurationAfterSuccess);
            UpdateSpinSlotView();
        }

        private void FailGame()
        {
            DebugLogger.Log("Fail game!");
            SetFailPopupActive(true);
        }

        private void SaveRewardToRewardPack(CardGameRewardModel cardGameRewardModel)
        {
            _cardGameModel.AddRewardToPack(cardGameRewardModel);
        }
        private int ChooseRandomSlot(List<CardGameSlotModel> slotModelList)
        {
            var randomIndex = MathHelper.GetRandomIndex(slotModelList);
            return randomIndex;
        }
        
        public void InitializeScene()
        {
            _cardGameLevelGenerator.InitializeLevel();
            var model = _cardGameModel.CurrentZoneModel;
            _cardGameSceneView.SetSpinSlotView(model);
            MessageBroker.Default.Publish(new SpinRestartSignal(model));
        }

        public void SetSpinningAvailable(bool isActive)
        {
            _cardGameSceneView.SetSpinningAvailable(isActive);
            if (_cardGameModel.CurrentZoneIndex > 0) _cardGameSceneView.SetExitButtonActive(true);
        }

        public void SetFailPopupActive(bool isActive)
        {
            _cardGameSceneView.SetFailPopupActive(isActive);
        }

        public async Task StartSpin(int slotIndex)
        {
            SetSpinningAvailable(false);
            _cardGameSceneView.SetExitButtonActive(false);
            await SpinAndStopAt(slotIndex);
        }
        
        private void RestartSpin()
        {
            _cardGameLevelGenerator.ResetLevel();
            UpdateSpinSlotView();

        }

        public void SetExitButtonActive(bool isActive)
        {
            _cardGameSceneView.SetExitButtonActive(isActive);
        }

        public void UpdateSpinSlotView()
        {
            _cardGameSceneView.SetSpinSlotView(_cardGameModel.CurrentZoneModel);
            SetSpinningAvailable(true);
            MessageBroker.Default.Publish(new SpinUpdateSignal(_cardGameModel.CurrentZoneModel));
        }

        public UniTask PlayFailAnimation()
        {
            return _cardGameSceneView.PlayFailAnimation();
        }

        private UniTask SpinAndStopAt(int slotIndex)
        {
            return _cardGameSceneView.SpinAndStopAt(slotIndex);
        }
    }
}