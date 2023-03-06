using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "IngredientData", menuName = "Static Data/Ingredient")]
    public class IngredientStaticData : ScriptableObject
    {
        public new string name;
        public Sprite lootIcon;
    }
}