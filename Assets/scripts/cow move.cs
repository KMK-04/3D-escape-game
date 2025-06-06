using UnityEngine;
using System.Collections;

public class cowmove : MonoBehaviour
{
    public Transform target; // ������ ��� ������Ʈ

    private bool toggleState = false; // ������ �����̱� ���� ���� ����
    private GameManager gameManager;  // �ڵ����� ������ GameManager

    void Start()
    {
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
            if (toggleState)
            {
                StartCoroutine(MoveXThenY(new Vector3(1.99f, 0.16998f, 52.62308f), new Vector3(1.99f, 0.16998f, 55.50617f)));
            }
            else
            {
                StartCoroutine(MoveXThenY(new Vector3(1.99f, 0.16998f, 55.50617f), new Vector3(1.99f, 0.16998f, 52.62308f)));
            }
            toggleState = !toggleState;
        }
        else if (cond4)
        {
            StartCoroutine(MoveXThenY(new Vector3(1.99f, 0.16998f, 52.62308f), new Vector3(1.99f, 0.16998f, 55.50617f)));
        }
        else if (cond5)
        {
            StartCoroutine(MoveXThenY(new Vector3(1.99f, 0.16998f, 55.50617f), new Vector3(1.99f, 0.16998f, 52.62308f)));
        }
        else
        {
            Debug.Log("���ǿ� �ش����� ����");
        }
    }

    private IEnumerator MoveXThenY(Vector3 posX, Vector3 posY)
    {
        // x�� �̵�
        while (Mathf.Abs(target.position.x - posX.x) > 0.01f)
        {
            target.position = Vector3.MoveTowards(target.position, new Vector3(posX.x, target.position.y, target.position.z), Time.deltaTime * 2f);
            yield return null;
        }

        // z�� �̵�
        while (Mathf.Abs(target.position.z - posY.z) > 0.01f)
        {
            target.position = Vector3.MoveTowards(target.position, new Vector3(target.position.x, target.position.y, posY.z), Time.deltaTime * 2f);
            yield return null;
        }
    }
}
