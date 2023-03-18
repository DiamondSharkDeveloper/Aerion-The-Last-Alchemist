using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Lab
{
    public class IngredientHandler : MonoBehaviour
    {
      [SerializeField]private List<Bubble> _bubbles = new List<Bubble>();
        private readonly List<string> _ingredientsNames = new List<string>();

        private void Start()
        {
            RemoveBubbles();
        }

        public void ActiveBubble(Sprite sprite, string name)
        {
            if ((_ingredientsNames.Count > 4))
            {
                RemoveBubbles();
            }

            _ingredientsNames.Add(name);
            if (_ingredientsNames.Count <= 1)
            {
                foreach (Bubble bubble in _bubbles)
                {
                    bubble.ActivateBubble();
                }
            }

            _bubbles[_ingredientsNames.Count - 1].SetSprite(sprite);
        }

        public void Craft(Action action)
        {
            for (var i = 0; i < _ingredientsNames.Count; i++)
            {
                var count = i;
                _bubbles[count].MoveTo(transform.position, () =>
                {
                    if (count == _ingredientsNames.Count - 1)
                    {
                        action.Invoke();
                    }

                    _bubbles[count].DestroyBubble();
                });
            }
            
        }

        public void RemoveBubbles()
        {
            foreach (Bubble bubble in _bubbles)
            {
                bubble.DestroyBubble();
            }

            if (_ingredientsNames != null) _ingredientsNames.Clear();
        }
    }
}