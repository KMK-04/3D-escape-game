using UnityEngine;
using UnityEngine.SceneManagement;
public class toghome : MonoBehaviour
{
    public void CheckActivatedCounts()
    {
        DeferredDialogue.Request(
csvName: "fail",
flagName: "fail3"
);
        GameManager.Instance.ReturnToOriginalScene();
    }
}
