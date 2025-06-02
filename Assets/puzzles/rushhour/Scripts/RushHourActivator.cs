using UnityEngine;

public class RushHourActivator : MonoBehaviour
{
    public GameObject rushHourRoom;   // RealRushHourRoom (퍼즐 전체 공간)
    public Camera rushHourCamera;     // RushHourCamera (퍼즐 전용)
    public Camera mainCamera;         // Main Camera (메인 방 카메라)
    public GameObject rushHourUI;     // RushHourCanvas (X 버튼 등 UI)

    private void OnMouseDown()
    {
        if (rushHourRoom != null) rushHourRoom.SetActive(true);
        if (rushHourCamera != null) rushHourCamera.gameObject.SetActive(true);
        if (mainCamera != null) mainCamera.gameObject.SetActive(false);
        if (rushHourUI != null) rushHourUI.SetActive(true);
    }
}
