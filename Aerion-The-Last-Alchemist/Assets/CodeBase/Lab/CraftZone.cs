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
        [SerializeField] private ParticleSystem bubblesParticleSystem;
        [SerializeField] private GameObject fire;
        [SerializeField] private IngredientHandler _ingredientHandler;
        [SerializeField] private OpenWindowButton _openWindowButton;

        public void Init(IWindowService windowService,IPersistentProgressService persistentProgressService)
        {
            _openWindowButton.Init(windowService);
        }
        public void Init(IWindowService windowService, IPersistentProgressService persistentProgressService,
            FormulaStaticData formulaStaticData)
        {
            _openWindowButton.Init(windowService);
        }
        
    }
}