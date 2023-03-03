using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Infrastructure.Factory;
using CodeBase.Map;
using CodeBase.Services.Randomizer;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CodeBase.Services.Level
{
    public class LevelGenerator : ILevelGenerator
    {
        private IStaticDataService _staticDataService;
        private IRandomService _randomService;
        private readonly List<MyTile> _mapCoordinates = new List<MyTile>();

        public LevelGenerator(IStaticDataService staticDataService, IRandomService randomService)
        {
            _staticDataService = staticDataService;
            _randomService = randomService;
        }


        public List<MyTile> GetMap(LevelStaticData staticData)
        {
            GenerateMap(staticData);
            return _mapCoordinates;
        }

        private void GenerateMap(LevelStaticData staticData)
        {
            for (int column = 0; column < staticData.mapSize; column++)
            {
                for (int row = 0; row < staticData.mapSize; row++)
                {
                    if (IstEdge(column, row, staticData.mapSize, staticData.seaInterval))
                    {
                        GenerateTileByType(column, row, TileTypeEnum.Grass, false, TileObjectType.None);
                    }
                    else
                    {
                        GenerateTileByType(column, row, TileTypeEnum.Water, true, TileObjectType.Unavailable);
                    }
                }
            }

            GenerateHouseTile(staticData);
            GenerateCreatureTile(staticData);
            GenerateHeroTile(staticData);
            GenerateIngredientTile(staticData.ingredientsValue);
            GenerateObstaclesByType(TileTypeEnum.Swamp, staticData.swampSize, staticData.mapSize);
            GenerateObstaclesByType(TileTypeEnum.Water, staticData.waterSize, staticData.mapSize);
            GenerateObstaclesByType(TileTypeEnum.Rock, staticData.rocksSize, staticData.mapSize);
        }

        private void GenerateIngredientTile(int amount)
        {
            do
            {
                int randTileNumber = _randomService.Next(0, _mapCoordinates.Count);
                if ((_mapCoordinates[randTileNumber].TileObjectType == TileObjectType.None))
                {
                    _mapCoordinates[randTileNumber].TileObjectType = TileObjectType.Ingredient;
                    amount--;
                }
            } while (amount > 0);
        }

        private void GenerateHeroTile(LevelStaticData staticData)
        {
            _mapCoordinates[staticData.heroPosition].TileObjectType = TileObjectType.Hero;
        }

        private void GenerateCreatureTile(LevelStaticData staticData)
        {
            _mapCoordinates[staticData.creaturePosition].TileObjectType = TileObjectType.Creature;
            foreach (int neighbour in GetNeighbours(staticData.housePosition, staticData.mapSize))
            {
                _mapCoordinates[neighbour].TileObjectType = TileObjectType.Unavailable;
            }
        }

        private void GenerateHouseTile(LevelStaticData staticData)
        {
            _mapCoordinates[staticData.housePosition].TileObjectType = TileObjectType.House;
            foreach (int neighbour in GetNeighbours(staticData.housePosition, staticData.mapSize))
            {
                _mapCoordinates[neighbour].TileObjectType = TileObjectType.House;
            }
        }

        void GenerateTileByType(int column, int row, TileTypeEnum typeEnum, bool isEdge, TileObjectType tileObjectType)
        {
            _mapCoordinates.Add(new MyTile(new Vector3Int(column, row, 0), typeEnum, isEdge, tileObjectType));
        }

        private void GenerateObstaclesByType(TileTypeEnum type, int amount, int mapSize)
        {
            do
            {
                int randTileNumber = _randomService.Next(0, _mapCoordinates.Count);
                int[] neighbours = GetNeighbours(randTileNumber, mapSize);
                for (int i = 0; i < _randomService.Next(0, neighbours.Length); i++)
                {
                    if (amount > 0)
                    {
                        if (_mapCoordinates.Count >= neighbours[i]&&_mapCoordinates.Count >= neighbours[i] - 1 && neighbours[i] >= 0)
                        {
                            if (_mapCoordinates.Count>neighbours[i]&&_mapCoordinates[neighbours[i]].TileObjectType == TileObjectType.None)
                            {
                                _mapCoordinates[neighbours[i]].TileObjectType = TileObjectType.Unavailable;
                                _mapCoordinates[neighbours[i]].Type = type;
                                amount--;
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            } while (amount > 0);
        }

        private bool IstEdge(int column, int row, int mapSize, int seaInterval) =>
            (column >= seaInterval && column < mapSize - seaInterval) && (row >= seaInterval &&
                                                                          row < mapSize - seaInterval);

        private int[] GetNeighbours(int value, int mapSize) =>
            new[]
            {
                value, value + 1, value - 1, value + mapSize, value - mapSize, value + mapSize + 1, value + mapSize - 1
            };
    }
}