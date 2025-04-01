using CardGame.Model;
using CardGame.Model.Spin;
using Zenject;

namespace CardGame.Controller
{
    public interface ICardGameMainController
    {
    }

    public class CardGameMainController : ICardGameMainController, IInitializable
    {
        [Inject] private readonly ICardGameSceneController _cardGameSceneController;
        [Inject] private readonly CardGameModel _cardGameModel;
        [Inject] private readonly ICardGameDataTransferController _cardGameDataTransferController;
        [Inject] private readonly ICardGameLevelGenerator _cardGameLevelGenerator;

        public void Initialize()
        {
            _cardGameDataTransferController.SetGameModelFromLevelData();
            _cardGameLevelGenerator.InitializeZones();
            _cardGameSceneController.InitializeScene();
        }
    }
}