using UnityEngine;

public class RushHourExit : MonoBehaviour
{
    [Header("연동")]
    public GameObject canvasRoot;           // RushHourCanvas

    // RushHour 퍼즐 복구용 변수들 (Inspector에서 할당)
    public RealRushHour rushHourPuzzle;

    public static RushHourExit Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        if (canvasRoot == null) canvasRoot = gameObject;
    }

    /* --- X 버튼에서 호출 --- */
    public void ExitPuzzle()
{
    if (rushHourPuzzle != null)
    {
        rushHourPuzzle.RestoreMainScene(false); // 오브젝트 삭제 안 함
    }
    else
    {
        canvasRoot.SetActive(false);
            Debug.Log("1");
        }
}

    /* === 어디서든 퍼즐 닫기 === */
    public static void ClosePuzzleIfOpen()
    {
        if (Instance != null && Instance.canvasRoot.activeSelf)
            Instance.ExitPuzzle();
    }
}
