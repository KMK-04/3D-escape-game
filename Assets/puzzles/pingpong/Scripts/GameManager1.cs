using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class GameManagerpingpong : MonoBehaviour
{
    [SerializeField] private Ball ball;
    [SerializeField] private Paddle playerPaddle;
    [SerializeField] private Paddle computerPaddle;
    [SerializeField] private Text playerScoreText;
    [SerializeField] private Text computerScoreText;
    public int goalScore ;
    private int playerScore;
    private int computerScore;
    private bool gameOver = false; // 게임 종료 여부를 확인하는 플래그

    private void Start()
    {
        NewGame();
        goalScore = 2;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            NewGame();
        }
    }

    public void NewGame()
    {
        SetPlayerScore(0);
        SetComputerScore(0);
        gameOver = false; // 새 게임 시작 시 게임 종료 플래그 초기화
        NewRound();
    }

    public void NewRound()
    {
        if (!gameOver) // 게임이 종료되지 않았을 때만 새 라운드 시작
        {
            playerPaddle.ResetPosition();
            computerPaddle.ResetPosition();
            ball.ResetPosition();

            CancelInvoke();
            Invoke(nameof(StartRound), 1f);
        }
    }

    private void StartRound()
    {
        ball.AddStartingForce();
    }

    public void OnPlayerScored()
    {
        SetPlayerScore(playerScore + 1);
        CheckGameOver(); // 점수 업데이트 후 게임 종료 여부 확인
        if (!gameOver)
        {
            NewRound();
        }
    }

    public void OnComputerScored()
    {
        SetComputerScore(computerScore + 1);
        CheckGameOver(); // 점수 업데이트 후 게임 종료 여부 확인
        if (!gameOver)
        {
            NewRound();
        }
    }

    private void SetPlayerScore(int score)
    {
        playerScore = score;
        playerScoreText.text = score.ToString();
    }

    private void SetComputerScore(int score)
    {
        computerScore = score;
        computerScoreText.text = score.ToString();
    }

    private void CheckGameOver()
    {
        if (playerScore >= goalScore)
        {
            gameOver = true;
            GameManager.Instance.SetBoolean(7, false);
            DeferredDialogue.Request(
csvName: "animal",
flagName: "ping"
);
            GameManager.Instance.ReturnToOriginalScene();
        }
        else if (computerScore >= goalScore)
        {
            gameOver = true;
            DeferredDialogue.Request(
csvName: "fail",
flagName: "fail3"
);
            GameManager.Instance.ReturnToOriginalScene();
        }
    }
}
