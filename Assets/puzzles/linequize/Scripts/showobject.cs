using UnityEngine;

public class ObjectClick : MonoBehaviour
{
    public GameObject image1;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // ���콺 ��ġ�� world ��ǥ�� ��ȯ
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 2D Raycast ���
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                image1.SetActive(true);

                // �θ� ��ü�� �ڽĵ� �� �ڽ��� ������ ��ü ��Ȱ��ȭ
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
