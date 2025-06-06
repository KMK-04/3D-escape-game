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

    public void AddItemToInventory(Item item) //아이템을 클릭해서 받아오는 방식
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
    public void AddItemToInventory(string spritePath, string itemName, string description) //외부스크립트에서 아이템을 추가할때 사용하는방식
    {
        if (currentSlotIndex >= inventorySlots.Count)
        {
            Debug.Log("Inventory is full!");
            return;
        }

        Sprite sprite = Resources.Load<Sprite>(spritePath);
        if (sprite == null)
        {
            Debug.LogError($"Sprite not found at path: {spritePath}");
            return;
        }

        ItemData data = new ItemData
        {
            spritePath = spritePath,
            sprite = sprite,
            ITEM_Name = itemName,
            explain = description
        };

        inventorySlots[currentSlotIndex].SetAll(data);
        inventoryexplain[currentSlotIndex].SetAll(data);

        InventorySaveManager.savedItems.Add(data);

        currentSlotIndex++;
    }

    public void LoadInventory()
    {
        int index = 0;
        foreach (var savedItem in InventorySaveManager.savedItems)
        {
            if (index >= inventorySlots.Count) break;

            // Resources에서 sprite 로드 sprite가 null이고 path가 존재한다면
            if (savedItem.sprite == null && !string.IsNullOrEmpty(savedItem.spritePath))
            {
                savedItem.sprite = Resources.Load<Sprite>(savedItem.spritePath);
            }

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