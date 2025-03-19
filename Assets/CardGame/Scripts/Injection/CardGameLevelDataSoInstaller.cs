using CardGame.Model;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace CardGame.Injection
{
    [CreateAssetMenu(fileName = "CoreGameLevelDataSoInstaller", menuName = "SO/Core/CoreGameLevelDataInstaller",
        order = 0)]
    public class CardGameLevelDataSoInstaller : ScriptableObjectInstaller<CardGameLevelDataSoInstaller>
    {
        [SerializeField] private CardGameLevelDataSo cardGameLevelDataSo;

        public override void InstallBindings()
        {
            Container.Bind<CardGameLevelDataSo>().FromInstance(cardGameLevelDataSo).AsSingle();
        }
    }
}