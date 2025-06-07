using System;
using SojaExiles;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Phone : MonoBehaviour {
    public RectTransform phone;
    public PlayerMovement player;
    public float Desty;
    public ContextData data;
    public float _openy;
    public float _closey;
    public Button[] uiButtons;
    public bool isOpen = false;
    [HideInInspector]
    public string context;
    public void TogglePhone() {
        isOpen = true;
    }
    public void ClosePhone() {
        isOpen = false;
    }

    private void Start() {
        context = data.Context;
        //Debug.Log(context);
    }
    private void Update() {
        if (isOpen) {
            Desty = _openy;
            player.canMove = false;
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

