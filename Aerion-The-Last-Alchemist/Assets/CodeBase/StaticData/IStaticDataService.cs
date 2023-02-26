using CodeBase.StaticData;

namespace CodeBase.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        void Load();
        MonsterStaticData ForMonster(CreatureTypeId typeId);
        LevelStaticData ForLevel(string sceneKey);
    }
}