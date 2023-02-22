using System;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.Services.Level;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using UnityEngine;
using Object = UnityEngine.Object;

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
            _services.RegisterSingle<IGameStateMachine>(_stateMachine);
            RegisterInputService();
            RegisterStaticDataService();
            RegisterAssetProvider();
            _services.RegisterSingle<IRandomService>(new RandomService());
            _services.RegisterSingle<IGameFactory>(new GameFactory(
                _services.Single<IAssetProvider>(),
                _services.Single<IStaticDataService>(),
                _services.Single<IRandomService>(),
                _services.Single<IPersistentProgressService>(),
                _services.Single<IGameStateMachine>()
            ));
          
            _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(
                _services.Single<IPersistentProgressService>(),
                _services.Single<IGameFactory>()));
            _services.RegisterSingle<ILevelGenerator>(new LevelGenerator(_services.Single<StaticDataService>(),_services.Single<IRandomService>()));
        }

        private void RegisterInputService()
        {
            _services.RegisterSingle<IInputService>( Object.Instantiate(new GameObject()).AddComponent<InputService>());
        }

        private void RegisterStaticDataService()
        {
            IStaticDataService staticData = new StaticDataService();
             staticData.Load();
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