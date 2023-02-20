using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enemy;
using CodeBase.Enums;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.States;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using ModestTree.Util;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assets;
        private readonly IStaticDataService _staticData;
        private readonly IRandomService _randomService;
        private readonly IPersistentProgressService _persistentProgressService;
        private GameObject _heroGameObject;
        private readonly IGameStateMachine _stateMachine;
        private Tilemap _tilemap;
        private GameObject _mapGameObject;

        public GameFactory(
            IAssetProvider assets,
            IStaticDataService staticData,
            IRandomService randomService,
            IPersistentProgressService persistentProgressService,
            IGameStateMachine stateMachine)
        {
            _assets = assets;
            _staticData = staticData;
            _randomService = randomService;
            _persistentProgressService = persistentProgressService;
            _stateMachine = stateMachine;
        }

        public List<ISavedProgressReader> ProgressReaders { get; }
        public List<ISavedProgress> ProgressWriters { get; }

        public void CreateMap(List<MyTile> mapCoordinates)
        {
            CreateTileMap();
            foreach (MyTile mapCoordinate in mapCoordinates)
            {
                _tilemap.SetTile(mapCoordinate.CellPosition, mapCoordinate.Tile);
                mapCoordinate.StartWorldPosition = _tilemap.CellToWorld(mapCoordinate.CellPosition);
                GetTyleByType(mapCoordinate.Type, mapCoordinate.StartWorldPosition, tile =>
                {
                    tile.transform.localRotation = Quaternion.Euler(-90, 30, 0);
                    mapCoordinate.Tile.gameObject = tile;
                });
            }
        }

        private async void GetTyleByType(TileTypeEnum mapCoordinateType, Vector3 at, Action<GameObject> action)
        {
            string path = AssetAddress.WaterGexPath;
            switch (mapCoordinateType)
            {
                case TileTypeEnum.Grass:
                    path = AssetAddress.GrassGexPath;
                    break;
                case TileTypeEnum.Swamp:
                    path = AssetAddress.SwampGexPath;
                    ;
                    break;
                case TileTypeEnum.Water:
                    path = AssetAddress.WaterGexPath;
                    break;
                case TileTypeEnum.Rock:
                    path = AssetAddress.RockGexPath;
                    break;
            }

            GameObject prefab = await _assets.Load<GameObject>(path);
            action.Invoke(InstantiateRegistered(prefab, at));
        }

        private async Task CreateTileMap()
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.MapPath);
            _mapGameObject = InstantiateRegistered(prefab, Vector3.zero);
            _tilemap = _mapGameObject.GetComponentInChildren<Tilemap>();
        }

        public Task<GameObject> CreateHero(Vector3 at)
        {
            throw new System.NotImplementedException();
        }

        public Task<GameObject> CreateHero(MyTile at)
        {
            throw new System.NotImplementedException();
        }

        public Task<GameObject> CreateHud()
        {
            throw new System.NotImplementedException();
        }

        public Task<GameObject> CreateMonster(MonsterTypeId typeId, Transform parent)
        {
            throw new System.NotImplementedException();
        }

        public Task<GameObject> CreateMonster(MonsterTypeId typeId, MyTile parent)
        {
            throw new System.NotImplementedException();
        }

        public Task<LootPiece> CreateLoot()
        {
            throw new System.NotImplementedException();
        }


        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();

            _assets.Cleanup();
        }

        public async Task WarmUp()
        {
            await _assets.Load<GameObject>(AssetAddress.MapPath);
            await _assets.Load<GameObject>(AssetAddress.GrassGexPath);
            await _assets.Load<GameObject>(AssetAddress.RockGexPath);
            await _assets.Load<GameObject>(AssetAddress.SwampGexPath);
            await _assets.Load<GameObject>(AssetAddress.WaterGexPath);
        }

        private GameObject InstantiateRegistered(GameObject prefab, Vector3 at)
        {
            GameObject gameObject = Object.Instantiate(prefab, at, Quaternion.identity);
            RegisterProgressWatchers(gameObject);

            return gameObject;
        }

        private GameObject InstantiateRegistered(GameObject prefab)
        {
            GameObject gameObject = Object.Instantiate(prefab);
            RegisterProgressWatchers(gameObject);

            return gameObject;
        }

        private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath, Vector3 at)
        {
            GameObject gameObject = await _assets.Instantiate(path: prefabPath, at: at);
            RegisterProgressWatchers(gameObject);

            return gameObject;
        }

        private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath)
        {
            GameObject gameObject = await _assets.Instantiate(path: prefabPath);
            RegisterProgressWatchers(gameObject);

            return gameObject;
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
                Register(progressReader);
        }

        private void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
                ProgressWriters.Add(progressWriter);

            ProgressReaders.Add(progressReader);
        }
    }
}