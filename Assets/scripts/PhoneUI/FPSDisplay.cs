using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPSDisplay : MonoBehaviour {
    public TextMeshProUGUI fpsText;  // UI 텍스트 연결

    private float deltaTime = 0.0f;

    void Update() {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = string.Format("FPS: {0:0.}", fps);
    }
}