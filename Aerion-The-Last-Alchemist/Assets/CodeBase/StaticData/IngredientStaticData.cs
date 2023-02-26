using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "IngredientData", menuName = "Static Data/Ingredient")]
    public abstract class IngredientStaticData : ScriptableObject
    {
        public new string name;
        public AssetReferenceGameObject lootPrefab;
        public Sprite lootIcon;
    }
}