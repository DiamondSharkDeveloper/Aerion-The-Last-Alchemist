using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Creature;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Map;
using CodeBase.Services.Cursor;
using CodeBase.Services.Input;
using CodeBase.Services.Level;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
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
        private readonly IImageService _imageService;
        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
            IGameFactory gameFactory, IPersistentProgressService progressService, IStaticDataService staticDataService,
            ILevelGenerator levelGenerator, IInputService inputService,IImageService imageService)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _staticData = staticDataService;
            _levelGenerator = levelGenerator;
            _inputService = inputService;
            _imageService = imageService;
        }

        private async Task InitGameWorld()
        {
            LevelStaticData levelData = LevelStaticData();
            List<MyTile> mapCoordinates = _levelGenerator.GetMap(levelData);
            _imageService.SetDefaultCursor(levelData.cursor);
            
            GameObject cameraController = await _gameFactory.CreateCameraController();
            
            GameObject hudGameObject = await _gameFactory.CreateHud(data =>
            {
                _stateMachine.Enter<LabState, FormulaStaticData>(data);
            });
            HUD hud = hudGameObject.GetComponent<HUD>();
            StrategyCamera strategyCamera = cameraController.GetComponent<StrategyCamera>();
            _imageService.Init(hud);
            _inputService.SetCamera(strategyCamera.cam);
            _stateMachine.OnStateChange += state =>
            {
              strategyCamera.ChangeCameraActiveStatus(!state.IsOnPause(), 3);
                
                if (state.IsOnPause())
                {
                    hud.Hide();
                }
                else
                {
                    _inputService.SetCamera(strategyCamera.cam);
                    hud.Show();
                }
            };

            Hero.Hero hero = null;
            await _gameFactory.CreateMap(mapCoordinates,levelData, args =>
            {
                hero.Move(args);
            } );
             await _gameFactory.CreateHouse(mapCoordinates[levelData.housePosition],
                () => { _stateMachine.Enter<LabState, FormulaStaticData>(null); });

            GameObject heroGameObject = await _gameFactory.CreateHero(mapCoordinates[levelData.heroPosition]);
            if (heroGameObject.TryGetComponent(out hero))
            {
                hero.Construct();
            }
           
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