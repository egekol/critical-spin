using System.Collections.Generic;
using CardGame.EventBus.Spin;
using CardGame.Managers.Spin;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Main.Scripts.Utilities;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;

namespace CardGame.View.Levels
{
    public class CardGameLevelsNumbersView : MonoBehaviour
    {
        [SerializeField] private List<TextMeshProUGUI> _numberTextList;
        [SerializeField] private List<Transform> _numberTfList;

        [SerializeField] private Transform _pivotTransformLeft;
        [SerializeField] private Transform _pivotTransformRight;
        [SerializeField] private Transform _numberTextParent;
        [SerializeField] private Transform _numberPositionsTfParent;
        [SerializeField] private CardGameLevelsMiddleNumberView _middleNumberView;

        private CompositeDisposable _compositeDisposable;
        private CardGameLevelsUIDataSo _dataSo;

        private void Awake()
        {
            _dataSo = ScriptableSpinSlotManager.Instance.levelsUIDataSo;
            _compositeDisposable = new CompositeDisposable();
            MessageBroker.Default.Receive<SpinUpdateSignal>().Subscribe(OnSpinUpdate).AddTo(_compositeDisposable);
            MessageBroker.Default.Receive<SpinRestartSignal>().Subscribe(OnSpinRestart).AddTo(_compositeDisposable);
            InstantiateObjects();
        }

        private void OnDestroy()
        {
            _compositeDisposable.Dispose();
        }

        private void OnSpinUpdate(SpinUpdateSignal signal)
        {
            var level = signal.ZoneModel.ZoneIndex+1;
            SetCurrentLevel(level-1);
            AnimateNextLevel(level).Forget();
        }
        
        private void OnSpinRestart(SpinRestartSignal signal)
        {
            var level = signal.ZoneModel.ZoneIndex+1;
            SetCurrentLevel(level);
        }
        
        private void InstantiateObjects()
        {
            _numberTfList = new();
            _numberTextList = new();
            var distance = Vector3.Distance(_pivotTransformLeft.position, _pivotTransformRight.position);
            var count = _dataSo.LevelsVisibleNumberCount;
            for (int i = 0; i < count; i++)
            {
                var offset = distance / count * (i + 1) * Vector3.right;
                var position = _pivotTransformLeft.position + offset;
                var text = Instantiate(_dataSo.LevelNumberTextPrefab, position, Quaternion.identity, _numberTextParent);
                _numberTextList.Add(text);
            }

            for (int i = 0; i < count+1; i++)
            {
                var offset = distance / count * i * Vector3.right;
                var position = _pivotTransformLeft.position + offset;
                var tf = Instantiate(_dataSo.NumberPositionTransformPrefab, position, Quaternion.identity, _numberPositionsTfParent);
                _numberTfList.Add(tf);
            }
        }

        [Button]
        public async UniTask AnimateNextLevel(int currentLevel)
        {
            var halfCount = _dataSo.LevelsVisibleNumberCount / 2 -1;

            _numberTextList[^1].text = (currentLevel + halfCount).ToString();
            _numberTextList[^1].gameObject.SetActive(true);
            
            for (int i = 0; i < _numberTextList.Count; i++)
            {
                _ = _numberTextList[i].transform.DOMove(_numberTfList[i].position, .5f).SetEase(Ease.OutSine);
            }

            await _middleNumberView.PlayNextNumberAnimationAsync(currentLevel, _dataSo);
            var firstText = _numberTextList[0];
            _numberTextList.RemoveAt(0);
            _numberTextList.Add(firstText);
            firstText.transform.position = _numberTfList[^1].position;
        }
        
        [Button]
        public void SetCurrentLevel(int number)
        {
            _middleNumberView.SetText(number);
                
            var middleCount = _dataSo.LevelsVisibleNumberCount / 2;
            var middleIndex = middleCount - 1;
            int startIndex = 0;
            if (number < middleCount)
            {
                startIndex = middleCount - number;
            }
            
            var startingNumber = 1;
            
            if (number > middleCount)
            {
                startingNumber = number - middleIndex;
            }
            
            var count = 0;
            for (int i = startIndex; i < _numberTextList.Count; i++)
            {
                SetLevelText(startingNumber + count, i);
                count++;
                DebugLogger.Log($"SetCurrentLevel {startingNumber + count} ; startIndex {startIndex}; count {count}; index {i}");
            }
            for (int i = 0; i < startIndex; i++)
            {
                SetLevelTextBlank(i);
            }
        }

        private void SetLevelText(int number, int index)
        {
            if (_numberTextList.Count <= index || index < 0)
            {
                return;
            }

            var text = _numberTextList[index];
            text.text = number.ToString();
            if (number % CardGameConstants.SafeZoneMod == 0)
            {
                text.color = _dataSo.MiddleTextColorSafeZone;
            }
            else
            {
                text.color = _dataSo.MiddleTextColorNormalZone;
            }
            text.gameObject.SetActive(true);
        }

        private void SetLevelTextBlank(int index)
        {
            if (_numberTextList.Count <= index || index < 0)
            {
                return;
            }
            var text = _numberTextList[index];
            text.gameObject.SetActive(false);
        }
    }
}