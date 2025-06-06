using UnityEngine;

public class DistanceActivator : MonoBehaviour
{
    public GameObject targetObject; // Ȱ��ȭ�� ������Ʈ
    public float activationDistance = 5f; // Ȱ��ȭ ���� �Ÿ�

    private Transform playerTransform;

    void Start()
    {
        // "player" �±׸� ���� ������Ʈ ã��
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            Debug.Log("player �±׸� ���� ������Ʈ�� ã�ҽ��ϴ�: " + player.name);
        }
        else
        {
            Debug.LogWarning("player �±׸� ���� ������Ʈ�� ã�� �� �����ϴ�.");
        }

        // targetObject�� �Ҵ�Ǿ� �ִٸ� �ʱ⿡�� ��Ȱ��ȭ
        if (targetObject != null)
        {
            targetObject.SetActive(false);
            Debug.Log("targetObject �ʱ� ��Ȱ��ȭ ���� �Ϸ�: " + targetObject.name);
        }
        else
        {
            Debug.LogWarning("targetObject�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    void Update()
    {
        // playerTransform �Ǵ� targetObject�� null�̸� ������Ʈ �ߴ�
        if (playerTransform == null || targetObject == null) return;

        // ���� ������Ʈ�� �÷��̾� ���� �Ÿ� ���
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // �Ÿ��� activationDistance �����̸� targetObject Ȱ��ȭ, �ƴϸ� ��Ȱ��ȭ
        if (distance <= activationDistance)
        {
            targetObject.SetActive(true);
            //Debug.Log($"�Ÿ� {distance} <= {activationDistance}: targetObject Ȱ��ȭ��");
        }
        else
        {
            targetObject.SetActive(false);
            //Debug.Log($"�Ÿ� {distance} > {activationDistance}: targetObject ��Ȱ��ȭ��");
        }
    }
}
