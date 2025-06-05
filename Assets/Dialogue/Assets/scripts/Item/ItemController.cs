using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    public static ItemController Instance { get; private set; }
    public Transform inventoryGrid; // �θ� ������Ʈ (InventoryGrid)
    public Transform explainGrid;
    public List<ItemSlot> inventorySlots = new List<ItemSlot>();
    public List<ItemSlot> inventoryexplain = new List<ItemSlot>();

    private int currentSlotIndex = 0;
    private void Awake()
    {
        // �̱��� �ʱ�ȭ
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); // �ߺ� ����
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
        currentSlotIndex++;
    }


    public void ClearInventorySlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= inventorySlots.Count)
        {
            Debug.LogWarning("Invalid slot index.");
            return;
        }

        int maxCount = inventorySlots.Count - 1;

        for (int i = slotIndex; i < maxCount; i++)
        {
            if (inventorySlots[i + 1].isFilled)
            {
                inventorySlots[i].SetAll(inventorySlots[i + 1].storedItem);
                inventoryexplain[i].SetAll(inventoryexplain[i + 1].storedItem); // 설명도 같이 이동
            }
            else
            {
                inventorySlots[i].ClearSlot();
                inventoryexplain[i].ClearSlot();
                currentSlotIndex = i;
                return;
            }
        }

        // 마지막 슬롯 비우기
        inventorySlots[maxCount].ClearSlot();
        inventoryexplain[maxCount].ClearSlot();

        // currentSlotIndex ������Ʈsdds
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (!inventorySlots[i].isFilled)
            {
                currentSlotIndex = i;
                return;
            }
        }

        currentSlotIndex = inventorySlots.Count;
    }

}
