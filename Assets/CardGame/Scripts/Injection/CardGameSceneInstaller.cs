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
            Container.BindInterfacesTo<InputController>().AsSingle().NonLazy();
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