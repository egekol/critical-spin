using System;
using System.IO;
using CardGame.Model;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace CardGame.View
{
    [CreateAssetMenu(fileName = "RewardViewModelSo", menuName = "SO/RewardViewModelSo", order = 0)]
    public class RewardViewModelSo : ScriptableObject
    {
        public RewardType Type;
        public string Value;
        public Sprite Icon;

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

        private const string RewardViewModelPrefix = "so_reward_";
        private const string RewardViewModelIconPrefix = "ui_icon_";
        private const string RewardViewModelIconPathRoot = "Assets/CardGame/Sprites/Icons";
    }
}