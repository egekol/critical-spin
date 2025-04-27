using System.Threading.Tasks;
using CardGame.Model.Spin;
using CardGame.Scripts.EventBus;
using CardGame.View.Spin;
using Cysharp.Threading.Tasks;
using Main.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CardGame.View
{
    public interface ICardGameSceneView
    {
        void SetSpinSlotView(CardGameZoneModel zoneModelList);
        UniTask SpinAndStopAt(int slotIndex);
        void Initialize(ICardGameViewDelegate cardGameViewDelegate);
        void SetFailPopupActive(bool isActive);
        void SetSpinningAvailable(bool isActive);
        void SetExitButtonActive(bool isActive);
        UniTask PlayFailAnimation();
    }

    public class CardGameSceneView : MonoBehaviour, ICardGameSceneView
    {
        private const float SpinLoopDuration = 1;
        [SerializeField] private CardGameSpinView _cardGameSpinView;
        [SerializeField] private Button _spinButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private CardGameFailPopup cardGameFailPopup;

        [Inject] private SignalBus _signalBus;

        private ICardGameViewDelegate _delegate;
        private bool _isInSpinState;

        private void OnEnable()
        {
            _spinButton.onClick.AddListener(OnSpinButtonClicked);
            _exitButton.onClick.AddListener(OnExitButtonClicked);
            cardGameFailPopup.OnReviveButtonClick += OnReviveButtonClick;
            cardGameFailPopup.OnGiveUpButtonClick += OnGiveUpButtonClick;
        }

        private void OnDisable()
        {
            _spinButton.onClick.RemoveListener(OnSpinButtonClicked);
            cardGameFailPopup.OnReviveButtonClick -= OnReviveButtonClick;
            cardGameFailPopup.OnGiveUpButtonClick -= OnGiveUpButtonClick;
        }

        public void Initialize(ICardGameViewDelegate cardGameViewDelegate)
        {
            _delegate = cardGameViewDelegate;
        }

        public void SetFailPopupActive(bool isActive)
        {
            cardGameFailPopup.gameObject.SetActive(isActive);
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
            _spinButton.gameObject.SetActive(isActive);
            _isInSpinState = !isActive;
        }

        public void SetExitButtonActive(bool isActive)
        {
            _exitButton.gameObject.SetActive(isActive);
        }

        public UniTask PlayFailAnimation()
        {
            return _cardGameSpinView.PlayFailAnimation();
        }

        private void OnGiveUpButtonClick() //todo Get rid of dependencies using event bus
        {
            _delegate.OnGiveUpButtonClicked();
        }

        private void OnReviveButtonClick()
        {
            _delegate.OnReviveButtonClick();
        }

        private void OnExitButtonClicked()
        {
            _delegate.OnExitButtonClicked();
        }

        private void OnSpinButtonClicked()
        {
            if (_isInSpinState)
            {
                DebugLogger.Log("Spinning, cant click to button");
                return;
            }

            _signalBus.Fire<SpinButtonClickSignal>();
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            var buttons = GetComponentsInChildren<Button>(true);
            foreach (var button in buttons)
            {
                if (button.transform.name == UiSpinButtonName) _spinButton = button;

                if (button.transform.name == UiExitButtonName) _exitButton = button;
            }
        }

        private const string UiSpinButtonName = "ui_elements_spinButton_value";
        private const string UiExitButtonName = "ui_elements_exit_button";
#endif
    }

    public interface ICardGameViewDelegate
    {
        void OnGiveUpButtonClicked();
        void OnReviveButtonClick();
        void OnExitButtonClicked();
    }
}