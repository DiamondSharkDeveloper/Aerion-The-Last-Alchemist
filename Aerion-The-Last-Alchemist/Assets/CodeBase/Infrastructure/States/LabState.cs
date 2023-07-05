using System;
using CodeBase.Lab;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using CodeBase.UI.Windows;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
    public class LabState : IPayloadedState <FormulaStaticData>
    {
        private const string LabScene = "Lab";
        private LaboratoryWindow _laboratoryWindow;
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IInputService _inputService;

        public LabState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices allServices,
            LoadingCurtain loadingCurtain)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = allServices;
            _loadingCurtain = loadingCurtain;
           
        }


        public void Enter(FormulaStaticData isGameRun)
        {
            _loadingCurtain.Show();
            _sceneLoader.LoadAdditive(LabScene, scene1 =>
            {
                Scene scene = scene1;
                if (scene.IsValid())
                {
                    foreach (var rootGameObject in scene.GetRootGameObjects())
                    {
                        if (rootGameObject.TryGetComponent(out _laboratoryWindow))
                        {
                            _laboratoryWindow.Init(isGameRun, _services.Single<IPersistentProgressService>(),
                                _services.Single<IWindowService>(),_services.Single<IInputService>(),_services.Single<IStaticDataService>());
                            _laboratoryWindow.OnClose += () =>
                            {
                                _loadingCurtain.Show();
                                _stateMachine.Enter<GameLoopState>();
                            };
                            return;
                        }
                    }
                }
            });

            _loadingCurtain.Hide();
        }

        public void Exit()
        {
            _sceneLoader.UpLoadAdditive(LabScene);
            _loadingCurtain.Hide();
        }

        public bool IsOnPause()
        {
            return true;
        }
    }
}