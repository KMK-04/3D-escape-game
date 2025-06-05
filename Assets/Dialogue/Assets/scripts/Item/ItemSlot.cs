using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemSlot : MonoBehaviour
{
    public Image icon;
    public bool isFilled;
    public ItemData storedItem;
    public Text Name;
    public ExplainUI explainUI; // 다른 스크립트 연결
    public void storeItem(Item item)
    {
        storedItem = new ItemData()
        {
            sprite = item.sprite,
            ITEM_Name = item.ITEM_Name,
            explain = item.explain
        };
    }
    public void SetSlot()
    {
        icon.sprite = storedItem.sprite;
        icon.color = Color.white;
        Name.text = storedItem.ITEM_Name;
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
            icon.sprite = storedItem.sprite;
            if (icon.sprite != null)
                explainUI.ShowExplain(storedItem.explain);
            else
                explainUI.ClearExplain();
        }

    }

    public void ClearSlot()
    {
        storedItem = null;
        icon.sprite = null;
        icon.color = Color.white;
        Name.text = "";
        isFilled = false;

        if (explainUI != null)
        {
            explainUI.ClearExplain();
        }
    }
}
