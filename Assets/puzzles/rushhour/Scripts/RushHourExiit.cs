using UnityEngine;
using UnityEngine.SceneManagement;
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
        string originalScene = GameManager.Instance.GetOriginalSceneName();
        SceneManager.LoadScene(originalScene);

    }
}