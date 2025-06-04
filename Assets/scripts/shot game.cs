using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class shotgame : MonoBehaviour
{
    public GameObject parentObject; // �ڽ� ������Ʈ���� �����ϴ� �θ� ������Ʈ
    public Button startButton; // ���� ���� ��ư
    public TextMeshProUGUI timerText; // Ÿ�̸� ǥ�ÿ� �ؽ�Ʈ
    public TextMeshProUGUI scoreText; // ���� ǥ�ÿ� �ؽ�Ʈ
    public TextMeshProUGUI gamestartt;
    public float gameDuration = 29f; // ���� ���� �ð� (��)
    public int score = 0; // ���� ���� ����

    private float timeRemaining; // ���� �ð�
    private bool isGameRunning = false; // ���� ���� ����
    private GameObject currentActiveChild; // ���� Ȱ��ȭ�� �ڽ� ������Ʈ

    void Start()
    {
        // ���� ��ư�� Ŭ�� �̺�Ʈ ������ �߰�
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }
        else
        {
            Debug.LogWarning("Start Button�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        // �ʱ� UI ����
        UpdateScoreText();
        UpdateTimerText();
    }

    void Update()
    {
        if (isGameRunning)
        {
            // Ÿ�̸� ����
            timeRemaining -= Time.deltaTime;
            UpdateTimerText();

            // ���� ���� ����
            if (timeRemaining <= 0)
            {
                EndGame();
            }
        }
    }

    // ���� ����
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
            Debug.Log("���� ����!");
        }
    }

    // ���� ����
    private void EndGame()
    {
        isGameRunning = false;
        if (currentActiveChild != null)
        {
            currentActiveChild.SetActive(false);
        }
        Debug.Log($"���� ����! ���� ����: {score}");
        gamestartt.text = "Game End!";
    }

    // ���� �ڽ� ������Ʈ Ȱ��ȭ
    private void ActivateRandomChild()
    {
        if (parentObject == null)
        {
            Debug.LogWarning("Parent Object�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }

        // ��� �ڽ� ������Ʈ ��Ȱ��ȭ
        foreach (Transform child in parentObject.transform)
        {
            child.gameObject.SetActive(false);
        }

        // �ڽ� ������Ʈ �� �ϳ��� �������� ����
        int childCount = parentObject.transform.childCount;
        if (childCount > 0)
        {
            int randomIndex = Random.Range(0, childCount);
            currentActiveChild = parentObject.transform.GetChild(randomIndex).gameObject;
            currentActiveChild.SetActive(true);
            Debug.Log($"Ȱ��ȭ�� �ڽ� ������Ʈ: {currentActiveChild.name}");
        }
        else
        {
            Debug.LogWarning("Parent Object�� �ڽ� ������Ʈ�� �����ϴ�.");
        }
    }

    // ���� ���� �� �� ������Ʈ Ȱ��ȭ (Ŭ�� �� ȣ��)
    public void OnTargetClicked()
    {
        if (isGameRunning && currentActiveChild != null)
        {
            currentActiveChild.SetActive(false);
            score++;
            UpdateScoreText();
            ActivateRandomChild();
            Debug.Log($"Ŭ�� ����! ���� ����: {score}");
        }
    }

    // ���� �ؽ�Ʈ ������Ʈ
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }

    // Ÿ�̸� �ؽ�Ʈ ������Ʈ
    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = $"Time: {Mathf.CeilToInt(timeRemaining)}";
        }
    }
}
