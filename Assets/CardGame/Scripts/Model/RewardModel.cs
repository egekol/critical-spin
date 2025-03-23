namespace CardGame.Model
{
    public class RewardModel
    {
        public RewardType RewardType { get; set; }
        public ushort Amount { get; set; }
        public string Value { get; set; }
    }

    public enum RewardType
    {
        None,
        Coin,
        Chest,
        GunPoint,
        Skin
    }
}