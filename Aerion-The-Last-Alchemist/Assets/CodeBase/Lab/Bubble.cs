using UnityEngine;

namespace CodeBase.Lab
{
    public class Bubble : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer ingredientSpriteRenderer;

        public void SetSprite(Sprite sprite)
        {
            ingredientSpriteRenderer.sprite = sprite;
        }

        public void DestroyBubble()
        {
            ingredientSpriteRenderer.sprite = null;
           
        }
        
    }
}