using System;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.View
{
    public class CardGameSpinSlotView : MonoBehaviour
    {
        [SerializeField] private bool _isRotating;
        [SerializeField] private Image _spinSlotImage;
        
        private const string SpinSlotObjectName = "ui_spin_slot_value";

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
    }
}