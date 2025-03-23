using CardGame.Model;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace CardGame.Injection
{
    [CreateAssetMenu(fileName = "CoreGameLevelDataSoInstaller", menuName = "SO/CoreGameLevelDataInstaller",
        order = 0)]
    public class CardGameLevelDataSoInstaller : ScriptableObjectInstaller<CardGameLevelDataSoInstaller>
    {
        [SerializeField] private CardGameLevelDtoSo cardGameLevelDataSo;

        public override void InstallBindings()
        {
            Container.Bind<CardGameLevelDtoSo>().FromInstance(cardGameLevelDataSo).AsSingle();
        }
    }
}