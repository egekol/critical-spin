namespace CardGame.Model
{
    public class CardGameSlotModel
    {
        public CardGameRewardModel CardGameRewardModel { get; set; }
        public int SlotIndex { get; set; }
    }

    public enum SlotType
    {
        None,
        Reward,
        Bomb,
    }
}