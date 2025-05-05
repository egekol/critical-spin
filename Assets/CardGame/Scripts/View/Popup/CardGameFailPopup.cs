using CardGame.EventBus;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.View.Popup
{
    public class CardGameFailPopup : Popup
    {
        [SerializeField] private Button _reviveButton;
        [SerializeField] private Button _giveUpButton;

        private void OnEnable()
        {
            _reviveButton.onClick.AddListener(OnReviveButtonClicked);
            _giveUpButton.onClick.AddListener(OnGiveUpButtonClicked);
        }

        private void OnDisable()
        {
            _reviveButton.onClick.RemoveListener(OnReviveButtonClicked);
            _giveUpButton.onClick.RemoveListener(OnGiveUpButtonClicked);
        }

        private void OnGiveUpButtonClicked()
        {
            MessageBroker.Default.Publish(new OnGiveUpButtonClickSignal());
        }

        private void OnReviveButtonClicked()
        {
            MessageBroker.Default.Publish(new OnReviveButtonClickSignal());
        }


#if UNITY_EDITOR

        private void OnValidate()
        {
            var buttons = GetComponentsInChildren<Button>(true);
            foreach (var button in buttons)
            {
                if (button.transform.name == UiFailReviveButtonName) _reviveButton = button;
                if (button.transform.name == UiFailGiveUpButtonName) _giveUpButton = button;
            }
        }

        private const string UiFailReviveButtonName = "ui_elements_fail_button_revive";

        private const string UiFailGiveUpButtonName = "ui_elements_fail_button_giveUp";
#endif
    }
}