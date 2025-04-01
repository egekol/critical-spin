using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CardGame.Model;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Main.Scripts.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CardGame.View
{
    public interface ICardGameSpinView
    {
    }

    public class CardGameSpinView : MonoBehaviour, ICardGameSpinView
    {
        [SerializeField] private Image _spinBaseImage;
        [SerializeField] private Image _spinIndicatorImage;
        [SerializeField] private Transform _spinParentTf;
        [SerializeField] private SpinAnimationParameterSo _spinAnimationParameter;
        [SerializeField] private List<CardGameSpinSlotView> _spinSlotViewList;
        [SerializeField] private List<CardGameSlotSpriteData> _cardGameSlotSpriteDataList;
        [Inject] private readonly IRewardViewIconSpriteCache _rewardIconSpriteCache; 
        private Tween _loopTween;
        private Tween _blurTween;
        private bool _isRotating;
        private float _spinVelocity;
        private float _spinRotationValue;
        private Tween _stopTween;
        private StringBuilder _sb = new StringBuilder();

        private const string SpinBaseObjectName = "ui_spin_base_value";
        private const string SpinParentObjectName = "ui_spin_parent";
        private const string SpinBlurValueFloatName = "_BlurSize";


        [Button]
        public void StartRotateSpinOnLoop()
        {
            SetSpinning(true);
            _spinVelocity = _spinAnimationParameter.LoopSpinVelocity;
        }

        private void SetSpinning(bool isSpinning)
        {
            _isRotating = isSpinning;
            foreach (var slotView in _spinSlotViewList)
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
            var currentRotation = _spinParentTf.transform.rotation.eulerAngles;
            var targetRotation = currentRotation + Vector3.forward * totalRotation;

            StopSpinAnimationAsync(currentRotation, targetRotation, rotationDuration).Forget();
            Debug.Log($"last :: {targetRotation}");
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
            var from = _spinBaseImage.material.GetFloat(SpinBlurValueFloatName);
            _blurTween = DOVirtual.Float(from, blurValue, _spinAnimationParameter.BlurChangeDuration,
                OnBlurValueChanged);
        }

        private void OnBlurValueChanged(float value)
        {
            _spinBaseImage.material.SetFloat(SpinBlurValueFloatName, value);
        }

        public void SetSpinSlots(CardGameZoneModel cardGameZoneModel)
        {
            _sb.Clear();
            _sb.Append("SetSpinSlots");
            for (int i = 0; i < _spinSlotViewList.Count; i++)
            {
                var slotModel = cardGameZoneModel.SlotModelList[i];
                if (slotModel.SlotType == SlotType.Bomb)
                {
                    SetSlotViewAsBomb(_spinSlotViewList[i]);
                    _sb.Append($" - BOMB ");
                    continue;
                }
                
                var rewardModel = slotModel.CardGameRewardModel;
                SetSlotViewAsReward(_spinSlotViewList[i], rewardModel);
                _sb.Append($" - {rewardModel.Value} x{rewardModel.Amount} ");
            }
            DebugLogger.Log(_sb.ToString());
        }

        private void SetSlotViewAsBomb(CardGameSpinSlotView spinSlotView)
        {
            var bombId = CardGameConstants.SlotBombAtlasId;
            var icon = _rewardIconSpriteCache.GetIconSpriteById(bombId);
            spinSlotView.SetSpinSlotImage(icon);
            spinSlotView.SetTextViewEnabled(false);
        }

        private void SetSlotViewAsReward(CardGameSpinSlotView spinSlotView, CardGameRewardModel rewardModel)
        {
            var icon = _rewardIconSpriteCache.GetIconSpriteById(rewardModel.Value);
            spinSlotView.SetSpinSlotImage(icon);
            spinSlotView.SetTextViewEnabled(true);
            spinSlotView.SetSpinSlotAmount(rewardModel.Amount);
        }

        public void SetSpinView(ZoneType zoneType)
        {
            var spriteData = _cardGameSlotSpriteDataList.FirstOrDefault(d => d.ZoneType == zoneType);
            if (spriteData is null)
            {
                DebugLogger.LogError($"Can not find zone type {zoneType}");
                return;
            }

            _spinBaseImage.sprite = spriteData.SlotBase;
            _spinIndicatorImage.sprite = spriteData.SlotIndicator;
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