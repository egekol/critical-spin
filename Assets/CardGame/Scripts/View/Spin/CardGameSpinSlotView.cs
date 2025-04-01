using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.View.Spin
{
    public class CardGameSpinSlotView : MonoBehaviour
    {
        private bool _isRotating;
        
        [SerializeField] private Image _spinSlotImage;
        [SerializeField] private TextMeshProUGUI _spinSlotAmountText;
        [SerializeField] private int _slotIndex;

        private const string SpinSlotObjectName = "ui_spin_slot_value";

        public int SlotIndex => _slotIndex;

        public void SetSpinSlotImage(Sprite sprite)
        {
            _spinSlotImage.sprite = sprite;
        }

        private void Update()
        {
            if (_isRotating)
            {
                _spinSlotImage.transform.rotation = Quaternion.identity;
            }
        }

        public void SetSlotIndex(int index)
        {
            _slotIndex = index;
        }

        public void SetSpinning(bool isSpinning)
        {
            _isRotating = isSpinning;
        }

        public void SetSpinSlotAmount(ushort amount)
        {
            _spinSlotAmountText.SetText($"x{amount}");
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            var images = GetComponentsInChildren<Image>(true);
            foreach (var image in images)
            {
                if (image.transform.name == SpinSlotObjectName)
                {
                    _spinSlotImage = image;
                }
            }
        }
#endif

        public void SetTextViewEnabled(bool isActive)
        {
            _spinSlotAmountText.gameObject.SetActive(isActive);
        }
    }
}