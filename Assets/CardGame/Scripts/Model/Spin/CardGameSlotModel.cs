namespace CardGame.Model.Spin
{
    public class CardGameSlotModel
    {
        public CardGameSlotModel(SlotType slotType, int slotIndex, CardGameRewardModel cardGameRewardModel)
        {
            SlotType = slotType;
            SlotIndex = slotIndex;
            CardGameRewardModel = cardGameRewardModel;
        }

        public CardGameRewardModel CardGameRewardModel { get; }
        public SlotType SlotType { get; }
        public int SlotIndex { get; }

        public override string ToString()
        {
            return $"SlotType: {SlotType}, SlotIndex: {SlotIndex}, Reward: {CardGameRewardModel}";
        }
    }

    public enum SlotType
    {
        None,
        Reward,
        Bomb
    }
}