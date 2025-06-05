using UnityEngine;

public class ItemOnClick : MonoBehaviour
{
    public bool isClicked = false;

    private void Start()
    {
        Item item = GetComponent<Item>();
    }

    private void OnMouseDown()
    {
        if (!isClicked)
        {
            Debug.Log($"Clicked: {gameObject.name}");
            Item item = GetComponent<Item>();
            if (item != null && ItemController.Instance != null)
            {
                ItemController.Instance.AddItemToInventory(item);
                isClicked = true;

                // ✅ 클릭된 이름 저장
                InventorySaveManager.clickedItemNames.Add(item.ITEM_Name);

                Destroy(gameObject); // 클릭 후 제거
            }
        }
    }
}
