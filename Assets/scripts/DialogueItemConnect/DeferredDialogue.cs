using System.Collections;
using System.Collections.Generic;
using System.Reflection; // 추가
using SojaExiles;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class DeferredDialogue
{
    private static bool hasRequest;
    private static string pendingCSV;
    private static string pendingFlag;
    private static string pendingItemPath;
    private static string pendingItemName;
    private static string pendingItemDesc;

    /// <summary>
    /// 대화 + 보상 예약
    /// </summary>
    public static void Request(
        string csvName,
        string flagName,
        string itemPath = null,
        string itemName = null,
        string itemDesc = null
    )
    {
        hasRequest = true;
        pendingCSV = csvName;
        pendingFlag = flagName;
        pendingItemPath = itemPath;
        pendingItemName = itemName;
        pendingItemDesc = itemDesc;
        Debug.Log($"[DeferredDialogue] Request 등록: CSV={csvName}, Flag={flagName}");
    }

    [RuntimeInitializeOnLoadMethod]
    private static void Init()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        if (!hasRequest) return;
        Debug.Log($"[DeferredDialogue] 씬 로드 후 처리 시작: {s.name}");

        // 이미 보상 완료된 이벤트라면 스킵
        var rewarder = Object.FindObjectOfType<FlagItemRewarder>(true);
        var entryChk = rewarder?.GetReward(pendingFlag, pendingCSV);
        if (entryChk != null && entryChk.rewarded)
        {
            Debug.Log($"[DeferredDialogue] 이미 처리된 이벤트, 스킵: Flag={pendingFlag}, CSV={pendingCSV}");
            hasRequest = false;
            pendingCSV = pendingFlag = pendingItemPath = pendingItemName = pendingItemDesc = null;
            return;
        }

        // 플래그 기록
        if (!string.IsNullOrEmpty(pendingFlag) && GameManager.Instance != null)
        {
            int idx = FlagCache.GetOrAdd(pendingFlag);
            GameManager.Instance.SetBoolean(idx, true);
            Debug.Log($"[DeferredDialogue] 플래그 설정: {pendingFlag} = true (인덱스: {idx})");
        }

        // Runner 생성
        var runner = new GameObject("DeferredDialogueRunner").AddComponent<Runner>();
        Object.DontDestroyOnLoad(runner);

        runner.csvName = pendingCSV;
        runner.flagName = pendingFlag;
        runner.itemPath = pendingItemPath;
        runner.itemName = pendingItemName;
        runner.itemDesc = pendingItemDesc;

        hasRequest = false;
    }

    private static class FlagCache
    {
        private static readonly Dictionary<string, int> map = new Dictionary<string, int>();

        public static int GetOrAdd(string name)
        {
            if (!map.TryGetValue(name, out int idx))
            {
                GameManager.Instance.AddBoolean(true);
                idx = GameManager.Instance.GetBooleanListSize() - 1;
                map[name] = idx;
                Debug.Log($"[FlagCache] 새 플래그 생성: {name} = 인덱스 {idx}");
            }
            return idx;
        }
    }

    private class Runner : MonoBehaviour
    {
        public string csvName;
        public string flagName;
        public string itemPath;
        public string itemName;
        public string itemDesc;

        void Start()
        {
            Debug.Log($"[DeferredDialogue.Runner] 시작: {csvName}");
            
            // 이미 보상된 이벤트이면 대화 스킵
            var rewarder = Object.FindObjectOfType<FlagItemRewarder>(true);
            var entryChk = rewarder?.GetReward(flagName, csvName);
            if (entryChk != null && entryChk.rewarded)
            {
                Debug.Log($"[DeferredDialogue.Runner] 이미 보상된 이벤트 - 대화 스킵: {csvName}");
                Destroy(gameObject);
                return;
            }

            // 대화 시작 전에 플레이어 이동 비활성화
            if (GameManager.Instance?.playerMovement != null)
            {
                GameManager.Instance.playerMovement.SetMovement(false);
                Debug.Log("[DeferredDialogue.Runner] 플레이어 이동 비활성화");
            }

            // MouseLook 상태 확인 및 로그
            if (MouseLook.instance != null)
            {
                Debug.Log($"[DeferredDialogue.Runner] 시작 시 MouseLook 상태 - isLockOn: {MouseLook.instance.isLockOn()}");
            }

            StartCoroutine(DelayedStart());
        }

        private IEnumerator DelayedStart()
        {
            // 1) 기본 매니저들이 준비될 때까지 대기
            yield return new WaitUntil(() =>
                Dialogue_Manage.Instance != null &&
                DatabaseManager.instance != null &&
                GameManager.Instance != null
            );

            // 2) UI 관련 컴포넌트들이 준비될 때까지 대기
            yield return new WaitUntil(() =>
            {
                var iconManager = Object.FindFirstObjectByType<Icon_Active_Manager>(FindObjectsInactive.Include);
                if (iconManager != null)
                {
                    // Icon_Active_Manager의 부모 오브젝트 활성화 (Phone으로 비활성화된 것들 복구)
                    for (var t = iconManager.transform; t != null; t = t.parent)
                    {
                        t.gameObject.SetActive(true);
                        Debug.Log($"[DeferredDialogue.Runner] UI 활성화: {t.name}");
                    }
                    iconManager.On_Panel();

                    // 추가: 대화 패널과 자식들 강제 활성화
                    var dialogueManager = Dialogue_Manage.Instance;
                    if (dialogueManager != null && dialogueManager.dialoguePanel != null)
                    {
                        dialogueManager.dialoguePanel.SetActive(true);
                        foreach (Transform child in dialogueManager.dialoguePanel.transform)
{
    string childName = child.name.ToLower();
    
    // 대화 관련 UI만 선택적으로 활성화 (log, history, inventory 등은 제외)
    if (childName.Contains("dialogue") || 
        childName.Contains("text") || 
        childName.Contains("name") || 
        childName.Contains("next") ||
        child == iconManager.transform)
    {
        child.gameObject.SetActive(true);
        Debug.Log($"[DeferredDialogue.Runner] 대화 관련 패널 활성화: {child.name}");

    }
}
                    }

                    return true;
                }
                Debug.LogWarning("[DeferredDialogue.Runner] Icon_Active_Manager not found");
                return false;
            });

            // 3) 추가 프레임 대기 (UI 초기화 완료 보장)
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(0.2f); // 대기 시간 약간 증가

            // 4) 재시도 로직
            bool dialogueStarted = false;
            int retryCount = 0;
            int maxRetries = 5;

            while (!dialogueStarted && retryCount < maxRetries)
            {
                bool hasError = false;
                System.Exception lastException = null;

                try
                {
                    // UI 강제 활성화 (Phone으로 비활성화된 패널들 복구)
                    var iconManager = Object.FindFirstObjectByType<Icon_Active_Manager>(FindObjectsInactive.Include);
                    if (iconManager != null)
                    {
                        // 부모 계층 전체 활성화
                        for (var t = iconManager.transform; t != null; t = t.parent)
                        {
                            t.gameObject.SetActive(true);
                        }
                        iconManager.On_Panel();

                        // 대화 패널과 자식들 강제 활성화
                        var dialogueManager = Dialogue_Manage.Instance;
                        if (dialogueManager?.dialoguePanel != null)
                        {
                            dialogueManager.dialoguePanel.SetActive(true);
                            foreach (Transform child in dialogueManager.dialoguePanel.transform)
                            {
                                child.gameObject.SetActive(true);
                            }
                            Debug.Log($"[DeferredDialogue.Runner] 대화 패널 활성화 완료: {dialogueManager.dialoguePanel.name}");
                        }
                    }

                    DialogueHelper.PrepareAndShowDialogue(csvName);
                }
                catch (System.Exception e)
                {
                    hasError = true;
                    lastException = e;
                }

                yield return new WaitForSeconds(0.3f);

                if (hasError)
                {
                    retryCount++;
                    Debug.LogError($"[DeferredDialogue.Runner] 대화 시작 실패 (시도 {retryCount}): {lastException?.Message}");
                    if (retryCount >= maxRetries)
                    {
                        Debug.LogError($"[DeferredDialogue.Runner] 최대 재시도 횟수 초과, 포기: {csvName}");

                        // 실패 시 MouseLook 상태 복구
                        if (MouseLook.instance != null && MouseLook.instance.isLockOn())
                        {
                            MouseLook.instance.ToggleLock();
                            Debug.Log("[DeferredDialogue.Runner] 실패 시 MouseLook 상태 복구");
                        }

                        // 플레이어 이동 복구
                        if (GameManager.Instance?.playerMovement != null)
                        {
                            GameManager.Instance.playerMovement.SetMovement(true);
                        }

                        Destroy(gameObject);
                        yield break;
                    }
                    continue;
                }

                // 대화가 실제로 시작되었는지 확인
                var dm = Dialogue_Manage.Instance;
                if (dm != null && dm.dialoguePanel != null && dm.dialoguePanel.activeInHierarchy &&
                    dm.nameText != null && dm.dialogueText != null && dm.nextButton != null)
                {
                    // currentDialogue 접근을 위해 리플렉션 사용 (Dialogue_Manage 수정 불가 가정)
                    var priv = BindingFlags.NonPublic | BindingFlags.Instance;
                    var currentDialogueField = typeof(Dialogue_Manage).GetField("currentDialogue", priv);
                    var currentDialogue = currentDialogueField?.GetValue(dm);

                    if (currentDialogue != null)
                    {
                        dialogueStarted = true;
                        Debug.Log($"[DeferredDialogue.Runner] 대화 시작 성공: {csvName} (시도 {retryCount + 1}회)");

                        // 대화 시작 후 MouseLook 상태 확인
                        if (MouseLook.instance != null)
                        {
                            Debug.Log($"[DeferredDialogue.Runner] 대화 시작 후 MouseLook 상태 - isLockOn: {MouseLook.instance.isLockOn()}");
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"[DeferredDialogue.Runner] currentDialogue is null: {csvName}");
                    }
                }
                else
                {
                    Debug.LogWarning($"[DeferredDialogue.Runner] UI 컴포넌트 준비 안됨: dialoguePanel={dm?.dialoguePanel != null}, nameText={dm?.nameText != null}, dialogueText={dm?.dialogueText != null}, nextButton={dm?.nextButton != null}");
                }

                if (!dialogueStarted)
                {
                    retryCount++;
                    Debug.LogWarning($"[DeferredDialogue.Runner] 대화 시작 실패, 재시도 {retryCount}/{maxRetries}: {csvName}");
                    if (retryCount < maxRetries)
                    {
                        yield return new WaitForSeconds(0.5f);
                    }
                }
            }

            if (dialogueStarted)
            {
                //  보상중에는 Z 안되게 설정
                if (CanvasController.Instance != null)
                {
                    CanvasController.Instance.canToggleByZ = false;
                    CanvasController.Instance.isCanvasOn = false;
                }
                StartCoroutine(WaitForEndAndReward());
            }
            else
            {
                Debug.LogError($"[DeferredDialogue.Runner] 대화 시작 최종 실패: {csvName}");

                // 최종 실패 시 MouseLook 상태 복구
                if (MouseLook.instance != null && MouseLook.instance.isLockOn())
                {
                    MouseLook.instance.ToggleLock();
                    Debug.Log("[DeferredDialogue.Runner] 최종 실패 시 MouseLook 상태 복구");
                }

                // 플레이어 이동 복구
                if (GameManager.Instance?.playerMovement != null)
                {
                    GameManager.Instance.playerMovement.SetMovement(true);
                }

                Destroy(gameObject);
            }
        }

        private IEnumerator WaitForEndAndReward()
        {
            yield return new WaitUntil(() =>
                Dialogue_Manage.Instance != null &&
                Dialogue_Manage.Instance.isEndLine()
            );

            // MouseLook 상태를 플레이어 이동용으로 확실히 설정
            if (MouseLook.instance != null)
            {
                // 대화 종료 시에는 플레이어 이동 모드로 강제 설정
                if (!MouseLook.instance.isLockOn())
                {
                    MouseLook.instance.ToggleLock();
                }
                Debug.Log($"[DeferredDialogue.Runner] 대화 종료 후 MouseLook 상태 - isLockOn: {MouseLook.instance.isLockOn()}");
            }

            // 플레이어 이동 활성화
            if (GameManager.Instance?.playerMovement != null)
            {
                GameManager.Instance.playerMovement.SetMovement(true);
                Debug.Log("[DeferredDialogue.Runner] 플레이어 이동 활성화");
            }

            Debug.Log("[DeferredDialogue.Runner] 대화 종료 감지, 보상 처리 시작");
            yield return StartCoroutine(ProcessReward());

            //  보상후에는 Z 되게 설정
            if (CanvasController.Instance != null)
            {
                CanvasController.Instance.canToggleByZ = true;
           
            }
           
            Debug.Log("[DeferredDialogue.Runner] 처리 완료, Runner 제거");
            Destroy(gameObject);
        }

        private IEnumerator ProcessReward()
        {
            var rewarder = Object.FindObjectOfType<FlagItemRewarder>(true);
            if (rewarder != null)
            {
                var entry = rewarder.GetReward(flagName, csvName);
                if (entry != null && !entry.rewarded)
                {
                    yield return new WaitForSeconds(0.5f);   
                    try
                    {
                        Debug.Log(entry.itemPath);
                        if (entry.itemPath != "")
                        {
                            ItemController.Instance.AddItemToInventory(
                                entry.itemPath,
                                entry.itemName,
                                entry.itemDescription
                            );
                            entry.rewarded = true;
                            Debug.Log($"[DeferredDialogue] 보상 지급 완료: {entry.itemName}");
                        }
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"[DeferredDialogue] 보상 지급 실패: {e.Message}");
                    }
                }
                else if (entry != null && entry.rewarded)
                {
                    Debug.Log($"[DeferredDialogue] 이미 보상된 항목, 건너뜀: {entry.itemName}");
                }
            }

            if (!string.IsNullOrEmpty(itemPath))
            {
                yield return new WaitForSeconds(0.5f);
                try
                {
                    ItemController.Instance.AddItemToInventory(itemPath, itemName, itemDesc);
                    Debug.Log($"[DeferredDialogue] 직접 보상 지급: {itemName}");
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[DeferredDialogue] 직접 보상 지급 실패: {e.Message}");
                }
            }
        }
    }
}