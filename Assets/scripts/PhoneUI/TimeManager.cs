using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class TimeManager : MonoBehaviour {
    public GameManager gameManager;
    public int InGameTime;
    void Update() {
        InGameTime = gameManager.InGameTime;
        GetComponent<TextMeshProUGUI>().text = (10 + (InGameTime / 60)).ToString("D2") + ":" + (InGameTime % 60).ToString("D2");
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            gameManager.InGameTime++;
        }
    }
}
