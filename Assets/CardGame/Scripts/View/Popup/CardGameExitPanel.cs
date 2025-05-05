using CardGame.EventBus;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.View.Popup
{
    public class CardGameExitPanel : Popup
    {
        [SerializeField] private Button _exitButton;

        private void OnEnable()
        {
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnDisable()
        {
            _exitButton.onClick.RemoveAllListeners();
        }
        
        private void OnExitButtonClicked()
        {
            MessageBroker.Default.Publish(new ExitButtonClickSignal());
        }
    }
}