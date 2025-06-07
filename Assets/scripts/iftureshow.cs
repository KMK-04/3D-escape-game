using UnityEngine;

public class iftureshow : MonoBehaviour
{
    public int n; // 확인할 인덱스
    private bool lastBooleanState = false; // 이전 상태 저장

    void Start()
    {
        CheckAndUpdateVisibility();
    }

    void Update()
    {
        // 매 프레임마다 boolean 상태 변화 확인
        if (GameManager.Instance != null && n >= 0 && n < GameManager.Instance.GetBooleanListSize())
        {
            bool currentState = GameManager.Instance.GetBoolean(n);
            
            // 상태가 변경되었을 때만 업데이트
            if (currentState != lastBooleanState)
            {
                lastBooleanState = currentState;
                CheckAndUpdateVisibility();
            }
        }
    }

    void CheckAndUpdateVisibility()
    {
        if (GameManager.Instance != null)
        {
            if (n >= 0 && n < GameManager.Instance.GetBooleanListSize())
            {
                bool currentState = GameManager.Instance.GetBoolean(n);
                
                if (currentState)
                {
                    Debug.Log($"booleanList[{n}]이 true이므로 오브젝트 활성 상태: {gameObject.name}");
                    gameObject.SetActive(true);
                }
                else
                {
                    Debug.Log($"booleanList[{n}]이 false이므로 오브젝트 비활성: {gameObject.name}");
                    gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.Log($"잘못된 인덱스이므로 오브젝트 비활성: {gameObject.name}");
                gameObject.SetActive(false);
            }
        }
    }
}