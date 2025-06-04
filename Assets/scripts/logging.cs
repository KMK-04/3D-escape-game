using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonClickHandler : MonoBehaviour
{
    public string targetSceneName = "TargetScene"; // �̵��� �� �̸� (�⺻�� ����)

    // ��ư Ŭ�� �� ȣ��� �޼���
    public void OnButtonClick()
    {
        // ���� �÷��̾� ��ġ�� �� �̸� ����
        GameManager.Instance.SavePlayerPosition(SceneManager.GetActiveScene().name);

        Debug.Log("Ŭ����! " + targetSceneName + " ������ �̵��մϴ�.");
        SceneManager.LoadScene(targetSceneName);
    }
}
