using UnityEngine;

public class ItemOnClick : MonoBehaviour
{
    public bool isClicked = false;

    private void OnMouseDown()
    {
        Debug.Log($"Clicked: {gameObject.name}");
        if (!isClicked)
        {
            Item item = GetComponent<Item>();
            if (item != null && ItemController.Instance != null)
            {
                ItemController.Instance.AddItemToInventory(item);
                isClicked = true;

                Destroy(gameObject); // Ŭ���� ������ ���� (�Ǵ� ��Ȱ��ȭ ó�� ����)
            }
        }
    }
}
