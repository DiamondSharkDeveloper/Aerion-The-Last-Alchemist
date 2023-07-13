using System;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Lab
{
    public class MovingObject : MonoBehaviour
    {
        protected Vector3 startPosition;
        public Action<MovingObject> OnClick;
        protected Action onComplete;
        protected bool _isHold;
        public Action OnCatle;
        [SerializeField] private new Camera camera;

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
            move.OnComplete(() => { onComplete?.Invoke(); });
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
    }
}