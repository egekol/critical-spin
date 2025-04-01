using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Main.Scripts.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardGame.View
{
    public class CardGameSpinAnimation : MonoBehaviour
    {
        [SerializeField] private SpinAnimationParameterSo _spinAnimationParameter;
        [SerializeField] private CardGameSpinView _cardGameSpinView;
        [SerializeField] private Transform _spinParentTf;
        private Tween _blurTween;
        private bool _isRotating;
        private Tween _loopTween;
        private float _spinVelocity;
        private float _spinRotationValue;
        private Tween _stopTween;
        private const string SpinBlurValueFloatName = "_BlurSize";
        private const string SpinParentObjectName = "ui_spin_parent";

        [Button]
        public void StartRotateSpinOnLoop()
        {
            SetSpinning(true);
            _spinVelocity = _spinAnimationParameter.LoopSpinVelocity;
        }

        private void SetSpinning(bool isSpinning)
        {
            _isRotating = isSpinning;
            foreach (var slotView in _cardGameSpinView.SpinSlotViewList)
            {
                slotView.SetSpinning(isSpinning);
            }
        }

        [Button]
        public void StopRotateSpinOnLoop()
        {
            _isRotating = false;
        }

        private void Update()
        {
            if (!_isRotating)
            {
                return;
            }

            if (_spinRotationValue>360f)
            {
                _spinRotationValue -= 360f;
            }
            Rotate(_spinVelocity * Time.deltaTime);
        }

        private void Rotate(float value)
        {
            _spinRotationValue += value;
            SetRotation(_spinRotationValue);
        }

        private void SetRotation(float rotationValue)
        {
            _spinParentTf.rotation = Quaternion.Euler(Vector3.forward * rotationValue);
        }

        [Button]
        public UniTask StopSpinRotationAt(int spinIndex)
        {
            if (spinIndex < 0 || spinIndex >= _cardGameSpinView.SpinSlotViewList.Count)
            {
                DebugLogger.LogError($"spin index {spinIndex} is out of range");
                return UniTask.CompletedTask;
            }

            StopRotateSpinOnLoop();

            var spinCountBeforeStop = _spinAnimationParameter.SpinCountBeforeStop;
            var slotView = _cardGameSpinView.SpinSlotViewList[spinIndex];
            var angle = CalculateAngleOfSlot(slotView);
            var rotationAngle = spinCountBeforeStop * 360f;
            var totalRotation = rotationAngle + angle;

            var rotationDuration = CalculateRotationDuration(angle, spinCountBeforeStop);
            var currentRotation = _spinParentTf.transform.rotation.eulerAngles;
            var targetRotation = currentRotation + Vector3.forward * totalRotation;

            var spinTask = StopSpinAnimationAsync(currentRotation, targetRotation, rotationDuration);
            Debug.Log($"last :: {targetRotation}");
            return spinTask;

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

        private async UniTask StopSpinAnimationAsync(Vector3 currentRotation, Vector3 targetRotation, float rotationDuration, CancellationTokenSource cts = null)
        {
            float elapsed = 0;
            
            while (elapsed < rotationDuration)
            {
                elapsed += Time.deltaTime;

                var lerp = elapsed / rotationDuration;
                Debug.Log($"lerp  {lerp}");
                var inverseLerp = Mathf.LerpUnclamped(currentRotation.z, targetRotation.z,
                    _spinAnimationParameter.StopRotationEase.Evaluate(lerp));
                var reelVelocity = inverseLerp - _spinRotationValue;

                Debug.Log($"inverseLerp : {inverseLerp}");
                Debug.Log($"vel : {reelVelocity} = {inverseLerp} - {_spinRotationValue}");
                Rotate(reelVelocity);

                await UniTask.WaitForSeconds(Time.deltaTime);
            }
        }

        public void SetSpinBlurActive(bool isActive)
        {
            var blurValue = isActive
                ? _spinAnimationParameter.EnabledBlurValue
                : _spinAnimationParameter.DisabledBlurValue;
            _blurTween?.Kill();
            var from = _cardGameSpinView.SpinMaterial.GetFloat(SpinBlurValueFloatName);
            _blurTween = DOVirtual.Float(from, blurValue, _spinAnimationParameter.BlurChangeDuration,
                OnBlurValueChanged);
        }

        private void OnBlurValueChanged(float value)
        {
            _cardGameSpinView.SpinMaterial.SetFloat(SpinBlurValueFloatName, value);
        }

        private void OnValidate()
        {
            
            var parent = transform.Find(SpinParentObjectName);
            if (parent)
            {
                _spinParentTf = parent;
            }

        }
    }
}