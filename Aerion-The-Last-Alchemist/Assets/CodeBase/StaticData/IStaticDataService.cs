using System.Collections.Generic;
using CodeBase.Enums;
using CodeBase.StaticData;
using CodeBase.StaticData.Windows;

namespace CodeBase.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        void Load();
        MonsterStaticData ForMonster(CreatureTypeId typeId);
        LevelStaticData ForLevel(string sceneKey);

        WindowConfig ForWindow(WindowId inventoryId);
        Dictionary<string,IngredientStaticData> ForIngredients();
        List<FormulaStaticData> ForFormulas();
    }
}