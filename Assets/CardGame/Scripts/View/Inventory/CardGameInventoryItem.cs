using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.View.Inventory
{
    public class CardGameInventoryItem : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _text;
        public string Value;
        
        public void SetImage(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        public void SetAmount(int amount)
        {
            _text.text = $"x{amount}";
        }

        public void UpdateAmount(int amount)
        {
            SetAmount(amount);
            _text.transform.DOComplete();
            _text.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f).SetLink(gameObject);
        }
    }
}