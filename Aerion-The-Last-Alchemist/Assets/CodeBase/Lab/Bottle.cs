using System;
using CodeBase.Enums;
using CodeBase.Map;
using CodeBase.Services.PersistentProgress;
using DG.Tweening;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace CodeBase.Lab
{
    public class Bottle : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Vector3 startPosition;
        [SerializeField] private Animator animator;
        [SerializeField] private new Camera camera;
        public Action OnPourOut;
        public Action<Bottle> OnClick;
        public BaseType type;
        private bool _isHold;
        private static readonly int IsOnCatleZone = Animator.StringToHash("IsOnCatleZone");

        void Start()
        {
            startPosition = transform.position;
            if (!lineRenderer)
            {
                lineRenderer = GetComponent<LineRenderer>();
            }

            OnPourOut += PourOut;
            animator = GetComponent<Animator>();
        }


        private void OnMouseDown()
        {
            if (!_isHold)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    OnClick?.Invoke(this);
                    transform.position -= new Vector3(0, 0, 1);
                    _isHold = true;
                }
            }
        }

        public void GoToStartPosition()
        {
            var move = transform.DOMove(startPosition, 1f);
            move.OnComplete(() => { animator.SetBool(IsOnCatleZone, false); });
        }


        private void Update()
        {
            if (_isHold)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    Release();
                    GoToStartPosition();
                    return;
                }

                Vector3 worldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
                transform.position = new Vector3(worldPosition.x, worldPosition.y, transform.position.z);
            }
        }

        public void Release()
        {
            transform.position += new Vector3(0, 0, 1);
            _isHold = false;
        }

        public void PourOut()
        {
            _isHold = false;
            animator.SetBool(IsOnCatleZone, true);
        }
    }
}