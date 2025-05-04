using CardGame.EventBus;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.View.Popup
{
    public class CardGameExitPanel : MonoBehaviour
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
        
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
        private void OnExitButtonClicked()
        {
            MessageBroker.Default.Publish(new ExitButtonClickSignal());
        }
    }
}