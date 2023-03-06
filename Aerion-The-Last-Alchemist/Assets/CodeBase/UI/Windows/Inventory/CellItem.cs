using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellItem:MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _itemCountText;

    public void SetItemSprite(Sprite sprite)
    {
        _itemImage.sprite = sprite;
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
}