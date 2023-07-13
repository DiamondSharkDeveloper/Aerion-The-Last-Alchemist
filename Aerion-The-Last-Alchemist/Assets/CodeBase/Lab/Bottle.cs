using CodeBase.Enums;
using UnityEngine;

namespace CodeBase.Lab
{
    public class Bottle : MovingObject
    {
        [SerializeField] private LineRenderer lineRenderer;

        [SerializeField] private Animator animator;

      
        public BaseType type;

        private static readonly int IsOnCatleZone = Animator.StringToHash("IsOnCatleZone");

        void Start()
        {
            startPosition = transform.position;
            if (!lineRenderer)
            {
                lineRenderer = GetComponent<LineRenderer>();
            }

            OnCatle += PourOut;
            animator = GetComponent<Animator>();
            onComplete += () => { animator.SetBool(IsOnCatleZone, false); };
        }


        public void PourOut()
        {
            _isHold = false;
            animator.SetBool(IsOnCatleZone, true);
        }
    }
}