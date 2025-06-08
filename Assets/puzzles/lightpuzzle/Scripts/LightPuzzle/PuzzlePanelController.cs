using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class item : MonoBehaviour
{
    public string targetSceneName; 

    // 버튼 클릭 시 호출될 메서드
    public void OnButtonClick2()
    {

        DeferredDialogue.Request(
            csvName: "fail",
            flagName: "fail"
        );
        GameManager.Instance.ReturnToOriginalScene();
    }
}
