﻿using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.Cursor;
using CodeBase.Services.Input;
using CodeBase.Services.Level;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;

namespace CodeBase.Infrastructure.States
{
    public class GameStateMachine : IGameStateMachine
    {
        private Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;
        private IExitableState _lastState;
        public void ChangeStateToPrevious()
        {
           
        }

        public event Action<IExitableState> OnStateChange;

        public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, AllServices allServices)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, allServices),
                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, loadingCurtain,
                    allServices.Single<IGameFactory>(),
                    allServices.Single<IPersistentProgressService>(), allServices.Single<IStaticDataService>(),
                    allServices.Single<ILevelGenerator>(), allServices.Single<IInputService>(),allServices.Single<IImageService>()),
                [typeof(LoadProgressState)] = new LoadProgressState(this,allServices.Single<IStaticDataService>(),
                    allServices.Single<IPersistentProgressService>(), allServices.Single<ISaveLoadService>(),allServices.Single<IRandomService>()),
                [typeof(GameLoopState)] = new GameLoopState(this),
                [typeof(LabState)] = new LabState(this, sceneLoader, allServices, loadingCurtain),
                [typeof(CreatureState)] = new CreatureState(this, sceneLoader, loadingCurtain,allServices.Single<IPersistentProgressService>(),allServices.Single<IGameFactory>(),allServices.Single<IStaticDataService>(),allServices.Single<IInputService>()), 
                [typeof(MenuState)] = new MenuState(this,allServices.Single<IUIFactory>(),loadingCurtain, sceneLoader)
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
            OnStateChange?.Invoke(state);
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
            OnStateChange?.Invoke(state);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();

            TState state = GetState<TState>();
            _lastState = _activeState;
            _activeState = state;

            return state;
        }

        public TState GetState<TState>() where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;

       
    }
}