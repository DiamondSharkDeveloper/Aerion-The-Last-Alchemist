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
        void Update()
        {
            RaycastHit hit;
            Ray rayOrigin = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(rayOrigin, out hit))
                {
                    WorldTile worldTile;
                    if (hit.transform.TryGetComponent(out worldTile)||hit.transform.parent.transform.TryGetComponent(out worldTile))
                    {
                        OnTileClick?.Invoke(worldTile);
                    }
                }
            }
        }
    }
}