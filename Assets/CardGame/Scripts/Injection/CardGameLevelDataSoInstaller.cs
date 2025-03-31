using CardGame.Scripts.Network;
using UnityEngine;
using Zenject;

namespace CardGame.Injection
{
    [CreateAssetMenu(fileName = "CardGameLevelDataSoInstaller", menuName = "SO/CardGameLevelDataSoInstaller",
        order = 0)]
    public class CardGameLevelDataSoInstaller : ScriptableObjectInstaller<CardGameLevelDataSoInstaller>
    {
        [SerializeField] private CardGameLevelDataTransferSo cardGameLevelDataSo;

        public override void InstallBindings()
        {
            Container.Bind<CardGameLevelDataTransferSo>().FromInstance(cardGameLevelDataSo).AsSingle();
        }
    }
}