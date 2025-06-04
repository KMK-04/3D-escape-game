using UnityEngine;
using TMPro; // << TextMeshPro 네임스페이스 추가

public class PlayerTeleportOnCollision : MonoBehaviour
{
    public GameObject targetObject;      // 충돌 감지 대상 오브젝트
    public GameObject teleportTarget;    // 텔레포트 목적지
    public GameObject callt;    // 텔레포트 목적지
    public float teleportThreshold = 0.1f;
    public TextMeshProUGUI countdownText; // TextMeshPro 3D 텍스트 사용

    void OnTriggerEnter(Collider other)
    {
        // 부딪친 오브젝트가 타겟 오브젝트인지 확인
        if (other.gameObject == targetObject)
        {
            countdownText.text = " "; // 카운트다운 끝나면 텍스트 제거
            Debug.Log("타겟 오브젝트와 충돌! 텔레포트 실행");
            ExecuteTeleport();
            countdownText.text = " "; // 카운트다운 끝나면 텍스트 제거
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

        // moveState를 0으로 변경
        telpo telpoScript = callt.GetComponent<telpo>();
        if (telpoScript != null)
        {
            telpoScript.moveState = 0;
            Debug.Log("moveState를 0으로 변경");
        }
      
    }
}
