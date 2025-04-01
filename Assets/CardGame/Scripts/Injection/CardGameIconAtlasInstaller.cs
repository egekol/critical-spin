using CardGame.View;
using UnityEngine;
using Zenject;

namespace CardGame.Injection
{
    [CreateAssetMenu(fileName = "CardGameIconSpriteAtlasCacheInstaller", menuName = "SO/CardGameIconSpriteAtlasCacheInstaller", order = 0)]
    public class CardGameIconAtlasInstaller : ScriptableObjectInstaller<CardGameLevelDataSoInstaller>
    {
        [SerializeField] private RewardViewIconSpriteAtlasSo cardGameLevelDataSo;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<RewardViewIconSpriteAtlasSo>().FromInstance(cardGameLevelDataSo).AsSingle();
        }
    }
}