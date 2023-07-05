using System.Collections.Generic;
using CodeBase.Enums;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "FormulaData", menuName = "Static Data/Formula")]
    public class FormulaStaticData : ScriptableObject
    {
        public Sprite sprite;
        public List<IngredientStaticData> ingredients=new List<IngredientStaticData>();
        public new string name;
        public int efect;
        public PotionType potionType;
        public BaseType baseType;
        [Range(1,4)]
        public int potionLevel;
        public string description;
    }
}