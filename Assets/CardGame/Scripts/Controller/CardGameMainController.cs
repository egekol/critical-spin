using CardGame.Model;
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
        [Inject] private readonly CardGameDataTransferController _cardGameDataTransferController;
        public void Initialize()
        {
            _cardGameDataTransferController.SetModelFromLevelData(_cardGameModel);
            _cardGameSceneController.InitializeScene();
        }
    }
}