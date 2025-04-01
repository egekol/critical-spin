using System;
using System.Threading.Tasks;
using CardGame.Model;
using Cysharp.Threading.Tasks;
using Main.Scripts.Utilities;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CardGame.View
{
    public interface ICardGameSceneView
    {
        void SetSpinSlotView(CardGameZoneModel zoneModelList);
        UniTask SpinAndStopAt(int slotIndex);
        void Initialize(ICardGameSceneViewDelegate cardGameSceneViewDelegate);
        void ShowFailPopup();
        void SetSpinningActive(bool isActive);
    }

    public class CardGameSceneView : MonoBehaviour, ICardGameSceneView
    {
        [SerializeField] private CardGameSpinView _cardGameSpinView;
        [SerializeField] private Button _spinButton;
        [SerializeField] private CardGameFailPopup cardGameFailPopup;
        
        private ICardGameSceneViewDelegate _delegate;
        private bool _isInSpinState;

        private void OnEnable()
        {
            _spinButton.onClick.AddListener(OnSpinButtonClicked);
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
            
        }

        private void OnReviveButtonClick()
        {
            
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

        public void Initialize(ICardGameSceneViewDelegate cardGameSceneViewDelegate)
        {
            _delegate = cardGameSceneViewDelegate;
        }

        public void ShowFailPopup()
        {
            cardGameFailPopup.gameObject.SetActive(true);
        }

        public void SetSpinSlotView(CardGameZoneModel zoneModelList)
        {
            _cardGameSpinView.SetSpinView(zoneModelList.ZoneType);
            _cardGameSpinView.SetSpinSlots(zoneModelList);
        }

        public async UniTask SpinAndStopAt(int slotIndex)
        {
            _cardGameSpinView.StartRotateSpinOnLoop();
            await UniTask.WaitForSeconds(1);
            await _cardGameSpinView.StopSpinRotationAt(slotIndex);
        }

        public void SetSpinningActive(bool isActive)
        {
            _spinButton.gameObject.SetActive(!isActive);
            _isInSpinState = isActive;
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
            }
        }

        private const string UiSpinButtonName = "ui_elements_spinButton_value";
#endif

    }
    
    public interface ICardGameSceneViewDelegate
    {
        Task OnSpinButtonClicked();
    }
}