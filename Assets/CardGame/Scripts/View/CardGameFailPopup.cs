using System;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.View
{
    public class CardGameFailPopup : MonoBehaviour
    {
        [SerializeField] private Button _reviveButton;
        [SerializeField] private Button _giveUpButton;

        public Action OnGiveUpButtonClick { get; set; }
        public Action OnReviveButtonClick { get; set; }
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
            OnGiveUpButtonClick?.Invoke();
        }

        private void OnReviveButtonClicked()
        {
            OnReviveButtonClick?.Invoke();
        }
    }
}