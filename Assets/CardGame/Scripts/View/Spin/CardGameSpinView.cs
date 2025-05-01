using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardGame.Model.Spin;
using CardGame.View.Spin.Animation;
using Cysharp.Threading.Tasks;
using Main.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CardGame.View.Spin
{
    public class CardGameSpinView : MonoBehaviour
    {
        private const string SpinBaseObjectName = "ui_spin_base_value";
        [SerializeField] private Image _spinBaseImage;
        [SerializeField] private Image _spinIndicatorImage;
        [SerializeField] private List<CardGameSpinSlotView> _spinSlotViewList;
        [SerializeField] private List<CardGameSpinSpriteDataSo> _cardGameSlotSpriteDataList;
        [SerializeField] private CardGameSpinAnimation _spinAnimation;
        [Inject] private readonly IRewardViewIconSpriteCache _rewardIconSpriteCache;
        private readonly StringBuilder _sb = new();
        private List<CardGameSpinSlotView> SpinSlotViewList => _spinSlotViewList;

        public void SetSpinSlots(CardGameZoneModel cardGameZoneModel)
        {
            _sb.Clear();
            _sb.Append("SetSpinSlots");
            for (var i = 0; i < _spinSlotViewList.Count; i++)
            {
                var slotModel = cardGameZoneModel.SlotModelList[i];
                if (slotModel.SlotType == SlotType.Bomb)
                {
                    SetSlotViewAsBomb(_spinSlotViewList[i]);
                    _sb.Append(" - BOMB ");
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

        public void StartRotateSpinOnLoop()
        {
            _spinAnimation.StartRotateSpinOnLoop();
            foreach (var slotView in SpinSlotViewList) slotView.SetSpinning(true);
        }

        public UniTask StopSpinRotationAt(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= SpinSlotViewList.Count)
            {
                DebugLogger.LogError($"spin index {slotIndex} is out of range");
                return UniTask.CompletedTask;
            }

            _spinAnimation.StopRotateSpinOnLoop();

            var slotView = SpinSlotViewList[slotIndex];
            return _spinAnimation.StopSpinRotationAt(slotView.transform.up);
        }

        public void SetBlurActive(bool isActive)
        {
            _spinAnimation.SetSpinBlurActive(isActive);
        }

        public UniTask StartClickAnimation()
        {
            return _spinAnimation.StartClickAnimation();
        }

        public UniTask PlayFailAnimation()
        {
            return _spinAnimation.PlayShakeAnimation();
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            var images = GetComponentsInChildren<Image>(true);
            foreach (var image in images)
                if (image.transform.name == SpinBaseObjectName)
                    _spinBaseImage = image;

            var slots = GetComponentsInChildren<CardGameSpinSlotView>(true);
            _spinSlotViewList = slots.ToList();
            UpdateSlotIndex();
        }

        private void UpdateSlotIndex()
        {
            for (var i = 0; i < _spinSlotViewList.Count; i++) _spinSlotViewList[i].SetSlotIndex(i);
        }
#endif
    }
}