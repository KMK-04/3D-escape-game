using UnityEngine;
using UnityEngine.UI; // Button Ŭ���� ���
public class ChildActivator : MonoBehaviour
{
    public GameObject[] children; // �ڽ� ������Ʈ�� (�ν����Ϳ��� ����)
    public int activatedCount = 0; // ���� Ȱ��ȭ�� ���� (�ܺο��� Ȯ�� ����)

    // ��ư���� �� �Լ��� ȣ���ϵ��� ����
    public void OnButtonClick()
    {
        if (activatedCount < children.Length)
        {
            // ���� �ڽ� Ȱ��ȭ
            children[activatedCount].SetActive(true);
            activatedCount++;
        }
        else
        {
            // ��� ��Ȱ��ȭ
            foreach (GameObject child in children)
            {
                if (child != null)
                    child.SetActive(false);
            }

            activatedCount = 0;
        }
    }

    void Start()
    {
        // ������ �� ��� ���α�
        foreach (GameObject child in children)
        {
            if (child != null)
                child.SetActive(false);
        }
    }
}
