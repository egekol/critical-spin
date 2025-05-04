
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Main.Scripts.Utilities;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.View.Levels
{
    public class CardGameLevelsMiddleNumberView : MonoBehaviour
    {
        [SerializeField] private Image _middleImage;
        [SerializeField] private TextMeshProUGUI _middleNumberText;
        [SerializeField] private Transform _pivotTransformLeft;
        [SerializeField] private Transform _pivotTransformRight;
        [SerializeField] private CardGameLevelsPopupDataSo _dataSo;

        [Button]
        public async UniTask PlayNextNumberAnimationAsync(int nextNumber)
        {
            ResetAnimationToBack(nextNumber);
            SetTextColor(nextNumber);
            _middleImage.gameObject.SetActive(true);
            _middleNumberText.gameObject.SetActive(true);
            _ = _middleNumberText.transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutSine);
            await _middleImage.transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutSine).ToUniTask();
        }


        public void SetTextColor(int nextNumber)
        {
            if (nextNumber % CardGameConstants.SafeZoneMod == 0)
            {
                _middleImage.sprite = _dataSo.MiddleImageSpriteSafeZone;
                _middleNumberText.color = _dataSo.MiddleTextColorSafeZone;
            }
            else
            {
                _middleImage.sprite = _dataSo.MiddleImageSpriteNormalZone;
                _middleNumberText.color = _dataSo.MiddleTextColorNormalZone;
            }
        }

        private void ResetAnimationToBack(int nextNumber)
        {
            _middleImage.gameObject.SetActive(false);
            _middleNumberText.gameObject.SetActive(false);
            _middleNumberText.text = nextNumber.ToString();
            _middleImage.transform.position = _pivotTransformRight.position;
            _middleNumberText.transform.position = _pivotTransformRight.position;
        }

        public void SetText(int number)
        {
            _middleNumberText.text = number.ToString();
        }
    }
}