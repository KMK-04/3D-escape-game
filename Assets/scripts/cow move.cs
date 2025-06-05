using UnityEngine;

public class cowmove : MonoBehaviour
{
    public Transform target; // ������ ��� ������Ʈ

    private bool toggleState = false; // ������ �����̱� ���� ���� ����
    private GameManager gameManager;  // �ڵ����� ������ GameManager

    void Start()
    {
        // GameManager �̱��� �ν��Ͻ��� �ڵ����� �޾ƿ�
        gameManager = GameManager.Instance;

        if (gameManager == null)
        {
            Debug.LogError("GameManager �ν��Ͻ��� ã�� �� �����ϴ�.");
        }
    }

    public void OnButtonClick()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameManager�� null�Դϴ�. OnButtonClick �ߴ�.");
            return;
        }

        if (gameManager.GetBooleanListSize() < 6)
        {
            Debug.LogWarning("booleanList�� �ʹ� ª���ϴ�.");
            return;
        }

        bool cond4 = gameManager.GetBoolean(4);
        bool cond5 = gameManager.GetBoolean(5);

        if (cond4 && cond5)
        {
            // �� �� true�� �����ư��� �����̱�
            if (toggleState)
            {
                MoveToPosition(new Vector3(3.453995f, 0.16998f, 52.62308f), Quaternion.identity);
            }
            else
            {
                MoveToPosition(new Vector3(1.623995f, 0.16998f, 55.50617f), Quaternion.Euler(0, 0, 0));
            }
            toggleState = !toggleState;
        }
        else if (cond4)
        {
            MoveToPosition(new Vector3(3.453995f, 0.16998f, 52.62308f), Quaternion.identity);
        }
        else if (cond5)
        {
            MoveToPosition(new Vector3(1.623995f, 0.16998f, 55.50617f), Quaternion.Euler(0, 0, 0));
        }
        else
        {
            Debug.Log("���ǿ� �ش����� ����");
        }
    }

    private void MoveToPosition(Vector3 position, Quaternion rotation)
    {
        target.position = position;
        target.rotation = rotation;
    }
}
