using System.Collections;
using UnityEngine;

public class telpo : MonoBehaviour
{
    public GameObject player;        // 이동시킬 플레이어 오브젝트
    public GameObject destination1;  // 첫 번째 목적지
    public GameObject destination2;  // 두 번째 목적지
    public GameObject targetObject;  // 활성/비활성화할 오브젝트
    public int moveState = 0;        // 상태 변수
    public GameObject hint;
    private Coroutine moveRoutine;

    // 버튼에 이 메서드를 연결하세요
    public void StartMoveSequence()
    {
        Debug.Log("asdf");
        if (moveRoutine == null)
            moveRoutine = StartCoroutine(MoveSequence());
    }

    IEnumerator MoveSequence()
    {
        // 첫 번째 목적지로 이동
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

        // 첫 번째 이동 후 오브젝트 활성화
        if (targetObject != null)
            targetObject.SetActive(true);

        yield return new WaitForSeconds(15f);

        if (moveState == 1)
        {
            GameManager.Instance.SetBoolean(5, true);
            // 두 번째 목적지로 이동
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
        // 두 번째 이동 후 오브젝트 비활성화
        if (targetObject != null)
            targetObject.SetActive(false);

        moveRoutine = null;
    }
}
