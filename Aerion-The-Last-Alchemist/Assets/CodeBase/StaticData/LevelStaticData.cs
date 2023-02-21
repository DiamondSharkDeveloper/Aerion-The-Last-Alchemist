using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Static Data/Level")]
    public class LevelStaticData : ScriptableObject
    {
        public string levelKey;
        public int seaInterval = 3;
        public int mapSize = 10;
        public int waterSize = 1;
        public int swampSize = 1;
        public int rocksSize = 1;
        public int heroPositionTileX,heroPositionTileY =1;
        public int labPositionTileX,labPositionTileY = 0;
    }
}