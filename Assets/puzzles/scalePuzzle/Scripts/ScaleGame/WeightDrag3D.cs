using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class WeightDrag3D : MonoBehaviour
{
    Rigidbody _rb;
    Plane _dragPlane;
    Vector3 _offset;
    Camera _cam;

    // **추가 변수**
    [Tooltip("뷰포트 클램핑에 사용할 카메라 (Inspector에서 설정)")]
    public Camera viewCam;
    float _initialDepth;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // viewCam이 할당되지 않았다면 활성 카메라로 자동 설정
        if (viewCam == null)
            viewCam = GetActiveCamera();

        // 최초 위치의 뎁스(z)를 저장
        _initialDepth = viewCam.WorldToViewportPoint(transform.position).z;
    }

    void OnMouseDown()
    {
        _cam = GetActiveCamera();
        _dragPlane = new Plane(-_cam.transform.forward, transform.position);

        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        if (_dragPlane.Raycast(ray, out float enter))
            _offset = transform.position - ray.GetPoint(enter);

        _rb.isKinematic = true;
    }

    void OnMouseDrag()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        if (_dragPlane.Raycast(ray, out float enter))
        {
            Vector3 worldPos = ray.GetPoint(enter) + _offset;

            // **기존 클램핑 코드** (드래그 중에만)
            Vector3 vp = _cam.WorldToViewportPoint(worldPos);
            vp.x = Mathf.Clamp01(vp.x);
            vp.y = Mathf.Clamp01(vp.y);
            worldPos = _cam.ViewportToWorldPoint(new Vector3(vp.x, vp.y, vp.z));

            transform.position = worldPos;
        }
    }

    void OnMouseUp()
    {
        _rb.isKinematic = false;
    }

    Camera GetActiveCamera()
    {
        foreach (var c in Camera.allCameras)
            if (c.isActiveAndEnabled)
                return c;
        return Camera.main;
    }

    // **추가: LateUpdate에서 뷰포트 밖으로 나간 위치 보정**
    void LateUpdate()
    {
        // 물리 시뮬레이션(충돌)으로 날아간 뒤에도 매 프레임 보정
        Vector3 vp = viewCam.WorldToViewportPoint(transform.position);
        vp.x = Mathf.Clamp01(vp.x);
        vp.y = Mathf.Clamp01(vp.y);
        vp.z = _initialDepth;  // 깊이는 초기값 유지
        transform.position = viewCam.ViewportToWorldPoint(vp);
    }
}
