using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class TimeManager : MonoBehaviour {
    public GameManager gameManager;
    public int InGameTime;
    void Update() {
        InGameTime = gameManager.InGameTime;
        GetComponent<TextMeshProUGUI>();
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            gameManager.InGameTime++;
        }
    }
}
