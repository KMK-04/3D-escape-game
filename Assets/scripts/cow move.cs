using UnityEngine;
using System.Collections;

public class cowmove : MonoBehaviour
{
    public Transform target; // 움직일 대상 오브젝트

    private bool toggleState = false; // 번갈아 움직이기 위한 상태 저장
    private GameManager gameManager;  // 자동으로 참조될 GameManager

    void Start()
    {
        gameManager = GameManager.Instance;

        if (gameManager == null)
        {
            Debug.LogError("GameManager 인스턴스를 찾을 수 없습니다.");
        }
    }

    public void OnButtonClick()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameManager가 null입니다. OnButtonClick 중단.");
            return;
        }

        if (gameManager.GetBooleanListSize() < 6)
        {
            Debug.LogWarning("booleanList가 너무 짧습니다.");
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
            Debug.Log("조건에 해당하지 않음");
        }
    }

    private IEnumerator MoveXThenY(Vector3 posX, Vector3 posY)
    {
        // x축 이동
        while (Mathf.Abs(target.position.x - posX.x) > 0.01f)
        {
            target.position = Vector3.MoveTowards(target.position, new Vector3(posX.x, target.position.y, target.position.z), Time.deltaTime * 2f);
            yield return null;
        }

        // z축 이동
        while (Mathf.Abs(target.position.z - posY.z) > 0.01f)
        {
            target.position = Vector3.MoveTowards(target.position, new Vector3(target.position.x, target.position.y, posY.z), Time.deltaTime * 2f);
            yield return null;
        }
    }
}
