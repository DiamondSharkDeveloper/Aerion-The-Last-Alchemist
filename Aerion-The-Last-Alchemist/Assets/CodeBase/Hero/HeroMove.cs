using System;
using CodeBase.Map;
using CodeBase.Services.Input;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace CodeBase.Hero
{
    public class HeroMove : MonoBehaviour
    {
        private IInputService _inputService;
        private MyTile _currentTile;
        [SerializeField] private float moveSpeed = 30;
        private TweenerCore<Vector3, Vector3, VectorOptions> _core;
        public event Action<bool> _isMove;

        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
            _inputService.OnTileClick += tile => { Move(tile.MyTile); };
        }

        private void Move(MyTile tile)
        {
            if (tile.IsAvailable && _currentTile != tile)
            {
                _core?.Kill();
                transform.SetParent(tile.Tile.gameObject.transform);
                _core = transform.DOMove(transform.parent.position, 10);
                _core.onComplete += () => { _isMove?.Invoke(false); };
                _core.onKill += () => { _isMove?.Invoke(false); };
                _currentTile = tile;
                transform.LookAt(transform.parent);
                _isMove?.Invoke(true);
            }
        }
    }
}