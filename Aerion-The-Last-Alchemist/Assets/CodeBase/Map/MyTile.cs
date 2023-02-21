using CodeBase.Enums;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CodeBase.Map
{

    public class MyTile 
    {
        public Vector3 StartWorldPosition;
        public Vector3Int CellPosition;
        public TileTypeEnum Type;
        public readonly Tile Tile;
        public readonly bool IsEdge;
        public bool IsAvailable;

        public MyTile(Vector3Int cellPosition, TileTypeEnum type,bool isEdge)
        {
            CellPosition = cellPosition;
            Type = type;
            IsEdge = isEdge;
            Tile = ScriptableObject.CreateInstance<Tile>();
        }
    
    }
}