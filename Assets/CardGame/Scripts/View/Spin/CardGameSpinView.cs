using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardGame.Managers.Spin;
using CardGame.Model.Spin;
using CardGame.View.Spin.Animation;
using Cysharp.Threading.Tasks;
using Main.Scripts.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.View.Spin
{
    public class CardGameSpinView : MonoBehaviour
    {
        private const string SpinBaseObjectName = "ui_spin_base_value";
        private const string SpinSlotObjectName = "ui_spin_slot";
        [SerializeField] private Image _spinBaseImage;
        [SerializeField] private Image _spinIndicatorImage;
        [SerializeField] private Transform _spinSlotParent;

        [ReadOnly] private List<CardGameSpinSlotView> _spinSlotViewList = new();
        [SerializeField] private List<CardGameSpinSpriteDataSo> _cardGameSlotSpriteDataList;
        [SerializeField] private CardGameSpinAnimation _spinAnimation;
        [SerializeField] private RewardViewIconSpriteAtlasSo _rewardIconSpriteCache;
        private List<CardGameSpinSlotView> SpinSlotViewList => _spinSlotViewList;

        private void Awake()
        {
           InstantiateSlots();
        }
        
        private void InstantiateSlots()
        {
            _spinSlotViewList.Clear();
            var totalCount = CardGameConstants.TotalSlotCount;
            var prefab = ScriptableSpinSlotManager.Instance.SpinSlotPrefab;
            for (var i = 0; i < totalCount; i++)
            {
                var angle = GetStartingAngle(i, totalCount);
                var rotation = Quaternion.Euler(Vector3.forward * angle);
                var slot = Instantiate(prefab, transform.position, rotation, _spinSlotParent);
                slot.gameObject.name = $"{SpinSlotObjectName}_{i}";
                _spinSlotViewList.Add(slot);
            }
        }
        

        private int GetStartingAngle(int index, int totalCount)
        {
            var degree = 360 / totalCount;
            var angle = index * degree;
            var rad = angle % 180 * -1;
            if (angle >= 180)
            {
                rad += 180;
            }

            return rad; 
        }

        public void SetSpinSlots(CardGameZoneModel cardGameZoneModel)
        {
            var builder = StringHelper.CreateNewBuild();
            builder.Append("SetSpinSlots");
            for (var i = 0; i < _spinSlotViewList.Count; i++)
            {
                SetSlotView(cardGameZoneModel, i, builder);
            }
            DebugLogger.Log(builder.ToString());
        }

        private void SetSlotView(CardGameZoneModel cardGameZoneModel, int i, StringBuilder builder)
        {
            var slotModel = cardGameZoneModel.SlotModelList[i];
            if (slotModel.SlotType == SlotType.Bomb)
            {
                SetSlotViewAsBomb(_spinSlotViewList[i]);
                builder.Append(" - BOMB ");
                return;
            }

            var rewardModel = slotModel.CardGameRewardModel;
            SetSlotViewAsReward(_spinSlotViewList[i], rewardModel);
            builder.Append($" - {rewardModel.Value} x{rewardModel.Amount} ");
        }

        private void SetSlotViewAsBomb(CardGameSpinSlotView spinSlotView)
        {
            var bombId = CardGameConstants.SlotBombAtlasId;
            InitSlotViewSprite(spinSlotView, bombId,false);
        }

        private void SetSlotViewAsReward(CardGameSpinSlotView spinSlotView, CardGameRewardModel rewardModel)
        {
            InitSlotViewSprite(spinSlotView, rewardModel.Value,true);
            spinSlotView.SetSpinSlotAmount(rewardModel.Amount);
        }

        private void InitSlotViewSprite(CardGameSpinSlotView spinSlotView, string spriteId, bool hasText)
        {
            var icon = _rewardIconSpriteCache.GetIconSpriteById(spriteId);
            spinSlotView.SetSpinSlotImage(icon);
            spinSlotView.SetTextViewEnabled(hasText);
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