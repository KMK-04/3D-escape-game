using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class BallWeight : MonoBehaviour
{
    [HideInInspector] public int    id;
    [HideInInspector] public bool   isHeavy;
    Rigidbody _rb;

    [Header("Label (TMP)")]
    public TMP_Text label;                  // 구슬 위 a,b,c 레이블

    [Header("Outline Highlight")]
    public Behaviour outline;               // Assign an Outline component or similar

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        _rb.useGravity  = true;

        if (outline != null)
            outline.enabled = false; // Disable outline initially
    }

    public void SetAsHeavy(float mass)  { isHeavy = true;  _rb.mass = mass; }
    public void SetAsNormal(float mass) { isHeavy = false; _rb.mass = mass; }
    public float GetMass()              { return _rb.mass; }

    void OnMouseDown()
    {
        var mgr = Object.FindFirstObjectByType<WeighManager>();
        if (mgr != null)
            mgr.SelectBall(this);
    }

    /// <summary>
    /// Display answer text above ball
    /// </summary>
    public void ShowAnswerText(string txt, Color color)
    {
        if (label != null)
        {
            label.text  = txt;
            label.color = color;
        }
    }

    /// <summary>
    /// Activate outline highlight
    /// </summary>
    public void Highlight()
    {
        if (outline != null)
            outline.enabled = true;
    }
}
