// Assets/scripts/ItemDialogueTrigger.cs
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SojaExiles;                    // PlayerMovement

[RequireComponent(typeof(Collider))]
public class ItemDialogueTrigger : MonoBehaviour
{
    [Header("CSV (Resources, 확장자 제외)")]
    public string dialogueCSVName = "example";

    [Header("플래그 이름")]
    public string flagName = "MyFlag";

    [Header("Icon_Active_Manager (Icons 오브젝트)")]
    [SerializeField] private Icon_Active_Manager iconManager;

    private static readonly Dictionary<string, int> FlagIdx = new();
    private bool hasTriggered;

    /* ■ 비활성 포함 탐색 (Unity 2022+) */
    private static T FindInactive<T>() where T : Object =>
        Object.FindFirstObjectByType<T>(FindObjectsInactive.Include);

    /* ■ 씬 로드 이벤트 등록/해제 */
    private void OnEnable()  => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    /* ■ 새 씬 로드 시 UI 레퍼런스 갱신 */
    private void OnSceneLoaded(Scene s, LoadSceneMode m)
    {
        iconManager = FindInactive<Icon_Active_Manager>();
        if (Dialogue_Manage.Instance != null)
            RebindUI(Dialogue_Manage.Instance);
    }

    /* ■ 초기 Awake (첫 씬) */
    private void Awake()
    {
        if (iconManager == null)
            iconManager = FindInactive<Icon_Active_Manager>();
    }

    /* ■ UI·매니저 재바인딩 + 로그 배열 존재 시 유지 */
    private void RebindUI(Dialogue_Manage dm)
    {
        if (dm.logManager == null)
            dm.logManager = FindInactive<LogManager>();

        if (dm.dialoguePanel == null && iconManager != null)
            dm.dialoguePanel = iconManager.transform.parent?.gameObject;

        if (dm.dialoguePanel != null)
        {
            foreach (var t in dm.dialoguePanel.GetComponentsInChildren<Text>(true))
            {
                if (t.name == "Name")           dm.nameText     ??= t;
                else if (t.name is "DialogueText" or "TextWindow")
                                                 dm.dialogueText ??= t;
            }
            foreach (var b in dm.dialoguePanel.GetComponentsInChildren<Button>(true))
                if (b.name == "NextButton")     dm.nextButton   ??= b;
        }

        if (dm.player == null)
            dm.player = FindInactive<PlayerMovement>();

        /* 로그 배열이 없을 때만 Init_Log() */
        if (dm.logManager != null)
        {
            var flags  = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var fiLogs = typeof(LogManager).GetField("_logs", flags);
            var arr    = (GameObject[])fiLogs.GetValue(dm.logManager);

            if (arr == null || arr.Length == 0)
                dm.logManager.Init_Log();
        }

        
    }

    /* ■ 아이템 클릭 */
    private void OnMouseDown()
    {
        if (iconManager == null)
            iconManager = FindInactive<Icon_Active_Manager>();
        if (hasTriggered) return;

        if (GameManager.Instance == null ||
            DatabaseManager.instance == null ||
            Dialogue_Manage.Instance == null ||
            iconManager == null)
        {
            Debug.LogError("[ItemDialogueTrigger] 필수 매니저(iconManager) 누락");
            return;
        }

        /* 1) 플래그 true */
        if (!FlagIdx.TryGetValue(flagName, out int idx))
        {
            GameManager.Instance.AddBoolean(true);
            idx = GameManager.Instance.GetBooleanListSize() - 1;
            FlagIdx[flagName] = idx;
        }
        else
            GameManager.Instance.SetBoolean(idx, true);

        /* 2) UI 전체 ON */
        iconManager.gameObject.SetActive(true);
        for (Transform p = iconManager.transform; p != null; p = p.parent)
            p.gameObject.SetActive(true);
        iconManager.On_Panel();
        Dialogue_Manage.Instance.dialoguePanel?.SetActive(true);
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true;

        /* 3) UI·매니저 레퍼런스 재바인딩 */
        var dm = Dialogue_Manage.Instance;
        RebindUI(dm);

        /* 4) CSV 로드·주입 */
        DatabaseManager.instance.LoadDialogueFromCSV(dialogueCSVName);
        var dialogs = DatabaseManager.instance.GetDialogue(1, DatabaseManager.instance.dialogueCount);

        var priv = BindingFlags.NonPublic | BindingFlags.Instance;
        typeof(Dialogue_Manage).GetField("currentDialogue", priv)?.SetValue(dm, dialogs);
        typeof(Dialogue_Manage).GetField("dialogueIndex",   priv)?.SetValue(dm, 0);
        typeof(Dialogue_Manage).GetField("contextIndex",    priv)?.SetValue(dm, 0);

        dm.currentProgress.csvFileName   = dialogueCSVName;
        dm.currentProgress.dialogueIndex = 0;
        dm.currentProgress.contextIndex  = 0;

        /* 5) 첫 줄 출력 */
        dm.ShowNextLine();
        hasTriggered = true;
    }
}
