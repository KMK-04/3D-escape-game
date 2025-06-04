using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class shotgame : MonoBehaviour
{
    public GameObject parentObject; // 자식 오브젝트들을 포함하는 부모 오브젝트
    public Button startButton; // 게임 시작 버튼
    public TextMeshProUGUI timerText; // 타이머 표시용 텍스트
    public TextMeshProUGUI scoreText; // 점수 표시용 텍스트
    public TextMeshProUGUI gamestartt;
    public float gameDuration = 29f; // 게임 지속 시간 (초)
    public int score = 0; // 점수 저장 변수

    private float timeRemaining; // 남은 시간
    private bool isGameRunning = false; // 게임 진행 여부
    private GameObject currentActiveChild; // 현재 활성화된 자식 오브젝트

    void Start()
    {
        // 시작 버튼에 클릭 이벤트 리스너 추가
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }
        else
        {
            Debug.LogWarning("Start Button이 할당되지 않았습니다.");
        }

        // 초기 UI 설정
        UpdateScoreText();
        UpdateTimerText();
    }

    void Update()
    {
        if (isGameRunning)
        {
            // 타이머 감소
            timeRemaining -= Time.deltaTime;
            UpdateTimerText();

            // 게임 종료 조건
            if (timeRemaining <= 0)
            {
                EndGame();
            }
        }
    }

    // 게임 시작
    public void StartGame()
    {
        if (!isGameRunning)
        {
            isGameRunning = true;
            timeRemaining = gameDuration; gamestartt.text = "";
            score = 0;
            UpdateScoreText();
            UpdateTimerText();
            ActivateRandomChild();
            Debug.Log("게임 시작!");
        }
    }

    // 게임 종료
    private void EndGame()
    {
        isGameRunning = false;
        if (currentActiveChild != null)
        {
            currentActiveChild.SetActive(false);
        }
        Debug.Log($"게임 종료! 최종 점수: {score}");
        gamestartt.text = "Game End!";
    }

    // 랜덤 자식 오브젝트 활성화
    private void ActivateRandomChild()
    {
        if (parentObject == null)
        {
            Debug.LogWarning("Parent Object가 할당되지 않았습니다.");
            return;
        }

        // 모든 자식 오브젝트 비활성화
        foreach (Transform child in parentObject.transform)
        {
            child.gameObject.SetActive(false);
        }

        // 자식 오브젝트 중 하나를 랜덤으로 선택
        int childCount = parentObject.transform.childCount;
        if (childCount > 0)
        {
            int randomIndex = Random.Range(0, childCount);
            currentActiveChild = parentObject.transform.GetChild(randomIndex).gameObject;
            currentActiveChild.SetActive(true);
            Debug.Log($"활성화된 자식 오브젝트: {currentActiveChild.name}");
        }
        else
        {
            Debug.LogWarning("Parent Object에 자식 오브젝트가 없습니다.");
        }
    }

    // 점수 증가 및 새 오브젝트 활성화 (클릭 시 호출)
    public void OnTargetClicked()
    {
        if (isGameRunning && currentActiveChild != null)
        {
            currentActiveChild.SetActive(false);
            score++;
            UpdateScoreText();
            ActivateRandomChild();
            Debug.Log($"클릭 성공! 현재 점수: {score}");
        }
    }

    // 점수 텍스트 업데이트
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }

    // 타이머 텍스트 업데이트
    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = $"Time: {Mathf.CeilToInt(timeRemaining)}";
        }
    }
}
