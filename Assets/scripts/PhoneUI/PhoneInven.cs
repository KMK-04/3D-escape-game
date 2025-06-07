using System;
using SojaExiles;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class PhoneInven : MonoBehaviour
{
    private float chatScrollDesty, chatBarDesty, Desty;

    public float _chatOpeny, _chatClosey, _barOpeny, _barClosey, Openy, Closey;
    public RectTransform chatScroll, chatBar;
    public bool isOpen = false;
    public void ToggleInven()
    {
        isOpen = !isOpen;
    }
    public void CloseInven()
    {
        isOpen = false;
    }
    private void Update()
    {
        if (isOpen)
        {
            chatScrollDesty = _chatOpeny;
            chatBarDesty = _barOpeny;
            Desty = Openy;
        }
        else
        {
            chatScrollDesty = _chatClosey;
            chatBarDesty = _barClosey;
            Desty = Closey;
        }


        RectTransform rect = GetComponent<RectTransform>();
        float _y = rect.anchoredPosition.y;
        if (Mathf.Abs(_y - Desty) > 1)
        {
            _y += (Desty - _y) / 10f;
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, _y);
        }
        float __y = chatScroll.anchoredPosition.y;
        if (Mathf.Abs(__y - chatScrollDesty) > 1)
        {
            __y += (chatScrollDesty - __y) / 10f;
            chatScroll.anchoredPosition = new Vector2(chatScroll.anchoredPosition.x, __y);
        }
        float ___y = chatBar.anchoredPosition.y;
        if (Mathf.Abs(___y - chatBarDesty) > 1)
        {
            ___y += (chatBarDesty - ___y) / 10f;
            chatBar.anchoredPosition = new Vector2(chatBar.anchoredPosition.x, ___y);
        }
    }
}

