using UniRx;

namespace Main.Scripts.ScriptableSingleton
{
    public class ScriptableSingletonManager<T> : ScriptableManagerBase where T : ScriptableSingletonManager<T>
    {
        public static T Instance;
        protected CompositeDisposable _compositeDisposable = null;

        public override void Initialize()
        {
             _compositeDisposable = new CompositeDisposable();
             Instance = this as T;
             base.Initialize();
        }

        public override void BeforeStart()
        {
            base.BeforeStart();
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Destroy()
        {
             base.Destroy();
             _compositeDisposable.Dispose();
        }
    }
}