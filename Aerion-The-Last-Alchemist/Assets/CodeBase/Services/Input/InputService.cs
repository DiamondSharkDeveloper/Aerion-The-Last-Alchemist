using System;
using CodeBase.Infrastructure.States;
using CodeBase.Map;
using CodeBase.Menu;
using CodeBase.Services.PersistentProgress;
using Unity.VisualScripting;
using UnityEngine;

namespace CodeBase.Services.Input
{
    public class InputService : MonoBehaviour, IInputService
    {
        private IGameStateMachine _stateMachine;
        private bool _canRayCast;
        public event Action OnMouseButtonDown;
        private Camera _camera;


        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void Construct(IGameStateMachine stateMachine, IPersistentProgressService persistentProgressService)
        {
            _stateMachine = stateMachine;
        }

        public void SetCamera(Camera camera)
        {
            _camera = camera;
        }

        void Update()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
              
                if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()||OnMouseButtonDown != null)
                {
                    RaycastHit hit;
                    Ray rayOrigin = _camera.ScreenPointToRay(UnityEngine.Input.mousePosition);
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
                if (OnMouseButtonDown != null)
                {
                    OnMouseButtonDown?.Invoke();
                    OnMouseButtonDown = null;
                }
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                _stateMachine.Enter<MenuState,bool>(true);
            }
        }
    }
}