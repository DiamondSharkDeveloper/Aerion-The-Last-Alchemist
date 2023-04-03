using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private List<SpriteRenderer> lights = new List<SpriteRenderer>();
        private IPersistentProgressService _persistentProgressService;
        private static readonly int Craft1 = Animator.StringToHash("Craft");
        private static readonly int FinishCraft = Animator.StringToHash("FinishCraft");
        private static readonly int Idle = Animator.StringToHash("Idle");

        public void Init(IWindowService windowService, IPersistentProgressService persistentProgressService)
        {
            _persistentProgressService = persistentProgressService;
            potionHandler.gameObject.SetActive(false);
            bubblesParticleSystem.gameObject.SetActive(false);
            fire.gameObject.SetActive(false);
            SetLightColor(Color.clear);
            kettlePotionsSprite.gameObject.SetActive(false);
            openWindowButton.Init(windowService, data =>
            {
                AddAllIngredients(data);
                StartCraft(data);
            }, false);
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
            kettlePotionsSprite.color = waterColour;
            StartCoroutine(Craft(formulaStaticData));
        }

        private IEnumerator Craft(FormulaStaticData formulaStaticData)
        {
            Color potionColor = GetPotionsColor(formulaStaticData.potionType);

            yield return new WaitForSecondsRealtime(2);
            fire.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(2);
            kettleAnimator.SetTrigger(Craft1);
            yield return new WaitForSecondsRealtime(1);
            kettlePotionsSprite.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(1);
            StartCoroutine(SmoothColourChange(waterColour, potionColor));
            bubblesParticleSystem.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(4);
            kettleAnimator.SetTrigger(FinishCraft);
            fire.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(2);
            bubblesParticleSystem.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(3);
            SetLightColor(Color.clear);
            ingredientHandler.RemoveBubbles();
            kettlePotionsSprite.gameObject.SetActive(false);
            kettleAnimator.SetTrigger(Idle);
            _persistentProgressService.Progress.gameData.lootData.Collect(new Loot(formulaStaticData.name,1));

        StopAllCoroutines();
        }

        public Color GetPotionsColor(PotionType potionType)
        {
            switch (potionType)
            {
                case PotionType.Blue:
                    return Color.cyan;
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

        public IEnumerator SmoothColourChange(Color startColor, Color targetColour)
        {
            Color color;
            for (float i = 0; i < 1; i += Time.deltaTime / 4)
            {
                
                color=kettlePotionsSprite.color = bubblesParticleSystem.startColor = Color.Lerp(startColor, targetColour, i);
                SetLightColor(Color.Lerp(new Color(startColor.r,startColor.g,startColor.b, 0.001f),new Color(color.r,color.g,color.b,0.3f), i));
                yield return null;
            }
        }

        public void SetLightColor(Color color)
        {
            for (int j = 0; j < lights.Count; j++)
            {
                lights[j].color = color;
            }
        }
    }
}