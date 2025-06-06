using UnityEngine;
using UnityEngine.UI; // Button 클래스 사용
public class ChildActivator : MonoBehaviour
{
    public GameObject[] children; // 자식 오브젝트들 (인스펙터에서 연결)
    public int activatedCount = 0; // 현재 활성화된 개수 (외부에서 확인 가능)

    // 버튼에서 이 함수를 호출하도록 연결
    public void OnButtonClick()
    {
        if (activatedCount < children.Length)
        {
            // 다음 자식 활성화
            children[activatedCount].SetActive(true);
            activatedCount++;
        }
        else
        {
            // 모두 비활성화
            foreach (GameObject child in children)
            {
                if (child != null)
                    child.SetActive(false);
            }

            activatedCount = 0;
        }
    }

    void Start()
    {
        // 시작할 때 모두 꺼두기
        foreach (GameObject child in children)
        {
            if (child != null)
                child.SetActive(false);
        }
    }
}
