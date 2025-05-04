using CardGame.EventBus;
using CardGame.Managers.Spin;
using Main.Scripts.Utilities;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.View.Popup
{
    public class CardGameSpinButtonPopup : MonoBehaviour
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
            if (ScriptableSpinSlotManager.Instance.IsInSpinState)
            {
                DebugLogger.Log("Spinning, cant click to button");
                return;
            }

            MessageBroker.Default.Publish(new SpinButtonClickSignal());
        }
    }
}