using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleExit : MonoBehaviour
{
    [Header("연동")]
    public ChestController chest;           // 드래그: ChestRoot
    public GameObject canvasRoot;           // 드래그: RushHourCanvas
    public string targetSceneName = "TargetScene";
    /* --- 싱글턴 핸들 --- */
    public static PuzzleExit Instance { get; private set; }
    void Awake()
    {
        Instance = this;
        if (canvasRoot == null) canvasRoot = gameObject;
    }

    /* --- X 버튼에서 호출 --- */
    public void ExitPuzzle()
    {
        chest.ResetZoom();
        canvasRoot.SetActive(false);
        string originalScene = GameManager.Instance.GetOriginalSceneName();
        if (!string.IsNullOrEmpty(originalScene))
        {
            DeferredDialogue.Request(
csvName: "fail",
flagName: "fail3"
);
            GameManager.Instance.ReturnToOriginalScene();
        }
        else
        {
            Debug.LogWarning("원래 씬 이름이 저장되지 않았습니다. 기본 씬으로 이동합니다.");
            SceneManager.LoadScene("Scene_01"); // 기본 씬 이름으로 대체
        }
    }

    /* === 🚩 새로 추가: 어디서든 퍼즐 닫기 === */
    public static void ClosePuzzleIfOpen()
    {
        if (Instance != null && Instance.canvasRoot.activeSelf)
            Instance.ExitPuzzle();          // 이미 위에서 줌 리셋 포함
    }
}
