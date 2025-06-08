using UnityEngine;
using UnityEngine.UI;
using System.Linq; // FirstOrDefault 를 쓰려면 필요

public class ToggleChecker : MonoBehaviour {
    public ToggleGroup myToggleGroup;

    public void CheckSelectedToggle() {
        Toggle selectedToggle = myToggleGroup.ActiveToggles().FirstOrDefault();
        if (selectedToggle != null) {
            GameObject SelectedInven = selectedToggle.transform.parent.gameObject;
            Debug.Log("현재 선택된 Toggle: " + SelectedInven.name);
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