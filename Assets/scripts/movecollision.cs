using UnityEngine;
using TMPro; // << TextMeshPro ���ӽ����̽� �߰�

public class PlayerTeleportOnCollision : MonoBehaviour
{
    public GameObject targetObject;      // �浹 ���� ��� ������Ʈ
    public GameObject teleportTarget;    // �ڷ���Ʈ ������
    public GameObject callt;    // �ڷ���Ʈ ������
    public float teleportThreshold = 0.1f;
    public TextMeshProUGUI countdownText; // TextMeshPro 3D �ؽ�Ʈ ���

    void OnTriggerEnter(Collider other)
    {
        // �ε�ģ ������Ʈ�� Ÿ�� ������Ʈ���� Ȯ��
        if (other.gameObject == targetObject)
        {
            countdownText.text = " "; // ī��Ʈ�ٿ� ������ �ؽ�Ʈ ����
            Debug.Log("Ÿ�� ������Ʈ�� �浹! �ڷ���Ʈ ����");
            ExecuteTeleport();
            countdownText.text = " "; // ī��Ʈ�ٿ� ������ �ؽ�Ʈ ����
        }
    }

    void ExecuteTeleport()
    {
        Vector3 newPosition = teleportTarget.transform.position;
        newPosition.y += 2f;
        CharacterController cc = targetObject.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
            targetObject.transform.position = newPosition;
            cc.enabled = true;
        }
        else
        {
            targetObject.transform.position = newPosition;
        }

        // moveState�� 0���� ����
        telpo telpoScript = callt.GetComponent<telpo>();
        if (telpoScript != null)
        {
            telpoScript.moveState = 0;
            Debug.Log("moveState�� 0���� ����");
        }
      
    }
}
