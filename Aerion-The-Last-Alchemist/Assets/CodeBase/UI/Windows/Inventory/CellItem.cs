using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Inventory
{
    public class CellItem:MonoBehaviour
    {
        [SerializeField] private Image _itemImage;
        [SerializeField] private Image _itemBackImage;
        [SerializeField] private TextMeshProUGUI _itemCountText;
        private string _id;
        public Action<Sprite,string>OnClick;

        private void Start()
        {
            gameObject.AddComponent<Button>().onClick.AddListener(() => OnClick?.Invoke(_itemImage.sprite,_id));
        }

        public void SetItemBackSprite(Sprite sprite)
        {
            _itemBackImage.sprite = sprite;
        } public void SetItem(Sprite sprite,string id)
        {
            _itemImage.sprite = sprite;
            _id = id;
        }

        public void SetItemCount(int count)
        {
            _itemImage.color = count > 0 ? Color.white : Color.grey;
            _itemCountText.text = count.ToString();
        }
        

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            OnClick = null;
        }
    }
}