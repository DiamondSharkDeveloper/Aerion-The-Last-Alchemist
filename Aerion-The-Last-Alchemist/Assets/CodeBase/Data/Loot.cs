using System;
using UnityEngine.Serialization;

namespace CodeBase.Data
{
    [Serializable]
    public class Loot
    {
        public TileObjectType type;
        public int value;
    }
}