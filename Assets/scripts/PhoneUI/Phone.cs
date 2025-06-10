using System;
using SojaExiles;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Phone : MonoBehaviour {
    public RectTransform phone;
    public PlayerMovement player;
    public float Desty;
    public float _openy;
    public float _closey;
    public Button[] uiButtons;
    public bool isOpen = false;
    public Dialogue_Manage dialogue_Manage;
    [HideInInspector]
    public void TogglePhone() {

        isOpen = true;
        dialogue_Manage.CheckNext();
    }
    public void ClosePhone() {
        isOpen = false;
        dialogue_Manage.CheckNext();
    }
    private void Update() {
        if (isOpen) {
            Desty = _openy;

        }
        else {
            Desty = _closey;

        }
        float __y = phone.anchoredPosition.y;
        if (Mathf.Abs(__y - Desty) > 1) {
            __y += (Desty - __y) / 10f;
            phone.anchoredPosition = new Vector2(phone.anchoredPosition.x, __y);
        }
    }
}

