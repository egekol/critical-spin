using System;
using System.Threading.Tasks;
using CardGame.Model;
using CardGame.Model.Spin;
using CardGame.View.Spin;
using Cysharp.Threading.Tasks;
using Main.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

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
    }

    public class CardGameSceneView : MonoBehaviour, ICardGameSceneView
    {
        [SerializeField] private CardGameSpinView _cardGameSpinView;
        [SerializeField] private Button _spinButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private CardGameFailPopup cardGameFailPopup;

        private ICardGameViewDelegate _delegate;
        private bool _isInSpinState;
        private const float SpinLoopDuration = 1;

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

        private void OnGiveUpButtonClick()
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
                DebugLogger.Log($"Spinning, cant click to button");
                return;
            }

            _delegate.OnSpinButtonClicked();
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
            _cardGameSpinView.StartRotateSpinOnLoop();
            await UniTask.WaitForSeconds(SpinLoopDuration);
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


#if UNITY_EDITOR
        private void OnValidate()
        {
            var buttons = GetComponentsInChildren<Button>(true);
            foreach (var button in buttons)
            {
                if (button.transform.name == UiSpinButtonName)
                {
                    _spinButton = button;
                }

                if (button.transform.name == UiExitButtonName)
                {
                    _exitButton = button;
                }
            }
        }

        private const string UiSpinButtonName = "ui_elements_spinButton_value";
        private const string UiExitButtonName = "ui_elements_exit_button";
#endif
    }

    public interface ICardGameViewDelegate
    {
        Task OnSpinButtonClicked();
        void OnGiveUpButtonClicked();
        void OnReviveButtonClick();
        void OnExitButtonClicked();
    }
}