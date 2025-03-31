using CardGame.Model;
using CardGame.View;
using Zenject;

namespace CardGame.Controller
{
    public interface ICardGameSceneController
    {
        void InitializeScene();
    }

    public class CardGameSceneController : ICardGameSceneController
    {
        [Inject] private readonly CardGameModel _cardGameModel;
        [Inject] private readonly ICardGameSceneView _cardGameSceneView;

        public void InitializeScene()
        {
            _cardGameSceneView.SetSpinSlotView(_cardGameModel.CurrentZoneModel);
            
        }
    }
}