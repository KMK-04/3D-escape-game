using UnityEngine;
using UnityEngine.UI; // UI ���� ����� ����ϱ� ���� �ʿ�
using TMPro; // TextMeshPro ���� ����� ����ϱ� ���� �ʿ�
using System.Collections; // �ڷ�ƾ ����� ���� �ʿ�

public class showtext : MonoBehaviour
{
    public TextMeshProUGUI targetText; // ������ Text (TMP) ������Ʈ (Inspector���� �Ҵ�)
    public string newText = "Hello!"; // ������ �ؽ�Ʈ (Inspector���� ���� ����)

    // ��ư Ŭ�� �� ȣ��� �޼���
    public void OnButtonClick()
    {
        if (targetText != null)
        {
            // �ؽ�Ʈ ���� �� Ȱ��ȭ
            targetText.text = newText;
            targetText.gameObject.SetActive(true);
            Debug.Log($"�ؽ�Ʈ �����: {newText}");

            // 3�� �� ��Ȱ��ȭ�ϴ� �ڷ�ƾ ����
            StartCoroutine(DisableTextAfterDelay(3.0f));
        }
        else
        {
            Debug.LogWarning("Text (TMP) ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    // ������ �ð� �� �ؽ�Ʈ�� ��Ȱ��ȭ�ϴ� �ڷ�ƾ
    private IEnumerator DisableTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (targetText != null)
        {
            targetText.gameObject.SetActive(false);
            Debug.Log("�ؽ�Ʈ ��Ȱ��ȭ��");
        }
    }
}
