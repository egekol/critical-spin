using System;
using System.Collections.Generic;
using Main.Scripts.Utilities;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace CardGame.View.Levels
{
    public class CardGameLevelsNumbersView : MonoBehaviour
    {
        [SerializeField] private List<TextMeshProUGUI> _numberTextList;
        [SerializeField] private List<Vector3> _numberPositionList;
        [SerializeField] private List<Transform> _numberTfList;

        [SerializeField] private Transform _pivotTransformLeft;
        [SerializeField] private Transform _pivotTransformRight;
        [SerializeField] private Transform _numberTextParent;
        [SerializeField] private Transform _numberPositionsTfParent;
        [SerializeField] private CardGameLevelsPopupDataSo _dataSo;

        private void Awake()
        {
            _numberPositionList = new();
            _numberTfList = new();
            _numberTextList = new();
            var distance = Vector3.Distance(_pivotTransformLeft.position, _pivotTransformRight.position);
            var count = _dataSo.LevelsVisibleNumberCount;
            for (int i = 0; i < count; i++)
            {
                var offset = distance / count * (i + 1) * Vector3.right;
                var position = _pivotTransformLeft.position + offset;
                _numberPositionList.Add(position);
                var tf = Instantiate(_dataSo.NumberPositionTransformPrefab, position, Quaternion.identity, _numberPositionsTfParent);
                var text = Instantiate(_dataSo.LevelNumberTextPrefab, position, Quaternion.identity, _numberTextParent);
                _numberTextList.Add(text);
                _numberTfList.Add(tf);
            }
        }

        [Button]
        public void SetCurrentLevel(int number)
        {
            var middleCount = _dataSo.LevelsVisibleNumberCount / 2;
            var middleIndex = middleCount - 1;
            int startIndex = 0;
            if (number < middleCount)
            {
                startIndex = middleIndex - number;
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