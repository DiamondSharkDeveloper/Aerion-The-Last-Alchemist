using System;
using System.Collections.Generic;
using System.Linq;
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
        private IRandomService _randomService;
        private readonly List<MyTile> _mapCoordinates = new List<MyTile>();

        public LevelGenerator(IStaticDataService staticDataService, IRandomService randomService)
        {
            _randomService = randomService;
            GenerateMap(staticDataService.ForLevel("forest"));
        }


        public List<MyTile> GetMap(LevelStaticData staticData)
        {
            // if (_mapCoordinates != null && _mapCoordinates.Count != 0)
            // {
            //     return _mapCoordinates;
            // }

            GenerateMap(staticData);
            return _mapCoordinates;
        }

        private void GenerateMap(LevelStaticData staticData)
        {
            if (_mapCoordinates?.Count != 0)
            {
                _mapCoordinates.Clear();
            }
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
                        GenerateTileByType(column, row, TileTypeEnum.Grass, true, TileObjectType.Edge);
                    }
                }
            }

            foreach (CreatureTypeId creatureTypeId in staticData.creaturesType)
            {
                GenerateCreatureTile(staticData);
            }

            GenerateHouseTile(staticData);
            GenerateHeroTile(staticData);
            GenerateIngredientTile(staticData.ingredientsValue);
            GenerateObstaclesByType(TileTypeEnum.Grass, staticData.flowerSize, staticData.mapSize,true, TileObjectType.Flower);
            GenerateObstaclesByType(TileTypeEnum.Grass, staticData.flowerSize, staticData.mapSize,true, TileObjectType.Grass);
            GenerateObstaclesByType(TileTypeEnum.Grass, staticData.mushroomSize, staticData.mapSize,true, TileObjectType.Mushrooms);
            GenerateObstaclesByType(TileTypeEnum.Grass, staticData.treesSize, staticData.mapSize,true, TileObjectType.Trees);
            GenerateObstaclesByType(TileTypeEnum.Swamp, staticData.swampSize, staticData.mapSize,false);
            GenerateObstaclesByType(TileTypeEnum.Water, staticData.waterSize, staticData.mapSize,true);
            GenerateObstaclesByType(TileTypeEnum.Rock, staticData.rocksSize, staticData.mapSize,true, TileObjectType.Rock);
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
            foreach (int neighbour in GetNeighbours(staticData.heroPosition, staticData.mapSize))
            {
                _mapCoordinates[neighbour].TileObjectType = TileObjectType.Unavailable;
            }

            _mapCoordinates[staticData.heroPosition].TileObjectType = TileObjectType.Hero;
        }

        private void GenerateCreatureTile(LevelStaticData staticData)
        {
            int randTileNumber = _randomService.Next(0, _mapCoordinates.Count);
            if (_mapCoordinates[randTileNumber].TileObjectType != TileObjectType.None)
            {
                GenerateCreatureTile(staticData);
                return;
            }

            List<int> neighbours = GetNeighbours(randTileNumber, staticData.mapSize);
            foreach (int neighbour in neighbours)
            {
                if (_mapCoordinates[neighbour].TileObjectType != TileObjectType.None)
                {
                    GenerateCreatureTile(staticData);
                    return;
                }
            }

            foreach (int neighbour in neighbours)
            {
                _mapCoordinates[neighbour].TileObjectType = TileObjectType.Unavailable;
            }

            _mapCoordinates[randTileNumber].TileObjectType = TileObjectType.Creature;
        }

        private void GenerateHouseTile(LevelStaticData staticData)
        {
            foreach (int neighbour in GetNeighbours(staticData.housePosition, staticData.mapSize))
            {
                _mapCoordinates[neighbour].TileObjectType = TileObjectType.House;
            }
        }

        void GenerateTileByType(int column, int row, TileTypeEnum typeEnum, bool isEdge, TileObjectType tileObjectType)
        {
            _mapCoordinates.Add(new MyTile(new Vector3Int(column, row, 0), typeEnum, isEdge, tileObjectType));
        }

        private void GenerateObstaclesByType(TileTypeEnum type, int amount, int mapSize, bool OnEdge,
            TileObjectType objectType = TileObjectType.Unavailable)
        {
            do
            {
                
                int randTileNumber = _randomService.Next(0, _mapCoordinates.Count);
                List<int> neighbours = GetNeighbours(randTileNumber, mapSize);
                int trees=_randomService.Next(0, 3);
                
                for (int i = 0; i < neighbours.Count; i++)
                {
                    if (amount > 0)
                    {
                        if (_mapCoordinates[neighbours[i]].TileObjectType == TileObjectType.None||OnEdge&&_mapCoordinates[neighbours[i]].TileObjectType == TileObjectType.Edge)
                        {
                            _mapCoordinates[neighbours[i]].TileObjectType = objectType;
                            _mapCoordinates[neighbours[i]].Type = type;
                            switch (objectType)
                            {
                                case TileObjectType.Trees:
                                    switch (trees)
                                    {
                                        case 0:
                                            _mapCoordinates[neighbours[i]].FloraTipe = FloraTipe.Palm;
                                            break;  
                                        case 1:
                                            _mapCoordinates[neighbours[i]].FloraTipe = FloraTipe.Pine;
                                            break; 
                                        case 2:
                                            _mapCoordinates[neighbours[i]].FloraTipe = FloraTipe.Xmas;
                                            break;
                                    }
                                    break;
                                case TileObjectType.Mushrooms:
                                        _mapCoordinates[neighbours[i]].FloraTipe = FloraTipe.Mushroom;
                                    break;
                                case TileObjectType.Flower:
                                    _mapCoordinates[neighbours[i]].FloraTipe = FloraTipe.Flower;
                                    break;
                                case TileObjectType.Grass:
                                    _mapCoordinates[neighbours[i]].FloraTipe = FloraTipe.Grass;
                                    break;
                            }
                          
                            amount--;
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

        private List<int> GetNeighbours(int value, int mapSize)
        {
            List<int> neighbours = new List<int>
            {
                value, value + 1,
                value - 1,
                value + mapSize,
                value - mapSize,
                value + mapSize + 1,
                value + mapSize - 1
            };
            List<int> result = new List<int>();
            foreach (int neighbour in neighbours)
            {
                if (neighbour < _mapCoordinates.Count && neighbour >= 0)
                {
                    result.Add(neighbour);
                }
            }

            return result;
        }
    }
}