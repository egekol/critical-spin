using System;

namespace CardGame.Model
{
    [Serializable]
    public class RewardData
    {
        public RewardType RewardType;
        public ushort Amount;
        public string Value;
    }
}