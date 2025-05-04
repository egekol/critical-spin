using System.Collections.Generic;
using UnityEngine;

namespace Main.Scripts.ScriptableSingleton.PrefabManager
{
    public class PrefabInitializerMono : InitializerBase
    {
        private PrefabInitializerManager _prefabInitializerManager;
        [SerializeField] private MonoBehaviour[] _prefabArray;
        [SerializeField] private Transform _transformParent;
        private List<MonoBehaviour> _prefabList;

        protected override void Initialize()
        {
            _prefabInitializerManager = PrefabInitializerManager.Instance;
            _prefabInitializerManager.SetTransformParent(_transformParent);
            _prefabList = new List<MonoBehaviour>(_prefabArray.Length);
            for (int i = 0; i < _prefabArray.Length; i++)
            {
                var instantiated = _prefabInitializerManager.InstantiatePrefabInScene(_prefabArray[i]);
                _prefabList.Add(instantiated);
            }
        }

        protected override void LateAwake()
        {
        }
    }
}