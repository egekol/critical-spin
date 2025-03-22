using UnityEngine;

namespace CardGame.View
{
    [CreateAssetMenu(fileName = "SpinAnimationParameterSo", menuName = "Spin", order = 0)]
    public class SpinAnimationParameterSo : ScriptableObject
    {
        public float LoopRotationDuration = 1f;
        public float EnabledBlurValue = 1f;
        public float DisabledBlurValue = 0f;
        public float BlurChangeDuration = .2f;
    }
}