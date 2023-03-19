using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Map;
using CodeBase.Services.Input;
using CodeBase.Services.Level;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticData;
        private readonly ILevelGenerator _levelGenerator;
        private readonly IInputService _inputService;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
            IGameFactory gameFactory, IPersistentProgressService progressService, IStaticDataService staticDataService,
            ILevelGenerator levelGenerator, IInputService inputService)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _staticData = staticDataService;
            _levelGenerator = levelGenerator;
            _inputService = inputService;
        }

        private async Task InitGameWorld()
        {
            LevelStaticData levelData = LevelStaticData();
            List<MyTile> mapCoordinates = _levelGenerator.GetMap(levelData);
            GameObject cameraController = await _gameFactory.CreateCameraController();
            _stateMachine.OnStateChange += state =>
            {
                cameraController.GetComponent<StrategyCamera>().ChangeCameraActiveStatus(state.IsOnPause(),3);
            };
            await _gameFactory.CreateMap(mapCoordinates);
            GameObject lab = await _gameFactory.CreateHouse(mapCoordinates[levelData.housePosition],
                () => { _stateMachine.Enter<LabState,FormulaStaticData>(null); });


            GameObject heroGameObject = await _gameFactory.CreateHero(mapCoordinates[levelData.heroPosition]);
            if (heroGameObject.TryGetComponent(out Hero.Hero hero))
            {
                hero.Construct(_inputService);
            }

            

            await _gameFactory.CreateCreature(levelData.creatureTypeId, mapCoordinates[levelData.creaturePosition],
                () => { _stateMachine.Enter<CreatureState>(); });
            await _gameFactory.CreateHud(data =>
            {
                _stateMachine.Enter<LabState,FormulaStaticData>(data);
            } );
        }

        public void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            _gameFactory.Cleanup();
            _gameFactory.WarmUp();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit() =>
            _loadingCurtain.Hide();

        public bool IsOnPause()
        {
            return true;
        }

        private async void OnLoaded()
        {
            await InitGameWorld();
            _stateMachine.Enter<GameLoopState>();
        }

        private LevelStaticData LevelStaticData() =>
            _staticData.ForLevel("forest");
    }
}