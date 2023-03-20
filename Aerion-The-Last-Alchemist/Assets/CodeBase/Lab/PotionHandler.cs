using UnityEngine;

namespace CodeBase.Lab
{
    public class PotionHandler : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _potionImage;

        public void SetPotionImage(Sprite sprite)
        {
            _potionImage.sprite = sprite;
        }
    }
}