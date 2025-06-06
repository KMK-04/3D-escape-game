using UnityEngine;
using UnityEngine.SceneManagement;
public class toghome : MonoBehaviour
{
    public void CheckActivatedCounts()
    {
        string originalScene = GameManager.Instance.GetOriginalSceneName();
        SceneManager.LoadScene(originalScene);
    }
}
