using UnityEngine;

namespace CodeBase.UI.Windows.Inventory.Formula
{
    public class MyPage:MonoBehaviour
    {
        [SerializeField] protected CellItem potion;
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}