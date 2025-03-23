using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Model
{
    [CreateAssetMenu(fileName = "CardGameLevelDtoSo", menuName = "SO/CardGameLevelDtoSo", order = 0)]
    public class CardGameLevelDtoSo : ScriptableObject
    {
        public List<SpinZoneDto> SpinZoneList;
    }

    [Serializable]
    public class SpinZoneDto
    {
        public List<RewardDto> RewardList;
    }

    [Serializable]
    public class RewardDto
    {
        public RewardData RewardData;
        public int RewardProbability;
    }

}