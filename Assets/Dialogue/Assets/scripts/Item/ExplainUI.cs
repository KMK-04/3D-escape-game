using UnityEngine;
using UnityEngine.UI;

public class ExplainUI : MonoBehaviour
{
    public Text explainText;

    public void ShowExplain(string message)
    {
        explainText.text = message;
    }

    public void ClearExplain()
    {
        explainText.text = "아이템 설명";
    }
}
