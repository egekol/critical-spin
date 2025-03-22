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
        [SerializeField] private List<CardGameSpinSlotView> _spinSlotViews;
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
                .SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        }

        [Button]
        public void StopRotateSpinOnLoop()
        {
            _loopTween?.Kill();
        }

        public void SetSpinBlurActive(bool isActive)
        {
            var blurValue = isActive ? 1 : 0f;
            _blurTween?.Kill();
            var from = _spinBaseImage.material.GetFloat(SpinBlurValueFloatName);
            _blurTween = DOVirtual.Float(from, blurValue, _spinAnimationParameter.BlurChangeDuration, OnBlurValueChanged);

        }

        private void OnBlurValueChanged(float value)
        {
            _spinBaseImage.material.SetFloat(SpinBlurValueFloatName, value);
        }

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
            _spinSlotViews = slots.ToList();
        }
    }
}