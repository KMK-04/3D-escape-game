using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public static GameControl instance;
    public GameObject gameOverText;
    public bool isGameover = false;
    public float scrollSpeed = -1.5f;

    public Text scoreText;

    private int score = 0;

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else if (instance != null) {
            Destroy(this);
        }
    }
    private void Update() {
        if(isGameover == true && Input.GetMouseButtonDown(0)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
      }
    }

    public void BirdDied() {
        gameOverText.SetActive(true);
        isGameover = true;
        DeferredDialogue.Request(
csvName: "failf",
flagName: "failf"
);
        GameManager.Instance.ReturnToOriginalScene();
    }

    public void BirdScored() {
        if(isGameover) {
            return;
        }
        score++;
        scoreText.text = "Score : " + score.ToString();
        if (score >= 10)
        {
            GameManager.Instance.SetBoolean(6, false);
            DeferredDialogue.Request(
    csvName: "animal",
    flagName: "flappy"
);
            GameManager.Instance.ReturnToOriginalScene();
        }
    }
}