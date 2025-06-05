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

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        StartDialogue("example"); // Resources 폴더 내 example.csv
    }

    public void StartDialogue(string csvFileName)
    {
        DatabaseManager.instance.LoadDialogueFromCSV(csvFileName);
        int count = DatabaseManager.instance.dialogueCount;
        currentDialogue = DatabaseManager.instance.GetDialogue(1, count);
        dialogueIndex = 0;
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
            dialogueText.text = "[대화 종료]";
            nameText.text = "";
            return;
        }

        var dialogue = currentDialogue[dialogueIndex];

        if (contextIndex >= dialogue.contexts.Length)
        {
            // 다음 NPC로 이동
            dialogueIndex++;
            contextIndex = 0;
            ShowNextLine(); // 재귀 호출로 다음 NPC 처리
            return;
        }

        fullText = dialogue.contexts[contextIndex];

        nameText.text = dialogue.name;
        dialogueText.text = "";
        logManager.Add_Log(new string[] { $"[{dialogue.name}] {fullText}" });

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(fullText));
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
