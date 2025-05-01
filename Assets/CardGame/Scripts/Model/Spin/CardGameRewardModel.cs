namespace CardGame.Model.Spin
{
    public class CardGameRewardModel //todo create base reward model to main layer
    {
        public CardGameRewardType CardGameRewardType { get; set; }
        public ushort Amount { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return $"{CardGameRewardType}: {Amount} {Value}";
        }
    }

    public enum CardGameRewardType
    {
        Coin,
        Chest,
        GunPoint,
        Skin
    }
}