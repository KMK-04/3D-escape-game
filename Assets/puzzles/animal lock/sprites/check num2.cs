using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonChecknum2 : MonoBehaviour
{
    public string targetSceneName = "TargetScene"; // 이동할 씬 이름 (기본값 설정)

    // 버튼 클릭 시 호출될 메서드
    public void OnButtonClick()
    {
        string originalScene = GameManager.Instance.GetOriginalSceneName();
        if (!string.IsNullOrEmpty(originalScene))
        {
            DeferredDialogue.Request(
    csvName: "fail",
    flagName: "fail"
);
            GameManager.Instance.ReturnToOriginalScene();
        }
        else
        {
            Debug.LogWarning("원래 씬 이름이 저장되지 않았습니다. 기본 씬으로 이동합니다.");
            SceneManager.LoadScene("DefaultScene"); // 기본 씬 이름으로 대체
        }
    }
}
