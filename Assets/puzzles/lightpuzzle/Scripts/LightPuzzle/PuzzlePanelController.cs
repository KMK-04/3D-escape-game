using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class item : MonoBehaviour
{
    public string targetSceneName; 

    // ��ư Ŭ�� �� ȣ��� �޼���
    public void OnButtonClick2()
    {

        Debug.Log("Ŭ����! " + targetSceneName + " ������ �̵��մϴ�.");
        SceneManager.LoadScene(targetSceneName);
    }
}
