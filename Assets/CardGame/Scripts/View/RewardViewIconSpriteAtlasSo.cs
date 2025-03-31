using System;
using System.Collections.Generic;
using Main.Scripts.Utilities;
using UnityEngine;
using UnityEngine.U2D;

namespace CardGame.View
{
    public interface IRewardViewIconSpriteCache
    {
        Sprite GetIconSpriteById(string id);
    }

    [CreateAssetMenu(fileName = "RewardViewIconSpriteAtlasSo", menuName = "SO/View/RewardViewIconSpriteAtlasSo", order = 0)]
    public class RewardViewIconSpriteAtlasSo : ScriptableObject, IRewardViewIconSpriteCache
    {
        public SpriteAtlas IconSpriteAtlas;

        public Sprite GetIconSpriteById(string id)
        {
            var value = CardGameConstants.RewardViewModelIconPrefix + id;
            var sprite = IconSpriteAtlas.GetSprite(value);
            if (sprite == null)
            {
                DebugLogger.LogError($"sprite {value} not found");
            }
            return sprite;
        }
    }
}