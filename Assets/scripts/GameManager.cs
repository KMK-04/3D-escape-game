using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using SojaExiles;
using Unity.VisualScripting;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public Vector3 playerPosition; // �÷��̾� ��ġ ����
    public Vector3 cameraRotation; // ī�޶� ȸ���� ���� (Euler Angles)
    public string originalSceneName; // ���� �� �̸� ����
    public Transform playerTransform; // �÷��̾� ������Ʈ�� Transform (Inspector���� �Ҵ� �Ǵ� �ڵ� ����)
    public List<bool> booleanList; // ������ �����ϱ� ���� boolean ����Ʈ
    public GameObject phone;
    public int InGameTime = 0;
    public PlayerMovement playerMovement;


    void Awake()
    {
        // �̱��� ���� ����: �̹� �ν��Ͻ��� ������ ���� ������Ʈ�� �ı�
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� �ı����� ����
        }
        else
        {
            Destroy(gameObject);
        }

        // booleanList �ʱ�ȭ
        if (booleanList == null)
        {
            booleanList = new List<bool>();
            Debug.Log("Boolean ����Ʈ �ʱ�ȭ��");
        }

        // playerTransform�� �Ҵ���� �ʾҴٸ� "MainCamera" �±׸� ���� ������Ʈ�� �ڵ����� ã��
        if (playerTransform == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                playerTransform = playerObject.transform;
                playerMovement = playerObject.GetComponent<PlayerMovement>();
                Debug.Log("MainCamera �±׸� ���� ������Ʈ�� playerTransform�� �ڵ� �Ҵ�: " + playerObject.name);
            }
            else
            {
                Debug.LogWarning("MainCamera �±׸� ���� ������Ʈ�� ã�� �� �����ϴ�. playerTransform�� �Ҵ���� ����.");
            }
        }
        StartCoroutine(TimeCoroutine());
    }


    // �÷��̾� ��ġ �� ī�޶� ȸ���� ����
    // 위치 저장
    public void SavePlayerPosition(string sceneName)
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
            playerPosition = playerTransform.position;
            cameraRotation = playerTransform.eulerAngles; // 필요 시 조정
            originalSceneName = sceneName;
            Debug.Log($"플레이어 위치 저장: {playerPosition}, 회전: {cameraRotation}");
        }
        else
        {
            Debug.LogWarning("Player 태그를 가진 오브젝트를 찾을 수 없습니다.");
        }
    }


    // �÷��̾� ��ġ ��������
    public Vector3 GetPlayerPosition()
    {
        return playerPosition;
    }

    // ī�޶� ȸ���� ��������
    public Vector3 GetCameraRotation()
    {
        return cameraRotation;
    }

    // ���� �� �̸� ��������
    public string GetOriginalSceneName()
    {
        return originalSceneName;
    }

    // Boolean ����Ʈ�� �� �߰�
    public void AddBoolean(bool value)
    {
        booleanList.Add(value);
        Debug.Log($"Boolean �� �߰�: {value}, ����Ʈ ũ��: {booleanList.Count}");
    }

    // Boolean ����Ʈ���� Ư�� �ε����� �� ��������
    public bool GetBoolean(int index)
    {
        if (index >= 0 && index < booleanList.Count)
        {
            return booleanList[index];
        }
        Debug.LogWarning($"�߸��� �ε���: {index}, ����Ʈ ũ��: {booleanList.Count}");
        return false; // �⺻�� ��ȯ
    }

    // Boolean ����Ʈ���� Ư�� �ε����� �� ����
    public void SetBoolean(int index, bool value)
    {
        if (index >= 0 && index < booleanList.Count)
        {
            booleanList[index] = value;
            Debug.Log($"Boolean �� ����: �ε��� {index} = {value}");
        }
        else
        {
            Debug.LogWarning($"�߸��� �ε���: {index}, ����Ʈ ũ��: {booleanList.Count}");
        }
    }

    // Boolean ����Ʈ �ʱ�ȭ (��� �� ����)
    public void ClearBooleanList()
    {
        booleanList.Clear();
        Debug.Log("Boolean ����Ʈ �ʱ�ȭ��");
    }

    // Boolean ����Ʈ ũ�� ��ȯ
    public int GetBooleanListSize()
    {
        return booleanList.Count;
    }

    // �̱��� �ν��Ͻ��� �����ϴ� ������Ƽ
    public static GameManager Instance
    {
        get { return instance; }
    }

    public void ReturnToOriginalScene()
    {
        if (!string.IsNullOrEmpty(originalSceneName))
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(originalSceneName);
        }
        else
        {
            Debug.LogWarning("originalSceneName이 비어있습니다. DefaultScene으로 이동합니다.");
            SceneManager.LoadScene("DefaultScene");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Instance.StartCoroutine(RestorePlayerPosition());

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            playerMovement = playerObj.GetComponent<PlayerMovement>();
            Debug.Log("playerTransform & playerMovement 재설정 완료: " + playerObj.name);
        }
        else
        {
            Debug.LogWarning("Player 태그 오브젝트를 찾을 수 없습니다.");
        }
    }

    // 위치 복원
    private IEnumerator RestorePlayerPosition()
    {
        yield return new WaitForEndOfFrame();

        if (playerTransform == null)
        {
            Debug.LogWarning("playerTransform이 비어있어 위치 복원 불가.");
            yield break;
        }

        playerTransform.position = playerPosition;
        playerTransform.eulerAngles = cameraRotation;

        Debug.Log($"플레이어 위치 복원 완료: {playerPosition}, 회전: {cameraRotation}");

        if (!MouseLook.instance.isLockOn())
        {
            MouseLook.instance.ToggleLock();
        }
    }
    private IEnumerator TimeCoroutine()
    {
        while (true)
        {
            Debug.Log(InGameTime);
            InGameTime++;
            yield return new WaitForSeconds(10f);
        }
    }
}
