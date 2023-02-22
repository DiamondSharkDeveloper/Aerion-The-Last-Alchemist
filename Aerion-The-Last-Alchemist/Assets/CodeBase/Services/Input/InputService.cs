using System;
using CodeBase.Map;
using UnityEngine;

namespace CodeBase.Services.Input
{
    public class InputService : MonoBehaviour, IInputService
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public event Action<WorldTile> OnTileClick;
        void FixedUpdate()
        {
            RaycastHit hit;
            Ray rayOrigin = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                Debug.Log("Click");
                if (Physics.Raycast(rayOrigin, out hit))
                {
                    Debug.Log("Ray");
                    WorldTile worldTile;
                    if (hit.transform.TryGetComponent(out worldTile))
                    {
                        OnTileClick?.Invoke(worldTile);
                    }
                }
            }
        }
    }
}