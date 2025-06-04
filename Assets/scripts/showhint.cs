using UnityEngine;
using UnityEngine.UI; // UI 관련 기능을 사용하기 위해 필요

public class ButtonActivator : MonoBehaviour
{
    public GameObject targetObject; // 활성화할 오브젝트 (Inspector에서 할당)

    // 버튼 클릭 시 호출될 메서드
    public void OnButtonClick()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true); // 오브젝트 활성화
            Debug.Log($"오브젝트 활성화됨: {targetObject.name}");
        }
        else
        {
            Debug.LogWarning("활성화할 오브젝트가 할당되지 않았습니다.");
        }
    }
}
