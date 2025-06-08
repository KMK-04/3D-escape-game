// Assets/Scripts/Dialogue/DeferredDialogue.cs
using System.Collections;
using System.Collections.Generic;
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
            StartCoroutine(DelayedStart());
        }

        private IEnumerator DelayedStart()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(0.1f);

            try
            {
                DialogueHelper.PrepareAndShowDialogue(csvName);
                Debug.Log($"[DeferredDialogue.Runner] 대화 시작 완료: {csvName}");
                StartCoroutine(WaitForEndAndReward());
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[DeferredDialogue.Runner] 대화 시작 실패: {e.Message}");
                Destroy(gameObject);
            }
        }

        private IEnumerator WaitForEndAndReward()
        {
            yield return new WaitUntil(() =>
                Dialogue_Manage.Instance != null &&
                Dialogue_Manage.Instance.isEndLine()
            );

            Debug.Log("[DeferredDialogue.Runner] 대화 종료 감지, 보상 처리 시작");
            yield return StartCoroutine(ProcessReward());
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
                        ItemController.Instance.AddItemToInventory(
                            entry.itemPath,
                            entry.itemName,
                            entry.itemDescription
                        );
                        entry.rewarded = true;
                        Debug.Log($"[DeferredDialogue] 보상 지급 완료: {entry.itemName}");
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
