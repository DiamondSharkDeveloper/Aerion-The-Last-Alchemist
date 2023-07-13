using System.Collections.Generic;
using CodeBase.Enums;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Lab
{
    public class PotionCrafter
    {
        private Dictionary<BaseType, List<FormulaStaticData>> _formulaStaticDataDic =
            new Dictionary<BaseType, List<FormulaStaticData>>();

        private Sprite _empty;
        private Sprite _poison;

        public PotionCrafter(List<FormulaStaticData> formulaStaticDatas, Sprite empty, Sprite poison)
        {
            _empty = empty;
            _poison = poison;
            foreach (FormulaStaticData formulaStaticData in formulaStaticDatas)
            {
                if (!_formulaStaticDataDic.ContainsKey(formulaStaticData.baseType))
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

        public FormulaStaticData CheckFormula(List<string> ingredients, BaseType baseType, bool isCattleEmpty)
        {
            FormulaStaticData formulaStaticData = new FormulaStaticData();
            if (isCattleEmpty)
            {
                formulaStaticData.sprite = _empty;
               
                return formulaStaticData;
            }

            if (ingredients == null && ingredients.Count == 0)
            {
                formulaStaticData.sprite = _empty;
                formulaStaticData.name = "emty";
                return formulaStaticData;
            }

            foreach (FormulaStaticData data in _formulaStaticDataDic[baseType])
            {
                int count = 0;
                foreach (IngredientStaticData ingredientStaticData in data.ingredients)
                {
                    if (ingredients.Contains(ingredientStaticData.name))
                    {
                        count++;
                    }
                }

                if (count == data.ingredients.Count)
                {
                    return data;
                }
            }

            formulaStaticData.sprite = _poison;
            formulaStaticData.name = "poison";
            return formulaStaticData;
        }
    }
}