using UnityEngine;
using UnityEngine.SceneManagement;

public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance;
    public Camera puzzleCam;
    public GameObject PuzzleUI;
    public int totalTargets = 4;
    private int currentHits = 0;
    private bool isCleared = false;


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void NotifyLaserHit()
    {
        if (isCleared) return;

        currentHits++;
        if (currentHits >= totalTargets)
        {
            isCleared = true;
            PuzzleClear();
        }
    }

    private void PuzzleClear()
    {
        Debug.Log("Puzzle Cleared!");
        Debug.Log("탈출!");
        // GameManager의 booleanList 1번째 값을 true로 설정 (예: 인덱스 1 사용)
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.GetBooleanListSize() > 1)
            {
                GameManager.Instance.SetBoolean(11, true);
            }
            else
            {
                // 리스트 크기가 충분하지 않다면 필요한 만큼 추가
                while (GameManager.Instance.GetBooleanListSize() <= 1)
                {
                    GameManager.Instance.AddBoolean(false);
                }
                GameManager.Instance.SetBoolean(11, true);
            }
            Debug.Log("GameManager booleanList[2]을 true로 설정 완료");
        }
        else
        {
            Debug.LogWarning("GameManager 인스턴스가 존재하지 않습니다. booleanList 값을 설정할 수 없습니다.");
        }

        // 원래 씬으로 돌아가기 (GameManager에서 저장된 씬 이름 사용)
        // 👉 씬 복귀 요청
        DeferredDialogue.Request(
csvName: "sol",
flagName: "light"
);
        GameManager.Instance.ReturnToOriginalScene();
        // UI 처리 등
    }

    public void ResetTargets(bool isTake)
    {
        currentHits = 0;
        isCleared = false;
        // 1. 모든 LaserTarget의 상태 초기화
        LaserTarget[] targets = FindObjectsOfType<LaserTarget>();
        foreach (var target in targets)
        {
            target.ResetTarget();
        }

        // 2. 모든 LineController에게 레이저 다시 쏘기
        LineController[] lasers = FindObjectsOfType<LineController>();
        foreach (var laser in lasers)
        {
            Vector3 dir = laser.GetDirectionVector(laser.initialDirection);
            laser.CastLaser(laser.startPoint.position, dir);
        }
    }

    public void ResetMirrors()
    {
        MirrorDragHandler3D[] mirrors = FindObjectsOfType<MirrorDragHandler3D>();
        foreach (var mirror in mirrors)
        {
            mirror.ResetToOriginalPosition();
        }
    }
}
