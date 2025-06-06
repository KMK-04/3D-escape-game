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
        string originalScene = GameManager.Instance.GetOriginalSceneName();
        SceneManager.LoadScene(originalScene);
    }

    public void BirdScored() {
        if(isGameover) {
            return;
        }
        score++;
        scoreText.text = "Score : " + score.ToString();
        if (score >= 3)
        {
            GameManager.Instance.SetBoolean(6, false);
            string originalScene = GameManager.Instance.GetOriginalSceneName();
            SceneManager.LoadScene(originalScene);
        }
    }
}