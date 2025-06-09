using UnityEngine;
using UnityEngine.UI;
using System.Linq; // FirstOrDefault 를 쓰려면 필요

public class PhoneInvManager : MonoBehaviour {
    public ToggleGroup myToggleGroup;
    public ItemData selectedItem;

    public void CheckSelectedToggle() {
        Toggle selectedToggle = myToggleGroup.ActiveToggles().FirstOrDefault();
        if (selectedToggle != null) {
            GameObject SelectedInven = selectedToggle.transform.parent.gameObject;
            selectedItem = SelectedInven.GetComponent<ItemSlot>().storedItem;
            Debug.Log("현재 선택된 Toggle: " + selectedItem.ITEM_Name);
        }
        else {
            Debug.Log("선택된 Toggle 없음");
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            CheckSelectedToggle();
        }
    }
}