using CodeBase.Creature;
using CodeBase.Logic;
using CodeBase.StaticData;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
    public class CreatureState : IPayloadedState<CreatureTypeId>
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

        public void Enter(CreatureTypeId payload)
        {
            _loadingCurtain.Show();
            _sceneLoader.LoadAdditive(CreatureScene, scene1 =>
            {
                Scene scene = scene1;
                if (scene.IsValid())
                {
                    if (scene.GetRootGameObjects()[0].TryGetComponent(out _creatureWindow))
                    {
                        _creatureWindow.Construct(payload);
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