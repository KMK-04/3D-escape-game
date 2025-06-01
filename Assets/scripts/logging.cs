using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonClickHandler : MonoBehaviour
{
    public string targetSceneName = "TargetScene"; // 이동할 씬 이름 (기본값 설정)

    // 버튼 클릭 시 호출될 메서드
    public void OnButtonClick()
    {
        // 현재 플레이어 위치와 씬 이름 저장
        GameManager.Instance.SavePlayerPosition(SceneManager.GetActiveScene().name);

        Debug.Log("클릭됨! " + targetSceneName + " 씬으로 이동합니다.");
        SceneManager.LoadScene(targetSceneName);
    }
}
