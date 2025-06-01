using UnityEngine;

public class ObjectClick : MonoBehaviour
{
    public GameObject image1;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 위치를 world 좌표로 변환
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 2D Raycast 사용
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                image1.SetActive(true);

                // 부모 객체의 자식들 중 자신을 제외한 객체 비활성화
                Transform parent = transform.parent;
                if (parent != null)
                {
                    foreach (Transform sibling in parent)
                    {
                        if (sibling.gameObject != gameObject)
                        {
                            sibling.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }
}
