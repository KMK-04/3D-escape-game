using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SavedInventory
{
    public List<ItemData> items = new List<ItemData>();
}
public class ItemController : MonoBehaviour
{
    public static ItemController Instance { get; private set; }

    public Transform inventoryGrid;
    public Transform explainGrid;
    public List<ItemSlot> inventorySlots = new List<ItemSlot>();
    public List<ItemSlot> inventoryexplain = new List<ItemSlot>();

    private int currentSlotIndex = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        inventorySlots.AddRange(inventoryGrid.GetComponentsInChildren<ItemSlot>());
        inventoryexplain.AddRange(explainGrid.GetComponentsInChildren<ItemSlot>());
    }

    public void AddItemToInventory(Item item)
    {
        if (currentSlotIndex >= inventorySlots.Count)
        {
            Debug.Log("Inventory is full!");
            return;
        }

        inventorySlots[currentSlotIndex].SetAll(item);
        inventoryexplain[currentSlotIndex].SetAll(item);

        InventorySaveManager.savedItems.Add(new ItemData()
        {
            sprite = item.sprite,
            ITEM_Name = item.ITEM_Name,
            explain = item.explain
        });

        currentSlotIndex++;
    }

    public void LoadInventory()
    {
        int index = 0;
        foreach (var savedItem in InventorySaveManager.savedItems)
        {
            if (index >= inventorySlots.Count) break;
            inventorySlots[index].SetAll(savedItem);
            inventoryexplain[index].SetAll(savedItem);
            index++;
        }

        currentSlotIndex = index;
    }

    public void ClearInventorySlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= inventorySlots.Count) return;

        int maxCount = inventorySlots.Count - 1;

        for (int i = slotIndex; i < maxCount; i++)
        {
            if (inventorySlots[i + 1].isFilled)
            {
                var item = inventorySlots[i + 1].storedItem;
                inventorySlots[i].SetAll(item);
                inventoryexplain[i].SetAll(item);
                InventorySaveManager.savedItems[i] = item;
            }
            else
            {
                inventorySlots[i].ClearSlot();
                inventoryexplain[i].ClearSlot();
                InventorySaveManager.savedItems.RemoveAt(i);
                currentSlotIndex = i;
                return;
            }
        }

        inventorySlots[maxCount].ClearSlot();
        inventoryexplain[maxCount].ClearSlot();

        if (InventorySaveManager.savedItems.Count > maxCount)
            InventorySaveManager.savedItems.RemoveAt(maxCount);

        currentSlotIndex = inventorySlots.Count;
    }
}