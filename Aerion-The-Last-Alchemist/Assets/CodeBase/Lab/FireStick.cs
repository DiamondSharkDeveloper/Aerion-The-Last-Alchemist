using UnityEngine;

namespace CodeBase.Lab
{
    public class FireStick:MovingObject
    {
        [SerializeField] private GameObject fire;
        void Start()
        {
            startPosition = transform.position;
           
            fire.SetActive(false);
            OnClick += o => { fire.SetActive(true);}; 
            onComplete += () => {fire.SetActive(false);};
        }
    }
}