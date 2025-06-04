using UnityEngine;
using UnityEngine.UI; // UI ���� ����� ����ϱ� ���� �ʿ�
using TMPro; // TextMeshPro ���� ����� ����ϱ� ���� �ʿ�

public class Bcnmin : MonoBehaviour
{
    public TextMeshProUGUI counterText; // ���ڸ� ǥ���� Text (TMP) ������Ʈ (Inspector���� �Ҵ�)

    // ��ư Ŭ�� �� ȣ��� �޼���
    public void OnButtonClick()
    {
        if (counterText != null)
        {
            // ���� �ؽ�Ʈ�� ���ڷ� ��ȯ (�Ľ� ���� �� 0���� ����)
            int currentNumber = int.Parse(counterText.text);

            if (!int.TryParse(counterText.text, out currentNumber))
            {
                currentNumber = 0;
            }

            // ���� ����
            currentNumber -= 1;

            // 9�� �ʰ��ϸ� 0���� �ǵ���

            // �ؽ�Ʈ ������Ʈ
            counterText.text = currentNumber.ToString();
            Debug.Log($"���� ������Ʈ: {currentNumber}");
        }
        else
        {
            Debug.LogWarning("Text (TMP) ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }
}
