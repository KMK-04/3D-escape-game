using UnityEngine;
using TMPro;

public class NumberClick : MonoBehaviour
{
    public TextMeshProUGUI targetText;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 위치를 월드 좌표로 변환
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 2D Raycast
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // 숫자 증가 처리
                if (targetText != null)
                {
                    int number = 0;
                    int.TryParse(targetText.text, out number);
                    number = (number + 1) % 10; // 9에서 0으로 되돌림
                    targetText.text = number.ToString();
                }
                else
                {
                    Debug.LogWarning("TextMeshProUGUI가 연결되지 않았습니다.");
                }
            }
        }
    }
}
