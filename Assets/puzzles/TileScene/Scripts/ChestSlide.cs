using UnityEngine;

public class ChestSlide : MonoBehaviour
{
    public Vector3 hiddenPos;    // 책상 속
    public Vector3 shownPos;     // 책상 앞
    public Vector3 hiddenScale = Vector3.one * 0.6f;
    public Vector3 shownScale  = Vector3.one;

    public float slideSpeed = 1.5f;

    bool moving;
    float t;

    public void SlideOut()
    {
        moving = true;
        t = 0f;
    }

    void Update()
    {
        if (!moving) return;

        t += Time.deltaTime * slideSpeed;
        transform.position = Vector3.Lerp(hiddenPos, shownPos, t);
        transform.localScale = Vector3.Lerp(hiddenScale, shownScale, t);

        if (t >= 1f) moving = false;
    }
}
