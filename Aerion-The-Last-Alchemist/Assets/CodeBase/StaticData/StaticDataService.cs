using System.Collections.Generic;
using System.Linq;
using CodeBase.Enums;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Windows;
using UnityEngine;

namespace CodeBase.StaticData
{
    internal class StaticDataService : IStaticDataService
    {
        private const string LevelsDataPath = "Static Data/Levels";
        private const string StaticDataWindowPath = "Static Data/UI/WindowStaticData";
        private const string StaticDataIngredientsPath = "Static Data/Ingredients";
        private const string StaticDataFormulasPath = "Static Data/Formulas";
        private Dictionary<string, LevelStaticData> _levels;
        private List<IngredientStaticData> _ingredients; 
        private List<FormulaStaticData> _formulas; 
        private Dictionary<WindowId, WindowConfig> _windowConfigs;

        public void Load()
        {
            
            _windowConfigs = Resources
                .Load<WindowStaticData>(StaticDataWindowPath)
                .configs
                .ToDictionary(x => x.WindowId, x => x);
            _levels = Resources
                .LoadAll<LevelStaticData>(LevelsDataPath)
                .ToDictionary(x => x.levelKey, x => x);
            _ingredients = Resources.LoadAll<IngredientStaticData>(StaticDataIngredientsPath).ToList();
            _formulas=Resources.LoadAll<FormulaStaticData>(StaticDataFormulasPath).ToList();
        }

        public MonsterStaticData ForMonster(CreatureTypeId typeId)
        {
            throw new System.NotImplementedException();
        }

        public LevelStaticData ForLevel(string sceneKey) =>
            _levels.TryGetValue(sceneKey, out LevelStaticData staticData)
                ? staticData
                : null;

        public WindowConfig ForWindow(WindowId windowId) =>
            _windowConfigs.TryGetValue(windowId, out WindowConfig windowConfig)
                ? windowConfig
                : null;

        public List<IngredientStaticData> ForIngredients() => _ingredients;
        public List<FormulaStaticData> ForFormulas() => _formulas;
    }
}