using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.StaticData;

namespace CodeBase.Services.Level
{
    public interface ILevelGenerator:IService
    {
        List<MyTile>GetMap(LevelStaticData staticData);
    }
}