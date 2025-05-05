using UnityEngine;

namespace CardGame.View.Popup
{
    public class Popup : MonoBehaviour
    {
        public virtual void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}