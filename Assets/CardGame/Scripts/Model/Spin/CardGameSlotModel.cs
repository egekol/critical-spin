namespace CardGame.Model.Spin
{
    public class CardGameSlotModel
    {
        public CardGameRewardModel CardGameRewardModel { get; private set; }
        public SlotType SlotType { get; private set; }
        public int SlotIndex { get; private set; }

        public CardGameSlotModel(SlotType slotType, int slotIndex, CardGameRewardModel cardGameRewardModel)
        {
            SlotType = slotType;
            SlotIndex = slotIndex;
            CardGameRewardModel = cardGameRewardModel;
        }

        public override string ToString()
        {
            return $"SlotType: {SlotType}, SlotIndex: {SlotIndex}, Reward: {CardGameRewardModel}";
        }
    }

    public enum SlotType
    {
        None,
        Reward,
        Bomb,
    }
}