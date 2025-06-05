using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;

public class PopupScale : MonoBehaviour {
    public GameObject ApplicationIcon;
    private GameObject HomeScreen;
    private Transform startPosition;
    private Transform targetPosition;
    private Vector3 position;
    private Vector3 destPosition;
    public Vector3 scale;
    public Vector3 destScale;
    public float scaleSpeed;
    public bool isOpen = false;


    void Start() {
        HomeScreen = GameObject.Find("HomeScreen");
        scale = Vector3.zero;
        transform.localScale = scale;
        startPosition = ApplicationIcon.transform;
        targetPosition = HomeScreen.transform;
        position = startPosition.position;
        transform.position = position;
    }

    void Update() {
        destScale = isOpen ? Vector3.one : Vector3.zero;
        destPosition = isOpen ? targetPosition.position : startPosition.position;
        if (Vector3.Distance(scale, destScale) < 0.01f) {
            scale = destScale;
        }
        else {
            scale += (destScale - scale) / scaleSpeed;
        }
        if (Vector3.Distance(position, destPosition) < 0.01f) {
            position = destPosition;

        }
        else {
            position += (destPosition - position) / scaleSpeed;
        }
        transform.localScale = scale;
        transform.position = position;
    }

    public void TogglePopup() {
        isOpen = !isOpen;

    }
    public void CloseApp() {
        isOpen = false;
    }
}