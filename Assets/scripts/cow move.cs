using UnityEngine;

public class cowmove : MonoBehaviour
{
    public Transform target; // 움직일 대상 오브젝트

    private bool toggleState = false; // 번갈아 움직이기 위한 상태 저장
    private GameManager gameManager;  // 자동으로 참조될 GameManager

    void Start()
    {
        // GameManager 싱글톤 인스턴스를 자동으로 받아옴
        gameManager = GameManager.Instance;

        if (gameManager == null)
        {
            Debug.LogError("GameManager 인스턴스를 찾을 수 없습니다.");
        }
    }

    public void OnButtonClick()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameManager가 null입니다. OnButtonClick 중단.");
            return;
        }

        if (gameManager.GetBooleanListSize() < 6)
        {
            Debug.LogWarning("booleanList가 너무 짧습니다.");
            return;
        }

        bool cond4 = gameManager.GetBoolean(4);
        bool cond5 = gameManager.GetBoolean(5);

        if (cond4 && cond5)
        {
            // 둘 다 true면 번갈아가며 움직이기
            if (toggleState)
            {
                MoveToPosition(new Vector3(3.453995f, 0.16998f, 52.62308f), Quaternion.identity);
            }
            else
            {
                MoveToPosition(new Vector3(1.623995f, 0.16998f, 55.50617f), Quaternion.Euler(0, 0, 0));
            }
            toggleState = !toggleState;
        }
        else if (cond4)
        {
            MoveToPosition(new Vector3(3.453995f, 0.16998f, 52.62308f), Quaternion.identity);
        }
        else if (cond5)
        {
            MoveToPosition(new Vector3(1.623995f, 0.16998f, 55.50617f), Quaternion.Euler(0, 0, 0));
        }
        else
        {
            Debug.Log("조건에 해당하지 않음");
        }
    }

    private void MoveToPosition(Vector3 position, Quaternion rotation)
    {
        target.position = position;
        target.rotation = rotation;
    }
}
