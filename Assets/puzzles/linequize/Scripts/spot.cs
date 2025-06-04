using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MatchstickLineDrawer : MonoBehaviour
{
    public Transform[] points; // 15개 점의 Transform 배열 (Inspector에서 순서대로 할당)
    public GameObject lineObjectPrefab; // SpriteRenderer가 붙은 사각형 프리팹

    private int clickCount = 0;
    private Transform firstPoint, secondPoint;
    private Queue<GameObject> drawnLines = new Queue<GameObject>(); // 최근 두 개의 선만 저장
    private Queue<Vector2Int> linesDrawn = new Queue<Vector2Int>(); // 최근 두 개의 인덱스 저장
    public string targetSceneName = "TargetScene"; // 퍼즐 씬 이름 (임시, 필요 시 사용)

    void Start()
    {
        // 모든 점에 Collider2D 추가
        foreach (var point in points)
        {
            if (point.GetComponent<Collider2D>() == null)
                point.gameObject.AddComponent<CircleCollider2D>();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                foreach (var point in points)
                {
                    if (hit.collider.gameObject == point.gameObject)
                    {
                        OnPointClicked(point);
                        break;
                    }
                }
            }
        }
    }

    public void OnPointClicked(Transform point)
    {
        Debug.Log($"포인트 클릭됨: {point.name} (위치: {point.position})");

        if (clickCount == 0)
        {
            firstPoint = point;
            clickCount = 1;
        }
        else if (clickCount == 1 && point != firstPoint)
        {
            secondPoint = point;
            DrawLine(firstPoint.position, secondPoint.position, firstPoint, secondPoint);
            clickCount = 0;
        }
    }

    void DrawLine(Vector3 start, Vector3 end, Transform pointA, Transform pointB)
    {
        // 두 점의 중간 위치 계산
        Vector3 centerPos = (start + end) / 2f;
        float length = Vector3.Distance(start, end);

        // 부모의 scale 보정
        Vector3 parentScale = this.transform.lossyScale;
        float correctedLength = length / parentScale.x;
        float correctedThickness = lineObjectPrefab.transform.localScale.y / parentScale.y;

        // 선 오브젝트 생성 (부모를 현재 오브젝트로)
        GameObject newLine = Instantiate(lineObjectPrefab, centerPos, Quaternion.identity, this.transform);
        newLine.transform.right = (end - start).normalized;
        newLine.transform.localScale = new Vector3(correctedLength, correctedThickness, 1);

        // 선 큐에 추가, 2개 초과시 가장 오래된 선 삭제
        drawnLines.Enqueue(newLine);
        if (drawnLines.Count > 2)
        {
            GameObject oldLine = drawnLines.Dequeue();
            Destroy(oldLine);
        }

        // 선 인덱스 큐에 추가, 2개 초과시 가장 오래된 인덱스 삭제
        int index1 = System.Array.IndexOf(points, pointA);
        int index2 = System.Array.IndexOf(points, pointB);
        Vector2Int lineIndices = new Vector2Int(Mathf.Min(index1, index2), Mathf.Max(index1, index2));
        linesDrawn.Enqueue(lineIndices);
        if (linesDrawn.Count > 2)
        {
            linesDrawn.Dequeue();
        }

        // 탈출 조건 체크 (최근 두 개의 선이 0-1 & 6-7 인지)
        bool hasLine1 = false, hasLine2 = false;
        foreach (var line in linesDrawn)
        {
            if (line == new Vector2Int(0, 1)) hasLine1 = true;
            if (line == new Vector2Int(6, 7)) hasLine2 = true;
        }
        if (hasLine1 && hasLine2)
        {
            Debug.Log("탈출!");
            // GameManager의 booleanList 1번째 값을 true로 설정 (예: 인덱스 1 사용)
            if (GameManager.Instance != null)
            {
                if (GameManager.Instance.GetBooleanListSize() > 1)
                {
                    GameManager.Instance.SetBoolean(1, true);
                }
                else
                {
                    // 리스트 크기가 충분하지 않다면 필요한 만큼 추가
                    while (GameManager.Instance.GetBooleanListSize() <= 1)
                    {
                        GameManager.Instance.AddBoolean(false);
                    }
                    GameManager.Instance.SetBoolean(1, true);
                }
                Debug.Log("GameManager booleanList[1]을 true로 설정 완료");
            }
            else
            {
                Debug.LogWarning("GameManager 인스턴스가 존재하지 않습니다. booleanList 값을 설정할 수 없습니다.");
            }

            // 원래 씬으로 돌아가기 (GameManager에서 저장된 씬 이름 사용)
            string originalScene = GameManager.Instance.GetOriginalSceneName();
            if (!string.IsNullOrEmpty(originalScene))
            {
                SceneManager.LoadScene(originalScene);
            }
            else
            {
                Debug.LogWarning("원래 씬 이름이 저장되지 않았습니다. 기본 씬으로 이동합니다.");
                SceneManager.LoadScene("DefaultScene"); // 기본 씬 이름으로 대체
            }
        }
    }
}
