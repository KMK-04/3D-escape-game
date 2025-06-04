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
        SceneManager.LoadScene("Scene_01");
        // UI ó�� ��
    }

    public void ResetTargets(bool isTake)
    {
        currentHits = 0;
        isCleared = false;
        // 1. ��� LaserTarget�� ���� �ʱ�ȭ
        LaserTarget[] targets = FindObjectsOfType<LaserTarget>();
        foreach (var target in targets)
        {
            target.ResetTarget();
        }

        // 2. ��� LineController���� ������ �ٽ� ���
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
