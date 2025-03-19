using UnityEngine;
using Zenject;

namespace CardGame.View
{
    public struct InputData
    {
        public readonly Vector2 ScreenDelta;
        public readonly bool IsActive;

        public InputData(Vector2 screenDelta, bool isActive)
        {
            ScreenDelta = screenDelta;
            IsActive = isActive;
        }

        public override string ToString()
        {
            return $"InputData: ScreenDelta: {ScreenDelta}, IsActive: {IsActive}";
        }
    }

    public interface IInputUpdater
    {
        void OnDelta(InputData inputData);
        void OnFireButton();
    }

    public class InputAdapter : MonoBehaviour
    {
        // [SerializeField] private LeanMultiUpdate _leanTouch;
        [Inject] private readonly IInputUpdater _inputUpdater;

        public void OnEnable()
        {
            // _leanTouch.OnDelta.AddListener(OnFingerUpdate);
        }

        private void OnDisable()
        {
            // _leanTouch.OnDelta.RemoveListener(OnFingerUpdate);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                _inputUpdater.OnFireButton();
            }
        }

        private void OnFingerUpdate(Vector2 arg0)
        {
            _inputUpdater.OnDelta(new InputData(arg0, true));
        }
    }
}