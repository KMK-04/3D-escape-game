using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CellToggle : MonoBehaviour, IPointerClickHandler
{
    [Header("Colors (순서대로 순환)")]
    public Color[] cycleColors = { Color.gray, Color.red, Color.green };

    Image img;
    public int index;   // 파일 맨 위에 public 필드 하나 추가
    int state;  // 현재 색 인덱스

    void Awake()
    {
        img = GetComponent<Image>();
        state = 0;
        img.color = cycleColors[state];

        index = transform.GetSiblingIndex();   // ← 추가
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        state = (state + 1) % cycleColors.Length;
        img.color = cycleColors[state];

        // 그리드 매니저에게 상태 변경 알림
        GridPuzzleManager.Instance?.NotifyCellChanged();
    }

    public int GetState() => state;          // 매니저가 읽을 수 있도록
}
