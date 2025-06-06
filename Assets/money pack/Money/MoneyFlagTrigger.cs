using UnityEngine;
using System.Reflection;         // ★ 리플렉션
                                  //  (private 필드 주입용)

public class MoneyFlagTrigger : MonoBehaviour
{
    [Tooltip("Resources 폴더 내 CSV 이름(확장자 제외)")]
    public string dialogueCSVName = "moneyDialogue";

    [SerializeField] private Icon_Active_Manager iconManager;

    bool hasTriggered = false;

    void Awake()
    {
        if (iconManager == null)
            iconManager = FindObjectOfType<Icon_Active_Manager>();
    }

    void OnMouseDown()
    {
        if (hasTriggered) return;
        if (GameManager.Instance == null) return;

        /* 1. GameManager 플래그(원래 로직) */
        GameManager.Instance.AddBoolean(true);

        /* 2. UI 전체 켜기 */
        if (iconManager)
        {
            var panel = iconManager.transform.parent;
            if (panel) panel.gameObject.SetActive(true);
            iconManager.On_Panel();
        }

        /* 3. CSV를 DatabaseManager로 읽어 둠 */
        DatabaseManager.instance.LoadDialogueFromCSV(dialogueCSVName);
        int count = DatabaseManager.instance.dialogueCount;
        Dialgoue[] dialogs =
            DatabaseManager.instance.GetDialogue(1, count);

        /* 4. Dialogue_Manage 내부(private) 필드에 직접 주입 */
        Dialogue_Manage dm = Dialogue_Manage.Instance;
        if (dm == null) { Debug.LogError("Dialogue_Manage 없음"); return; }

        var t = typeof(Dialogue_Manage);
        // currentDialogue
        t.GetField("currentDialogue",
            BindingFlags.NonPublic | BindingFlags.Instance)
         .SetValue(dm, dialogs);

        // 인덱스 0으로 리셋
        t.GetField("dialogueIndex",
            BindingFlags.NonPublic | BindingFlags.Instance)
         .SetValue(dm, 0);
        t.GetField("contextIndex",
            BindingFlags.NonPublic | BindingFlags.Instance)
         .SetValue(dm, 0);

        // 진행 저장 구조도 초기화
        dm.currentProgress.csvFileName = dialogueCSVName;
        dm.currentProgress.dialogueIndex = 0;
        dm.currentProgress.contextIndex = 0;

        /* 5. 로그는 지우지 않고 ShowNextLine()만 호출 */
        dm.ShowNextLine();        // → 1줄째(플레이어) 출력
                                  //    Next 버튼 → 2줄째(System) …
        hasTriggered = true;
    }
}
