using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Model
{
    [CreateAssetMenu(fileName = "CoreGameLevelDataSo", menuName = "SO/Core/CoreGameLevelData", order = 0)]
    public class CardGameLevelDataSo : ScriptableObject
    {
        [SerializeField] public List<MultiplierData> MultiplierModelList;
        [SerializeField] public float MobAttackDuration;

    }

    [Serializable]
    public class MultiplierData
    {
        public int TeamId;
        public int Id;
        public float multiplierCount;
    }

}