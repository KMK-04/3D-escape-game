using UnityEngine;

public class iftureshow : MonoBehaviour
{
    public int n; // Ȯ���� �ε���

    void Start()
    {
        if (GameManager.Instance != null)
        {
            if (n >= 0 && n < GameManager.Instance.GetBooleanListSize() && GameManager.Instance.GetBoolean(n))
            {
                Debug.Log($"booleanList[{n}]�� true�̹Ƿ� ������Ʈ ���� ����: {gameObject.name}");
                gameObject.SetActive(true);
            }
            else
            {
                Debug.Log($"booleanList[{n}]�� false�̰ų� ���� ���̹Ƿ� ������Ʈ �ı�: {gameObject.name}");
                gameObject.SetActive(false);
            }
        }
    }
}
