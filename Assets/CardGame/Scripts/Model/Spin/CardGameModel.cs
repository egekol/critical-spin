using System.Collections.Generic;

namespace CardGame.Model.Spin
{
    public class CardGameModel
    {
        public IReadOnlyList<CardGameZoneModel> ZoneModelList => _zoneModelList;
        private readonly List<CardGameZoneModel> _zoneModelList = new();

        public CardGameZoneModel CurrentZoneModel { get; private set; }
        public List<CardGameRewardModel> RewardPack { get; private set; } = new();

        public int CurrentZoneIndex { get; private set; }

        public void SetCurrentZoneModelFromList()
        {
            CurrentZoneModel = _zoneModelList[CurrentZoneIndex];
        }

        private void ClearZoneModelList()
        {
            _zoneModelList.Clear();
        }

        public void AddZoneToList(CardGameZoneModel zone)
        {
            _zoneModelList.Add(zone);
        }

        public void AddRewardToPack(CardGameRewardModel cardGameRewardModel)
        {
            RewardPack.Add(cardGameRewardModel);
        }

        private void ClearRewardPack()
        {
            RewardPack.Clear();
        }

        public void IncreaseCountIndex()
        {
            CurrentZoneIndex++;
        }

        public void Reset()
        {
            CurrentZoneIndex = 0;
            ClearZoneModelList();
            ClearRewardPack();
            CurrentZoneModel = null;
        }
    }


    public enum CardGameRewardRarity
    {
        Common,
        Uncommon,
        Rare,
        Legendary,
        Epic,
    }
}