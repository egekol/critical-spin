using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace CardGame.View.Levels
{
    [CreateAssetMenu(fileName = "CardGameLevelsPopupDataSo", menuName = "SO/f", order = 0)]
    public class CardGameLevelsPopupDataSo : ScriptableObject
    {
        public Sprite MiddleImageSpriteNormalZone;
        public Sprite MiddleImageSpriteSafeZone;
        public Color MiddleTextColorNormalZone;
        [FormerlySerializedAs("MiddleTextColorSuperZone")] public Color MiddleTextColorSafeZone;
        public int LevelsVisibleNumberCount;
        public TextMeshProUGUI LevelNumberTextPrefab;
        public Transform NumberPositionTransformPrefab;
    }
}