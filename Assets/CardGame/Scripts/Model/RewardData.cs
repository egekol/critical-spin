using System;
using UnityEngine.Serialization;

namespace CardGame.Model
{
    [Serializable]
    public class RewardData
    {
        public CardGameRewardType cardGameRewardType;
        public ushort Amount;
        public string Value;
    }
}