using System;
using CodeBase.Infrastructure.States;
using CodeBase.Map;

namespace CodeBase.Services.Input
{
    public interface IInputService: IService
    {
        public void Construct(IGameStateMachine stateMachine);
    }
}