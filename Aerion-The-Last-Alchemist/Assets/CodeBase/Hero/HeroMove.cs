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
        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
            _inputService.OnTileClick += tile =>{Move(tile.MyTile);} ;
        }
        private void Move(MyTile tile)
        {
            if (tile.IsAvailable&&_currentTile!=tile)
            {
                transform.SetParent(tile.Tile.gameObject.transform);
                transform.DOMove( transform.parent.position, 10);
                _currentTile = tile;
            }
        }
    }
}
