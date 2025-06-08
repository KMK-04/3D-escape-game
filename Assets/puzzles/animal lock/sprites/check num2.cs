using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonChecknum2 : MonoBehaviour
{
    public string targetSceneName = "TargetScene"; // �̵��� �� �̸� (�⺻�� ����)

    // ��ư Ŭ�� �� ȣ��� �޼���
    public void OnButtonClick()
    {
        string originalScene = GameManager.Instance.GetOriginalSceneName();
        if (!string.IsNullOrEmpty(originalScene))
        {
            DeferredDialogue.Request(
    csvName: "fail",
    flagName: "fail"
);
            GameManager.Instance.ReturnToOriginalScene();
        }
        else
        {
            Debug.LogWarning("���� �� �̸��� ������� �ʾҽ��ϴ�. �⺻ ������ �̵��մϴ�.");
            SceneManager.LoadScene("DefaultScene"); // �⺻ �� �̸����� ��ü
        }
    }
}
