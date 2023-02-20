using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;

namespace CodeBase.Services.Level
{
    public interface ILevelGenerator
    {
        List<MyTile>GetMap(string level);
    }
}