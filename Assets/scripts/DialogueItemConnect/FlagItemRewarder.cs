// Assets/Scripts/Dialogue/FlagItemRewarder.cs
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class FlagItemRewarder : MonoBehaviour
{
    private static FlagItemRewarder instance;
    [System.Serializable]
    public class RewardEntry
    {
        [Tooltip("플래그 이름 (DeferredDialogue.Request 시 사용한 flagName)")]
        public string flagName;

        [Tooltip("CSV 이름 (Resources 폴더 내 .csv 확장자 제외)")]
        public string csvName;

        [Header("인벤토리 추가 아이템 정보 (Resources/Sprites/Items 기준)")]
        [Tooltip("아이템 스프라이트 경로 (확장자 제외)")]
        public string itemPath;

        [Tooltip("아이템 이름")] public string itemName;
        [Tooltip("아이템 설명")] public string itemDescription;
    }

    [Tooltip("이벤트별 보상 항목 목록")] public List<RewardEntry> entries = new List<RewardEntry>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("[FlagItemRewarder] 중복 인스턴스 발견, 파괴됨");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 주어진 flagName 또는 csvName 로 매칭되는 보상 정보 조회
    /// </summary>
    public RewardEntry GetReward(string flagName, string csvName)
{
    Debug.Log($"[FlagItemRewarder] 보상 검색 - Flag: {flagName}, CSV: {csvName}");
    Debug.Log($"[FlagItemRewarder] 총 {entries.Count}개 항목 검색 중...");
    
    for (int i = 0; i < entries.Count; i++)
    {
        var e = entries[i];
        Debug.Log($"[FlagItemRewarder] 항목 {i}: flagName='{e.flagName}', csvName='{e.csvName}'");
        
        if (!string.IsNullOrEmpty(e.flagName) && e.flagName == flagName)
        {
            Debug.Log($"[FlagItemRewarder] flagName 매칭됨: {e.itemName}");
            return e;
        }
        if (!string.IsNullOrEmpty(e.csvName) && e.csvName == csvName)
        {
            Debug.Log($"[FlagItemRewarder] csvName 매칭됨: {e.itemName}");
            return e;
        }
    }
    Debug.Log("[FlagItemRewarder] 매칭되는 항목 없음");
    return null;
}
}