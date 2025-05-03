using CardGame.View.Spin;
using Main.Scripts.SingletonSO;

namespace CardGame.Scripts.Managers.Spin
{
    public class ScriptableSpinSlotManager : ScriptableSingletonManager<ScriptableSpinSlotManager>
    {
        public CardGameSpinSlotView SpinSlotPrefab;
    }
}