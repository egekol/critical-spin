using System.IO;
using UnityEditor;
using UnityEngine;

namespace CardGame.Scripts.Network
{
    [CreateAssetMenu(fileName = "RewardViewDataTransferSo", menuName = "SO/RewardViewDataTransferSo", order = 0)]
    public class RewardViewDataTransferSo : ScriptableObject
    {
        public RewardType Type;
        public string Value;
        public Sprite Icon;

        private const string RewardViewModelPrefix = "so_reward_";
        private const string RewardViewModelIconPrefix = "ui_icon_";
        private const string RewardViewModelIconPathRoot = "Assets/CardGame/Sprites/Icons";
        
        private void OnValidate()
        {
            if (!name.StartsWith(RewardViewModelPrefix))
            {
                Debug.LogError($"SO name must include the prefix : {RewardViewModelPrefix}");
                return;
            }
            Value = name.Substring(RewardViewModelPrefix.Length);
            var iconPath = Path.Combine(RewardViewModelIconPathRoot, RewardViewModelIconPrefix + Value + ".png");
            var icon = AssetDatabase.LoadAssetAtPath<Sprite>(iconPath);
            if (icon != null)
            {
                Icon = icon;
            }
        }

    }
}