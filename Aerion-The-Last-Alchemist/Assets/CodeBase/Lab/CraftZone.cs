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
        [SerializeField] private Color waterColour;
        private IPersistentProgressService _persistentProgressService;
        private static readonly int Craft1 = Animator.StringToHash("Craft");
        private static readonly int FinishCraft = Animator.StringToHash("FinishCraft");
        private static readonly int Idle = Animator.StringToHash("Idle");

        public void Init(IWindowService windowService, IPersistentProgressService persistentProgressService)
        {
            openWindowButton.Init(windowService, AddAllIngredients, false);
            potionHandler.gameObject.SetActive(false);
            bubblesParticleSystem.gameObject.SetActive(false);
            fire.gameObject.SetActive(false);
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
            ingredientHandler.RemoveBubbles();
            foreach (IngredientStaticData ingredientStaticData in formulaStaticData.ingredients)
            {
                ingredientHandler.ActiveBubble(ingredientStaticData.lootIcon, ingredientStaticData.name);
            }
        }

        [Obsolete("Obsolete")]
        private void StartCraft(FormulaStaticData formulaStaticData)
        {
            potionHandler.SetPotionImage(formulaStaticData.sprite);
           

          StartCoroutine(Craft(formulaStaticData));
        }

        private IEnumerator Craft(FormulaStaticData formulaStaticData)
        {
            Color potionColor = GetPotionsColor(formulaStaticData.potionType);
           
            yield return new WaitForSecondsRealtime(2);
            fire.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(2);
            kettleAnimator.SetTrigger(Craft1);
            yield return new WaitForSecondsRealtime(2);
            StartCoroutine(SmoothColourChange(waterColour, potionColor));
            bubblesParticleSystem.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(6);
            kettleAnimator.SetTrigger(FinishCraft);
          
            fire.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(2);
            bubblesParticleSystem.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(3);
            
            kettlePotionsSprite.color =  waterColour;;
            ingredientHandler.RemoveBubbles();
            kettleAnimator.SetTrigger(Idle);
            StopAllCoroutines();
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

        public IEnumerator SmoothColourChange(Color startColor,Color targetColour)
        {
         
            for (float i = 0; i < 1; i+=Time.deltaTime/4)
            {
                kettlePotionsSprite.color = bubblesParticleSystem.startColor = Color.Lerp(startColor, targetColour, i);
                yield return null;
            }
        
        }
    }
}