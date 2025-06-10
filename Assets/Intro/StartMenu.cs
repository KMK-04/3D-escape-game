using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class StartMenu : MonoBehaviour
{
    #if UNITY_EDITOR
        public SceneAsset sceneToLoad; // �����Ϳ��� �巡���� �� ����
    #endif
        [SerializeField, HideInInspector]
        private string sceneName; // ��Ÿ�ӿ� ���Ǵ� ���� �� �̸�

    public void OnStartButton()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene name is not set.");
        }
    }

    public void OnQuitButton()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    #if UNITY_EDITOR
        // ���� ����Ǹ� �ڵ����� sceneName �ʵ� ������Ʈ
        private void OnValidate()
        {
            if (sceneToLoad != null)
            {
                string path = AssetDatabase.GetAssetPath(sceneToLoad);
                sceneName = System.IO.Path.GetFileNameWithoutExtension(path);
            }
        }
    #endif
}
