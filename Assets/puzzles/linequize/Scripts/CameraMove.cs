using UnityEngine;

public class CameraMove : MonoBehaviour
{
    
    public Transform RoomTop;
    public Transform RoomLeft;
    public Transform RoomRight;
    public Transform RoomFront;
    public Transform RoomBack;
    public GameObject LeftButton;
    public GameObject RightButton;
    public GameObject TopButton;
    public GameObject FrontButton;
    private int CurrentRoom;

    private void Update() {
        switch (CurrentRoom) {
            case 0 : GoToRoom(RoomFront);
            break;
            case 1 : GoToRoom(RoomRight);
            break;
            case 2 : GoToRoom(RoomBack);
            break;
            case 3 : GoToRoom(RoomLeft);
            break;
            case 100 : GoToRoom(RoomTop);
            break;
            }

            FrontButton.SetActive(CurrentRoom==100);
            TopButton.SetActive(CurrentRoom!=100);
            LeftButton.SetActive(CurrentRoom!=100);
            RightButton.SetActive(CurrentRoom!=100);
            if (Input.GetKeyDown(KeyCode.Space)) {
                Debug.Log(CurrentRoom);
            }
    }
    public void GoToRoom(Transform room) {
        Vector3 pos = new Vector3(room.position.x,room.position.y,-10);
        transform.position = pos;
    }
    public void PlusCurrentRoom() {
        CurrentRoom ++;
        if (CurrentRoom > 3) {
            CurrentRoom = 0;
        }
    }
    public void MinusCurrentRoom() {
        CurrentRoom --;
        if (CurrentRoom < 0) {
            CurrentRoom = 3;
        }
    }
    public void ViewTopRoom() {
        CurrentRoom = 100;
        
    }
    public void ViewFrontRoom() {
        CurrentRoom = 0;
    }

}
