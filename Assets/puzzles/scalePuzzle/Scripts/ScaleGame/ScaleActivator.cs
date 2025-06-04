using UnityEngine;
using UnityEngine.SceneManagement;

public class ScaleActivator : MonoBehaviour
{
    public GameObject rushHourRoom;   // RealRushHourRoom (퍼즐 전체 공간)
    public Camera rushHourCamera;     // RushHourCamera (퍼즐 전용)
    public Camera mainCamera;         // Main Camera (메인 방 카메라)
    public GameObject rushHourUI;     // RushHourCanvas (X 버튼 등 UI)
    public WeighManager weighManager;   // 추가

    private void OnMouseDown()
    {
        if (rushHourRoom != null) rushHourRoom.SetActive(true);
        if (rushHourCamera != null) rushHourCamera.gameObject.SetActive(true);
        if (mainCamera != null) mainCamera.gameObject.SetActive(false);
        if (rushHourUI != null) rushHourUI.SetActive(true);
        Debug.Log("[ScaleActivator] OnMouseDown()");
        weighManager.SetupPuzzle();     // 퍼즐 초기화
    }

    public void ExitPuzzle()
    {
        rushHourRoom.SetActive(false);
        rushHourCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
        rushHourUI.SetActive(false);
    }
    
       public static void ClosePuzzleIfOpen()
    {
        // 활성 상태인 ScaleActivator 인스턴스 찾기
        var activator = Object.FindFirstObjectByType<ScaleActivator>();
        if (activator != null && activator.rushHourRoom != null && activator.rushHourRoom.activeSelf)
        {
            activator.ExitPuzzle();
        }
    }
}
