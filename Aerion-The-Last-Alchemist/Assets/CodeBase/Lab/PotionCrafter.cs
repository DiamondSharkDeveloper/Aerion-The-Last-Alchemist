using System.Collections.Generic;
using CodeBase.Enums;
using CodeBase.StaticData;
using UnityEngine.PlayerLoop;

namespace CodeBase.Lab
{
    public class PotionCrafter
    {
        private Dictionary<BaseType, List<FormulaStaticData>> _formulaStaticDataDic =
            new Dictionary<BaseType, List<FormulaStaticData>>();

        public PotionCrafter(List<FormulaStaticData> formulaStaticDatas)
        {
            foreach (FormulaStaticData formulaStaticData in formulaStaticDatas)
            {
                if (_formulaStaticDataDic.ContainsKey(formulaStaticData.baseType))
                {
                    _formulaStaticDataDic.Add(formulaStaticData.baseType,
                        new List<FormulaStaticData>(formulaStaticDatas));
                }
                else
                {
                    _formulaStaticDataDic[formulaStaticData.baseType].Add(formulaStaticData);
                }
            }
        }

        public string CheckFormula(List<string> ingredients, BaseType baseType)
        {
            foreach (string ingredient in ingredients)
            {
                foreach (FormulaStaticData formulaStaticData in _formulaStaticDataDic[baseType])
                {
                    int count = 0;
                    foreach (IngredientStaticData ingredientStaticData in formulaStaticData.ingredients)
                    {
                        if (ingredients.Contains(ingredientStaticData.name))
                        {
                            count++;
                        }
                    }

                    if (count == formulaStaticData.ingredients.Count)
                    {
                        return formulaStaticData.name;
                    }
                }
            }

            return "";
        }
    }
}