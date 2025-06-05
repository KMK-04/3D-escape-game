using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ChatListManage : MonoBehaviour
{
    public GameObject ImageObject;
    public GameObject ChatArea;
    [HideInInspector]
    public GameObject ChatApp;
    public bool Notice = false;
    public TextMeshProUGUI lastText;

    void Start()
    {
        ChatApp = GameObject.Find("ChatApp");
        lastText = transform.Find("LastText").GetComponent<TextMeshProUGUI>();
        Button button = GetComponent<Button>();
        ChatManager chatManager = ChatArea.GetComponent<ChatManager>();
        PopupScale popupScale = ChatArea.GetComponent<PopupScale>();
        ChatAppUIManager chatApp = ChatApp.GetComponent<ChatAppUIManager>();
        button.onClick.AddListener(chatManager.LoadProfile);
        button.onClick.AddListener(NoticeChecked);
        button.onClick.AddListener(chatApp.ShowChatting);
        button.onClick.AddListener(popupScale.TogglePopup);
    }
    void Update()
    {
        if (ImageObject != null)
        {
            Image imageComponent = ImageObject.GetComponent<Image>();
            if (imageComponent != null)
            {
                Color c = imageComponent.color;
                c.a = Notice ? 1f : 0f;  // Notice가 true면 불투명, false면 반투명
                imageComponent.color = c;
            }
        }
    }

    public string LastChat()
    {
        // 경로: ChatArea > Scroll View > Viewport > Content
        Transform content = ChatArea.transform
            .Find("Scroll View")
            ?.Find("Viewport")
            ?.Find("Content");

        if (content == null || content.childCount == 0)
        {
            Debug.LogWarning("Content가 없거나 자식이 없습니다.");
            return "";
        }

        // 마지막 자식 찾기
        Transform lastChild = content.GetChild(content.childCount - 1);

        // 마지막 자식 > Box > Text 탐색
        Transform textTransform = lastChild.Find("Box/Text");
        if (textTransform != null)
        {
            Text textComp = textTransform.GetComponent<Text>();
            if (textComp != null)
            {
                Debug.Log("마지막 메시지 텍스트: " + textComp.text);
                if (lastText != null)
                {
                    lastText.text = textComp.text;
                }
                else
                {
                    Debug.Log("lastText가 없어");
                }
                return textComp.text;
            }
            else
            {
                Debug.LogWarning("Text 컴포넌트를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("Box/Text 경로가 잘못됐거나 오브젝트가 없습니다.");
        }
        return null;
    }

    public void NoticeChecked()
    {
        Notice = false;
    }

}
