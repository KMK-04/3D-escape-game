// Assets/Scripts/DialogueItemConnect/DialogueHelper.cs
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using SojaExiles; // PlayerMovement namespace

// alias UnityEngine.Object to avoid ambiguity with System.Object
using UnityObject = UnityEngine.Object;

public static class DialogueHelper
{
    /// <summary>
    /// ItemDialogueTrigger의 UI 바인딩 및 CSV 로드 후 대사를 표시합니다.
    /// </summary>
    public static void PrepareAndShowDialogue(string csvName)
    {
        Debug.Log($"[DialogueHelper] 대화 준비 시작: {csvName}");

        // 1) Icon_Active_Manager 찾아 비활성 포함 검색
        var iconManager = UnityObject.FindFirstObjectByType<Icon_Active_Manager>(FindObjectsInactive.Include);
        if (iconManager == null)
        {
            Debug.LogError("[DialogueHelper] Icon_Active_Manager 를 찾을 수 없습니다.");
            return;
        }

        // 부모 계층 전체 활성화 (Phone으로 인해 비활성화된 패널들도 강제 활성화)
        for (var t = iconManager.transform; t != null; t = t.parent)
        {
            t.gameObject.SetActive(true);
            Debug.Log($"[DialogueHelper] 활성화: {t.name}");
        }
        
        iconManager.On_Panel();

        // 2) Dialogue_Manage 인스턴스 바인딩
        var dm = Dialogue_Manage.Instance;
        if (dm == null)
        {
            Debug.LogError("[DialogueHelper] Dialogue_Manage 인스턴스가 없습니다.");
            return;
        }

        // LogManager 바인딩
        if (dm.logManager == null)
            dm.logManager = UnityObject.FindFirstObjectByType<LogManager>(FindObjectsInactive.Include);

        // dialoguePanel 및 UI 컴포넌트 연결
        if (dm.dialoguePanel == null)
            dm.dialoguePanel = iconManager.transform.parent?.gameObject;

        // dialoguePanel 활성화 (하지만 자식들은 선택적으로)
        if (dm.dialoguePanel != null)
        {
            dm.dialoguePanel.SetActive(true);
            Debug.Log($"[DialogueHelper] dialoguePanel 활성화: {dm.dialoguePanel.name}");
            
            // (수정) 'dialogue·text·name·next' 만 ON, 나머지는 OFF
foreach (Transform child in dm.dialoguePanel.transform)
{
    string n = child.name.ToLower();

    bool isDialogueUI =
        n.Contains("dialogue") ||   // DialoguePanel 내부 텍스트 영역
        n.Contains("text")     ||   // 대화 본문(TextWindow 등)
        n.Contains("name")     ||   // 화자 이름
        n.Contains("next")     ||   // NextButton
        child.GetComponent<Icon_Active_Manager>() != null; // 말풍선 아이콘 컨트롤러

    child.gameObject.SetActive(isDialogueUI);
}
        }

        // UI 컴포넌트들 재검색 및 바인딩
        foreach (var txt in dm.dialoguePanel.GetComponentsInChildren<Text>(true))
        {
            if (txt.name == "Name") 
            {
                dm.nameText = txt;
                txt.gameObject.SetActive(true);
            }
            else if (txt.name == "DialogueText" || txt.name == "TextWindow")
            {
                dm.dialogueText = txt;
                txt.gameObject.SetActive(true);
            }
        }
        
        foreach (var btn in dm.dialoguePanel.GetComponentsInChildren<Button>(true))
        {
            if (btn.name == "NextButton") 
            {
                dm.nextButton = btn;
                btn.gameObject.SetActive(true);
            }
        }

        // UI 컴포넌트 검증
        if (dm.nameText == null || dm.dialogueText == null || dm.nextButton == null)
        {
            Debug.LogError($"[DialogueHelper] UI 컴포넌트 바인딩 실패 - nameText: {dm.nameText != null}, dialogueText: {dm.dialogueText != null}, nextButton: {dm.nextButton != null}");
            return;
        }

        // PlayerMovement 바인딩
        if (dm.player == null)
            dm.player = UnityObject.FindFirstObjectByType<PlayerMovement>(FindObjectsInactive.Include);

        // 3) 로그 초기화가 필요할 때만 수행
        var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        var fiLogs = typeof(LogManager).GetField("_logs", flags);
        var logsArr = (GameObject[])fiLogs?.GetValue(dm.logManager);
        if (logsArr == null || logsArr.Length == 0)
            dm.logManager?.Init_Log();

        // 4) CSV 로드 및 currentDialogue 세팅
        try
        {
            Debug.Log($"[DialogueHelper] CSV 로드 시작: {csvName}");
            DatabaseManager.instance.LoadDialogueFromCSV(csvName);
            var dialogs = DatabaseManager.instance.GetDialogue(1, DatabaseManager.instance.dialogueCount);

            if (dialogs == null || dialogs.Length == 0)
            {
                Debug.LogError($"[DialogueHelper] CSV에서 대화를 로드할 수 없습니다: {csvName}");
                return;
            }

            var priv = BindingFlags.NonPublic | BindingFlags.Instance;

            // 인덱스를 확실히 0으로 초기화
            typeof(Dialogue_Manage).GetField("currentDialogue", priv)?.SetValue(dm, dialogs);
            typeof(Dialogue_Manage).GetField("dialogueIndex", priv)?.SetValue(dm, 0);
            typeof(Dialogue_Manage).GetField("contextIndex", priv)?.SetValue(dm, 0);

            dm.currentProgress.csvFileName = csvName;
            dm.currentProgress.dialogueIndex = 0;
            dm.currentProgress.contextIndex = 0;

            Debug.Log($"[DialogueHelper] 대화 설정 완료 - 총 {dialogs.Length}개 대화, 첫 번째: {dialogs[0]?.name}");

            // 최종 패널 활성화 확인
            if (dm.dialoguePanel != null)
            {
                dm.dialoguePanel.SetActive(true);
                Debug.Log($"[DialogueHelper] 최종 dialoguePanel 활성화 상태: {dm.dialoguePanel.activeInHierarchy}");
            }

            if (MouseLook.instance.isLockOn())//UI 선택으로 바꾸기
            {
                MouseLook.instance.ToggleLock();
            }
            
            // 5) 대사 첫 줄 출력
            dm.ShowNextLine();
            Debug.Log("[DialogueHelper] 첫 번째 대화 라인 표시 완료");
            
            // 최종 UI 상태 로그
            Debug.Log($"[DialogueHelper] UI 상태 - dialoguePanel: {dm.dialoguePanel?.activeInHierarchy}, nameText: {dm.nameText?.gameObject.activeInHierarchy}, dialogueText: {dm.dialogueText?.gameObject.activeInHierarchy}, nextButton: {dm.nextButton?.gameObject.activeInHierarchy}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[DialogueHelper] 대화 설정 중 오류 발생: {e.Message}\n{e.StackTrace}");
        }
    }
}