using System.Collections;
using UnityEngine;

public class telpo : MonoBehaviour
{
    public GameObject player;        // �̵���ų �÷��̾� ������Ʈ
    public GameObject destination1;  // ù ��° ������
    public GameObject destination2;  // �� ��° ������
    public GameObject targetObject;  // Ȱ��/��Ȱ��ȭ�� ������Ʈ
    public int moveState = 0;        // ���� ����
    public GameObject hint;
    private Coroutine moveRoutine;

    // ��ư�� �� �޼��带 �����ϼ���
    public void StartMoveSequence()
    {
        Debug.Log("asdf");
        if (moveRoutine == null)
            moveRoutine = StartCoroutine(MoveSequence());
    }

    IEnumerator MoveSequence()
    {
        // ù ��° �������� �̵�
        Debug.Log("what");
        moveState = 1;

        Vector3 firstPos = destination1.transform.position;
        firstPos.y += 1f;
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
            player.transform.position = firstPos;
            cc.enabled = true;
        }
        else
        {
            player.transform.position = firstPos;
        }

        // ù ��° �̵� �� ������Ʈ Ȱ��ȭ
        if (targetObject != null)
            targetObject.SetActive(true);

        yield return new WaitForSeconds(15f);

        if (moveState == 1)
        {
            GameManager.Instance.SetBoolean(5, true);
            // �� ��° �������� �̵�
            if (cc != null)
            {
                cc.enabled = false;
                player.transform.position = destination2.transform.position;
                cc.enabled = true;
            }
            else
            {
                player.transform.position = firstPos;
            }
            hint.SetActive(true);
        }
        // �� ��° �̵� �� ������Ʈ ��Ȱ��ȭ
        if (targetObject != null)
            targetObject.SetActive(false);

        moveRoutine = null;
    }
}
