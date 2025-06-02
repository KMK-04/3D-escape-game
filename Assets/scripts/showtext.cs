using UnityEngine;
using UnityEngine.UI; // UI 관련 기능을 사용하기 위해 필요
using TMPro; // TextMeshPro 관련 기능을 사용하기 위해 필요
using System.Collections; // 코루틴 사용을 위해 필요

public class showtext : MonoBehaviour
{
    public TextMeshProUGUI targetText; // 변경할 Text (TMP) 컴포넌트 (Inspector에서 할당)
    public string newText = "Hello!"; // 변경할 텍스트 (Inspector에서 설정 가능)

    // 버튼 클릭 시 호출될 메서드
    public void OnButtonClick()
    {
        if (targetText != null)
        {
            // 텍스트 변경 및 활성화
            targetText.text = newText;
            targetText.gameObject.SetActive(true);
            Debug.Log($"텍스트 변경됨: {newText}");

            // 3초 후 비활성화하는 코루틴 시작
            StartCoroutine(DisableTextAfterDelay(3.0f));
        }
        else
        {
            Debug.LogWarning("Text (TMP) 컴포넌트가 할당되지 않았습니다.");
        }
    }

    // 지정된 시간 후 텍스트를 비활성화하는 코루틴
    private IEnumerator DisableTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (targetText != null)
        {
            targetText.gameObject.SetActive(false);
            Debug.Log("텍스트 비활성화됨");
        }
    }
}
