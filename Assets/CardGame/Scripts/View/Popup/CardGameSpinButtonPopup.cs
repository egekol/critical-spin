using CardGame.EventBus;
using CardGame.EventBus.Spin;
using CardGame.Managers.Spin;
using Main.Scripts.Utilities;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.View.Popup
{
    public class CardGameSpinButtonPopup : Popup
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