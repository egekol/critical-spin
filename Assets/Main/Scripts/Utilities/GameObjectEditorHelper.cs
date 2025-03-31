using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.Utilities
{
    public class GameObjectEditorHelper : MonoBehaviour
    {
        [SerializeField] private List<Transform> _objectList;


        [Button]
        public void NameObjectsWithOrderNumber(string objectName)
        {
            for (int i = 0; i < _objectList.Count; i++)
            {
                if (i>0)
                {
                    _objectList[i].name = objectName+$"_{i}";
                } 
            }
        }
        [Button]
        public void RotateObjectsWithOrderNumber(float angle)
        {
            for (int i = 0; i < _objectList.Count; i++)
            {
                _objectList[i].rotation = Quaternion.Euler(0f, 0f, angle * i);
            }
        }
    }
}
