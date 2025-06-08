using UnityEngine;

public class SimpleScene : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Dialogue_Manage.Instance.OnNextButtonClicked();
        }
    }
}
