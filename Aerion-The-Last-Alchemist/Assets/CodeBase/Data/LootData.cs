using System;

namespace CodeBase.Data
{
    [Serializable]
    public class LootData
    {
        public LootPieceDataDictionary lootPiecesInDataDictionary = new LootPieceDataDictionary();

        public Action<Loot> Changed;

        public void Collect(Loot loot)
        {
            if (lootPiecesInDataDictionary.Dictionary.ContainsKey(loot.name))
            {
                lootPiecesInDataDictionary.Dictionary[loot.name].value += loot.value;
            }
            else
            {
                lootPiecesInDataDictionary.Dictionary[loot.name] = loot;
            }

            Changed?.Invoke(loot);
        }
    }
}