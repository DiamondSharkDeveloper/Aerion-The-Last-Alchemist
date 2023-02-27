using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Lab
{
    public class Laboratory : MonoBehaviour
    {
        [SerializeField] private Button closeButton;

        // Start is called before the first frame update
        void Start()
        {
            closeButton.onClick.AddListener(Close);
        }
        private void Close()
        {
        }
    }
}