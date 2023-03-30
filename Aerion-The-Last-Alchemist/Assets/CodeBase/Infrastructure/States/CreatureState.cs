using CodeBase.Creature;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
    public class CreatureState : IPayloadedState<CreatureStats>
    {
        private const string CreatureScene = "Creature";
        private CreatureWindow _creatureWindow;
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IPersistentProgressService _progressService;
        private IGameFactory _gameFactory;
        private readonly IStaticDataService _staticdata;
        private readonly IInputService _inputService;

        public CreatureState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain,IPersistentProgressService progressService,IGameFactory gameFactory,IStaticDataService staticData,IInputService inputService)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _progressService = progressService;
            _gameFactory = gameFactory;
            _staticdata = staticData;
            _inputService = inputService;
        }
        

        public void Enter(CreatureStats payload)
        {
            _loadingCurtain.Show();
            _sceneLoader.LoadAdditive(CreatureScene, scene1 =>
            {
                Scene scene = scene1;
                if (scene.IsValid())
                {
                    if (scene.GetRootGameObjects()[0].TryGetComponent(out _creatureWindow))
                    {
                        _creatureWindow.Construct(payload,_gameFactory,_progressService,_staticdata,_inputService);
                        _creatureWindow.OnClose += () =>
                        {
                            _loadingCurtain.Show();
                            _stateMachine.Enter<GameLoopState>();
                        };
                    }
                }
            });

            _loadingCurtain.Hide();
        }

        public void Exit()
        {
            _sceneLoader.UpLoadAdditive(CreatureScene);
            _loadingCurtain.Hide();
        }

        public bool IsOnPause()
        {
            return false;
        }
    }

}