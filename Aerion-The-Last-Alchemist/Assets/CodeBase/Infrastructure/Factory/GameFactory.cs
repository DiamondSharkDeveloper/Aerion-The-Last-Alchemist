using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Creature;
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
        private GameObject _trees;
        private GameObject _tiles;
        private GameObject _grass;
        private List<MyTile> _mapCoordinates;
        private List<Vector3> _positionsInTile;
        private List<string> _ingredients = new List<string>();
        private float _step = 0.5f;

        private List<string> _rockPath = new List<string>();
        private List<string> _grassPath = new List<string>();
        private List<string> _mushroomsPath = new List<string>();
        private List<string> _flowerPath = new List<string>();

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
            Dictionary<string, IngredientStaticData> ingredientStaticDic = _staticData.ForIngredients();
            foreach (string ingredientStaticData in ingredientStaticDic.Keys)
            {
                _ingredients.Add(ingredientStaticData);
            }


            _rockPath.Add(AssetAddress.RockPath01);
            _rockPath.Add(AssetAddress.RockPath02);
            _rockPath.Add(AssetAddress.RockPath03);
            _rockPath.Add(AssetAddress.RockPath04);

            _flowerPath.Add(AssetAddress.FlowerPath01);
            _flowerPath.Add(AssetAddress.FlowerPath02);
            _flowerPath.Add(AssetAddress.FlowerPath03);
            _flowerPath.Add(AssetAddress.FlowerPath04);
            _flowerPath.Add(AssetAddress.FlowerPath06);
            _flowerPath.Add(AssetAddress.FlowerPath05);

            _grassPath.Add(AssetAddress.GrassPath01);
            _grassPath.Add(AssetAddress.GrassPath02);
            _grassPath.Add(AssetAddress.GrassPath03);
            _grassPath.Add(AssetAddress.GrassPath04);
            _grassPath.Add(AssetAddress.GrassPath05);
            _grassPath.Add(AssetAddress.GrassPath06);
            _grassPath.Add(AssetAddress.GrassPath07);
            _grassPath.Add(AssetAddress.GrassPath08);
            _grassPath.Add(AssetAddress.GrassPath09);
            _grassPath.Add(AssetAddress.GrassPath10);

            _mushroomsPath.Add(AssetAddress.MushroomPath01);
            _mushroomsPath.Add(AssetAddress.MushroomPath02);
            _mushroomsPath.Add(AssetAddress.MushroomPath03);
            _mushroomsPath.Add(AssetAddress.MushroomPath04);
            _mushroomsPath.Add(AssetAddress.MushroomPath05);
            _mushroomsPath.Add(AssetAddress.MushroomPath06);
            _mushroomsPath.Add(AssetAddress.MushroomPath07);
            _mushroomsPath.Add(AssetAddress.MushroomPath08);
            _mushroomsPath.Add(AssetAddress.MushroomPath09);

            _positionsInTile = new List<Vector3>(9);
            _positionsInTile.Add(new Vector3(0, 0, 0));
            _positionsInTile.Add(new Vector3(_step, 0, 0));
            _positionsInTile.Add(new Vector3(0, 0, _step));
            _positionsInTile.Add(new Vector3(_step, 0, _step));
            _positionsInTile.Add(new Vector3(-_step, 0, _step));
            _positionsInTile.Add(new Vector3(_step, 0, -_step));
            _positionsInTile.Add(new Vector3(-_step, 0, -_step));
            _positionsInTile.Add(new Vector3(-_step, 0, 0));
            _positionsInTile.Add(new Vector3(0, 0, -_step));
        }

        public List<ISavedProgressReader> ProgressReaders { get; }
        public List<ISavedProgress> ProgressWriters { get; }

        public async Task CreateMap(List<MyTile> mapCoordinates, LevelStaticData staticData, Action<EventArgs> move)
        {
            _trees = new GameObject("forrest");
            _grass = new GameObject("grass");
            _tiles = new GameObject("Tiles");
            await CreateTileMap();
            List<MyTile> creatureTile = new List<MyTile>();
            for (var i = 0; i < mapCoordinates.Count; i++)
            {
                int count = i;
                _tilemap.SetTile(mapCoordinates[count].CellPosition, mapCoordinates[count].Tile);
                mapCoordinates[count].IsAvailable = mapCoordinates[count].Type == TileTypeEnum.Grass;
                mapCoordinates[count].StartWorldPosition = _tilemap.CellToWorld(mapCoordinates[count].CellPosition);
                mapCoordinates[count].Tile.gameObject = await GetTyleByType(mapCoordinates[count].Type,
                    mapCoordinates[count].StartWorldPosition);
                mapCoordinates[count].Tile.gameObject.transform.localRotation = Quaternion.Euler(0, 30, 0);
                mapCoordinates[count].Tile.gameObject.transform.localScale *= 1.75f;
                if (mapCoordinates[count].IsEdge)
                {
                    MeshRenderer renderer =
                        mapCoordinates[count].Tile.gameObject.GetComponentsInChildren<MeshRenderer>()[1];
                    renderer.enabled = true;
                    float size = _randomService.Next(100, 500);

                    if (mapCoordinates[count].Type == TileTypeEnum.Water)
                    {
                        renderer.transform.localScale = new Vector3(1,
                            3000, 1);
                        renderer.transform.localPosition = new Vector3(renderer.transform.localPosition.x,
                            -300, renderer.transform.localPosition.z);
                    }
                    else
                    {
                        renderer.transform.localScale = new Vector3(renderer.transform.localScale.x,
                            renderer.transform.localScale.y, size);
                        renderer.transform.localPosition = new Vector3(renderer.transform.localPosition.x,
                            -0.0045f * size, renderer.transform.localPosition.z);
                    }
                }

                WorldTile worldTile = mapCoordinates[count].Tile.gameObject.AddComponent<WorldTile>();
                worldTile.Construct(mapCoordinates[count]);
                worldTile.Event += (sender, args) => { move?.Invoke(args); };
                switch (mapCoordinates[count].TileObjectType)
                {
                    case TileObjectType.Trees:
                        await CreateTree(mapCoordinates[count]);
                        break;
                    case TileObjectType.Ingredient:
                        await CreateLoot(mapCoordinates[count]);
                        break;
                    case TileObjectType.Rock:
                        await CreateRock(mapCoordinates[count]);
                        break;
                    case TileObjectType.Creature:
                        creatureTile.Add(mapCoordinates[count]);
                        break;
                    case TileObjectType.Flower:
                        await CreateGrass(mapCoordinates[count]);
                        break;
                    case TileObjectType.Mushrooms:
                        await CreateGrass(mapCoordinates[count]);
                        break;
                    case TileObjectType.Grass:
                        await CreateGrass(mapCoordinates[count]);
                        break;
                }
            }

            for (var i1 = 0; i1 < creatureTile.Count; i1++)
            {
                int count = i1;
                await CreateCreature(staticData.creaturesType[count], creatureTile[count], () =>
                {
                    _stateMachine.Enter<CreatureState, CreatureStats>(_persistentProgressService.Progress.gameData
                        .CreatureDada.ForCreature(staticData.creaturesId[count]));
                });
            }

            Cleanup();
        }

        private async Task CreateTree(MyTile mapCoordinate)
        {
            GameObject forestPlantation = new GameObject(mapCoordinate.CellPosition + " forestPlantation");
            forestPlantation.transform.SetParent(_trees.transform);
            GameObject prefab= await _assets.Load<GameObject>(AssetAddress.TreePath01);
            switch (mapCoordinate.FloraTipe)
            {
                case FloraTipe.Palm:
                    break;
                case FloraTipe.Pine:
                    prefab = await _assets.Load<GameObject>(AssetAddress.TreePath02);
                    break;
                case FloraTipe.Xmas:
                    prefab = await _assets.Load<GameObject>(AssetAddress.TreePath03);
                    break;
                case FloraTipe.None:
                    return;
                    break;
            }

            int count = _randomService.Next(0, _positionsInTile.Count);
            float minSize = 13;
            int maxSize = 40;

            _positionsInTile.Shuffle();

            for (int i = 0; i < count; i++)
            {
                GameObject gameObject = InstantiateRegistered(prefab,
                    mapCoordinate.StartWorldPosition + _positionsInTile[i], forestPlantation.transform);
                RandomSize(gameObject, minSize, maxSize);
                Quaternion transformLocalRotation = gameObject.transform.localRotation;
                transformLocalRotation.eulerAngles = new Vector3(0, _randomService.Next(0, 360), 0);
                gameObject.transform.localRotation = transformLocalRotation;
            }
        }

        private async Task CreateGrass(MyTile mapCoordinate)
        {
            GameObject grassPlantation = new GameObject(mapCoordinate.CellPosition + " grassPlantation");
            grassPlantation.transform.SetParent(_grass.transform);
          
            int count = _randomService.Next(0, 200);
            float faunaCount=0;
            float minSize = 80;
            int maxSize = 175;
            GameObject floraPrefab =await _assets.Load<GameObject>(_mushroomsPath[_randomService.Next(0, _mushroomsPath.Count)]);
            switch (mapCoordinate.FloraTipe)
            {
                case FloraTipe.Mushroom:
                     faunaCount = _randomService.Next(0, count*0.03f);
                    break;
                case FloraTipe.Flower:
                    minSize = 20;
                    maxSize = 40;
                    faunaCount = _randomService.Next(0, count*0.15f);
                    floraPrefab =await _assets.Load<GameObject>(_flowerPath[_randomService.Next(0, _flowerPath.Count)]);
                    break;
                case FloraTipe.Grass:
                   
                    faunaCount = 0;
                    break;
            }

            
          
            List<Vector3> positions = new List<Vector3>();
            for (int i = 0; i < count; i++)
            {
                Vector3 pos = new Vector3();
                do
                {
                    pos = new Vector3(_randomService.Next(-0.75f, 0.75f), 0.17f, _randomService.Next(-0.75f, 0.75f));
                    if (!positions.Contains(pos))
                    {
                        positions.Add(pos);
                    }
                } while (!positions.Contains(pos));

                GameObject gameObject;
                if (faunaCount > 0)
                {
                    gameObject = InstantiateRegistered(floraPrefab,
                        mapCoordinate.StartWorldPosition+pos, grassPlantation.transform);
                    faunaCount--;
                }
                else
                {
                    maxSize =30 ;
                    minSize = 100;
                    gameObject = InstantiateRegistered(await _assets.Load<GameObject>(_grassPath[_randomService.Next(0, _grassPath.Count)]),
                        mapCoordinate.StartWorldPosition +pos, grassPlantation.transform);
                }

                RandomSize(gameObject, minSize, maxSize);
                Quaternion transformLocalRotation = gameObject.transform.localRotation;
                transformLocalRotation.eulerAngles = new Vector3(0, _randomService.Next(0, 360), 0);
                gameObject.transform.localRotation = transformLocalRotation;
            }
        }

        private async Task CreateRock(MyTile mapCoordinate)
        {
            GameObject prefab = await _assets.Load<GameObject>(_rockPath[_randomService.Next(0, _rockPath.Count)]);
            GameObject gameObject = InstantiateRegistered(prefab, mapCoordinate.StartWorldPosition,
                mapCoordinate.Tile.gameObject.transform); //);
            gameObject.transform.localPosition = new Vector3(0, 0.2f, 0);
            Quaternion transformLocalRotation = gameObject.transform.localRotation;
            transformLocalRotation.eulerAngles = new Vector3(90, _randomService.Next(0, 360), 0);
            gameObject.transform.localRotation = transformLocalRotation;
            RandomSize(gameObject, 60, 140);
        }

        private float RandomSize(GameObject gameObject, float min, float max)
        {
            float size = _randomService.Next(min, max);
            gameObject.transform.localScale *= size * 0.01f;
            return size;
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
                    path = AssetAddress.GrassGexPath;
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
            _hero = InstantiateRegistered(prefab, at.StartWorldPosition); //);
            return _hero;
        }

        public async Task<GameObject> CreateHouse(MyTile parent, Action action)
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.HousePath);
            parent.OnStandAction = action;
            GameObject house =
                InstantiateRegistered(prefab, parent.StartWorldPosition); //);
            house.transform.localRotation = Quaternion.Euler(-90, 0, 0);
            return house;
        }

        public async Task<GameObject> CreateLoot(MyTile at)
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.LootPath);
            GameObject loot = InstantiateRegistered(prefab, at.StartWorldPosition + new Vector3(0, 0.3f, 0)); //);
            LootPiece lootPiece = loot.GetComponent<LootPiece>();
            at.OnStandAction = () =>
            {
                lootPiece.Pickup();
                at.OnStandAction = null;
            };
            lootPiece.Construct(_persistentProgressService.Progress.gameData);
            lootPiece.Initialize(new Loot(_ingredients[_randomService.Next(0, _ingredients.Count)],
                _randomService.Next(1, 5)));
            return loot;
        }


        public async Task<GameObject> CreateHud(Action<FormulaStaticData> action)
        {
            GameObject hud = await InstantiateRegisteredAsync(AssetAddress.HUDPath);
            hud.GetComponent<HUD>().Construct(_windowService, action);
            return hud;
        }

        public async Task<GameObject> CreateCameraController()
        {
            GameObject cameraController = await InstantiateRegisteredAsync(AssetAddress.StrategyCameraPath);
            return cameraController;
        }

        public async Task<GameObject> CreateCreature(CreatureTypeId typeId, MyTile parent, Action action)
        {
            GameObject creatureGameObject =
                await CreateCreature(typeId, parent.Tile.gameObject.transform, parent.StartWorldPosition);
            CreatureOnMap creatureOnMap = creatureGameObject.AddComponent<CreatureOnMap>();
            creatureOnMap.Construct(_hero);
            parent.OnStandAction = action;
            return creatureGameObject;
        }

        public async Task<GameObject> CreateCreature(CreatureTypeId typeId, Transform parent, Vector3 position)
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
            GameObject creatureGameObject = InstantiateRegistered(prefab, position); //);
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
            await _assets.Load<GameObject>(AssetAddress.StrategyCameraPath);
            await _assets.Load<GameObject>(AssetAddress.LootPath);
            await _assets.Load<GameObject>(AssetAddress.HeroPath);
            await _assets.Load<GameObject>(AssetAddress.CreatureLisovicPath);
            await _assets.Load<GameObject>(AssetAddress.CreatureVodianicPath);
            await _assets.Load<GameObject>(AssetAddress.GrassGexPath);
            await _assets.Load<GameObject>(AssetAddress.RockGexPath);
            await _assets.Load<GameObject>(AssetAddress.SwampGexPath);
            await _assets.Load<GameObject>(AssetAddress.WaterGexPath);

            await _assets.Load<GameObject>(AssetAddress.TreePath01);
            await _assets.Load<GameObject>(AssetAddress.TreePath02);
            await _assets.Load<GameObject>(AssetAddress.TreePath03);

            await _assets.Load<GameObject>(AssetAddress.RockPath01);
            await _assets.Load<GameObject>(AssetAddress.RockPath02);
            await _assets.Load<GameObject>(AssetAddress.RockPath03);
            await _assets.Load<GameObject>(AssetAddress.RockPath04);

            await _assets.Load<GameObject>(AssetAddress.FlowerPath01);
            await _assets.Load<GameObject>(AssetAddress.FlowerPath02);
            await _assets.Load<GameObject>(AssetAddress.FlowerPath03);
            await _assets.Load<GameObject>(AssetAddress.FlowerPath04);
            await _assets.Load<GameObject>(AssetAddress.FlowerPath06);
            await _assets.Load<GameObject>(AssetAddress.FlowerPath05);

            await _assets.Load<GameObject>(AssetAddress.GrassPath01);
            await _assets.Load<GameObject>(AssetAddress.GrassPath02);
            await _assets.Load<GameObject>(AssetAddress.GrassPath03);
            await _assets.Load<GameObject>(AssetAddress.GrassPath04);
            await _assets.Load<GameObject>(AssetAddress.GrassPath05);
            await _assets.Load<GameObject>(AssetAddress.GrassPath06);
            await _assets.Load<GameObject>(AssetAddress.GrassPath07);
            await _assets.Load<GameObject>(AssetAddress.GrassPath08);
            await _assets.Load<GameObject>(AssetAddress.GrassPath09);
            await _assets.Load<GameObject>(AssetAddress.GrassPath10);

            await _assets.Load<GameObject>(AssetAddress.MushroomPath01);
            await _assets.Load<GameObject>(AssetAddress.MushroomPath02);
            await _assets.Load<GameObject>(AssetAddress.MushroomPath03);
            await _assets.Load<GameObject>(AssetAddress.MushroomPath04);
            await _assets.Load<GameObject>(AssetAddress.MushroomPath05);
            await _assets.Load<GameObject>(AssetAddress.MushroomPath06);
            await _assets.Load<GameObject>(AssetAddress.MushroomPath07);
            await _assets.Load<GameObject>(AssetAddress.MushroomPath08);
            await _assets.Load<GameObject>(AssetAddress.MushroomPath09);
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