using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class item : MonoBehaviour
{
    public string targetSceneName; 

    // ��ư Ŭ�� �� ȣ��� �޼���
    public void OnButtonClick2()
    {

        DeferredDialogue.Request(
            csvName: "fail",
            flagName: "fail"
        );
        GameManager.Instance.ReturnToOriginalScene();
    }
}
