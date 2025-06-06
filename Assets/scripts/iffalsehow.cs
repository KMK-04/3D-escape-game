using UnityEngine;

public class falseshow : MonoBehaviour
{
    public int n; // 확인할 인덱스

    void Start()
    {
        if (GameManager.Instance != null)
        {
            if (n >= 0 && n < GameManager.Instance.GetBooleanListSize() && GameManager.Instance.GetBoolean(n))
            {
                Debug.Log($"booleanList[{n}]이 true이므로 오브젝트 생성 유지: {gameObject.name}");
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log($"booleanList[{n}]이 false이거나 범위 밖이므로 오브젝트 파괴: {gameObject.name}");
                gameObject.SetActive(true);
            }
        }
    }
}
