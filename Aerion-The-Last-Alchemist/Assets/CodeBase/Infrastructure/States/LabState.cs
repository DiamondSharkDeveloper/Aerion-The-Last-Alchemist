using CodeBase.Lab;
using CodeBase.Logic;
using CodeBase.Services;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
    public class LabState:IState
    {
        private const string LabScene = "Lab";
        private LaboratoryWindow _laboratoryWindow;
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;
        private readonly LoadingCurtain _loadingCurtain;
        public LabState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices allServices,LoadingCurtain loadingCurtain)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = allServices;
            _loadingCurtain = loadingCurtain;
        }
    
  
        public void Exit()
        {
            _sceneLoader.UpLoadAdditive(LabScene);
            _loadingCurtain.Show();
        }

        public void Enter()
        {
            _loadingCurtain.Show();
            _sceneLoader.LoadAdditive(LabScene, scene1 =>
            {
                Scene scene = scene1;
                if (scene.IsValid())
                {
                    if (scene.GetRootGameObjects()[0].TryGetComponent(out _laboratoryWindow))
                    {
                        _laboratoryWindow.OnClose += () =>
                        {
                            _stateMachine.Enter<GameLoopState>();
                            _loadingCurtain.Show();
                        };
                    }
                }
            } );
      
            _loadingCurtain.Hide();
        }
    }
}