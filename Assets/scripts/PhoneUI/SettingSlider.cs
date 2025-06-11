using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingSlider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public float normalHeight = 1f;
    public float hoverHeight = 1.2f;
    public float clickedHeight = 1.5f;
    public float transitionSpeed = 5f;

    private Slider slider;
    private TextMeshProUGUI VText;
    public TextMeshProUGUI VName;
    public GameObject holder;
    public GameObject valueText;
    private float targetHeight;
    private Vector3 initialScale;
    private Vector3 initialholderScale;
    public string ValueName;

    void Start()
    {
        initialScale = transform.localScale;
        targetHeight = normalHeight;
        initialholderScale = holder.transform.localScale;
        slider = GetComponent<Slider>();
        VText = valueText.GetComponent<TextMeshProUGUI>();
        VName.text = ValueName;
    }

    void Update()
    {
        Vector3 targetScale = new(initialScale.x, initialScale.y * targetHeight, initialScale.z);
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * transitionSpeed);
        Vector3 targetholderScale = new(initialholderScale.x * (1 + ((targetHeight - 1) / 16)), initialholderScale.y * (1 + ((targetHeight - 1) / 16)), initialholderScale.z);
        holder.transform.localScale = Vector3.Lerp(holder.transform.localScale, targetholderScale, Time.deltaTime * transitionSpeed);
        if ((Input.GetMouseButtonUp(0)) && (!RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), Input.mousePosition)))
        {
            targetHeight = normalHeight;
        }
        VText.text = "" + Mathf.RoundToInt(slider.value);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetHeight = hoverHeight;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!Input.GetMouseButton(0))
        {
            targetHeight = normalHeight;
            valueText.SetActive(false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        targetHeight = clickedHeight;
        valueText.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        targetHeight = hoverHeight;
        slider.value = Mathf.RoundToInt(slider.value);
        valueText.SetActive(false);
    }
    public void SetInteractable(bool _b)
    {
        slider.interactable = !_b;
    }
}