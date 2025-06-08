using UnityEngine;
using UnityEngine.SceneManagement;

public class ScaleExit : MonoBehaviour
{

    /// <summary>
    /// X 버튼에서 호출합니다.
    /// </summary>
    public void OnExitButton()
    {
        DeferredDialogue.Request(
csvName: "fail",
flagName: "fail3"
);
        GameManager.Instance.ReturnToOriginalScene();
    }
}