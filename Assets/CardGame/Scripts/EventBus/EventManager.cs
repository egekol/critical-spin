using System;
using System.Collections.Generic;
using Zenject;

namespace CardGame.Scripts.EventBus
{
    public interface ISpinEventListener
    {
        void OnSpinButtonClicked();
    }


    public class EventManager : IInitializable, IDisposable
    {
        [Inject] private SignalBus _signalBus;
        
        private  readonly HashSet<ISpinEventListener> _listeners = new();

        public void Initialize()
        {
            _signalBus.Subscribe<SpinButtonClickSignal>(OnSpinButtonClicked);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<SpinButtonClickSignal>(OnSpinButtonClicked);
        }

        private void OnSpinButtonClicked()
        {
            foreach (var eventListener in _listeners)
            {
                eventListener.OnSpinButtonClicked();
            }
        }

        public void SubscribeToSpinEvents(ISpinEventListener spinEventListener)
        {
            _listeners.Add(spinEventListener);
        }

        public void UnsubscribeToSpinEvents(ISpinEventListener spinEventListener)
        {
            if (_listeners.Contains(spinEventListener))
            {
                _listeners.Remove(spinEventListener);
            }
        }
    }
}