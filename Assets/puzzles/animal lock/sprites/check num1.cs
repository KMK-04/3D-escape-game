using UnityEngine;
using UnityEngine.UI; // UI ���� ����� ����ϱ� ���� �ʿ�
using TMPro; // TextMeshPro ���� ����� ����ϱ� ���� �ʿ�
using UnityEngine.SceneManagement;

public class check_num1 : MonoBehaviour
{
    public TextMeshProUGUI text1; // ù ��° Text (TMP) ������Ʈ (Inspector���� �Ҵ�)
    public TextMeshProUGUI text2; // �� ��° Text (TMP) ������Ʈ (Inspector���� �Ҵ�)
    public TextMeshProUGUI text3; // �� ��° Text (TMP) ������Ʈ (Inspector���� �Ҵ�)
    public string targetSceneName = "TargetScene";

    // ��ư Ŭ�� �� ȣ��� �޼���
    public void OnButtonClick()
    {
        if (text1 != null && text2 != null && text3 != null)
        {
            // �� �ؽ�Ʈ�� ���ڷ� ��ȯ (�Ľ� ���� �� 0���� ����)
            int number1 = 0;
            int number2 = 0;
            int number3 = 0;

            if (!int.TryParse(text1.text, out number1))
            {
                number1 = 0;
            }
            if (!int.TryParse(text2.text, out number2))
            {
                number2 = 0;
            }
            if (!int.TryParse(text3.text, out number3))
            {
                number3 = 0;
            }

            // ���� Ȯ�� (11, 3, 2)
            if (number1 == 2 && number2 == 6 && number3 == 11)
            {
                Debug.Log("���� ����! ���� ����: 11, 6, 2");
                GameManager.Instance.SetBoolean(2, false);
                string originalScene = GameManager.Instance.GetOriginalSceneName();
                if (!string.IsNullOrEmpty(originalScene))
                {
                               DeferredDialogue.Request(
    csvName: "animal",
    flagName: "animal"
);
            GameManager.Instance.ReturnToOriginalScene();
                }
                else
                {
                    Debug.LogWarning("���� �� �̸��� ������� �ʾҽ��ϴ�. �⺻ ������ �̵��մϴ�.");
                    SceneManager.LoadScene("DefaultScene"); // �⺻ �� �̸����� ��ü
                }
            }
            else
            {
                Debug.Log($"���� ������. ���� ����: {number1}, {number2}, {number3}");
            }
        }
        else
        {
            Debug.LogWarning("�ϳ� �̻��� Text (TMP) ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }
}
