using System.Threading.Tasks;
using Cinemachine;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Services.Level;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, IGameFactory gameFactory, IPersistentProgressService progressService, IStaticDataService staticDataService,ILevelGenerator  levelGenerator)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _staticData = staticDataService;
            _levelGenerator = levelGenerator;

        }
        private async Task InitGameWorld()
        {
            LevelStaticData levelData = LevelStaticData();

            await _gameFactory.CreateMap(_levelGenerator.GetMap(levelData));
            GameObject hero=await _gameFactory.CreateHero(levelData.heroPositionTileX,levelData.heroPositionTileY);
            await SetCameraTarget(hero);
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