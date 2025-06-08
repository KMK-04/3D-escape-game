using UnityEngine;
using UnityEngine.UI; // UI 관련 기능을 사용하기 위해 필요
using TMPro; // TextMeshPro 관련 기능을 사용하기 위해 필요
using UnityEngine.SceneManagement;

public class check_num1 : MonoBehaviour
{
    public TextMeshProUGUI text1; // 첫 번째 Text (TMP) 컴포넌트 (Inspector에서 할당)
    public TextMeshProUGUI text2; // 두 번째 Text (TMP) 컴포넌트 (Inspector에서 할당)
    public TextMeshProUGUI text3; // 세 번째 Text (TMP) 컴포넌트 (Inspector에서 할당)
    public string targetSceneName = "TargetScene";

    // 버튼 클릭 시 호출될 메서드
    public void OnButtonClick()
    {
        if (text1 != null && text2 != null && text3 != null)
        {
            // 각 텍스트를 숫자로 변환 (파싱 실패 시 0으로 설정)
            int number1 = 0;
            int number2 = 0;
            int number3 = 0;

            if (!int.TryParse(text1.text, out number1))
            {
                number1 = 0;
            }
            if (!int.TryParse(text2.text, out number2))
            {
                number2 = 0;
            }
            if (!int.TryParse(text3.text, out number3))
            {
                number3 = 0;
            }

            // 조건 확인 (11, 3, 2)
            if (number1 == 2 && number2 == 6 && number3 == 11)
            {
                Debug.Log("조건 충족! 숫자 조합: 11, 6, 2");
                GameManager.Instance.SetBoolean(2, false);
                string originalScene = GameManager.Instance.GetOriginalSceneName();
                if (!string.IsNullOrEmpty(originalScene))
                {
                               DeferredDialogue.Request(
    csvName: "animal",
    flagName: "animal"
);
            GameManager.Instance.ReturnToOriginalScene();
                }
                else
                {
                    Debug.LogWarning("원래 씬 이름이 저장되지 않았습니다. 기본 씬으로 이동합니다.");
                    SceneManager.LoadScene("DefaultScene"); // 기본 씬 이름으로 대체
                }
            }
            else
            {
                Debug.Log($"조건 미충족. 현재 숫자: {number1}, {number2}, {number3}");
            }
        }
        else
        {
            Debug.LogWarning("하나 이상의 Text (TMP) 컴포넌트가 할당되지 않았습니다.");
        }
    }
}
