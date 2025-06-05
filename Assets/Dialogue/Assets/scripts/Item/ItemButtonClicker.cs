using UnityEngine;
using UnityEngine.UI;

public class ItemButtonClicker : MonoBehaviour
{
    public ItemController itemController;
    public GameObject itemDisplayImage; 

    public void OnInventoryButtonClick(int slotIndex)
    {
        ItemSlot slot = itemController.inventorySlots[slotIndex];
        if (slot.isFilled)
        {
            ItemData item = slot.storedItem;
            Image imageComponent = itemDisplayImage.GetComponentInChildren<Image>();
            imageComponent.sprite = item.sprite;
            imageComponent.enabled = true;
            itemDisplayImage.SetActive(true);
        }
    }

    public void OnInventoryButtonExit()
    {
        Image imageComponent = itemDisplayImage.GetComponentInChildren<Image>();
        imageComponent.sprite = null;
        imageComponent.enabled = false;

        itemDisplayImage.SetActive(false);
    }
}
