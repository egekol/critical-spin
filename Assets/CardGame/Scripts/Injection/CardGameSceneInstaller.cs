using CardGame.Controller;
using CardGame.Model;
using CardGame.Model.Spin;
using CardGame.Scripts.EventBus;
using CardGame.View;
using UnityEngine;
using Zenject;

namespace CardGame.Injection
{
    public class CardGameSceneInstaller : MonoInstaller
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private CardGameSceneView _cardGameSpinView;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<CardGameMainController>().AsSingle().NonLazy();
            Container.BindInterfacesTo<CardGameLevelGenerator>().AsSingle().NonLazy();
            Container.BindInterfacesTo<CardGameRarityCountCalculator>().AsSingle().NonLazy();
            Container.BindInterfacesTo<CardGameDataTransferController>().AsSingle().NonLazy();
            Container.BindInterfacesTo<CardGameSceneController>().AsSingle().NonLazy();
            Container.BindInterfacesTo<CardGameSceneView>().FromInstance(_cardGameSpinView).AsSingle();
            SignalBusInstaller.Install(Container);
            BindModels();
            BindSignals();
        }

        private void BindSignals()
        {
            Container.DeclareSignal<SpinButtonClickSignal>();
            Container.DeclareSignal<OnGiveUpButtonClickSignal>();
            Container.DeclareSignal<OnReviveButtonClickSignal>();
            Container.DeclareSignal<ExitButtonClickSignal>();
        }

        private void BindModels()
        {
            Container.Bind<CardGameModel>().AsSingle().NonLazy();
            Container.Bind<CardGameEventModel>().AsSingle().NonLazy();
            Container.Bind<PlayerModel>().AsSingle().NonLazy();
        }
    }
}