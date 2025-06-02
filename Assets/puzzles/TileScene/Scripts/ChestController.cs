using UnityEngine;

public class ChestController : MonoBehaviour
{
    [Header("Transforms")]
    public Transform zoomPoint;          // 카메라 앞 Empty

    [Header("Animation / UI")]
    public Animator chestAnimator;       // 모델이 붙은 Animator
    public GameObject miniGameCanvas;    // RushHourCanvas
    public GameObject notePrefab; 

    [Header("Positions / Scales")]
    public Vector3 hiddenPos;            // 책상 속
    public Vector3 shownPos;             // 책상 밖
    public Vector3 hiddenScale = Vector3.one * 0.6f;
    public Vector3 shownScale  = Vector3.one;          // 슬라이드 후(보통 1)
    public Vector3 zoomScale   = Vector3.one * 2.5f;   // 줌인 시 더 크게

    [Header("Timings")]
    public float slideDuration = 1f;
    public float zoomDuration  = 1f;

    [Header("Options")]
    public bool keepRotation   = true;   // true면 줌인 때 회전 유지

    public static bool TileClear = false;


    enum State { Hidden, Sliding, Ready, Zooming, Opening, Open }
    State state = State.Hidden;

    float t;
    Vector3 startPos, startScale;
    Quaternion startRot;
    /* 클래스 맨 위 필드에 추가 */
    Quaternion shownRot;          // 슬라이드 끝난 후의 정방향 회전을 저장

    void Start()
    {
        transform.position   = hiddenPos;
        transform.localScale = hiddenScale;
        if (miniGameCanvas)  miniGameCanvas.SetActive(false);
    }

    void OnMouseDown()
    {
        if      (state == State.Hidden) BeginSlide();
        else if (state == State.Ready)  BeginZoom();
        
    }

    /* ---------- 슬라이드 ---------- */
    void BeginSlide()
    {
        state      = State.Sliding;
        t          = 0f;
        startPos   = hiddenPos;
        startScale = hiddenScale;
        
    }

    /* ---------- 줌 인 ---------- */
    void BeginZoom()
    {
        state      = State.Zooming;
        t          = 0f;
        startPos   = transform.position;
        startScale = transform.localScale;
        startRot   = transform.rotation;
    }

    /* ---------- 매 프레임 ---------- */
    void Update()
    {
        switch (state)
        {
            case State.Sliding:
                t += Time.deltaTime / slideDuration;
                transform.position   = Vector3.Lerp(startPos, shownPos, t);
                transform.localScale = Vector3.Lerp(startScale, shownScale, t);
                if (t >= 1f) {
                    state = State.Ready;
                    shownRot = transform.rotation;
                }
                break;

            case State.Zooming:
                t += Time.deltaTime / zoomDuration;
                transform.position   = Vector3.Lerp(startPos, zoomPoint.position, t);
                transform.localScale = Vector3.Lerp(startScale, zoomScale, t);

                if (!keepRotation)
                    transform.rotation = Quaternion.Slerp(startRot, zoomPoint.rotation, t);

                if (t >= 1f)
                {
                    chestAnimator.SetTrigger("Open");   // 트리거 이름은 반드시 “Open”
                    state = State.Opening;
                }
                
                break;
        }
    }

    /* ---------- 애니메이션 이벤트 ---------- */
    public void OnChestOpened()          // 열림 클립 마지막 프레임에서 호출
    {
        if (state != State.Opening) return;
        state = State.Open;
        if (miniGameCanvas) miniGameCanvas.SetActive(true);
        GetComponent<Collider>().enabled = false;
        Debug.Log("ChestOpened Event OK");  // ← 콘솔 확인용
    }

    public void GiveNote()
{
    // 쪽지 Instantiation

    if (notePrefab)
    {
        Vector3 dropPos = transform.position + transform.forward * 0.3f + Vector3.up * 0.1f;
        // 인스턴스 반환값 저장!
        GameObject note = Instantiate(notePrefab, dropPos, Quaternion.identity);
        TileClear = true;

        // 인스턴스만 Destroy (프리팹은 X)
            Destroy(note, 0.5f);
    }

      

    // 체스트 제거
        gameObject.SetActive(false);
}

    public void RemoveChest()
{
    // 확대 상태 그대로 사라지게 하기 (비활성 또는 파괴)
    gameObject.SetActive(false);      // 깔끔하게 숨김
    // Destroy(gameObject);           // 완전 삭제하고 싶다면 이 줄로 교체
}
    /* 맨 아래 원하는 위치에 새 메서드 삽입 */
public void ResetZoom()
{
        if (state == State.Zooming || state == State.Open ||
            state == State.Opening)
        {
            // 언제든지 퍼즐 닫기 요청이 오면 강제로 원래 위치로 되돌림
            transform.position = shownPos;
            transform.localScale = shownScale;
            transform.rotation = keepRotation ? transform.rotation : shownRot;

            // Animator를 확실히 초기 상태로 리셋
            chestAnimator.ResetTrigger("Open");
            chestAnimator.Play("Closed", 0, 0);   // 0프레임부터 Closed 상태로 강제 전환

            // 상태 및 클릭 가능 복구
            state = State.Ready;
            GetComponent<Collider>().enabled = true;
        }
}





}
