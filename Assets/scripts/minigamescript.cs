using System.Collections;
using UnityEngine;
using TMPro; // << TextMeshPro 네임스페이스 추가

public class EnhancedObjectActivator : MonoBehaviour
{
    [Header("1차 그룹 (4개)")]
    public GameObject[] group1 = new GameObject[4];

    [Header("2차 그룹 (4개)")]
    public GameObject[] group2 = new GameObject[4];
    public GameObject the;

    [Header("카운트다운 텍스트")]
    public TextMeshProUGUI countdownText; // TextMeshPro 3D 텍스트 사용

    void OnEnable()
    {
        if (countdownText != null)
        {
            if (countdownText.text == " ")
            {
                the.SetActive(false);
                countdownText.text = "";
            }
        }
        StartCoroutine(ActivationCycle());
    }

    IEnumerator ActivationCycle()
    {
        ResetAllObjects();
        StartCoroutine(CountdownCoroutine(13)); // 10부터 카운트다운 시작
        yield return new WaitForSeconds(4f);

        while (true)
        {
            ResetAllObjects();
            ActivateRandomGroup1();
            yield return new WaitForSeconds(2f);
        }
    }

    void ActivateRandomGroup1()
    {
        int count = Random.Range(1, 4);
        for (int i = 0; i < count; i++)
        {
            int index = GetUniqueRandomIndex(group1);
            group1[index].SetActive(true);
            StartCoroutine(TriggerGroup2(index));
        }
    }

    IEnumerator TriggerGroup2(int group1Index)
    {
        yield return new WaitForSeconds(0.5f);
        group2[group1Index].SetActive(true);
        group1[group1Index].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        group2[group1Index].SetActive(false);
    }

    int GetUniqueRandomIndex(GameObject[] group)
    {
        int index;
        do
        {
            index = Random.Range(0, group.Length);
        } while (group[index].activeSelf);
        return index;
    }

    void ResetAllObjects()
    {
        foreach (var obj in group1) obj.SetActive(false);
        foreach (var obj in group2) obj.SetActive(false);
    }

    IEnumerator CountdownCoroutine(int start)
    {
        for (int i = start; i >= 0; i--)
        {
            if (countdownText != null)
            {
                if (countdownText.text!= " "){
                    countdownText.text = i.ToString();
                }
            }
            yield return new WaitForSeconds(1f);
        }

        if (countdownText != null)
        {
            countdownText.text = ""; // 카운트다운 끝나면 텍스트 제거
        }
    }
}
