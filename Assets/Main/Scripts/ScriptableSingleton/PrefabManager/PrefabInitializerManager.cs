using UnityEngine;

namespace Main.Scripts.ScriptableSingleton.PrefabManager
{
    [CreateAssetMenu(fileName = "PrefabInitializerManager", menuName = "SO/Manager/PrefabInitializerManager", order = 0)]
    public class PrefabInitializerManager : ScriptableSingletonManager<PrefabInitializerManager>
    {
        private Transform _transformParent;

        public T InstantiatePrefabInScene<T>(T prefab, Transform transformParent) where T : MonoBehaviour
        {
            var obj = Instantiate(prefab, transformParent);
            return obj;
        }
        public T InstantiatePrefabInScene<T>(T prefab) where T : MonoBehaviour
        {
            var obj = Instantiate(prefab, _transformParent);
            return obj;
        }

        public void SetTransformParent(Transform transformParent)
        {
            _transformParent = transformParent;
        }
    }
}