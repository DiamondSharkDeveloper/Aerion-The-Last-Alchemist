using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace CodeBase.Lab
{
    public class Bubble : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _ingredientSpriteRenderer;
        private Vector3 _startPosition;

        private void Start()
        {
            _startPosition = transform.position;
        }

        public void SetSprite(Sprite sprite)
        {
            _ingredientSpriteRenderer.sprite = sprite;
        }

        public void ActivateBubble()
        {
            gameObject.SetActive(true);
            transform.position = _startPosition;
        }

        public void DestroyBubble()
        {
            _ingredientSpriteRenderer.sprite = null;
            gameObject.SetActive(false);
        }

        public void MoveTo(Vector3 at, Action onComplete)
        {
            TweenerCore<Vector3, Vector3, VectorOptions> core = transform.DOMove(at, 3);
            core.onComplete = () => onComplete?.Invoke();
        }
    }
}