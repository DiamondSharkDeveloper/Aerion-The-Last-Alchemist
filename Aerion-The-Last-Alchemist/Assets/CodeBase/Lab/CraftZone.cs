using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Enums;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Windows;
using DG.Tweening;
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
        [SerializeField] private OpenWindowButton openFormulaButton;
        [SerializeField] private OpenWindowButton openIngredientButton;
        [SerializeField] private PotionHandler potionHandler;
        [SerializeField] private Color waterColour;
        [SerializeField] private List<SpriteRenderer> lights = new List<SpriteRenderer>();
        [SerializeField] private List<Bottle> bottles = new List<Bottle>();
        [SerializeField] private FireStick _fireStick;
        [SerializeField]private Sprite _empty;
        [SerializeField]private Sprite _poison;
        private IPersistentProgressService _persistentProgressService;
        private BaseType _waterBase;
        private IStaticDataService _staticDataService;
        private PotionCrafter _potionCrafter;
        private static readonly int Craft1 = Animator.StringToHash("Craft");
        private static readonly int FinishCraft = Animator.StringToHash("FinishCraft");
        private static readonly int Idle = Animator.StringToHash("Idle");
        private MovingObject _currentObject;
        private bool _isCattleEmpty = true;

        public void Init(IWindowService windowService, IPersistentProgressService persistentProgressService,
            IStaticDataService staticDataService)
        {
            _potionCrafter = new PotionCrafter(staticDataService.ForFormulas(),_empty,_poison);
            _staticDataService = staticDataService;
            _persistentProgressService = persistentProgressService;
            potionHandler.gameObject.SetActive(false);
            bubblesParticleSystem.gameObject.SetActive(false);
            fire.gameObject.SetActive(false);
            kettlePotionsSprite.gameObject.SetActive(false);
            SetLightColor(new Color(1, 1, 1, 0));
            kettlePotionsSprite.color *= new Color(1, 1, 1, 0);
            openIngredientButton.Init(windowService);
            openFormulaButton.Init(windowService, data => { }, false);
            foreach (Bottle bottle in bottles)
            {
                bottle.OnClick += bottle1 => { _currentObject = bottle1; };
                bottle.OnCatle += () =>
                {
                    if (bottle != null) _waterBase = bottle.type;
                    kettlePotionsSprite.color = GetPotionsBaseColor(_waterBase);
                    StartCoroutine(SmoothCattleFill());
                    _isCattleEmpty = false;
                    _currentObject = null;
                };
            }

            _fireStick.OnClick += o =>
            {
                _currentObject = o;
            }; 
            _fireStick.OnCatle += () =>
            {
                _currentObject = null;
                StartCraft();
            };


            ingredientHandler.OnMouseOverClick += () =>
            {
                Loot loot = persistentProgressService.Progress.gameData.lootData._onHoldLoot;
                if (loot != null)
                {
                    if (_staticDataService.ForIngredients().ContainsKey(loot.name))
                    {
                        persistentProgressService.Progress.gameData.lootData.Use();
                        ingredientHandler.ActiveBubble(staticDataService.ForIngredients()[loot.name].lootIcon,
                            loot.name);
                    }

                    persistentProgressService.Progress.gameData.lootData.LetGo();
                }
            };
            ingredientHandler.RemoveBubbles();
            ingredientHandler.OnMouseOverCattle += () =>
            {
                _currentObject?.OnCatle?.Invoke();
                
            };
        }

        [Obsolete("Obsolete")]
        public void Init(IWindowService windowService, IPersistentProgressService persistentProgressService,
            FormulaStaticData formulaStaticData, IStaticDataService staticDataService)
        {
            Init(windowService, persistentProgressService, staticDataService);
            AddAllIngredients(formulaStaticData);
            StartCraft();
        }

        private void AddAllIngredients(FormulaStaticData formulaStaticData)
        {
            foreach (Bottle bottle in bottles)
            {
                if (bottle.type == formulaStaticData.baseType)
                {
                    bottle.transform.DOMoveY( 0.9f, 2f);
                    bottle.OnCatle.Invoke();
                }
            }

            ingredientHandler.RemoveBubbles();
            foreach (IngredientStaticData ingredientStaticData in formulaStaticData.ingredients)
            {
                ingredientHandler.ActiveBubble(ingredientStaticData.lootIcon, ingredientStaticData.name);
            }
        }

        [Obsolete("Obsolete")]
        private void StartCraft()
        {
            fire.gameObject.SetActive(true);
            FormulaStaticData formulaStaticData = ScriptableObject.CreateInstance<FormulaStaticData>();

            formulaStaticData =
                _potionCrafter.CheckFormula(ingredientHandler.GetIngredients(), _waterBase, _isCattleEmpty);

            potionHandler.SetPotionImage(formulaStaticData.sprite);


            StartCoroutine(Craft(formulaStaticData));
        }

        private IEnumerator Craft(FormulaStaticData formulaStaticData)
        {
            Color potionColor = GetPotionsColor(formulaStaticData.potionType);
            if (!string.IsNullOrEmpty(formulaStaticData.name))
            {
             
                switch (formulaStaticData.name)
                {
                    case "poison":
                        potionColor = new Color(0.31f, 0.3f, 0.145f, 1);
                        break;
                    case "empty":
                        potionColor =  kettlePotionsSprite.color;
                        break;
                    default:
                        break;
                }
                
                _persistentProgressService.Progress.gameData.lootData.Collect(new Loot(formulaStaticData.name, 1));
              
            }
            else
            {
                
                potionColor = new Color(0, 0, 0, 0);
            }
            
           
            yield return new WaitForSecondsRealtime(2);
            kettleAnimator.SetTrigger(Craft1);
            yield return new WaitForSecondsRealtime(1);
            StartCoroutine(SmoothColourChange(kettlePotionsSprite.color, potionColor));
            bubblesParticleSystem.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(4);
            kettleAnimator.SetTrigger(FinishCraft);
            fire.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(2);
            bubblesParticleSystem.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(3);
            kettlePotionsSprite.gameObject.SetActive(false);
            SetLightColor(new Color(1, 1, 1, 0));
            ingredientHandler.RemoveBubbles();
            kettleAnimator.SetTrigger(Idle);
            _isCattleEmpty = true;
            ingredientHandler.RemoveBubbles();
            StopAllCoroutines();
        }

        public Color GetPotionsColor(PotionType potionType)
        {
            switch (potionType)
            {
                case PotionType.Blue:
                    return new Color(0.57f, 0.36f, 0.96f, 1);
                    break;
                case PotionType.Green:
                    return new Color(0.12f, 0.71f, 0.05f, 1);
                    break;
                case PotionType.Red:
                    return new Color(0.785f, 0.055f, 0.475f, 1);
                    break;
                case PotionType.Yellow:
                    return new Color(0.91f, 0.8f, 0f, 1);
                    break;
            }

            return Color.white;
        }

        public Color GetPotionsBaseColor(BaseType baseType)
        {
            switch (baseType)
            {
                case BaseType.Water:
                    return new Color(0, 0.57f, 1, 1);
                    break;
                case BaseType.Alcohol:
                    return new Color(0.12f, 0.71f, 0.05f, 1);
                    break;
                case BaseType.Acid:
                    return new Color(0.785f, 0.055f, 0.475f, 1);
                    break;
                case BaseType.Oil:
                    return new Color(0.91f, 0.8f, 0f, 1);
                    break;
            }

            return waterColour;
        }

        public IEnumerator SmoothColourChange(Color startColor, Color targetColour)
        {
            Color color;
            for (float i = 0; i < 1; i += Time.deltaTime / 4)
            {
                color = kettlePotionsSprite.color =
                    bubblesParticleSystem.startColor = Color.Lerp(startColor, targetColour, i);
                SetLightColor(Color.Lerp(new Color(startColor.r, startColor.g, startColor.b, 0.1f),
                    new Color(color.r, color.g, color.b, 1f), i));
                yield return null;
            }
        }

        public IEnumerator SmoothCattleFill()
        {
            kettlePotionsSprite.transform.localScale = Vector3.zero;
            kettlePotionsSprite.gameObject.SetActive(true);
            Transform transform1 = kettlePotionsSprite.transform;
            yield return new WaitForSecondsRealtime(1.9f);

            for (float i = 0; i < 1; i += Time.deltaTime * 1.2f)
            {
                transform1.localScale =
                    Vector3.Lerp(new Vector3(0, 0, 1), new Vector3(1.4f, 1.4f, 1), i);
                var position = transform1.localPosition;
                position = Vector3.Lerp(
                    new Vector3(position.x, 3, position.z),
                    new Vector3(position.x, 4.78f,
                        position.z), i);
                kettlePotionsSprite.transform.localPosition = position;
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