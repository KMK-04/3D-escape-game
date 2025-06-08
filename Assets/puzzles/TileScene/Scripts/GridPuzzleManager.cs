// Assets/scripts/GridPuzzleManager.cs
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridPuzzleManager : MonoBehaviour
{
    [Header("연동")]
    public ChestController chest;        // 쪽지·체스트 컨트롤
    public GameObject      canvasRoot;   // 퍼즐 Canvas

    [Header("색 인덱스 팔레트")]
    public Color[] palette = { Color.gray, Color.red, Color.green };

    [Header("목표 패턴(36글자 또는 36개 쉼표)")]
    [TextArea(2, 4)]
    public string targetMap =
          "111110" +
          "122220" +
          "122220" +
          "122220" +
          "111110" +
          "000000";

    int[] target;          // 36칸 목표 배열
    bool  solved;          // 중복 처리 방지

    /* ── 싱글톤 ── */
    public static GridPuzzleManager Instance { get; private set; }
    void Awake()
    {
        Instance = this;
        ParseTarget();
    }

    /* ── 셀 클릭 시 외부에서 호출 ── */
    // GridPuzzleManager.cs - NotifyCellChanged 메서드만 수정

public void NotifyCellChanged()
{
    if (solved) return;
    if (IsSolved())
    {
        solved = true;
        Debug.Log("Grid Puzzle Solved!");

        // ★ 퍼즐 클리어 시 GameManager의 boolean 값을 false로 설정
        // 인덱스 3번을 false로 변경하여 패널이 사라지도록 함
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetBoolean(3, false);
            Debug.Log("퍼즐 클리어: booleanList[3]을 false로 설정");
        }

        // ★ 미니게임 매니저 자신도 꺼 주기
        gameObject.SetActive(false);

        // ★ 대화 + 플래그를 메인 씬으로 돌아온 뒤 실행하도록 예약
        DeferredDialogue.Request(
            csvName : "TileClear",
            flagName: "tileClear"
        );

            // 원래 씬으로 복귀
            GameManager.Instance.ReturnToOriginalScene();
        }
}

    /* ── 목표 달성 검사 ── */
    bool IsSolved()
    {
        CellToggle[] cells = GetComponentsInChildren<CellToggle>();
        if (cells.Length != 36) return false;

        Array.Sort(cells, (a, b) =>
            a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));

        for (int i = 0; i < 36; i++)
            if (cells[i].GetState() != target[i]) return false;

        return true;
    }

    /* ── 목표 문자열 파싱 ── */
    void ParseTarget()
    {
        string raw = targetMap.Replace(",", "").Replace("\n", "").Replace(" ", "");
        if (raw.Length != 36)
        {
            Debug.LogError("Target map은 정확히 36개의 숫자가 필요합니다.");
            raw = new string('0', 36);
        }

        target = new int[36];
        for (int i = 0; i < 36; i++)
            target[i] = Mathf.Clamp(raw[i] - '0', 0, palette.Length - 1);
    }
}
