using UnityEngine;
using UnityEngine.UI;

public class ChatAppUIManager : MonoBehaviour
{
    public GameObject friendsPanel;
    public GameObject chatPanel;

    public GameObject buttonPanel;
    public Button friendsButton;
    public Button chatButton;
    public GameObject chatapp;

    void Start()
    {
        ShowFriends(); // 앱 실행 시 친구 목록 먼저 표시
    }

    public void ShowFriends()
    {
        friendsPanel.SetActive(true);
        chatPanel.SetActive(false);
        buttonPanel.SetActive(true);
        friendsButton.interactable = false;
        chatButton.interactable = true;
        friendsPanel.transform.position = chatapp.transform.position;
    }

    public void ShowChats()
    {
        friendsPanel.SetActive(false);
        chatPanel.SetActive(true);
        buttonPanel.SetActive(true);
        friendsButton.interactable = true;
        chatButton.interactable = false;
        chatPanel.transform.position = chatapp.transform.position;
    }
    public void ShowChatting()
    {
        friendsPanel.SetActive(false);
        chatPanel.SetActive(false);
        buttonPanel.SetActive(false);
        friendsButton.interactable = false;
        chatButton.interactable = false;
        chatPanel.transform.position = chatapp.transform.position;
    }
    public void ScrollSet()
    {
        chatPanel.GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 1f;
        friendsPanel.GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 1f;
    }
}