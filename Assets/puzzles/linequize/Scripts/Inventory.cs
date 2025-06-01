using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public RectTransform inventoryPanel;
    public Button InvenGrid;
    public float Destx;
    public float _closex;
    public float _openx;
    public static Inventory Instance;

    void Awake() {
        Instance = this;
    }
    public bool isOpen = false;
    public void ToggleInventory() {
        isOpen = !isOpen;
        TextMeshProUGUI tmp = InvenGrid.GetComponentInChildren<TextMeshProUGUI>();
        if (isOpen) {
            Destx = _openx;
            tmp.text = ">";
        }
        else {
            Destx = _closex;
            tmp.text = "<";
        }
    }

    private void Update() {
        float __x = inventoryPanel.anchoredPosition.x;
        if (Mathf.Abs(__x - Destx) > 1) {
            __x += (Destx - __x) / 100f;
            inventoryPanel.anchoredPosition = new Vector2(__x, inventoryPanel.anchoredPosition.y);
        }
    }
}