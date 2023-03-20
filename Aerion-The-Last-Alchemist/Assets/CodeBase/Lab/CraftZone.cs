using System;
using System.Collections;
using CodeBase.Enums;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Windows;
using UnityEngine;

namespace CodeBase.Lab
{
    public class CraftZone : MonoBehaviour
    {
        [SerializeField] private Animator kettleAnimator;
        [SerializeField] private SpriteRenderer kettlePotionsSprite;
        [SerializeField] private ParticleSystem bubblesParticleSystem;
        [SerializeField] private GameObject fire;
        [SerializeField] private IngredientHandler ingredientHandler;
        [SerializeField] private OpenWindowButton openWindowButton;
        [SerializeField] private PotionHandler potionHandler;
        private IPersistentProgressService _persistentProgressService;
        private static readonly int Craft1 = Animator.StringToHash("Craft");
        private static readonly int FinishCraft = Animator.StringToHash("FinishCraft");

        public void Init(IWindowService windowService, IPersistentProgressService persistentProgressService)
        {
            openWindowButton.Init(windowService, AddAllIngredients, false);
            potionHandler.gameObject.SetActive(false);
            bubblesParticleSystem.gameObject.SetActive(false);
        }

        [Obsolete("Obsolete")]
        public void Init(IWindowService windowService, IPersistentProgressService persistentProgressService,
            FormulaStaticData formulaStaticData)
        {
            Init(windowService, persistentProgressService);
            AddAllIngredients(formulaStaticData);
            StartCraft(formulaStaticData);
        }

        private void AddAllIngredients(FormulaStaticData formulaStaticData)
        {
            foreach (IngredientStaticData ingredientStaticData in formulaStaticData.ingredients)
            {
                ingredientHandler.ActiveBubble(ingredientStaticData.lootIcon, ingredientStaticData.name);
            }
        }

        [Obsolete("Obsolete")]
        private void StartCraft(FormulaStaticData formulaStaticData)
        {
            Color potionColor = GetPotionsColor(formulaStaticData.potionType);
            kettlePotionsSprite.color = potionColor;
            bubblesParticleSystem.startColor = potionColor;
            bubblesParticleSystem.gameObject.SetActive(true);
            fire.gameObject.SetActive(true);
            ingredientHandler.Craft(() => { StartCoroutine(Craft()); });
        }

        private IEnumerator Craft()
        {
            yield return new WaitForSecondsRealtime(4);
            fire.gameObject.SetActive(false);
            bubblesParticleSystem.gameObject.SetActive(false);
            kettleAnimator.SetTrigger(Craft1);
            yield return new WaitForSecondsRealtime(4);
            kettleAnimator.SetTrigger(FinishCraft);
        }

        public Color GetPotionsColor(PotionType potionType)
        {
            switch (potionType)
            {
                case PotionType.Blue:
                    return Color.blue;
                    break;
                case PotionType.Green:
                    return Color.green;
                    break;
                case PotionType.Red:
                    return Color.red;
                    break;
                case PotionType.Yellow:
                    return Color.yellow;
                    break;
            }

            return Color.white;
        }
    }
}