using CardGame.View.Levels;
using CardGame.View.Spin;
using Main.Scripts.ScriptableSingleton;
using Main.Scripts.ScriptableSingleton.PrefabManager;
using UnityEngine;
namespace CardGame.Managers.Spin
{
    [CreateAssetMenu(fileName = "ScriptableSpinSlotManager", menuName = "SO/Manager/ScriptableSpinSlotManager", order = 0)]
    public class ScriptableSpinSlotManager : ScriptableSingletonManager<ScriptableSpinSlotManager>
    {
        public CardGameSpinSlotView SpinSlotPrefab;
        public CardGameLevelsUIDataSo levelsUIDataSo;
        public CardGameSpinView SpinPrefab;

        public bool IsInSpinState { get; private set; }

        public CardGameSpinView InstantiateSpinPrefab(Transform transformParent)
        {
             return PrefabInitializerManager.Instance.InstantiatePrefabInScene(SpinPrefab, transformParent);
        }

        public void SetSpinState(bool isActive)
        {
            IsInSpinState = isActive;
        }
    }
}