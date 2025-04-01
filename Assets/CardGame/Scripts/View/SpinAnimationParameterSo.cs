using DG.Tweening;
using UnityEngine;

namespace CardGame.View
{
    [CreateAssetMenu(fileName = "SpinAnimationParameterSo", menuName = "Spin", order = 0)]
    public class SpinAnimationParameterSo : ScriptableObject
    {
        public float LoopRotationDuration = 1f;
        public float EnabledBlurValue = 4f;
        public float DisabledBlurValue = 0f;
        public float BlurChangeDuration = .2f;
        public AnimationCurve StopRotationEase = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        public int SpinCountBeforeStop = 2;
        public float LoopSpinVelocity;
    }
}