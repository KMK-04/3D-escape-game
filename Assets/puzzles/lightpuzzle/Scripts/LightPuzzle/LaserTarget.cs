using UnityEngine;

public class LaserTarget : MonoBehaviour
{
    private bool isHit = false;

    public void OnLaserHit()
    {
        if (isHit) return;

        isHit = true;
        Debug.Log("HIT!!");
        TargetManager.Instance.NotifyLaserHit();
    }

    public void ResetTarget()
    {
        isHit = false;
    }
}
