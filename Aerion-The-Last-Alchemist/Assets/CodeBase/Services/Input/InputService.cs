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
        }


      

        private bool CanRaycast()
        {
            return (UnityEngine.Input.GetMouseButtonDown(0) &&
                    !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject());
        }

        void Update()
        {
            if (CanRaycast())
            {
                RaycastHit hit;
                Ray rayOrigin = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
                if (Physics.Raycast(rayOrigin, out hit))
                {
                    WorldObject worldObject;
                    if (hit.transform.TryGetComponent(out worldObject) ||
                        hit.transform.parent.transform.TryGetComponent(out worldObject))
                    {
                        worldObject.OnTileEvent();
                    }
                }
            }
        }
    }
}