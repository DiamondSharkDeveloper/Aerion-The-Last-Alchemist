using UnityEngine;

namespace CodeBase.Lab
{
    public class Bubble : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer ingredientSpriteRenderer;
        [SerializeField] private SpriteRenderer bubbleSpriteRenderer;

        public void SetSprite(Sprite sprite)
        {
            bubbleSpriteRenderer.enabled = true;
            ingredientSpriteRenderer.sprite = sprite;
        }

        public void DestroyBubble()
        {
            bubbleSpriteRenderer.enabled = false;
            ingredientSpriteRenderer.sprite = null;
           
        }
        
    }
}