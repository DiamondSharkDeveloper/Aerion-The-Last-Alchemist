using System.Threading.Tasks;
using Cinemachine;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Services.Input;
using CodeBase.Services.Level;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using UnityEngine;
using CodeBase.Hero;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState :IPayloadedState<string>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticData;
        private readonly ILevelGenerator _levelGenerator;
        private readonly IInputService _inputService;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, IGameFactory gameFactory, IPersistentProgressService progressService, IStaticDataService staticDataService,ILevelGenerator  levelGenerator,IInputService inputService)
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

            await _gameFactory.CreateMap(_levelGenerator.GetMap(levelData));
            GameObject heroGameObject=await _gameFactory.CreateHero(levelData.heroPositionTileX,levelData.heroPositionTileY);
            Hero.Hero hero;
            if (heroGameObject.TryGetComponent(out hero))
            {
                hero.Construct(_inputService);
            }
            
            await SetCameraTarget(heroGameObject);
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

        private async void OnLoaded()
        {
            await InitGameWorld();
            _stateMachine.Enter<GameLoopState>();
        }
        private LevelStaticData LevelStaticData() => 
            _staticData.ForLevel("forest");

        Task SetCameraTarget(GameObject target)
        {
            if (Camera.main)
            {
                CinemachineVirtualCamera camera = Camera.main.GetComponent<CinemachineVirtualCamera>();
               camera.LookAt = target.transform;
               camera.Follow = target.transform;
            }
            return Task.CompletedTask;
        }
    }
}