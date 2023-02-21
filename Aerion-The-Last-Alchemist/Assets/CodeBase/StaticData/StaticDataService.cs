using System.Collections.Generic;
using System.Linq;
using CodeBase.Services.StaticData;
using UnityEngine;

namespace CodeBase.StaticData
{
    internal class StaticDataService : IStaticDataService
    {
        private const string LevelsDataPath = "Static Data/Levels";
        private Dictionary<string, LevelStaticData> _levels;
        public void Load()
        {
            _levels = Resources
                .LoadAll<LevelStaticData>(LevelsDataPath)
                .ToDictionary(x => x.levelKey, x => x);
        }

        public MonsterStaticData ForMonster(MonsterTypeId typeId)
        {
            throw new System.NotImplementedException();
        }

        public LevelStaticData ForLevel(string sceneKey) =>
            _levels.TryGetValue(sceneKey, out LevelStaticData staticData)
                ? staticData
                : null;
    }
}