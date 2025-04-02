using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.View.Spin
{
    public class CardGameSpinSlotView : MonoBehaviour
    {
        private const string SpinSlotObjectName = "ui_spin_slot_value";

        [SerializeField] private Image _spinSlotImage;
        [SerializeField] private TextMeshProUGUI _spinSlotAmountText;
        [SerializeField] private int _slotIndex;
        private bool _isRotating;

        public int SlotIndex => _slotIndex;

        private void Update()
        {
            if (_isRotating) _spinSlotImage.transform.rotation = Quaternion.identity;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            var images = GetComponentsInChildren<Image>(true);
            foreach (var image in images)
                if (image.transform.name == SpinSlotObjectName)
                    _spinSlotImage = image;
        }
#endif

        public void SetSpinSlotImage(Sprite sprite)
        {
            _spinSlotImage.sprite = sprite;
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

        public void SetTextViewEnabled(bool isActive)
        {
            _spinSlotAmountText.gameObject.SetActive(isActive);
        }
    }
}