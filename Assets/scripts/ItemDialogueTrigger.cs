using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;      // ← 추가
using SojaExiles;

[RequireComponent(typeof(Collider))]
public class ItemDialogueTrigger : MonoBehaviour
{
    [Header("CSV (Resources/확장자 제외)")]
    public string dialogueCSVName = "example";

    [Header("플래그 이름")]
    public string flagName = "MyFlag";

    [Header("Icon_Active_Manager (Icons)")]
    [SerializeField] private Icon_Active_Manager iconManager;

    private static readonly Dictionary<string,int> FlagIdx = new();
    private bool hasTriggered;

    /* ■ 비활성 포함 탐색(Unity 2022+) */
    private T FindInactive<T>() where T : Object =>
        Object.FindFirstObjectByType<T>(FindObjectsInactive.Include);

    /* ■ 이벤트 구독 */
    private void OnEnable()  => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    /* ■ 새 씬이 로드될 때마다 UI 레퍼런스 리프레시 */
    private void OnSceneLoaded(Scene s, LoadSceneMode m)
    {
        iconManager = FindInactive<Icon_Active_Manager>();          // Icons
        if (Dialogue_Manage.Instance != null)
            RebindUI(Dialogue_Manage.Instance);                     // Name / Text / Log 등
    }
    /* ■ UI + 매니저 재바인딩 */
    private void RebindUI(Dialogue_Manage dm)
    {
        /* 1) LogManager */
        if (dm.logManager == null)
            dm.logManager = FindInactive<LogManager>();

        /* 2) dialoguePanel */
        if (dm.dialoguePanel == null && iconManager != null)
            dm.dialoguePanel = iconManager.transform.parent?.gameObject;

        /* 3) 텍스트 / 버튼 */
        if (dm.dialoguePanel != null)
        {
            foreach (var t in dm.dialoguePanel.GetComponentsInChildren<Text>(true))
            {
                if (t.name == "Name")          dm.nameText      = dm.nameText      ?? t;
                else if (t.name == "DialogueText" || t.name == "TextWindow")
                                                dm.dialogueText  = dm.dialogueText  ?? t;
            }
            foreach (var b in dm.dialoguePanel.GetComponentsInChildren<Button>(true))
                if (b.name == "NextButton")   dm.nextButton    = dm.nextButton    ?? b;
        }

        /* 4) PlayerMovement */
        if (dm.player == null)
            dm.player = FindInactive<PlayerMovement>();

        /* 5) _logTxts[] 다시 채우기 + 인덱스 0으로 초기화 */
        dm.logManager?.Init_Log();
    }

    /* ■ Awake */
    void Awake()
    {
        if (iconManager == null)
            iconManager = FindInactive<Icon_Active_Manager>();
    }

    /* ■ OnMouseDown */
    void OnMouseDown()
    {
        if (iconManager == null)                        // 씬 교체 후 재탐색
            iconManager = FindInactive<Icon_Active_Manager>();
        if (hasTriggered) return;

        /* 필수 매니저 체크 */
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

        /* 3) UI / 매니저 레퍼런스 재바인딩 */
        Dialogue_Manage dm = Dialogue_Manage.Instance;
        RebindUI(dm);                       // ★ NRE 방지 핵심

        /* 4) CSV → Dialogue_Manage 주입 */
        DatabaseManager.instance.LoadDialogueFromCSV(dialogueCSVName);
        var dialogs = DatabaseManager.instance.GetDialogue(1, DatabaseManager.instance.dialogueCount);

        BindingFlags P = BindingFlags.NonPublic | BindingFlags.Instance;
        typeof(Dialogue_Manage).GetField("currentDialogue", P)?.SetValue(dm, dialogs);
        typeof(Dialogue_Manage).GetField("dialogueIndex",   P)?.SetValue(dm, 0);
        typeof(Dialogue_Manage).GetField("contextIndex",    P)?.SetValue(dm, 0);

        dm.currentProgress.csvFileName   = dialogueCSVName;
        dm.currentProgress.dialogueIndex = 0;
        dm.currentProgress.contextIndex  = 0;

        /* 5) 첫 줄 출력 */
        dm.ShowNextLine();
        hasTriggered = true;
    }
}
