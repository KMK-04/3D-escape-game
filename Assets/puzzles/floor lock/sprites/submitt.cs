using UnityEngine;
using UnityEngine.SceneManagement;
public class SequenceChecker1 : MonoBehaviour
{
    public GameObject[] children; // ChildActivator 스크립트가 붙은 자식 오브젝트 4개

    public void CheckActivatedCounts()
    {
        int[] targetPattern = { 4, 2, 2, 3 };

        if (children.Length != targetPattern.Length)
        {
            Debug.LogWarning("오브젝트 수가 4개가 아닙니다.");
            return;
        }

        for (int i = 0; i < children.Length; i++)
        {
            ChildActivator activator = children[i].GetComponent<ChildActivator>();

            if (activator != null)
            {
                if (activator.activatedCount != targetPattern[i])
                {
                    Debug.Log($"❌ 패턴 불일치: [{i}]번째 값은 {activator.activatedCount}, 기대값은 {targetPattern[i]}");
                    return;
                }
            }
        }

        Debug.Log("✅ 패턴 일치! (4, 2, 2, 3)");
        GameManager.Instance.SetBoolean(8, false);
        string originalScene = GameManager.Instance.GetOriginalSceneName();
        SceneManager.LoadScene(originalScene);
    }
}
