using System;
using System.Collections.Generic;
using CodeBase.Map;
using UnityEngine;

namespace CodeBase.Lab
{
    public class IngredientHandler : MonoBehaviour
    {
        [SerializeField] private List<Bubble> bubbles = new List<Bubble>();
        private readonly List<string> _ingredientsNames = new List<string>();
        public Action OnMouseOverClick;
        public Action OnMouseOverCattle;
        

        private void OnMouseOver()
        {
            OnMouseOverCattle?.Invoke();
        }

        private void OnMouseDown()
        {
            OnMouseOverClick?.Invoke();
        }


        public void ActiveBubble(Sprite sprite, string name)
        {
            if (_ingredientsNames.Count>4)
            {
                RemoveBubbles();
            }
            _ingredientsNames.Add(name);
            bubbles[_ingredientsNames.Count - 1].SetSprite(sprite);
        }


        public void RemoveBubbles()
        {
            foreach (Bubble bubble in bubbles)
            {
                bubble.DestroyBubble();
            }

            if (_ingredientsNames != null) _ingredientsNames.Clear();
        }

        public List<string> GetIngredients() => _ingredientsNames;

    }
}