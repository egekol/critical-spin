using System.Collections.Generic;
using System.Threading.Tasks;
using CardGame.Model;
using CardGame.Model.Spin;
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
        Task StartSpin(int slotIndex);
    }

    public class CardGameSceneController : ICardGameSceneController
    {
        [Inject] private readonly CardGameModel _cardGameModel;
        [Inject] private readonly ICardGameSceneView _cardGameSceneView;
        [Inject] private readonly IRewardViewIconSpriteCache _cache;
        [Inject] private readonly ICardGameLevelGenerator _cardGameLevelGenerator;

        public void InitializeScene(ICardGameViewDelegate cardGameViewDelegate)
        {
            _cardGameSceneView.Initialize(cardGameViewDelegate);
            _cardGameSceneView.SetSpinSlotView(_cardGameModel.CurrentZoneModel);
        }

        public void SetSpinningAvailable(bool isActive)
        {
            _cardGameSceneView.SetSpinningAvailable(isActive);
        }

        public UniTask SpinAndStopAt(int slotIndex)
        {
           return _cardGameSceneView.SpinAndStopAt(slotIndex);
        }

        public void SetFailPopupActive(bool isActive)
        {
            _cardGameSceneView.SetFailPopupActive(isActive);
        }

        public async Task StartSpin(int slotIndex)
        {
            SetSpinningAvailable(false);
            await SpinAndStopAt(slotIndex);
        }
    }
}