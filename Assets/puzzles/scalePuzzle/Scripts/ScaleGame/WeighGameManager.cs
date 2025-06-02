using UnityEngine;
using UnityEngine.UI;
using JusticeScale.Scripts;

public class WeighGameManager : MonoBehaviour
{
    public static WeighGameManager Instance { get; private set; }

    [Header("퍼즐 오브젝트들")]
    public BallWeight[] balls;                   // 씬에 배치된 9구슬
    public ScaleBeamRotation beamRot;            // 기존 ScaleBeamRotation
    public Button weighButton;                   // 저울질 버튼
    public Text weighCountText;                  // 횟수 UI
    public Text resultText;                      // 결과 피드백

    [Header("매개변수")]
    public int maxWeighs = 2;
    public float normalMass = 1f;
    public float heavyMass = 2f;

    int usedWeighs;
    int heavyIndex;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // RushHourActivator에서 퍼즐 시작 시 호출
    public void SetupPuzzle()
    {
        // 1) 횟수 리셋
        usedWeighs = 0;
        weighButton.interactable = true;
        resultText.text = "";
        UpdateWeighUI();

        // 2) 무거운 구슬 랜덤 지정
        heavyIndex = Random.Range(0, balls.Length);
        for (int i = 0; i < balls.Length; i++)
        {
            balls[i].id = i;
            if (i == heavyIndex) balls[i].SetAsHeavy(heavyMass);
            else               balls[i].SetAsNormal(normalMass);
        }
    }

    void UpdateWeighUI()
    {
        weighCountText.text = $"남은 저울질: {maxWeighs - usedWeighs}회";
    }

    // 버튼 클릭으로 호출
    public void OnWeighButtonClicked()
    {
        if (usedWeighs >= maxWeighs) return;
        beamRot.Weigh();
        usedWeighs++;
        UpdateWeighUI();
        if (usedWeighs >= maxWeighs)
            weighButton.interactable = false;
    }

    // 구슬 클릭 시 호출
    public void SubmitGuess(int id)
    {
        if (usedWeighs == 0)
        {
            resultText.text = "먼저 저울질하세요!";
            return;
        }

        if (id == heavyIndex)
            resultText.text = "정답! 🎉 방 탈출 완료!";
        else
            resultText.text = "땡! 다시 시도해보세요.";
    }
}
