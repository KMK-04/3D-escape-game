// Block.cs
using UnityEngine;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    public int row, col;        // 블록 시작 위치 (왼쪽 위 칸 기준)
    public int length = 2;      // 블록 길이 (기본값 2)
    public bool isHorizontal;   // 가로(true) or 세로(false)
    public bool isGoal;         // 목표 블록 (출구 블록인지)
    
    private Vector2 dragStartPos;
    private RealRushHour puzzle;
    
    void Start()
    {
        puzzle = GetComponentInParent<RealRushHour>();
        UpdatePosition(); // 초기 위치 맞추기
        Debug.Log($"블록 초기화: {name}, row={row}, col={col}, horizontal={isHorizontal}, isGoal={isGoal}");
    }
    
    // 드래그 시작 지점 기록
    public void OnBeginDrag(PointerEventData eventData)
    {
        dragStartPos = eventData.position;
        Debug.Log($"드래그 시작: {name}");
    }
    
    // 드래그 중 처리
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.position - dragStartPos;
        float threshold = 30f; // 드래그 민감도 (픽셀)
        
        if (isHorizontal)
        {
            if (Mathf.Abs(delta.x) > threshold)
            {
                int move = delta.x > 0 ? 1 : -1;
                bool moved = TryMove(move);
                if (moved) {
                    dragStartPos = eventData.position;
                }
            }
        }
        else
        {
            if (Mathf.Abs(delta.y) > threshold)
            {
                int move = delta.y < 0 ? 1 : -1; // 화면상 Y축 반전 고려
                bool moved = TryMove(move);
                if (moved) {
                    dragStartPos = eventData.position;
                }
            }
        }
    }
    
    // 드래그 종료
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"드래그 종료: {name}");
    }
    
    bool TryMove(int delta)
    {
        return puzzle.MoveBlock(this, delta);
    }
    
    // 블록 위치를 배열 좌표에 맞게 업데이트
    public void UpdatePosition() {
        float cellSize = 1.0f;
        float offsetX = (puzzle.boardSize / 2.0f) * cellSize - cellSize / 2.0f;
        float offsetY = (puzzle.boardSize / 2.0f) * cellSize - cellSize / 2.0f;
        
        // 블록 위치 계산
        float x = col * cellSize - offsetX;
        float y = offsetY - row * cellSize;
        
        // 블록의 위치는 항상 왼쪽 위 칸을 기준으로 함
        Vector3 newPos = new Vector3(x, y, 0);
        
        // 블록의 scale 조정 (길이에 맞게)
        Vector3 newScale;
        if (isHorizontal)
        {
            // 가로 블록은 x축으로 늘림
            newScale = new Vector3(cellSize * length, cellSize, 0.1f);
            // 중심점 조정 (길이/2만큼 오른쪽으로)
            newPos.x += (length - 1) * cellSize / 2.0f;
        }
        else
        {
            // 세로 블록은 y축으로 늘림
            newScale = new Vector3(cellSize, cellSize * length, 0.1f);
            // 중심점 조정 (길이/2만큼 아래로)
            newPos.y -= (length - 1) * cellSize / 2.0f;
        }
        
        transform.localPosition = newPos;
        transform.localScale = newScale;
        
        Debug.Log($"블록 위치 업데이트: {name}, pos={newPos}, scale={newScale}");
    }
}