using System.Collections.Generic;

public static class InventorySaveManager
{
    public static List<ItemData> savedItems = new List<ItemData>();
    // ✅ 클릭한 아이템 이름 저장용
    public static HashSet<string> clickedItemNames = new HashSet<string>();
}
