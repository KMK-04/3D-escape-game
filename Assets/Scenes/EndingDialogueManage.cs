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
    public LogManager logManager;

    [Header("게임 시스템 참조")]
    public Phone phone;

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
    
    public bool canNext = true;    // 현재 대화를 넘길 수 있는 상태인지

    // 대화 진행 상태 저장용
    public DialogueProgress currentProgress = new DialogueProgress();

    void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
            
            // 현재 씬 이름 가져오기
            string sceneName = SceneManager.GetActiveScene().name;

            // Intro 씬이 아닐 때만 DontDestroyOnLoad (필요시)
            if (sceneName != "Intro") {
                // DontDestroyOnLoad(gameObject); // 엔딩 매니저는 보통 씬 전환 시 파괴되어야 하므로 주석 처리
            }
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartDialogue(dialogueCSVName);
    }

    void OnDestroy()
    {
        if (nextButton != null)
            nextButton.onClick.RemoveListener(OnNextButtonClicked);
    }

    public void StartDialogue(string csvFileName)
    {
        currentProgress.csvFileName = csvFileName;

        // 엔딩 CSV 로드
        DatabaseManager.instance.LoadDialogueFromCSV(csvFileName);
        int count = DatabaseManager.instance.dialogueCount;
        dialogues = DatabaseManager.instance.GetDialogue(1, count);

        dialogueIndex = currentProgress.dialogueIndex;
        contextIndex = currentProgress.contextIndex;

        if (logManager != null) {
            logManager.Init_Log();
        }

        if (nextButton != null) {
            nextButton.onClick.AddListener(OnNextButtonClicked);
        }
        
        ShowNextLine();
    }

    public void OnNextButtonClicked()
    {
        if (canNext) {
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
    }

    public bool isEndLine() 
    {
        return dialogueIndex >= dialogues.Length;
    }

    public bool CheckNext() 
    {
        canNext = (phone == null || !phone.isOpen);
        return canNext;
    }

    private void ShowNextLine()
    {
        // 대화가 모두 끝난 경우
        if (isEndLine())
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
                // 엔딩 완료 메시지 표시
                if (dialogueText != null) {
                    dialogueText.text = "[엔딩 대화가 완료되었습니다. 메뉴창을 열거나 닫으실려면 Z 키를 눌러주세요!]";
                }
                if (nameText != null) {
                    nameText.text = "System";
                }
                
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
        fullLine = block.contexts[contextIndex];
        nameText.text = block.name;
        dialogueText.text = "";
        
        // 로그 시스템에 추가
        if (logManager != null) {
            logManager.Add_Log(new string[] { $"[{block.name}] {fullLine}" });
        }

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
                Debug.LogWarning($"[EndingDialogueManager] 이미지 파일을 찾을 수 없습니다: Resources/{path}");
            }
        }
        else if (imageDisplay != null)
        {
            imageDisplay.gameObject.SetActive(false);
        }

        // 현재 대화 진행 상태 저장
        currentProgress.dialogueIndex = dialogueIndex;
        currentProgress.contextIndex = contextIndex;

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