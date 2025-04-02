using System.IO;
using Main.Scripts.Utilities;
using UnityEditor;
using UnityEngine;

namespace CardGame.Scripts.Network
{
    [CreateAssetMenu(fileName = "RewardViewDataTransferSo", menuName = "SO/RewardViewDataTransferSo", order = 0)]
    public class RewardViewDataTransferSo : ScriptableObject
    {
        private const string RewardViewModelIconPathRoot = "Assets/CardGame/Sprites/Icons";
        public RewardType Type;
        public string Value;
        public Sprite Icon;
#if UNITY_EDITOR

        private void OnValidate()
        {
            if (!name.StartsWith(CardGameConstants.RewardViewModelPrefix))
            {
                Debug.LogError($"SO name must include the prefix : {CardGameConstants.RewardViewModelPrefix}");
                return;
            }

            Value = name.Substring(CardGameConstants.RewardViewModelPrefix.Length);
            var iconPath = Path.Combine(RewardViewModelIconPathRoot,
                CardGameConstants.RewardViewModelIconPrefix + Value + ".png");
            var icon = AssetDatabase.LoadAssetAtPath<Sprite>(iconPath);
            if (icon != null) Icon = icon;
        }
#endif
    }
}