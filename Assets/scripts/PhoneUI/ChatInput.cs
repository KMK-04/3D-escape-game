using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChatInput : MonoBehaviour
{
    public TMP_InputField messageInput;
    public Button sendButton;
    public ChatManager chatManager;
    public AIChatManager aiChatManager;

    void Start()
    {
        sendButton.onClick.AddListener(OnSend);
    }

    void OnSend()
    {
        string text = messageInput.text;
        if (string.IsNullOrWhiteSpace(text)) return;

        chatManager.Chat(true, text, "나", null);
        aiChatManager.OnSendButtonClicked(text);
        messageInput.text = "";
        messageInput.ActivateInputField();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnSend();
        }
    }
}
