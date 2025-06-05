using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue_Manage : MonoBehaviour
{
    public static Dialogue_Manage Instance;
    public Text nameText;          // 이름 출력용
    public Text dialogueText;      // 대사 출력용
    public Button nextButton;
    public LogManager logManager;

    private Dialgoue[] currentDialogue;
    private int dialogueIndex = 0;
    private int contextIndex = 0;      // 현재 NPC의 대사 줄 번호

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private string fullText = "";

    public DialogueProgress currentProgress = new DialogueProgress();


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        } 
    }

    void Start()
    {
        StartDialogue("example"); // Resources 폴더 내 example.csv
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

    public void ShowNextLine()
    {
        if (dialogueIndex >= currentDialogue.Length)
        {
            dialogueText.text = "[마지막 대화입니다. 메뉴창을 열거나 닫으실려면 ESC 키를 눌러주세요!]";
            nameText.text = "System";

            //  ESC로 Canvas 열기 가능 설정
            if (CanvasController.Instance != null)
            {
                CanvasController.Instance.canToggleByESC = true;
            }
            
            return;
        }

        var dialogue = currentDialogue[dialogueIndex];

        //  대화 진행중에는 ESC안되게 설정
        if (CanvasController.Instance != null)
        {
            CanvasController.Instance.canToggleByESC = false;
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

        // 🔥 현재 대화 진행 상태 저장
        currentProgress.dialogueIndex = dialogueIndex;
        currentProgress.contextIndex = contextIndex;

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
