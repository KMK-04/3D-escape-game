using System.Collections.Generic;
using System.Reflection;   // ✨ 리플렉션 (private 필드 주입)
using UnityEngine;

/// <summary>
/// ● Inspector에 입력된 <b>dialogueCSVName</b>, <b>flagName</b>만 보고 동작합니다.
/// ● OnMouseDown 시 순서
///   1) <b>flagName</b>을 전역 딕셔너리에서 찾아 없으면 AddBoolean(true)로 추가,
///      있으면 SetBoolean(index, true)로 덮어쓰기.
///   2) 대화 UI(“Dialogue_Panel” + “Icons”)가 꺼져 있으면 강제 ON.
///   3) 지정 CSV를 <b>DatabaseManager</b>로 읽고
///      - Reflection 으로 Dialogue_Manage 내부 필드(currentDialogue 등) 주입,
///      - 인덱스를 0으로 리셋하지만 <i>Init_Log()</i>는 호출하지 않아 히스토리 유지.
///   4) <b>ShowNextLine()</b> 한 번 호출 → 첫 줄부터 새 대화 진행..
/// </summary>
[RequireComponent(typeof(Collider))]   // 클릭 감지를 위한 Collider 보장
public class ItemDialogueTrigger : MonoBehaviour
{
    /* ▼ Inspector  ------------------------------------------------------ */

    [Header("◆ 대화 CSV (Resources 경로 · 확장자 제외)")]
    [Tooltip("예) Assets/Resources/moneyDialogue.csv → \"moneyDialogue\"")]
    public string dialogueCSVName = "example";

    [Header("◆ 설정할 플래그 이름")]
    [Tooltip("동일 이름이면 언제나 동일 booleanList 인덱스를 재사용")]
    public string flagName = "MyFlag";

    [Header("◆ 대화 UI용 Icon_Active_Manager")]
    [Tooltip("Hierarchy의 ‘Icons’ 오브젝트에 붙은 Icon_Active_Manager."
           + " 비워 두면 Awake()에서 자동 탐색")]
    [SerializeField] private Icon_Active_Manager iconManager;

    /* ▼ 내부 상태 -------------------------------------------------------- */

    // flagName → GameManager.booleanList 인덱스 매핑
    private static readonly Dictionary<string, int> FlagToIndex = new();

    private bool hasTriggered = false;

    /* ▼ Unity 라이프사이클 ---------------------------------------------- */

    private void Awake()
    {
        if (iconManager == null)
            iconManager = FindObjectOfType<Icon_Active_Manager>();
    }

    private void OnMouseDown()
    {
        if (hasTriggered || GameManager.Instance == null) return;

        /* 1) ── 플래그 처리 ─────────────────────────────────────────── */
        if (!FlagToIndex.TryGetValue(flagName, out int index))
        {
            GameManager.Instance.AddBoolean(true);
            index = GameManager.Instance.GetBooleanListSize() - 1;
            FlagToIndex[flagName] = index;
            Debug.Log($"[ItemDialogueTrigger] 새 플래그 '{flagName}' 등록 → booleanList[{index}]");
        }
        else
        {
            GameManager.Instance.SetBoolean(index, true);
            Debug.Log($"[ItemDialogueTrigger] 플래그 '{flagName}' 재설정 → booleanList[{index}]");
        }

        /* 2) ── 대화 UI 강제 ON ──────────────────────────────────── */
        if (iconManager != null)
        {
            // ‘Icons’ 부모(= Dialogue_Panel) 먼저 켜기
            Transform panel = iconManager.transform.parent;
            if (panel != null) panel.gameObject.SetActive(true);
            iconManager.On_Panel();   // 아이콘 그룹 활성화
        }
        else
        {
            Debug.LogWarning("[ItemDialogueTrigger] Icon_Active_Manager를 찾지 못했습니다.");
        }

        /* 3) ── CSV 로드 & Dialogue_Manage 주입 ───────────────────── */
        DatabaseManager.instance.LoadDialogueFromCSV(dialogueCSVName);
        int cnt = DatabaseManager.instance.dialogueCount;
        Dialgoue[] dlgArr = DatabaseManager.instance.GetDialogue(1, cnt);

        Dialogue_Manage dm = Dialogue_Manage.Instance;
        if (dm == null)
        {
            Debug.LogError("[ItemDialogueTrigger] Dialogue_Manage.Instance가 null입니다.");
            return;
        }

        // private 필드 주입 (currentDialogue, indices)
        System.Type t = typeof(Dialogue_Manage);
        BindingFlags bf = BindingFlags.NonPublic | BindingFlags.Instance;

        t.GetField("currentDialogue", bf)?.SetValue(dm, dlgArr);
        t.GetField("dialogueIndex",   bf)?.SetValue(dm, 0);
        t.GetField("contextIndex",    bf)?.SetValue(dm, 0);

        // 진행 저장 구조도 초기화
        dm.currentProgress.csvFileName = dialogueCSVName;
        dm.currentProgress.dialogueIndex = 0;
        dm.currentProgress.contextIndex = 0;

        /* 4) ── 첫 줄 출력 (히스토리 유지) ───────────────────────── */
        dm.ShowNextLine();   // Init_Log() 호출 없이 새 대화 시작

        hasTriggered = true;
    }
}
