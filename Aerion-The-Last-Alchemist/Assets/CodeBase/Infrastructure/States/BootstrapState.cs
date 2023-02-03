using System;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.StaticData;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string Initial = "Initial";
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;
        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices allServices)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = allServices;
            RegisterServices();
        }

        public void Enter() =>
            _sceneLoader.Load(Initial, onLoaded:EnterLoadLevel );

        public void Exit()
        {
        }

        private void RegisterServices()
        {
            RegisterStaticDataService();
            RegisterAssetProvider();
            _services.RegisterSingle<IGameFactory>(new GameFactory(
                _services.Single<IAssetProvider>(),
                _services.Single<IStaticDataService>(),
                _services.Single<IRandomService>(),
                _services.Single<IPersistentProgressService>(),
                _services.Single<IGameStateMachine>()
            ));
        }
        private void RegisterStaticDataService()
        {
            IStaticDataService staticData = new StaticDataService();
          //  staticData.Load();
            _services.RegisterSingle(staticData);
        }
        private void RegisterAssetProvider()
        {
            AssetProvider assetProvider = new AssetProvider();
            _services.RegisterSingle<IAssetProvider>(assetProvider);
            assetProvider.Initialize();
        }
        private void EnterLoadLevel() =>
            _stateMachine.Enter<LoadProgressState>();
    }
}