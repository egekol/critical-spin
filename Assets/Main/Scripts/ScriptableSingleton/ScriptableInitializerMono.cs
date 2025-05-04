using System.Collections.Generic;
using UnityEngine;

namespace Main.Scripts.ScriptableSingleton
{
    [DefaultExecutionOrder(-100)]
    public class ScriptableInitializerMono : InitializerBase
    {
        [SerializeField] private ScriptableManagerBase[] _abstractScriptableManagerArray;
        private List<ScriptableManagerBase> _instantiatedAbstractScriptableManagerList;
   

        protected override void Initialize()
        {
            _instantiatedAbstractScriptableManagerList = new List<ScriptableManagerBase>(_abstractScriptableManagerArray.Length);
            for (int i = 0; i < _abstractScriptableManagerArray.Length; i++)
            {
                var instantiated = Instantiate(_abstractScriptableManagerArray[i]);
                instantiated.Initialize();
                _instantiatedAbstractScriptableManagerList.Add(instantiated);
            }
        }

        protected override void LateAwake()
        {
            foreach (var manager in _instantiatedAbstractScriptableManagerList)
            {
                manager.LateAwake();
            }
        }

        private void Start()
        {
            foreach (var manager in _instantiatedAbstractScriptableManagerList)
            {
                manager.Start();
            }
        }

        private void OnDestroy()
        {
            if (_instantiatedAbstractScriptableManagerList != null)
            {
                for (int i = 0; i < _instantiatedAbstractScriptableManagerList.Count; i++)
                {
                    _instantiatedAbstractScriptableManagerList[i].Destroy();
                }
            }
        }
    }
}