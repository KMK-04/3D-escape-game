using SojaExiles;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
    public static CanvasController Instance;
    public GameObject canvasUI;  // Canvas 내부 UI 루트 패널 (예: Panel, 또는 전체 UI 묶음)
    public GameObject phoneUI;
    public GameObject DialogueUI;
    public GameObject InvenUI;
    public GameObject LogUI;

    public Icon_Active_Manager iconActiveManager;

    public bool canToggleByZ = false;
    public bool isCanvasOn = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 씬 변경 시 콜백 등록
            SceneManager.sceneLoaded += OnSceneLoaded;

            // 현재 씬 체크
            CheckScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // 메모리 누수 방지용 콜백 해제
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckScene(scene.name);

        // 클릭된 아이템들 자동 제거
        RemoveClickedItemsInScene();
    }

    private void RemoveClickedItemsInScene()
    {
        Item[] itemsInScene = FindObjectsOfType<Item>();
        foreach (var item in itemsInScene)
        {
            if (InventorySaveManager.clickedItemNames.Contains(item.ITEM_Name))
            {
                Destroy(item.gameObject);
            }
        }
    }

    private void CheckScene(string sceneName)
    {
        if (sceneName == "Scene_01")
        {
            //canvasUI.SetActive(true); // UI 보이기
        }
        else
        {
            LogUI.SetActive(false);
            InvenUI.SetActive(false);
            canvasUI.SetActive(false); // UI 숨기기    
            phoneUI.SetActive(false);
        }
    }


    public void TrySetActiveIfSceneIs01()
    {
        CheckScene(SceneManager.GetActiveScene().name);
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "Scene_01") return;

        if (!canToggleByZ) //대화중일때는
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(phoneUI.activeSelf== false && LogUI.activeSelf== false && InvenUI.activeSelf== false) //모두 active가 해제되어있어야 사용가능
                {
                    Dialogue_Manage.Instance.OnNextButtonClicked();
                }
               
            }
        }
        else //대화가아닌상태 
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
              
                isCanvasOn = !isCanvasOn;

                bool currentState = canvasUI.activeSelf;
                canvasUI.SetActive(!currentState);

                iconActiveManager.On_Panel();
                iconActiveManager.Off_Panel();

                MouseLook.instance.ToggleLock();

                if (!isCanvasOn) // 끌때
                {
                    GameManager.Instance.playerMovement.SetMovement(true);                  
                }
                else //킬때
                {
                    GameManager.Instance.playerMovement.SetMovement(false);
                }

            }
        }
    }


}
