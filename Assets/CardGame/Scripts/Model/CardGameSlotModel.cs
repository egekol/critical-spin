namespace CardGame.Model
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
    }

    public enum SlotType
    {
        None,
        Reward,
        Bomb,
    }
}