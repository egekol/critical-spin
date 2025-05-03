
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.View.Levels
{
    public class CardGameLevelsMiddleNumberView : MonoBehaviour
    {
        [SerializeField] private Image _middleImage;
        // [SerializeField] private Image _middlebackgroundImage;
        [SerializeField] private TextMeshProUGUI _middleNumberText;
        [SerializeField] private Transform _pivotTransformLeft;
        [SerializeField] private Transform _pivotTransformRight;



        [Button]
        public async UniTask PlayNextNumberAnimationAsync(int nextNumber)
        {
            _middleImage.gameObject.SetActive(false);
            _middleNumberText.gameObject.SetActive(false);
            _middleNumberText.text = nextNumber.ToString();
            _middleImage.transform.position = _pivotTransformRight.position;
            _middleNumberText.transform.position = _pivotTransformRight.position;
            _middleImage.gameObject.SetActive(true);
            _middleNumberText.gameObject.SetActive(true);
            _middleNumberText.transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutSine);
            await _middleImage.transform.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.OutSine).ToUniTask();
            
        }
        
    }
}