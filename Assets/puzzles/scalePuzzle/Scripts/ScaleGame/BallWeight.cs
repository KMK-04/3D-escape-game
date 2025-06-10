using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class BallWeight : MonoBehaviour
{
    [HideInInspector] public int    id;
    [HideInInspector] public bool   isHeavy;
    Rigidbody _rb;

    [Header("Label (TMP)")]
    public TMP_Text label;                  // 구슬 위 a,b,c 레이블

    [Header("Outline Highlight")]
    public Behaviour outline;               // Assign an Outline component (예: QuickOutline)

    [Header("Selection Color")]
    public Color  selectedColor = Color.yellow;
    MeshRenderer _meshRenderer;
    Color        _originalColor;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        _rb.useGravity  = true;

        // MeshRenderer와 원래 색상 저장
        _meshRenderer = GetComponent<MeshRenderer>();
        if (_meshRenderer != null)
            _originalColor = _meshRenderer.material.color;

        if (outline != null)
            outline.enabled = false; // 하이라이트(Outline) 초기 비활성화
    }
    
        public Camera viewCamera; // 퍼즐용 카메라 지정

    void LateUpdate()
    {
        if (viewCamera == null) return;

        Vector3 vp = viewCamera.WorldToViewportPoint(transform.position);
        vp.x = Mathf.Clamp01(vp.x);
        vp.y = Mathf.Clamp01(vp.y);
        transform.position = viewCamera.ViewportToWorldPoint(vp);
    }

    public void SetAsHeavy(float mass) { isHeavy = true; _rb.mass = mass; }
    public void SetAsNormal(float mass) { isHeavy = false; _rb.mass = mass; }
    public float GetMass()              { return _rb.mass; }

    void OnMouseDown()
    {
        var mgr = Object.FindFirstObjectByType<WeighManager>();
        if (mgr != null)
            mgr.SelectBall(this);
    }

    /// <summary>
    /// 공 위에 정답 텍스트 보여주기
    /// </summary>
    public void ShowAnswerText(string txt, Color color)
    {
        if (label != null)
        {
            label.text  = txt;
            label.color = color;
        }
    }

    /// <summary>
    /// 하이라이트(아웃라인 + 색상 변경) 활성화
    /// </summary>
    public void Highlight()
    {
        // 1) Outline 켜기
        if (outline != null)
            outline.enabled = true;

        // 2) 머티리얼 색상을 선택 색상으로 변경
        if (_meshRenderer != null)
            _meshRenderer.material.color = selectedColor;
    }

    /// <summary>
    /// 하이라이트 비활성화 (원래 색상 복원)
    /// </summary>
    public void Unhighlight()
    {
        if (outline != null)
            outline.enabled = false;

        if (_meshRenderer != null)
            _meshRenderer.material.color = _originalColor;
    }
}
