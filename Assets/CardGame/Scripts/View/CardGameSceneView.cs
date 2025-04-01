using System;
using CardGame.Model;
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
        void Initialize(ICardGameSceneViewDelegate cardGameSceneViewDelegate);
    }

    public class CardGameSceneView : MonoBehaviour, ICardGameSceneView
    {
        [SerializeField] private CardGameSpinView _cardGameSpinView;
        [SerializeField] private Button _spinButton;
        private ICardGameSceneViewDelegate _delegate;
        private bool _isSpinning;

        private void OnEnable()
        {
            _spinButton.onClick.AddListener(OnSpinButtonClicked);
        }

        private void OnDisable()
        {
            _spinButton.onClick.RemoveListener(OnSpinButtonClicked);
        }

        private void OnSpinButtonClicked()
        {
            if (_isSpinning)
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

        public void SetSpinSlotView(CardGameZoneModel zoneModelList)
        {
            _cardGameSpinView.SetSpinView(zoneModelList.ZoneType);
            _cardGameSpinView.SetSpinSlots(zoneModelList);
        }

        public async UniTask SpinAndStopAt(int slotIndex)
        {
            SetSpinningActive(true);
            _cardGameSpinView.StartRotateSpinOnLoop();
            await UniTask.WaitForSeconds(1);
            _cardGameSpinView.StopSpinRotationAt(slotIndex);
            _isSpinning = false;
        }

        private void SetSpinningActive(bool isActive)
        {
            
            _spinButton.gameObject.SetActive(!isActive);
            _isSpinning = isActive;
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
        void OnSpinButtonClicked();
    }
}