using System;
using CodeBase.Logic;
using CodeBase.Map;
using CodeBase.Services.Input;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Unity.VisualScripting;
using UnityEngine;

namespace CodeBase.Hero
{
    public class HeroMove : MonoBehaviour
    {
        private IInputService _inputService;
        private MyTile _currentTile;
        private float moveSpeed = 2f;
        private TweenerCore<Vector3, Vector3, VectorOptions> _core;
        private LookAtTarget _lookAtTarget;
        public bool canMove = true;
        public event Action<bool> _isMove;
        public event Action OnInteractiveObject;


        public void Construct()
        {
            _lookAtTarget = transform.AddComponent<LookAtTarget>();
        }

        public void Move(MyTile tile)
        {
            if (tile.IsAvailable && _currentTile != tile && canMove)
            {
                _core?.Kill();
                transform.SetParent(tile.Tile.gameObject.transform);
                float speed = Vector3.Distance(transform.position,
                    tile.StartWorldPosition)*moveSpeed;
                _core = transform.DOMove(new Vector3(tile.StartWorldPosition.x,transform.position.y,tile.StartWorldPosition.z),
                    speed);
                _core.onComplete += () =>
                {
                    transform.localPosition = Vector3.zero;
                    _isMove?.Invoke(false);
                    if (tile.OnStandAction != null)
                    {
                        TileOnOnStandAction();
                        tile.OnStandAction.Invoke();
                    }
                };
                _core.onKill += () => { _isMove?.Invoke(false); };
                _currentTile = tile;
                _lookAtTarget.StartRotation(_currentTile.Tile.gameObject.transform);
                _isMove?.Invoke(true);
            }
        }

        private void TileOnOnStandAction()
        {
            canMove = false;
            OnInteractiveObject?.Invoke();
        }
    }
}