using CodeBase.Creature;
using CodeBase.Logic;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
    public class CreatureState:IState
    {
        private const string CreatureScene = "Creature";
        private CreatureWindow _creatureWindow;
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
    
        public CreatureState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
        }
        public void Exit()
        {
            _sceneLoader.UpLoadAdditive(CreatureScene);
            _loadingCurtain.Hide();
        }

        public void Enter()
        {
            _loadingCurtain.Show();
            _sceneLoader.LoadAdditive(CreatureScene, scene1 =>
            {
                Scene scene = scene1;
                if (scene.IsValid())
                {
                    if (scene.GetRootGameObjects()[0].TryGetComponent(out _creatureWindow))
                    {
                        _creatureWindow.OnClose += () =>
                        {
                            _loadingCurtain.Show();
                            _stateMachine.Enter<GameLoopState>();
                        };
                    }
                }
            } );
      
            _loadingCurtain.Hide();
        }
    }
}