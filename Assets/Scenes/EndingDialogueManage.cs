using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EndingDialogueManager : MonoBehaviour
{
    public static EndingDialogueManager Instance;

    [Header("대화(CSV) 설정")]
    [Tooltip("이 씬용 CSV 파일 이름 (Resources 폴더, 확장자 제외)")]
    public string dialogueCSVName       = "ending";
    [Tooltip("Intro 씬으로 돌아갈 때 초기화할 CSV 파일 이름")]
    public string initialDialogueCSVName = "example";

    [Header("UI References")]
    public Text    nameText;
    public Text    dialogueText;
    public Button  nextButton;
    public GameObject dialoguePanel;

    [Header("이미지 표시 (선택)")]
    public bool     showImage           = false;
    public RawImage imageDisplay;
    [Tooltip("Resources 폴더 내 이미지 폴더 경로 (예: Sprites/EndingImages)")]
    public string   imageResourceFolder = "Sprites/IntroImage";

    [Header("게임 오버 씬 로드 (선택)")]
    [Tooltip("대화 종료 후 로드할 씬 이름. 비어 있으면 onDialogueEnd만 실행")]
    public string gameOverSceneName = "";

    [Header("대화 종료 시 이벤트 (씬 미설정 시)")]
    public UnityEvent onDialogueEnd;

    private Dialgoue[] dialogues;
    private int dialogueIndex = 0;
    private int contextIndex  = 0;

    private Coroutine typingCoroutine;
    private bool      isTyping = false;
    private string    fullLine = "";

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // 엔딩 CSV 로드
        DatabaseManager.instance.LoadDialogueFromCSV(dialogueCSVName);
        dialogues = DatabaseManager.instance.GetDialogue(1, DatabaseManager.instance.dialogueCount);

        nextButton.onClick.AddListener(OnNextButtonClicked);
        ShowNextLine();
    }

    void OnDestroy()
    {
        nextButton.onClick.RemoveListener(OnNextButtonClicked);
    }

    public void OnNextButtonClicked()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = fullLine;
            isTyping = false;
        }
        else
        {
            ShowNextLine();
        }
    }

    private void ShowNextLine()
    {
        // 대화가 모두 끝난 경우
        if (dialogueIndex >= dialogues.Length)
        {
            dialoguePanel.SetActive(false);

            if (!string.IsNullOrEmpty(gameOverSceneName))
            {
                // Intro 씬 CSV 초기화
                DatabaseManager.instance.csv_FileName = initialDialogueCSVName;

                Destroy(gameObject);
                SceneManager.LoadScene(gameOverSceneName);
            }
            else
            {
                onDialogueEnd?.Invoke();
            }
            return;
        }

        var block = dialogues[dialogueIndex];

        // 블록 내 문장 모두 소진 시 다음 블록
        if (contextIndex >= block.contexts.Length)
        {
            dialogueIndex++;
            contextIndex = 0;
            ShowNextLine();
            return;
        }

        // 텍스트·이름 세팅
        fullLine     = block.contexts[contextIndex];
        nameText.text = block.name;
        dialogueText.text = "";

        // 이미지 표시 로직
        if (showImage && imageDisplay != null &&
            block.number != null && contextIndex < block.number.Length &&
            int.TryParse(block.number[contextIndex], out int imgIndex))
        {
            string path = $"{imageResourceFolder}/{imgIndex}";
            var tex = Resources.Load<Texture>(path);
            if (tex != null)
            {
                imageDisplay.texture = tex;
                imageDisplay.gameObject.SetActive(true);
            }
            else
            {
                imageDisplay.gameObject.SetActive(false);
                Debug.LogWarning($"이미지를 찾을 수 없습니다: Resources/{path}");
            }
        }
        else if (imageDisplay != null)
        {
            imageDisplay.gameObject.SetActive(false);
        }

        // 타이핑 애니메이션
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(fullLine));

        contextIndex++;
    }

    private IEnumerator TypeText(string text)
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
