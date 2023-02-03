using CodeBase.Services.StaticData;
using CodeBase.StaticData;

namespace CodeBase.Infrastructure.States
{
    internal class StaticDataService : IStaticDataService
    {
        public void Load()
        {
            throw new System.NotImplementedException();
        }

        public MonsterStaticData ForMonster(MonsterTypeId typeId)
        {
            throw new System.NotImplementedException();
        }

        public LevelStaticData ForLevel(string sceneKey)
        {
            throw new System.NotImplementedException();
        }
    }
}