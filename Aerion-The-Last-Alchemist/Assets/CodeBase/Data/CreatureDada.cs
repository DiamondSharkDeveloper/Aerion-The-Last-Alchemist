using System.Collections.Generic;
using CodeBase.Creature;
using CodeBase.Services.Randomizer;
using CodeBase.StaticData;

namespace CodeBase.Data
{
    public  class CreatureDada
    {
        private Dictionary<string, CreatureStats> _creatureStatsMap = new Dictionary<string, CreatureStats>();
        public void GenerateData(List<string>keys,IRandomService randomService,List<CreatureTypeId>creatureTypeIds)
        {
            for (var i = 0; i < keys.Count; i++)
            {
                _creatureStatsMap[keys[i]] = new CreatureStats(randomService.Next(0,CreatureStats.MaxStatValue+1),randomService.Next(0,CreatureStats.MaxStatValue+1),randomService.Next(0,CreatureStats.MaxStatValue+1),randomService.Next(0,CreatureStats.MaxStatValue+1),keys[i],creatureTypeIds[i]);

            }
        }
        public CreatureStats ForCreature(string creatureKey) =>
            _creatureStatsMap.TryGetValue(creatureKey, out CreatureStats creatureStats)
                ? creatureStats
                : null;
        
    }
}