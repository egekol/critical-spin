using System.Collections.Generic;
using System.Linq;
using CardGame.EventBus;
using CardGame.EventBus.Spin;
using CardGame.Managers;
using CardGame.Model.Spin;
using Main.Scripts.Utilities;
using UniRx;
using UnityEngine;

namespace CardGame.View.Inventory
{
    public class CardGameInventoryView : MonoBehaviour
    {
        [SerializeField] private Transform _contentParent;
        [SerializeField] private RewardViewIconSpriteAtlasSo _rewardAtlas;
        private List<CardGameInventoryItem> _itemList = new();
        private CompositeDisposable _compositeDisposable;

        private void Awake()
        {
            _compositeDisposable = new CompositeDisposable();

            MessageBroker.Default.Receive<RewardGainedSignal>().Subscribe(OnRewardGained).AddTo(_compositeDisposable);
            MessageBroker.Default.Receive<SpinRestartSignal>().Subscribe(OnSpinRestarted).AddTo(_compositeDisposable);
            _itemList.Clear();
        }

        private void OnSpinRestarted(SpinRestartSignal signal)
        {
            Reset();
        }
        
        private void Reset()
        {
            foreach (var item in _itemList)
            {
                Destroy(item.gameObject);
            }

            _itemList.Clear();
        }

        private void OnRewardGained(RewardGainedSignal rewardGainedSignal)
        {
            if (rewardGainedSignal.IsNewReward)
            {
                CreateItem(rewardGainedSignal.Model.Value);
            }

            UpdateItem(rewardGainedSignal.Model);
        }

        public void CreateItem(string value)
        {
            var prefab = CardGameInventoryManager.Instance.ItemPrefab;
            var item = Instantiate(prefab, _contentParent);
            item.SetImage(_rewardAtlas.GetIconSpriteById(value));
            item.Value = value;
            _itemList.Add(item);
        }

        public void UpdateItem(CardGameRewardModel model)
        {
            var item = _itemList.FirstOrDefault(t => t.Value == model.Value);
            if (item is null)
            {
                DebugLogger.LogError($"Item with value {model.Value} not found");
                return;
            }

            item.transform.SetAsFirstSibling();
            item.UpdateAmount(model.Amount);
        }
    }
}