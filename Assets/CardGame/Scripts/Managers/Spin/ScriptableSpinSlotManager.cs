using CardGame.View.Spin;
using Main.Scripts.ScriptableSingleton;
using UnityEngine;

namespace CardGame.Managers.Spin
{
    [CreateAssetMenu(fileName = "ScriptableSpinSlotManager", menuName = "SO/Manager/ScriptableSpinSlotManager", order = 0)]
    public class ScriptableSpinSlotManager : ScriptableSingletonManager<ScriptableSpinSlotManager>
    {
        public CardGameSpinSlotView SpinSlotPrefab;
    }
}