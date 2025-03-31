using CardGame.Controller;
using CardGame.Model;
using UnityEngine;
using Zenject;

namespace CardGame.Injection
{
    public class CardGameSceneInstaller : MonoInstaller
    {
        [SerializeField] private Camera _camera;


        public override void InstallBindings()
        {
            Container.BindInterfacesTo<CardGameMainController>().AsSingle().NonLazy();
            Container.BindInterfacesTo<CardGameDataTransferController>().AsSingle().NonLazy();
            Container.BindInterfacesTo<CardGameSceneController>().AsSingle().NonLazy();
            BindPools();
            BindModels();
#if UNITY_EDITOR
            Container.BindInterfacesTo<CheatController>().AsSingle();
#endif
        }



        private void BindPools()
        {
        }

        private void BindModels()
        {
            Container.Bind<CardGameModel>().AsSingle().NonLazy();
        }
    }
}