using System;
using CodeBase.Map;
using CodeBase.Services.Input;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Hero
{
    public class HeroMove : MonoBehaviour
    {
        private IInputService _inputService;
        private MyTile _currentTile;
        [SerializeField] private float moveSpeed = 30;
      

        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
            _inputService.OnTileClick += tile => { Move(tile.MyTile); };
        }

        private void Move(MyTile tile)
        {
            if (tile.IsAvailable && _currentTile != tile)
            {
                transform.SetParent(tile.Tile.gameObject.transform);
                transform.DOMove(transform.parent.position, 10);
                _currentTile = tile;
                transform.LookAt(transform.parent);
            }
        }

        private void Update()
        {
            DoMove();
        }

        private void DoMove()
        {
            if (Vector3.Distance(transform.parent.position, transform.position) > 0.1f)
            {
                Vector3.MoveTowards(transform.position, transform.parent.position, moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = transform.parent.position;
            }
        }
    }
}