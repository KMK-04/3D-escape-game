using UnityEngine;
using UnityEngine.UI; // Button 클래스 사용

namespace SojaExiles
{
    public class toclick2 : MonoBehaviour
    {
        public Transform Player;
        public Button targetButton; // Inspector에서 연결할 버튼

        void Start()
        {
        }

        void OnMouseOver()
        {
            Debug.Log("show");
            if (Player)
            {
                Debug.Log("show");
                float dist = Vector3.Distance(Player.position, transform.position);
                if (dist < 15)
                {
                    Debug.Log("show");
                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("show");
                        // 버튼의 OnClick()에 등록된 모든 메서드 실행
                        if (targetButton != null)
                        {
                            targetButton.onClick.Invoke();
                        }
                        else
                        {
                            Debug.LogWarning("Button이 연결되어 있지 않습니다.");
                        }
                    }
                }
            }
        }

    }
}
