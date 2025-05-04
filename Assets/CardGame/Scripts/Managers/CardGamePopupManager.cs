using CardGame.View.Popup;
using Main.Scripts.ScriptableSingleton;
using Main.Scripts.ScriptableSingleton.PrefabManager;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardGame.Managers
{
    [CreateAssetMenu(fileName = "CardGamePopupManager", menuName = "SO/Manager/CardGamePopupManager", order = 0)]
    public class CardGamePopupManager : ScriptableSingletonManager<CardGamePopupManager>
    {
        [SerializeField] private CardGamePopupPanel _popupPanelPrefab;
        [SerializeField] private CardGameSpinButtonPopup _spinButtonPopupPrefab;
        [SerializeField] private CardGameFailPopup _failPopupPrefab;
        [SerializeField] private CardGameExitPanel _exitPanelPrefab;
        
        [ReadOnly] public CardGameSpinButtonPopup SpinButtonPopup;
        [ReadOnly] public CardGameFailPopup FailPopup;
        [ReadOnly] public CardGamePopupPanel PopupPanel;
        [ReadOnly] public CardGameExitPanel ExitPanel;

        public override void Initialize()
        {
            base.Initialize();
            PopupPanel = PrefabInitializerManager.Instance.InstantiatePrefabInScene(_popupPanelPrefab);
            SpinButtonPopup = PrefabInitializerManager.Instance.InstantiatePrefabInScene(_spinButtonPopupPrefab, PopupPanel.transform);
            FailPopup = PrefabInitializerManager.Instance.InstantiatePrefabInScene(_failPopupPrefab, PopupPanel.transform);
            ExitPanel = PrefabInitializerManager.Instance.InstantiatePrefabInScene(_exitPanelPrefab, PopupPanel.transform);
        }
    }
}