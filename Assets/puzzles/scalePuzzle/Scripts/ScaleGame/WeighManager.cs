using UnityEngine;
using UnityEngine.UI;
using JusticeScale.Scripts;
using TMPro;
using System.Collections;

/// <summary>
/// Manages the 9-ball weight puzzle: 2 weighs, then submit guess.
/// Subclasses implement AddInventory to handle successful pickup.
/// </summary>
public class WeighManager : MonoBehaviour
{
    [Header("Puzzle Objects")]
    [Tooltip("Scene's 9 ball objects (BallWeight components)")]
    public BallWeight[] balls;

    [Tooltip("ScaleBeamRotation controlling the beam")]
    public ScaleBeamRotation beamRot;

    [Header("UI (TMP)")]
    [Tooltip("Button to perform weighing")]
    public Button weighButton;

    [Tooltip("TextMeshPro text for remaining weighs")]
    public TMP_Text weighCountText;

    [Tooltip("Button to submit guess (initially inactive)")]
    public Button submitButton;

    [Header("Settings")]
    [Tooltip("Allowed number of weighs")]
    public int maxWeighs = 2;

    [Tooltip("Normal ball mass")]
    public float normalMass = 1f;

    [Tooltip("Heavy ball mass")]
    public float heavyMass = 2f;

    int usedWeighs;
    int heavyIndex;
    BallWeight selectedBall;

    void Awake()
    {
        // Weigh button setup
        if (weighButton != null)
        {
            weighButton.onClick.RemoveAllListeners();
            weighButton.onClick.AddListener(OnWeighButtonClicked);
            weighButton.gameObject.SetActive(true);
        }

        // Submit button setup
        if (submitButton != null)
        {
            submitButton.onClick.RemoveAllListeners();
            submitButton.onClick.AddListener(OnSubmitButtonClicked);
            submitButton.gameObject.SetActive(false);  // hide initially
        }
    }

    void Start()
    {
    SetupPuzzle();
    }

    /// <summary>
    /// Call when entering puzzle
    /// </summary>
    public void SetupPuzzle()
    {
        Debug.Log("[콘솔] 퍼즐 초기화");

        // Reset state
        usedWeighs = 0;
        if (weighButton != null) { weighButton.interactable = true; }
        if (submitButton != null) { submitButton.gameObject.SetActive(false); }
        UpdateWeighUI();
        selectedBall = null;

        // Random heavy ball
        heavyIndex = Random.Range(0, balls.Length);
        for (int i = 0; i < balls.Length; i++)
        {
            if (balls[i] == null) continue;
            balls[i].id = i;
            if (i == heavyIndex) balls[i].SetAsHeavy(heavyMass);
            else balls[i].SetAsNormal(normalMass);

            Debug.Log($"[콘솔] Ball {i}: isHeavy={balls[i].isHeavy}, mass={balls[i].GetMass()}");
        }
    }

    void UpdateWeighUI()
    {
        int left = maxWeighs - usedWeighs;
        if (weighCountText != null)
            weighCountText.text = $"Weighs left: {left}";
        Debug.Log($"[콘솔] 남은 저울질: {left}회");
    }

    void OnWeighButtonClicked()
    {
        if (usedWeighs >= maxWeighs) return;

        beamRot?.Weigh();
        usedWeighs++;
        UpdateWeighUI();

        if (usedWeighs >= maxWeighs && submitButton != null)
        {
            if (weighButton != null) weighButton.interactable = false;
            submitButton.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Called by BallWeight on click
    /// </summary>
    public void SelectBall(BallWeight ball)
    {
        if (usedWeighs < maxWeighs)
        {
            Debug.Log("[콘솔] 먼저 to Weigh!");
            return;
        }
        selectedBall = ball;
        Debug.Log($"[콘솔] Ball {ball.id} selected");
    }

<<<<<<< HEAD
=======
    // 1) 이전에 선택된 모든 공의 색을 흰색(원래 색)으로 되돌리기
    foreach (var b in balls)
    {
        if (b == null) continue;
        var rend = b.GetComponent<Renderer>();
        if (rend != null)
            rend.material.color = Color.white;
    }

    // 2) 새로 선택된 공 색을 노란색으로 변경
    selectedBall = ball;
    var selectedRend = ball.GetComponent<Renderer>();
    if (selectedRend != null)
        selectedRend.material.color = Color.yellow;

    Debug.Log($"[콘솔] Ball {ball.id} selected");
}


>>>>>>> 3309f0ad2c76773bc534e04781884e5b5179397c
    void OnSubmitButtonClicked()
    {
        if (selectedBall == null)
        {
            Debug.Log("[콘솔] Please select a ball to submit.");
            return;
        }

        if (selectedBall.id == heavyIndex)
        {
<<<<<<< HEAD
            GameManager.Instance.SetBoolean(10, true);
=======
>>>>>>> 3309f0ad2c76773bc534e04781884e5b5179397c
            Debug.Log("[콘솔] 정답입니다! 🎉");
            DeferredDialogue.Request(
csvName: "scale",
flagName: "animal"
);
            GameManager.Instance.ReturnToOriginalScene();
        }
        else
        {
            Debug.Log("[콘솔] 틀렸습니다. 퍼즐을 다시 초기화합니다.");
            SetupPuzzle();
        }
    }

    IEnumerator HandleCorrect(BallWeight ball)
    {
        if (ball == null) yield break;

        // Move to viewport center at fixed distance
        var cam = Camera.main;
        if (cam != null)
        {
            float distance = Vector3.Distance(cam.transform.position, ball.transform.position);
            Vector3 center = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, distance));
            ball.transform.position = center;
        }

        // Highlight effect
        ball.Highlight();

        // Enlarge
        ball.transform.localScale *= 1.5f;

        // Show answer text
        ball.ShowAnswerText("329", Color.red);

        yield return new WaitForSeconds(1f);

        // Inventory hook
        try { AddInventory(ball); }
        catch { Debug.LogWarning("AddInventory not implemented."); }

        // Destroy activator object
        var activator = Object.FindFirstObjectByType<ScaleActivator>();
        if (activator != null)
            activator.ExitPuzzle();        // 카메라 & UI 상태 복귀
        Destroy(activator.gameObject);
    }

    /// <summary>
    /// Override to add item to inventory
    /// </summary>
    protected void AddInventory(BallWeight ball) { }
}
