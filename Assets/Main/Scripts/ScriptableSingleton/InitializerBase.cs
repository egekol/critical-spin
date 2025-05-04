using System;
using UnityEngine;

namespace Main.Scripts.ScriptableSingleton
{
    public abstract class InitializerBase : MonoBehaviour
    {
        private void Awake()
        {
            Initialize();

            LateAwake();
        }

        protected abstract void Initialize();
        
        protected abstract void LateAwake();
    }
}