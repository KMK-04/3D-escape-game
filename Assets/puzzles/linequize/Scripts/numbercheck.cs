using UnityEngine;
using TMPro;

public class NumberClickChecker : MonoBehaviour
{
    public TextMeshProUGUI[] numberTexts; // 숫자 텍스트 5개

    // 버튼 클릭 시 호출할 함수
    public void CheckCode()
    {
        string code = "";
        foreach (var tmp in numberTexts)
        {
            code += tmp.text;
        }

        if (code == "12345")
        {
            Debug.Log("탈출!");
        }
        else
        {
            Debug.Log("현재 입력: " + code);
        }
    }
}
