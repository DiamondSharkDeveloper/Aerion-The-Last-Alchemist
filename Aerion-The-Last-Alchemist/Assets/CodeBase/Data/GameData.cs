using System;

namespace CodeBase.Data
{
    [Serializable]
    public class GameData
    {
        public LootData lootData;

        public GameData(string initialLevel)
        {
            lootData = new LootData();
        }
    }
}