using System;

namespace CodeBase.Data
{
    [Serializable]
    public class GameData
    {
        public LootData lootData;
        public CreatureDada CreatureDada;

        public GameData(string initialLevel)
        {
            CreatureDada = new CreatureDada();
            lootData = new LootData();
        }
    }
}