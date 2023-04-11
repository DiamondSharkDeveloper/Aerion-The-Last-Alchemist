using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.UI.Windows.Inventory.Formula
{
    public class MyPage:MonoBehaviour
    {
        [SerializeField] protected CellItem potion;
        [SerializeField] protected List<CellItem> items;
        private bool _isFake;
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void MakePageFace()
        {
            foreach (CellItem cellItem in items)
            {
                cellItem.enabled=false;
            }
        }
    }
}