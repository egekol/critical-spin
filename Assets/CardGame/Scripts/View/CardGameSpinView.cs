using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.View
{
    public class CardGameSpinView : MonoBehaviour
    {
        [SerializeField] private Image _spinBaseImage;
        [SerializeField] private Transform _spinParentTf;
        [SerializeField] private SpinAnimationParameterSo _spinAnimationParameter;
        [SerializeField] private List<CardGameSpinSlotView> _spinSlotViewList;
        private Tween _loopTween;
        private Tween _blurTween;

        private const string SpinBaseObjectName = "ui_spin_base_value";
        private const string SpinParentObjectName = "ui_spin_parent";
        private const string SpinBlurValueFloatName = "_BlurSize";


        [Button]
        public void StartRotateSpinOnLoop()
        {
            var angle = _spinParentTf.rotation.eulerAngles;

            _loopTween = _spinParentTf.DORotate(angle + Vector3.forward * 360f,
                    _spinAnimationParameter.LoopRotationDuration, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear).SetLink(gameObject);
        }

        [Button]
        public void StopRotateSpinOnLoop()
        {
            _loopTween?.Kill();
        }

        private void Update()
        {
            if (!)
            {
                
            }
        }

        [Button]
        public void StopSpinRotationAt(int spinIndex, int spinCountBeforeStop = 0)
        {
            if (spinIndex < 0 || spinIndex >= _spinSlotViewList.Count)
            {
                return;
            }

            StopRotateSpinOnLoop();

            var slotView = _spinSlotViewList[spinIndex];
            var angle = CalculateAngleOfSlot(slotView);
            var rotationAngle = spinCountBeforeStop * 360f;
            var totalRotation = rotationAngle + angle;

            var rotationDuration = CalculateRotationDuration(angle, spinCountBeforeStop);
            var rotation = _spinParentTf.transform.rotation.eulerAngles + Vector3.forward * totalRotation;
            _spinParentTf.DORotate(rotation,
                    rotationDuration, RotateMode.FastBeyond360).SetEase(_spinAnimationParameter.StopRotationEase)
                .SetLink(gameObject);
            Debug.Log($"last :: {rotation}");
            return;

            float CalculateAngleOfSlot(CardGameSpinSlotView slot)
            {
                var rhs = slot.transform.up;
                Debug.Log($"rhs :: {rhs}");
                float slotAngle = Mathf.Atan2(rhs.x, rhs.y) * Mathf.Rad2Deg;
                slotAngle = (slotAngle < 0) ? slotAngle + 360 : slotAngle;
                Debug.Log($"angle :: {slotAngle}");
                return slotAngle;
            }

            float CalculateRotationDuration(float slotAngle, int spinCount)
            {
                var angleValue = slotAngle / 360f;
                var dur = _spinAnimationParameter.LoopRotationDuration * (spinCount + angleValue);
                return dur;
            }
        }

        public void SetSpinBlurActive(bool isActive)
        {
            var blurValue = isActive
                ? _spinAnimationParameter.EnabledBlurValue
                : _spinAnimationParameter.DisabledBlurValue;
            _blurTween?.Kill();
            var from = _spinBaseImage.material.GetFloat(SpinBlurValueFloatName);
            _blurTween = DOVirtual.Float(from, blurValue, _spinAnimationParameter.BlurChangeDuration,
                OnBlurValueChanged);
        }

        private void OnBlurValueChanged(float value)
        {
            _spinBaseImage.material.SetFloat(SpinBlurValueFloatName, value);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            var images = GetComponentsInChildren<Image>(true);
            foreach (var image in images)
            {
                if (image.transform.name == SpinBaseObjectName)
                {
                    _spinBaseImage = image;
                }
            }

            var parent = transform.Find(SpinParentObjectName);
            if (parent)
            {
                _spinParentTf = parent;
            }

            var slots = GetComponentsInChildren<CardGameSpinSlotView>(true);
            _spinSlotViewList = slots.ToList();
            UpdateSlotIndex();
        }

        private void UpdateSlotIndex()
        {
            for (int i = 0; i < _spinSlotViewList.Count; i++)
            {
                _spinSlotViewList[i].SetSlotIndex(i);
            }
        }
#endif
    }
}