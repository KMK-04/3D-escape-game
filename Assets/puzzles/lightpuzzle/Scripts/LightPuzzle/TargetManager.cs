using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance;
    public Camera mainCam;
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

        if (isTake && PuzzleUI != null)
        {
            puzzleCam.gameObject.SetActive(false);
            PuzzleUI.SetActive(false);
            mainCam.gameObject.SetActive(true);     // 메인 카메라 켜기
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
