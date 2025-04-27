using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardGame.Model.Spin;
using CardGame.Scripts.EventBus;
using CardGame.View;
using Cysharp.Threading.Tasks;
using Main.Scripts.Utilities;
using Zenject;

namespace CardGame.Controller
{
    public interface ICardGameSceneController
    {
        void InitializeScene(ICardGameViewDelegate cardGameViewDelegate);
        void SetSpinningAvailable(bool isActive);
        void SetFailPopupActive(bool isActive);
        void SetExitButtonActive(bool isActive);
        void UpdateSpinSlotView();
    }

    public class CardGameSceneController : ICardGameSceneController, ISpinEventListener, IInitializable, IDisposable
    {
        [Inject] private readonly IRewardViewIconSpriteCache _cache;
        [Inject] private readonly ICardGameLevelGenerator _cardGameLevelGenerator;
        [Inject] private readonly CardGameModel _cardGameModel;
        [Inject] private readonly ICardGameSceneView _cardGameSceneView;
        [Inject] private readonly EventManager _eventManager;
        private const float WaitDurationAfterSuccess = 1.2f;
        private const float FailWaitDuration = .5f;
        public void Initialize()
        {
            _eventManager.SubscribeToSpinEvents(this);
        }

        public void Dispose()
        {
            _eventManager.UnsubscribeToSpinEvents(this);
        }
        
        public void OnSpinButtonClicked()
        {
            var slotModelList = _cardGameModel.CurrentZoneModel.SlotModelList;
            var slotIndex = ChooseRandomSlot(slotModelList);
            var slotModel = slotModelList[slotIndex];
            DebugLogger.Log(
                $"Spin started! Number{_cardGameModel.CurrentZoneIndex} - index: {slotIndex}, reward {slotModel.CardGameRewardModel}");
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
        
        public void InitializeScene(ICardGameViewDelegate cardGameViewDelegate)
        {
            _cardGameSceneView.Initialize(cardGameViewDelegate);
            _cardGameSceneView.SetSpinSlotView(_cardGameModel.CurrentZoneModel);
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

        public void SetExitButtonActive(bool isActive)
        {
            _cardGameSceneView.SetExitButtonActive(isActive);
        }

        public void UpdateSpinSlotView()
        {
            _cardGameSceneView.SetSpinSlotView(_cardGameModel.CurrentZoneModel);
            SetSpinningAvailable(true);
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