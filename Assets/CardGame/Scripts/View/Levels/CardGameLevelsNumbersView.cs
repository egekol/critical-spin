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
            SetTextInMiddle(number);
        }

        private void SetTextInMiddle(int number)
        {
            var middle=_dataSo.LevelsVisibleNumberCount / 2;
            SetLevelText(number, middle-1);
        }

        private void SetLevelText(int number, int index)
        {
            if (_numberTextList.Count <= index || index < 0)
            {
                return;
            }
            var text = _numberTextList[number];
            text.text = number.ToString();
            if (number%CardGameConstants.SafeZoneMod == 0)
            {
                text.color = _dataSo.MiddleTextColorSafeZone;
            }
            else
            {
                text.color = _dataSo.MiddleTextColorNormalZone;
            }
        }
    }
}