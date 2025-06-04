using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MatchstickLineDrawer : MonoBehaviour
{
    public Transform[] points; // 15�� ���� Transform �迭 (Inspector���� ������� �Ҵ�)
    public GameObject lineObjectPrefab; // SpriteRenderer�� ���� �簢�� ������

    private int clickCount = 0;
    private Transform firstPoint, secondPoint;
    private Queue<GameObject> drawnLines = new Queue<GameObject>(); // �ֱ� �� ���� ���� ����
    private Queue<Vector2Int> linesDrawn = new Queue<Vector2Int>(); // �ֱ� �� ���� �ε��� ����
    public string targetSceneName = "TargetScene"; // ���� �� �̸� (�ӽ�, �ʿ� �� ���)

    void Start()
    {
        // ��� ���� Collider2D �߰�
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
        Debug.Log($"����Ʈ Ŭ����: {point.name} (��ġ: {point.position})");

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
        // �� ���� �߰� ��ġ ���
        Vector3 centerPos = (start + end) / 2f;
        float length = Vector3.Distance(start, end);

        // �θ��� scale ����
        Vector3 parentScale = this.transform.lossyScale;
        float correctedLength = length / parentScale.x;
        float correctedThickness = lineObjectPrefab.transform.localScale.y / parentScale.y;

        // �� ������Ʈ ���� (�θ� ���� ������Ʈ��)
        GameObject newLine = Instantiate(lineObjectPrefab, centerPos, Quaternion.identity, this.transform);
        newLine.transform.right = (end - start).normalized;
        newLine.transform.localScale = new Vector3(correctedLength, correctedThickness, 1);

        // �� ť�� �߰�, 2�� �ʰ��� ���� ������ �� ����
        drawnLines.Enqueue(newLine);
        if (drawnLines.Count > 2)
        {
            GameObject oldLine = drawnLines.Dequeue();
            Destroy(oldLine);
        }

        // �� �ε��� ť�� �߰�, 2�� �ʰ��� ���� ������ �ε��� ����
        int index1 = System.Array.IndexOf(points, pointA);
        int index2 = System.Array.IndexOf(points, pointB);
        Vector2Int lineIndices = new Vector2Int(Mathf.Min(index1, index2), Mathf.Max(index1, index2));
        linesDrawn.Enqueue(lineIndices);
        if (linesDrawn.Count > 2)
        {
            linesDrawn.Dequeue();
        }

        // Ż�� ���� üũ (�ֱ� �� ���� ���� 0-1 & 6-7 ����)
        bool hasLine1 = false, hasLine2 = false;
        foreach (var line in linesDrawn)
        {
            if (line == new Vector2Int(0, 1)) hasLine1 = true;
            if (line == new Vector2Int(6, 7)) hasLine2 = true;
        }
        if (hasLine1 && hasLine2)
        {
            Debug.Log("Ż��!");
            // GameManager�� booleanList 1��° ���� true�� ���� (��: �ε��� 1 ���)
            if (GameManager.Instance != null)
            {
                if (GameManager.Instance.GetBooleanListSize() > 1)
                {
                    GameManager.Instance.SetBoolean(1, true);
                }
                else
                {
                    // ����Ʈ ũ�Ⱑ ������� �ʴٸ� �ʿ��� ��ŭ �߰�
                    while (GameManager.Instance.GetBooleanListSize() <= 1)
                    {
                        GameManager.Instance.AddBoolean(false);
                    }
                    GameManager.Instance.SetBoolean(1, true);
                }
                Debug.Log("GameManager booleanList[1]�� true�� ���� �Ϸ�");
            }
            else
            {
                Debug.LogWarning("GameManager �ν��Ͻ��� �������� �ʽ��ϴ�. booleanList ���� ������ �� �����ϴ�.");
            }

            // ���� ������ ���ư��� (GameManager���� ����� �� �̸� ���)
            string originalScene = GameManager.Instance.GetOriginalSceneName();
            if (!string.IsNullOrEmpty(originalScene))
            {
                SceneManager.LoadScene(originalScene);
            }
            else
            {
                Debug.LogWarning("���� �� �̸��� ������� �ʾҽ��ϴ�. �⺻ ������ �̵��մϴ�.");
                SceneManager.LoadScene("DefaultScene"); // �⺻ �� �̸����� ��ü
            }
        }
    }
}
