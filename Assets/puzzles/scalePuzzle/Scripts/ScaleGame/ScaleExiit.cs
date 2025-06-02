using UnityEngine;

public class ScaleExit : MonoBehaviour
{
    [Header("퍼즐 제어용 Activator")]
    [Tooltip("퍼즐을 켜고 끄는 ScaleActivator")]
    public ScaleActivator activator;

    /// <summary>
    /// X 버튼에서 호출합니다.
    /// </summary>
    public void OnExitButton()
    {
        if (activator != null)
        {
            activator.ExitPuzzle();
            Debug.Log("[콘솔] 퍼즐 종료: ExitPuzzle() 호출");
        }
        else
        {
            Debug.LogWarning("ScaleExit ▶ activator가 할당되지 않았습니다!");
        }
    }
}