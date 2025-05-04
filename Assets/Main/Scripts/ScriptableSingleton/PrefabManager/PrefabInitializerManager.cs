using Main.Scripts.ScriptableSingleton;
using UnityEngine;

namespace Main.Scripts.PrefabManager
{
    public class PrefabInitializerManager : ScriptableSingletonManager<PrefabInitializerManager>
    {
                
        public T InstantiatePrefabInScene<T>(T prefab) where T : MonoBehaviour
        {
            var obj = Instantiate(prefab);
            return obj;
        }
    }
}