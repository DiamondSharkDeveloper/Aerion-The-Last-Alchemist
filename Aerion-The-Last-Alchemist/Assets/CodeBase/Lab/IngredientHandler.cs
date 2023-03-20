using System;
using System.Collections.Generic;
using CodeBase.Lab;
using UnityEngine;

public class IngredientHandler : MonoBehaviour
{
    [SerializeField] private List<Vector3> positions = new List<Vector3>();
    private readonly List<Bubble> _bubbles = new List<Bubble>();
    private readonly List<string> _ingredientsNames = new List<string>();

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

        foreach (Bubble bubble in _bubbles)
        {
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