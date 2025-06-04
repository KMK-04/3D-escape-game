using System.Collections;
using UnityEngine;

public class minigamehard: MonoBehaviour
{
    private Vector3 initialPosition;
    public GameObject[] children ;

    void Start()
    {
        initialPosition = transform.localPosition;
        children[0].SetActive(false);
        children[1].SetActive(false);
        StartCoroutine(MovementCycle());
    }

    IEnumerator MovementCycle()
    {
        while (true)
        {
            // �ʱ� ��ġ ����
            transform.localPosition = initialPosition;
            SetChildrenActive(false);

            // X�� �̵� (0~4 ����)
            float newX = initialPosition.x + Random.Range(0f, 4f);
            transform.localPosition = new Vector3(newX, initialPosition.y, initialPosition.z);

            // �ڽ� ������Ʈ ���� Ȱ��ȭ
            StartCoroutine(ActivateChildren());
            yield return new WaitForSeconds(3f);
        }
    }

    IEnumerator ActivateChildren()
    {
        children[0].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        children[1].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        children[0].SetActive(false);
        children[1].SetActive(false);
    }

    void SetChildrenActive(bool state)
    {
        foreach (var child in children)
        {
            child.SetActive(state);
        }
    }
}
