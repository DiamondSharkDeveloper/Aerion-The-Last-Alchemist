using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Enums;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.States;
using CodeBase.Map;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Randomizer;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Windows;
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
        private readonly IWindowService _windowService;
        private readonly IGameStateMachine _stateMachine;
        private Tilemap _tilemap;
        private GameObject _mapGameObject;
        private GameObject _hero;
        private List<MyTile> _mapCoordinates;
        private List<string> _ingredients = new List<string>();
        private List<string> _treesPath = new List<string>();

        public GameFactory(
            IAssetProvider assets,
            IStaticDataService staticData,
            IRandomService randomService,
            IPersistentProgressService persistentProgressService,
            IGameStateMachine stateMachine, IWindowService windowService)
        {
            _assets = assets;
            _staticData = staticData;
            _randomService = randomService;
            _persistentProgressService = persistentProgressService;
            _windowService = windowService;
            _stateMachine = stateMachine;
            foreach (IngredientStaticData ingredientStaticData in _staticData.ForIngredients())
            {
                _ingredients.Add(ingredientStaticData.name);
            }

            _treesPath.Add(AssetAddress.TreePrefab01);
            _treesPath.Add(AssetAddress.TreePrefab02);
            _treesPath.Add(AssetAddress.TreePrefab03);
            _treesPath.Add(AssetAddress.TreePrefab04);
            _treesPath.Add(AssetAddress.TreePrefab06);
        }

        public List<ISavedProgressReader> ProgressReaders { get; }
        public List<ISavedProgress> ProgressWriters { get; }

        public async Task CreateMap(List<MyTile> mapCoordinates)
        {
            await CreateTileMap();
            for (var i = 0; i < mapCoordinates.Count; i++)
            {
                int count = i;
                _tilemap.SetTile(mapCoordinates[count].CellPosition, mapCoordinates[count].Tile);
                mapCoordinates[count].IsAvailable = mapCoordinates[count].Type == TileTypeEnum.Grass;
                mapCoordinates[count].StartWorldPosition = _tilemap.CellToWorld(mapCoordinates[count].CellPosition);
                mapCoordinates[count].Tile.gameObject = await GetTyleByType(mapCoordinates[count].Type,
                    mapCoordinates[count].StartWorldPosition);
                mapCoordinates[count].Tile.gameObject.transform.localRotation = Quaternion.Euler(0, 30, 0);
                mapCoordinates[count].Tile.gameObject.AddComponent<WorldTile>().Construct(mapCoordinates[count]);
                if (mapCoordinates[count].TileObjectType == TileObjectType.Ingredient)
                {
                    await CreateLoot(mapCoordinates[count]);
                }
                else if (mapCoordinates[count].TileObjectType == TileObjectType.Trees)
                {
                    await CreateTree(mapCoordinates[count]);
                }
            }
        }

        private async Task CreateTree(MyTile mapCoordinate)
        {
            GameObject prefab = await _assets.Load<GameObject>(_treesPath[_randomService.Next(0, _treesPath.Count)]);
            InstantiateRegistered(prefab, mapCoordinate.StartWorldPosition,
                mapCoordinate.Tile.gameObject.transform);
        }

        private async Task<GameObject> GetTyleByType(TileTypeEnum mapCoordinateType, Vector3 at)
        {
            string path = AssetAddress.WaterGexPath;
            switch (mapCoordinateType)
            {
                case TileTypeEnum.Grass:
                    path = AssetAddress.GrassGexPath;
                    break;
                case TileTypeEnum.Swamp:
                    path = AssetAddress.SwampGexPath;
                    break;
                case TileTypeEnum.Water:
                    path = AssetAddress.WaterGexPath;
                    break;
                case TileTypeEnum.Rock:
                    path = AssetAddress.RockGexPath;
                    break;
            }

            GameObject prefab = await _assets.Load<GameObject>(path);
            return InstantiateRegistered(prefab, at, _tilemap.transform);
        }

        private async Task CreateTileMap()
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.MapPath);
            _mapGameObject = InstantiateRegistered(prefab, Vector3.zero);
            _tilemap = _mapGameObject.GetComponentInChildren<Tilemap>();
        }

        public async Task<GameObject> CreateHero(MyTile at)
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.HeroPath);
            _hero = InstantiateRegistered(prefab, at.StartWorldPosition, at.Tile.gameObject.transform);
            return _hero;
        }

        public async Task<GameObject> CreateHouse(MyTile parent, Action action)
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.HousePath);
            parent.OnStandAction = action;
            GameObject house =
                InstantiateRegistered(prefab, parent.StartWorldPosition, parent.Tile.gameObject.transform);
            house.transform.localRotation = Quaternion.Euler(-90, 0, 0);
            return house;
        }

        public async Task<GameObject> CreateLoot(MyTile at)
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.LootPath);
            GameObject loot = InstantiateRegistered(prefab, at.StartWorldPosition, at.Tile.gameObject.transform);
            LootPiece lootPiece = loot.GetComponent<LootPiece>();
            at.OnStandAction = () => { lootPiece.Pickup(); };
            lootPiece.Construct(_persistentProgressService.Progress.gameData);
            lootPiece.Initialize(new Loot(_ingredients[_randomService.Next(0, _ingredients.Count - 1)],
                _randomService.Next(1, 5)));
            return loot;
        }


        public async Task<GameObject> CreateHud(Action<FormulaStaticData> action)
        {
            GameObject hud = await InstantiateRegisteredAsync(AssetAddress.HUDPath);
            hud.GetComponent<HUD>().Construct(_windowService,action);
            return hud;
        }

        public async Task<GameObject> CreateCameraController()
        {
            GameObject cameraController = await InstantiateRegisteredAsync(AssetAddress.StrategyCamera);
            return cameraController;
        }

        public async Task<GameObject> CreateCreature(CreatureTypeId typeId, MyTile parent, Action action)
        {
            GameObject creatureGameObject = await CreateCreature(typeId,parent.Tile.gameObject.transform, parent.StartWorldPosition);
            Creature.CreatureOnMap creatureOnMap = creatureGameObject.AddComponent<Creature.CreatureOnMap>();
            creatureOnMap.Construct(_hero);
            parent.OnStandAction = action;
            return creatureGameObject;
        }

        public async Task<GameObject> CreateCreature(CreatureTypeId typeId, Transform parent,Vector3 position)
        {
            string path = AssetAddress.CreatureLisovicPath;
            switch (typeId)
            {
                case CreatureTypeId.Lisovic:
                    path = AssetAddress.CreatureLisovicPath; 
                    break;
                case CreatureTypeId.Vodianic:
                    path = AssetAddress.CreatureVodianicPath; 
                    break;
                    
            }
            GameObject prefab = await _assets.Load<GameObject>(path);
            GameObject creatureGameObject = InstantiateRegistered(prefab, position,
                parent.transform);
            return creatureGameObject;
        }


        public Task<LootPiece> CreateLoot()
        {
            throw new NotImplementedException();
        }


        public void Cleanup()
        {
            // ProgressReaders.Clear();
            // ProgressWriters.Clear();

            _assets.Cleanup();
        }

        public async Task WarmUp()
        {
            await _assets.Load<GameObject>(AssetAddress.MapPath);

            await _assets.Load<GameObject>(AssetAddress.HousePath);
            await _assets.Load<GameObject>(AssetAddress.HUDPath);
            await _assets.Load<GameObject>(AssetAddress.StrategyCamera);
            await _assets.Load<GameObject>(AssetAddress.LootPath);
            await _assets.Load<GameObject>(AssetAddress.HeroPath);
            await _assets.Load<GameObject>(AssetAddress.CreatureLisovicPath);
            await _assets.Load<GameObject>(AssetAddress.CreatureVodianicPath);
            await _assets.Load<GameObject>(AssetAddress.GrassGexPath);
            await _assets.Load<GameObject>(AssetAddress.RockGexPath);
            await _assets.Load<GameObject>(AssetAddress.SwampGexPath);
            await _assets.Load<GameObject>(AssetAddress.WaterGexPath);

            await _assets.Load<GameObject>(AssetAddress.TreePrefab01);
            await _assets.Load<GameObject>(AssetAddress.TreePrefab02);
            await _assets.Load<GameObject>(AssetAddress.TreePrefab03);
            await _assets.Load<GameObject>(AssetAddress.TreePrefab04);
            await _assets.Load<GameObject>(AssetAddress.TreePrefab06);
        }

        private GameObject InstantiateRegistered(GameObject prefab, Vector3 at)
        {
            GameObject gameObject = Object.Instantiate(prefab, at, Quaternion.identity);
            RegisterProgressWatchers(gameObject);

            return gameObject;
        }

        private GameObject InstantiateRegistered(GameObject prefab, Vector3 at, Transform perent)
        {
            GameObject gameObject = Object.Instantiate(prefab, at, Quaternion.identity, perent);
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