using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class WeightDrag3D : MonoBehaviour
{
    Rigidbody _rb;
    Plane _dragPlane;
    Vector3 _offset;
    Camera _cam;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void OnMouseDown()
    {
        // 1) 활성화된 카메라 찾기
        _cam = GetActiveCamera();

        // 2) 드래그 평면 정의 (카메라를 향해 수직)
        _dragPlane = new Plane(-_cam.transform.forward, transform.position);

        // 3) 마우스 눌린 지점의 월드 좌표 계산 → offset 저장
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        if (_dragPlane.Raycast(ray, out float enter))
            _offset = transform.position - ray.GetPoint(enter);

        // 4) 드래그 중 충돌 간섭 막기 위해 키네마틱
        _rb.isKinematic = true;
    }

    void OnMouseDrag()
{
    // 마우스 움직임에 따라 평면 위에서 오브젝트 위치 업데이트
    Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
    if (_dragPlane.Raycast(ray, out float enter))
    {
        // 1) 원래 위치 계산
        Vector3 worldPos = ray.GetPoint(enter) + _offset;

        // 2) 뷰포트 좌표로 변환
        Vector3 vp = _cam.WorldToViewportPoint(worldPos);
        
        // 3) x,y를 0~1로 클램프 (화면 밖 나가지 않게)
        vp.x = Mathf.Clamp01(vp.x);
        vp.y = Mathf.Clamp01(vp.y);

        // 4) 다시 월드 좌표로 변환 (z는 그대로 둡니다)
        worldPos = _cam.ViewportToWorldPoint(new Vector3(vp.x, vp.y, vp.z));

        // 5) 최종 적용
        transform.position = worldPos;
    }
}


    void OnMouseUp()
    {
        // 드래그 끝나면 다시 물리 시뮬레이션에 맡기기
        _rb.isKinematic = false;
    }

    // 현재 씬에서 활성화된 카메라(첫 번째)를 리턴
    Camera GetActiveCamera()
    {
        foreach (var c in Camera.allCameras)
            if (c.isActiveAndEnabled)
                return c;
        return Camera.main; // fallback
    }
} 