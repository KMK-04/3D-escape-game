using UnityEngine;
using UnityEngine.UI; // UI ���� ����� ����ϱ� ���� �ʿ�

public class ButtonActivator : MonoBehaviour
{
    public GameObject targetObject; // Ȱ��ȭ�� ������Ʈ (Inspector���� �Ҵ�)

    // ��ư Ŭ�� �� ȣ��� �޼���
    public void OnButtonClick()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true); // ������Ʈ Ȱ��ȭ
            Debug.Log($"������Ʈ Ȱ��ȭ��: {targetObject.name}");
        }
        else
        {
            Debug.LogWarning("Ȱ��ȭ�� ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }
}
