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

        public void InitializeScene()
        {
            _cardGameSceneView.SetSpinSlotView(_cardGameModel.CurrentZoneModel);
        }

        public void OnSpinButtonClicked()
        {
            var slotIndex = ChooseRandomSlot();
            
        }

        private int ChooseRandomSlot()
        {
            var zone = _cardGameModel.CurrentZoneModel;
            var randomIndex = MathHelper.GetRandomIndex(zone.SlotModelList);
            return randomIndex;
        }
    }

    public interface ICardGameSceneViewDelegate
    {
        void OnSpinButtonClicked();
    }
}