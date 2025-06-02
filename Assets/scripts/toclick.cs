using UnityEngine;
using UnityEngine.UI; // Button Ŭ���� ���

namespace SojaExiles
{
    public class toclick2 : MonoBehaviour
    {
        public Transform Player;
        public Button targetButton; // Inspector���� ������ ��ư

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
                        // ��ư�� OnClick()�� ��ϵ� ��� �޼��� ����
                        if (targetButton != null)
                        {
                            targetButton.onClick.Invoke();
                        }
                        else
                        {
                            Debug.LogWarning("Button�� ����Ǿ� ���� �ʽ��ϴ�.");
                        }
                    }
                }
            }
        }

    }
}
