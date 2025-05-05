using System.Collections.Generic;
using System.Linq;
using CardGame.EventBus;
using CardGame.Model.Spin;
using CardGame.View.Inventory;
using Main.Scripts.ScriptableSingleton;
using Main.Scripts.Utilities;
using UniRx;
using UnityEngine;

namespace CardGame.Managers
{
    [CreateAssetMenu(fileName = "CardGameInventoryManager", menuName = "SO/Manager/CardGameInventoryManager", order = 0)]
    public class CardGameInventoryManager : ScriptableSingletonManager<CardGameInventoryManager>
    {
        public CardGameInventoryItem ItemPrefab;
        private List<CardGameRewardModel> _gainedRewardList = new();

        public override void Initialize()
        {
            base.Initialize();
            _gainedRewardList.Clear();
        }

        public void GetNewItem(CardGameRewardModel model)
        {
            var containedModel = _gainedRewardList.FirstOrDefault(r => r.Value == model.Value);
            if (containedModel is not null)
            {
                AddAmountToReward(containedModel,model.Amount);
                MessageBroker.Default.Publish(new RewardGainedSignal(containedModel,false));
            }
            else
            {
                _gainedRewardList.Add(model);
                MessageBroker.Default.Publish(new RewardGainedSignal(model, true));
            }
        }

        private void AddAmountToReward(CardGameRewardModel model, ushort modelAmount)
        {
            model.Amount += modelAmount;
        }
    }
}