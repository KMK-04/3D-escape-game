using System.Collections;
using SojaExiles;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Dialogue_Manage : MonoBehaviour
{
    public static Dialogue_Manage Instance;
    public Text nameText;          // 이름 출력용
    public Text dialogueText;      // 대사 출력용
    public Button nextButton;
    public LogManager logManager;
    public GameObject dialoguePanel;

    private Dialgoue[] currentDialogue;
    private int dialogueIndex = 0;
    private int contextIndex = 0;      // 현재 NPC의 대사 줄 번호

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private string fullText = "";

    public DialogueProgress currentProgress = new DialogueProgress();
    public PlayerMovement player;
    public bool ShowImage = false;
    public RawImage Image_set;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // 현재 씬 이름 가져오기
            string sceneName = SceneManager.GetActiveScene().name;

            // Intro 씬이 아닐 때만 DontDestroyOnLoad
            if (sceneName != "Intro")
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        StartDialogue(DatabaseManager.instance.csv_FileName); // Resources 폴더 내 example.csv
        if(player!= null)
        {
            player.SetMovement(false);
        }  
    }

    public void StartDialogue(string csvFileName)
    {
        currentProgress.csvFileName = csvFileName;

        DatabaseManager.instance.LoadDialogueFromCSV(csvFileName);
        int count = DatabaseManager.instance.dialogueCount;
        currentDialogue = DatabaseManager.instance.GetDialogue(1, count);

        dialogueIndex = currentProgress.dialogueIndex;
        contextIndex = currentProgress.contextIndex;

        logManager.Init_Log();
        ShowNextLine();
    }



    public void OnNextButtonClicked()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = fullText;
            isTyping = false;
        }
        else
        {
            ShowNextLine();
        }
    }
    public bool isEndLine()
    {
        if (dialogueIndex >= currentDialogue.Length)
            return true;
        else
            return false;
    }
    public void ShowNextLine()
    {
        if (isEndLine())
        {
            if (SceneManager.GetActiveScene().name == "Intro")
            {
                DatabaseManager.instance.csv_FileName = "example";
                SceneManager.LoadScene("Scene_01");
                return;
            }

            if (player != null)
            {
                GameManager.Instance.playerMovement.SetMovement(true);

            }

            if (!MouseLook.instance.isLockOn()){
                MouseLook.instance.ToggleLock();
            }
             
        

            dialogueText.text = "[마지막 대화입니다. 메뉴창을 열거나 닫으실려면 Z 키를 눌러주세요!]";
            nameText.text = "System";

            //  Z로 Canvas 열기 가능 설정
            if (CanvasController.Instance != null)
            {
                CanvasController.Instance.canToggleByZ = true;
            }
            dialoguePanel.SetActive(false);
            
            return;
        }

        var dialogue = currentDialogue[dialogueIndex];
        if (player != null)
        {
            GameManager.Instance.playerMovement.SetMovement(false);
        }

        //  대화 진행중에는 Z 안되게 설정
        if (CanvasController.Instance != null)
        {
            CanvasController.Instance.canToggleByZ = false;
        }

        if (contextIndex >= dialogue.contexts.Length)
        {
            dialogueIndex++;
            contextIndex = 0;
            ShowNextLine(); // 다음 NPC 처리
            return;
        }

        fullText = dialogue.contexts[contextIndex];

        nameText.text = dialogue.name;
        dialogueText.text = "";
        logManager.Add_Log(new string[] { $"[{dialogue.name}] {fullText}" });

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(fullText));

        //  현재 대화 진행 상태 저장
        currentProgress.dialogueIndex = dialogueIndex;
        currentProgress.contextIndex = contextIndex;

        // 이미지 표시 로직 추가
        if (ShowImage && Image_set != null)
        {
            int imageNum;
            if (int.TryParse(dialogue.number[contextIndex], out imageNum))
            {
                Texture img = Resources.Load<Texture>($"Sprites/IntroImage/{imageNum}");
                if (img != null)
                {
                    Image_set.texture = img;
                    Image_set.gameObject.SetActive(true);
                }
                else
                {
                    Debug.LogWarning($"[Dialogue_Manage] 이미지 파일을 찾을 수 없습니다: Sprites/IntroImage/{imageNum}");
                }
            }

        }
        contextIndex++;
     
    }




    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.04f);
        }

        isTyping = false;
    }
}
