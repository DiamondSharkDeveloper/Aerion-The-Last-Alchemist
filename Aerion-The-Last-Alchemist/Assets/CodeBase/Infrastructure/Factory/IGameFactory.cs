using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enemy;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;
namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        Task CreateMap(List<MyTile>mapCoordinates);
        Task<GameObject> CreateHero(Vector3 at);
        Task<GameObject> CreateHero(MyTile at);
        Task<GameObject> CreateHud();
        Task<GameObject> CreateMonster(MonsterTypeId typeId, Transform parent);
        Task<GameObject> CreateMonster(MonsterTypeId typeId, MyTile parent);
        Task<LootPiece> CreateLoot();
        void Cleanup();
        Task WarmUp();
    }
}