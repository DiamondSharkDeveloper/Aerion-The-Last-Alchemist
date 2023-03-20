using System;
using System.Collections.Generic;
using CodeBase.Lab;
using UnityEngine;

public class IngredientHandler : MonoBehaviour
{
   
    [SerializeField]  private  List<Bubble> _bubbles = new List<Bubble>();
    private readonly List<string> _ingredientsNames = new List<string>();

    public void ActiveBubble(Sprite sprite, string name)
    {
        _ingredientsNames.Add(name);
        _bubbles[_ingredientsNames.Count - 1].SetSprite(sprite);
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