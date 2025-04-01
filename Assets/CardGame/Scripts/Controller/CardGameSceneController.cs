using System.Collections.Generic;
using System.Threading.Tasks;
using CardGame.Model;
using CardGame.Model.Spin;
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

        public async Task OnSpinButtonClicked()
        {
            var slotModelList = _cardGameModel.CurrentZoneModel.SlotModelList;
            var slotIndex = ChooseRandomSlot(slotModelList);
            var slotModel = slotModelList[slotIndex];
            DebugLogger.Log($"Spin started! Number{_cardGameModel.CurrentZoneIndex} - index: {slotIndex}, reward {slotModel.CardGameRewardModel}");
            _cardGameSceneView.SetSpinningActive(true);
            await _cardGameSceneView.SpinAndStopAt(slotIndex);

            if (slotModel.SlotType == SlotType.Bomb)
            {
                FailGame();
                return;
            }
            

            _cardGameLevelGenerator.SetNextZoneModel();
            _cardGameSceneView.SetSpinningActive(false);

        }

        private void FailGame()
        {
            _cardGameSceneView.ShowFailPopup();
        }

        private int ChooseRandomSlot(List<CardGameSlotModel> slotModelList)
        {
            var randomIndex = MathHelper.GetRandomIndex(slotModelList);
            return randomIndex;
        }
    }
}