using System.Collections.Generic;
using CardGame.View;

namespace CardGame.Controller
{
    public interface IInputListener
    {
        void OnDelta(InputData inputData);
        void OnFireInput();
    }

    public interface IInputController
    {
        void AddListener(IInputListener listener);
        void RemoveListener(IInputListener listener);
    }

    public class InputController : IInputController, IInputUpdater
    {
        private readonly List<IInputListener> _listeners = new();

        public void AddListener(IInputListener listener)
        {
            _listeners.Add(listener);
        }

        public void RemoveListener(IInputListener listener)
        {
            _listeners.Remove(listener);
        }

        public void OnDelta(InputData inputData)
        {
            throw new System.NotImplementedException();
        }

        public void OnFireButton()
        {
            throw new System.NotImplementedException();
        }
    }
}