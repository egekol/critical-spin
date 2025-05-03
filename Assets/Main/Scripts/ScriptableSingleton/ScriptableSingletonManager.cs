namespace Main.Scripts.ScriptableSingleton
{
    public class ScriptableSingletonManager<T> : ScriptableManagerBase where T : ScriptableSingletonManager<T>
    {
        public static T Instance;

        public override void Initialize()
        {
             Instance = this as T;
             base.Initialize();
        }

        public override void LateAwake()
        {
            base.LateAwake();
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Destroy()
        {
             base.Destroy();
        }
    }
}