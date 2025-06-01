using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Phone : MonoBehaviour
{
    public RectTransform phone;
    public int Desty;
    public Button[] uiButtons;
    private bool isOpen = false;
    public void TogglePhone()
    {
        isOpen = !isOpen;
        if (isOpen) {
            foreach (Button btn in uiButtons)
            btn.interactable = false;
            if (Inventory.Instance.isOpen) {
                Inventory.Instance.ToggleInventory();
            }
        }
        else {
            foreach (Button btn in uiButtons)
            btn.interactable = true;
            }
    }

    private void Update()
    {
        if (isOpen) {
            Desty = 0;
        }
        else {
            Desty = -1024;
        }
        float __y = phone.anchoredPosition.y;
        if (Mathf.Abs(__y - Desty) > 1) {
            __y += (Desty - __y)/100f;
            phone.anchoredPosition = new Vector2(phone.anchoredPosition.x,__y);
        }
    }
}

