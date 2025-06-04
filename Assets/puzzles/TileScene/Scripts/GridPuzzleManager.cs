using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GridPuzzleManager : MonoBehaviour
{
    public ChestController chest;           // 쪽지·체스트 제어
    public GameObject canvasRoot;           // RushHourCanvas

    [Header("색 인덱스 설명")]
    public Color[] palette = { Color.gray, Color.red, Color.green };

    [Header("목표 패턴 (36글자 혹은 36개 쉼표 구분)")]
    [TextArea(2,4)]
    public string targetMap = "111110" +
                               "122220" +
                               "122220" +
                               "122220" +
                               "111110" +
                               "000000";
    /* 위 예시는 “문” 같은 모양: 테두리=1, 안=2, 하단 빈=0 */

    int[] target;            // 36칸 목표 상태
    bool solved;             // 중복 처리 방지


    /* ------------ 싱글톤 ------------ */
    public static GridPuzzleManager Instance { get; private set; }
    void Awake() { Instance = this; ParseTarget();
        Debug.Log("wwwwwwwww");
    }

    /* ------------ 타 셀이 클릭 시 호출 ------------ */
    public void NotifyCellChanged()
    {
        Debug.Log("wwwwwwwww");
        if (solved) return;
        if (IsSolved())
        {
            solved = true;
            Debug.Log("Puzzle Solved!");

            chest.GiveNote();
            chest.RemoveChest();          // 확대된 체스트 숨기기

            if (canvasRoot) canvasRoot.SetActive(false);
        }
    }

    /* ------------ 목표 검증 ------------ */
    bool IsSolved()
    {
        CellToggle[] cells = GetComponentsInChildren<CellToggle>();

        if (cells.Length != 36) return false;

        Array.Sort(cells, (a, b) =>
            a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));

        for (int i = 0; i < 36; i++) { 
        if (cells[i].GetState() != target[i]) return false;
    }
        GameManager.Instance.SetBoolean(3, false);
        string originalScene = GameManager.Instance.GetOriginalSceneName();
        SceneManager.LoadScene(originalScene);
        return true;
}




    /* ------------ 목표 문자열 파싱 ------------ */
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
