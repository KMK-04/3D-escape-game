using UnityEngine;

public class DistanceActivator : MonoBehaviour
{
    public GameObject targetObject; // 활성화할 오브젝트
    public float activationDistance = 5f; // 활성화 기준 거리

    private Transform playerTransform;

    void Start()
    {
        // "player" 태그를 가진 오브젝트 찾기
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            Debug.Log("player 태그를 가진 오브젝트를 찾았습니다: " + player.name);
        }
        else
        {
            Debug.LogWarning("player 태그를 가진 오브젝트를 찾을 수 없습니다.");
        }

        // targetObject가 할당되어 있다면 초기에는 비활성화
        if (targetObject != null)
        {
            targetObject.SetActive(false);
            Debug.Log("targetObject 초기 비활성화 설정 완료: " + targetObject.name);
        }
        else
        {
            Debug.LogWarning("targetObject가 할당되지 않았습니다.");
        }
    }

    void Update()
    {
        // playerTransform 또는 targetObject가 null이면 업데이트 중단
        if (playerTransform == null || targetObject == null) return;

        // 본인 오브젝트와 플레이어 간의 거리 계산
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // 거리가 activationDistance 이하이면 targetObject 활성화, 아니면 비활성화
        if (distance <= activationDistance)
        {
            targetObject.SetActive(true);
            //Debug.Log($"거리 {distance} <= {activationDistance}: targetObject 활성화됨");
        }
        else
        {
            targetObject.SetActive(false);
            //Debug.Log($"거리 {distance} > {activationDistance}: targetObject 비활성화됨");
        }
    }
}
