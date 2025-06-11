using TMPro;
using UnityEngine;
using UnityEngine.Events; // 추가
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingToggle : MonoBehaviour, IPointerDownHandler
{
    public bool isOn;
    public bool interactable;
    private float destpos;
    public string valueName;
    public TextMeshProUGUI Vtext;
    public GameObject toggle;
    public Image backimage;
    private float initpos;
    private Image image;
    public int transSpeed;
    public Sprite blackback;
    public Sprite blacktoggle;
    public Sprite greenback;
    public Sprite greentoggle;

    // 이벤트 추가
    public UnityEvent<bool> onValueChanged;

    void Awake()
    {
        image = toggle.GetComponent<Image>();
        ApplyToggleVisual(isOn);
        destpos = initpos;
        Vtext.text = valueName;
    }

    void Update()
    {
        RectTransform rt = toggle.GetComponent<RectTransform>();
        Vector2 currentPos = rt.anchoredPosition;
        float newX = Mathf.Lerp(currentPos.x, destpos, Time.deltaTime * transSpeed);
        rt.anchoredPosition = new Vector2(newX, currentPos.y);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (interactable) { SetIsOn(!isOn); }  // 여기서 SetIsOn 사용
    }

    // 외부에서 값 설정할 수 있게 public 메서드 제공
    public void SetIsOn(bool value)
    {
        if (isOn == value) return;  // 변화 없으면 무시
        isOn = value;
        ApplyToggleVisual(isOn);

        // 이벤트 호출
        if (onValueChanged != null)
        {
            onValueChanged.Invoke(isOn);
        }
    }

    // 비주얼 갱신 처리
    private void ApplyToggleVisual(bool value)
    {
        if (value)
        {
            initpos = 15;
            destpos = 15;
            image.sprite = greentoggle;
            backimage.sprite = greenback;
        }
        else
        {
            initpos = -15;
            destpos = -15;
            image.sprite = blacktoggle;
            backimage.sprite = blackback;
        }
    }
    public void SetInteractable(bool _b)
    {
        interactable = !_b;
        if (interactable)
        {
            image.color = new(image.color.r, image.color.g, image.color.b, 1f);
            backimage.color = new(image.color.r, image.color.g, image.color.b, 1f);
            ApplyToggleVisual(isOn);
        }
        else
        {
            image.color = new(image.color.r, image.color.g, image.color.b, 0.5f);
            backimage.color = new(image.color.r, image.color.g, image.color.b, 0.5f);
            image.sprite = blacktoggle;
            backimage.sprite = blackback;
        }
    }
}