using CardGame.Managers;
using CardGame.Managers.Spin;
using CardGame.Model.Spin;
using CardGame.View.Spin;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CardGame.View
{
    public interface ICardGameSceneView
    {
        void SetSpinSlotView(CardGameZoneModel zoneModelList);
        UniTask SpinAndStopAt(int slotIndex);
        void SetFailPopupActive(bool isActive);
        void SetSpinningAvailable(bool isActive);
        void SetExitButtonActive(bool isActive);
        UniTask PlayFailAnimation();
    }

    public class CardGameSceneView : MonoBehaviour, ICardGameSceneView
    {
        private const float SpinLoopDuration = 1;
        private CardGameSpinView _cardGameSpinView;
        [SerializeField] private CardGamePopupManager _popupManager;

        private void Awake()
        {
            _cardGameSpinView = ScriptableSpinSlotManager.Instance.InstantiateSpinPrefab(transform);
        }

        public void SetFailPopupActive(bool isActive)
        {
            _popupManager.FailPopup.SetActive(isActive);
        }

        public void SetSpinSlotView(CardGameZoneModel zoneModelList)
        {
            _cardGameSpinView.SetSpinView(zoneModelList.ZoneType);
            _cardGameSpinView.SetSpinSlots(zoneModelList);
        }

        public async UniTask SpinAndStopAt(int slotIndex)
        {
            await _cardGameSpinView.StartClickAnimation();
            _cardGameSpinView.StartRotateSpinOnLoop();
            _cardGameSpinView.SetBlurActive(true);
            await UniTask.WaitForSeconds(SpinLoopDuration);
            _cardGameSpinView.SetBlurActive(false);
            await _cardGameSpinView.StopSpinRotationAt(slotIndex);
        }

        public void SetSpinningAvailable(bool isActive)
        {
            _popupManager.SpinButtonPopup.SetSpinningAvailable(isActive);
            ScriptableSpinSlotManager.Instance.SetSpinState(!isActive);
        }

        public void SetExitButtonActive(bool isActive)
        {
            _popupManager.ExitPanel.SetActive(isActive);
        }

        public UniTask PlayFailAnimation()
        {
            return _cardGameSpinView.PlayFailAnimation();
        }


    }
}