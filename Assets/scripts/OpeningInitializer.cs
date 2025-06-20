using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningInitializer : MonoBehaviour
{
    void Awake()
    {
        CleanupDontDestroyOnLoadObjects();
    }

    void CleanupDontDestroyOnLoadObjects()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>(true);

        foreach (GameObject obj in allObjects)
        {
            Debug.Log(obj.scene.name);
            if (obj.scene.name == "DontDestroyOnLoad")
            {
                Destroy(obj);
            }
        }
    }
}
