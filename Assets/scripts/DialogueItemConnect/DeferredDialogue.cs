// Assets/Scripts/Dialogue/DeferredDialogue.cs
using System.Collections;
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

        // 플래그 기록
        if (!string.IsNullOrEmpty(pendingFlag) && GameManager.Instance != null)
        {
            int idx = FlagCache.GetOrAdd(pendingFlag);
            GameManager.Instance.SetBoolean(idx, true);
            Debug.Log($"[DeferredDialogue] 플래그 설정: {pendingFlag} = true (인덱스: {idx})");
        }

        // Runner 생성
        var runner = new GameObject("DeferredDialogueRunner")
            .AddComponent<Runner>();
        Object.DontDestroyOnLoad(runner);

        runner.csvName = pendingCSV;
        runner.flagName = pendingFlag;
        runner.itemPath = pendingItemPath;
        runner.itemName = pendingItemName;
        runner.itemDesc = pendingItemDesc;

        // 요청 소모
        hasRequest = false;
    }

    private static class FlagCache
    {
        private static readonly System.Collections.Generic.Dictionary<string, int> map
            = new System.Collections.Generic.Dictionary<string, int>();

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
            
            // 약간의 지연 후 대화 시작 (씬 로딩 완료 대기)
            StartCoroutine(DelayedStart());
        }

        private IEnumerator DelayedStart()
        {
            // 프레임 대기
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
            // 대화 종료 대기
            yield return new WaitUntil(() =>
                Dialogue_Manage.Instance != null &&
                Dialogue_Manage.Instance.isEndLine()
            );

            Debug.Log("[DeferredDialogue.Runner] 대화 종료 감지, 보상 처리 시작");

            // 보상 처리
            yield return StartCoroutine(ProcessReward());

            Debug.Log("[DeferredDialogue.Runner] 처리 완료, Runner 제거");
            Destroy(gameObject);
        }

        private IEnumerator ProcessReward()
        {
            // FlagItemRewarder를 통한 보상
            var rewarder = Object.FindObjectOfType<FlagItemRewarder>(true);
            if (rewarder != null)
            {
                var entry = rewarder.GetReward(flagName, csvName);
                if (entry != null && !string.IsNullOrEmpty(entry.itemPath))
                {
                    yield return new WaitForSeconds(0.5f); // 약간의 지연
                    
                    try
                    {
                        ItemController.Instance.AddItemToInventory(
                            entry.itemPath,
                            entry.itemName,
                            entry.itemDescription
                        );
                        Debug.Log($"[DeferredDialogue] FlagItemRewarder 보상 지급: {entry.itemName}");
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"[DeferredDialogue] 보상 지급 실패: {e.Message}");
                    }
                }
                else
                {
                    Debug.Log($"[DeferredDialogue] FlagItemRewarder에서 보상을 찾을 수 없음: Flag={flagName}, CSV={csvName}");
                }
            }

            // 직접 전달된 아이템 정보로 보상 (fallback)
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