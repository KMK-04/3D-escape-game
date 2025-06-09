using UnityEngine;
using System.Collections.Generic;

public class ItemSelect : MonoBehaviour
{
    public List<ItemSlot> itemSlots = new List<ItemSlot>();

    private ItemSlot selectedSlot = null;

    public void RegisterSlot(ItemSlot slot)
    {
        if (!itemSlots.Contains(slot))
        {
            itemSlots.Add(slot);
        }
    }

    public void UnregisterSlot(ItemSlot slot)
    {
        if (itemSlots.Contains(slot))
        {
            itemSlots.Remove(slot);
        }
    }

    public void SelectSlot()
    {
        // 모든 슬롯 해제
        foreach (ItemSlot slot in itemSlots)
        {
            slot.isSelected = false;
        }

        // 현재 슬롯만 선택
        gameObject.GetComponent<ItemSlot>().isSelected = true;
        selectedSlot = gameObject.GetComponent<ItemSlot>();
    }

    public ItemSlot GetSelectedSlot()
    {
        return selectedSlot;
    }
}