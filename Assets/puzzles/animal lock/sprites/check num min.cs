using UnityEngine;
using UnityEngine.UI; // UI 관련 기능을 사용하기 위해 필요
using TMPro; // TextMeshPro 관련 기능을 사용하기 위해 필요

public class Bcnmin : MonoBehaviour
{
    public TextMeshProUGUI counterText; // 숫자를 표시할 Text (TMP) 컴포넌트 (Inspector에서 할당)

    // 버튼 클릭 시 호출될 메서드
    public void OnButtonClick()
    {
        if (counterText != null)
        {
            // 현재 텍스트를 숫자로 변환 (파싱 실패 시 0으로 설정)
            int currentNumber = int.Parse(counterText.text);

            if (!int.TryParse(counterText.text, out currentNumber))
            {
                currentNumber = 0;
            }

            // 숫자 증가
            currentNumber -= 1;

            // 9를 초과하면 0으로 되돌림

            // 텍스트 업데이트
            counterText.text = currentNumber.ToString();
            Debug.Log($"숫자 업데이트: {currentNumber}");
        }
        else
        {
            Debug.LogWarning("Text (TMP) 컴포넌트가 할당되지 않았습니다.");
        }
    }
}
