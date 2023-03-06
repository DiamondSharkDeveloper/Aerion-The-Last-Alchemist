using System;
using CodeBase.Infrastructure.States;
using CodeBase.Map;
using Unity.VisualScripting;
using UnityEngine;

namespace CodeBase.Services.Input
{
    public class InputService : MonoBehaviour, IInputService
    {
        private IGameStateMachine _stateMachine;
        private bool _canRayCast;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void Construct(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _stateMachine.OnStateChange += StateMachineOnOnStateChange;
        }

        private void StateMachineOnOnStateChange(IExitableState obj)
        {
            _canRayCast = !obj.IsOnPause();
        }

        public event Action<WorldTile> OnTileClick;

        void Update()
        {
            if (!_canRayCast)
                return;
            RaycastHit hit;
            Ray rayOrigin = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(rayOrigin, out hit))
                {
                    WorldTile worldTile;
                    if (hit.transform.TryGetComponent(out worldTile) ||
                        hit.transform.parent.transform.TryGetComponent(out worldTile))
                    {
                        OnTileClick?.Invoke(worldTile);
                    }
                }
            }
        }
    }
}