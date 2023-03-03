using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enemy;
using CodeBase.Map;
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
        Task<GameObject> CreateHero(MyTile parent);
        Task<GameObject> CreateHouse(MyTile parent,Action action);
        Task<GameObject> CreateHud();
        Task<GameObject> CreateCreature(CreatureTypeId typeId, MyTile parent,Action action);
        Task<LootPiece> CreateLoot();
        void Cleanup();
        Task WarmUp();
    }
}