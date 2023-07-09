using System.Collections.Generic;
using CodeBase.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Static Data/Level")]
    public class LevelStaticData : ScriptableObject
    {
        public string levelKey;
        public int seaInterval = 3;
        [Range(10,60)]
        public int mapSize = 10;
        public int waterSize = 1;
        public int swampSize = 1;
        public int rocksSize = 1;
        public int treesSize = 1;
        public int mushroomSize = 1;
        public int flowerSize = 1;
        public int grassSize = 1;
        
        public int ingredientsValue=10;
        public int heroPosition=220;
        public int housePosition=320;
        public List<CreatureTypeId> creaturesType=new List<CreatureTypeId>();
        public List<string> creaturesId=new List<string>();
        public Texture2D cursor;
        public float scale=2;
    }
}