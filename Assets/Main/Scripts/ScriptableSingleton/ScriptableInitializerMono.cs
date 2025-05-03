using System.Collections.Generic;
using UnityEngine;

namespace Main.Scripts.SingletonSO
{
    public class ScriptableInitializerMono : MonoBehaviour
    {
        [SerializeField] private ScriptableManagerBase[] _abstractScriptableManagerArray;
        private List<ScriptableManagerBase> _instantiatedAbstractScriptableManagerList;
        
        private void Start()
        {
            _instantiatedAbstractScriptableManagerList = new List<ScriptableManagerBase>(_abstractScriptableManagerArray.Length);
            for (int i = 0; i < _abstractScriptableManagerArray.Length; i++)
            {
                var instantiated = Instantiate(_abstractScriptableManagerArray[i]);
                instantiated.Initialize();
                _instantiatedAbstractScriptableManagerList.Add(instantiated);
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