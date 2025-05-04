using CardGame.EventBus;
using Main.Scripts.Utilities;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.View.Popup
{
    public class SpinButtonPopup : MonoBehaviour
    {
        [SerializeField] private Button _spinButton;

        
        private void OnEnable()
        {
            _spinButton.onClick.AddListener(OnSpinButtonClicked);
        }
        
        private void OnDisable()
        {
            _spinButton.onClick.RemoveListener(OnSpinButtonClicked);
        }
        
        public void SetSpinningAvailable(bool isActive)
        {
            _spinButton.gameObject.SetActive(isActive);
        }
        
        private void OnSpinButtonClicked()
        {
            if (_isInSpinState)
            {
                DebugLogger.Log("Spinning, cant click to button");
                return;
            }

            MessageBroker.Default.Publish(new SpinButtonClickSignal());
        }
    }
}