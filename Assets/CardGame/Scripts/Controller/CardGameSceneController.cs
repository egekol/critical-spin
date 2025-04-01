using System.Collections.Generic;
using CardGame.Model;
using CardGame.View;
using Main.Scripts.Utilities;
using Zenject;

namespace CardGame.Controller
{
    public interface ICardGameSceneController
    {
        void InitializeScene();
    }

    public class CardGameSceneController : ICardGameSceneController, ICardGameSceneViewDelegate
    {
        [Inject] private readonly CardGameModel _cardGameModel;
        [Inject] private readonly ICardGameSceneView _cardGameSceneView;
        [Inject] private readonly IRewardViewIconSpriteCache _cache;
        [Inject] private readonly ICardGameLevelGenerator _cardGameLevelGenerator;

        public void InitializeScene()
        {
            _cardGameSceneView.Initialize(this);
            _cardGameSceneView.SetSpinSlotView(_cardGameModel.CurrentZoneModel);
        }

        public void OnSpinButtonClicked()
        {
            var slotModelList = _cardGameModel.CurrentZoneModel.SlotModelList;
            var slotIndex = ChooseRandomSlot(slotModelList);
            var reward = slotModelList[slotIndex];
            DebugLogger.Log($"Spin started! - index: {slotIndex}, reward {reward}");
            _cardGameSceneView.SpinAndStopAt(slotIndex);
            
            _cardGameLevelGenerator.SetNextZoneModel();
        }

        private int ChooseRandomSlot(List<CardGameSlotModel> slotModelList)
        {
            var randomIndex = MathHelper.GetRandomIndex(slotModelList);
            return randomIndex;
        }
    }

}