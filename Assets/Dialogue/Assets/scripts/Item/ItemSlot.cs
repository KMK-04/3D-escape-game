using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image icon;              // 기존 Image용
    public RawImage rawIcon;        // 추가된 RawImage용
    public bool isFilled;
    public bool isSelected;
    public ItemData storedItem;
    public Text Name;
    public ExplainUI explainUI;

    public void storeItem(Item item)
    {
        storedItem = new ItemData()
        {
            sprite = item.sprite,
            ITEM_Name = item.ITEM_Name,
            explain = item.explain
        };
    }
    public void SaySelected()
    {
        Debug.Log(isSelected);
    }
    public void SetSlot()
    {
        if (icon != null)
        {
            icon.sprite = storedItem.sprite;
            icon.color = Color.white;
        }

        if (rawIcon != null && storedItem.sprite != null)
        {
            rawIcon.texture = storedItem.sprite.texture;
            rawIcon.color = Color.white;
        }

        if (Name != null)
        {
            Name.text = storedItem.ITEM_Name;
        }

        isFilled = true;
    }

    public void SetAll(Item item)
    {
        storeItem(item);
        SetSlot();
    }

    public void SetAll(ItemData data)
    {
        storedItem = new ItemData()
        {
            sprite = data.sprite,
            ITEM_Name = data.ITEM_Name,
            explain = data.explain
        };
        SetSlot();
    }

    public void ShowExplainExternally()
    {
        if (storedItem != null)
        {
            if (icon != null)
                icon.sprite = storedItem.sprite;

            if (rawIcon != null && storedItem.sprite != null)
                rawIcon.texture = storedItem.sprite.texture;

            if (storedItem.sprite != null)
                explainUI.ShowExplain(storedItem.explain);
            else
                explainUI.ClearExplain();
        }
    }

    public void ClearSlot()
    {
        storedItem = null;

        if (icon != null)
        {
            icon.sprite = null;
            icon.color = Color.white;
        }

        if (rawIcon != null)
        {
            rawIcon.texture = null;
            rawIcon.color = Color.white;
        }

        if (Name != null)
        {
            Name.text = "";
        }

        isFilled = false;

        if (explainUI != null)
        {
            explainUI.ClearExplain();
        }
    }
}
