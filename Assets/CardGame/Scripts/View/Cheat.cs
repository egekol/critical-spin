using CardGame.Model;
using CardGame.Model.Spin;
using UnityEngine;
using Zenject;

namespace CardGame.View
{
    public interface ICheatDelegate
    {
        void Animate();
    }

    public class Cheat : MonoBehaviour
    {
#if UNITY_EDITOR

        [Inject] private readonly ICheatDelegate _cheatDelegate;
        [Inject] private readonly CardGameModel _cardGameModel;

        private void Update()
        {
          
            if (_cheatDelegate is null)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                _cheatDelegate.Animate();
            }
        }
#endif
    }
}