using UnityEngine;

public class NoSimpleScene : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (EndingDialogueManager.Instance != null)
                EndingDialogueManager.Instance.OnNextButtonClicked();
            else
                Debug.LogWarning("EndingDialogueManager.Instance is null!");
        }
    }
}