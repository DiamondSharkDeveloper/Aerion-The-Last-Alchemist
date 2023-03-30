using System;
using CodeBase.Infrastructure.States;
using CodeBase.Map;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Services.Input
{
    public interface IInputService: IService
    {
        public void Construct(IGameStateMachine stateMachine, IPersistentProgressService persistentProgressService);
        public event Action OnMouseButtonDown;
        void SetCamera(Camera camera);
    }
}