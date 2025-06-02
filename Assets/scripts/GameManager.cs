using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public Vector3 playerPosition; // 플레이어 위치 저장
    public Vector3 cameraRotation; // 카메라 회전값 저장 (Euler Angles)
    public string originalSceneName; // 원래 씬 이름 저장
    public Transform playerTransform; // 플레이어 오브젝트의 Transform (Inspector에서 할당 또는 자동 설정)
    public List<bool> booleanList; // 정보를 저장하기 위한 boolean 리스트

    void Awake()
    {
        // 싱글톤 패턴 구현: 이미 인스턴스가 있으면 현재 오브젝트를 파괴
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않음
        }
        else
        {
            Destroy(gameObject);
        }

        // booleanList 초기화
        if (booleanList == null)
        {
            booleanList = new List<bool>();
            Debug.Log("Boolean 리스트 초기화됨");
        }

        // playerTransform이 할당되지 않았다면 "MainCamera" 태그를 가진 오브젝트를 자동으로 찾음
        if (playerTransform == null)
        {
            GameObject cameraObject = GameObject.FindWithTag("MainCamera");
            if (cameraObject != null)
            {
                playerTransform = cameraObject.transform;
                Debug.Log("MainCamera 태그를 가진 오브젝트를 playerTransform에 자동 할당: " + cameraObject.name);
            }
            else
            {
                Debug.LogWarning("MainCamera 태그를 가진 오브젝트를 찾을 수 없습니다. playerTransform이 할당되지 않음.");
            }
        }
    }

    // 플레이어 위치 및 카메라 회전값 저장
    public void SavePlayerPosition(string sceneName)
    {
        GameObject cameraObject = GameObject.FindWithTag("MainCamera");
        if (cameraObject != null)
        {
            playerTransform = cameraObject.transform;
            playerPosition = playerTransform.position; // 플레이어 오브젝트의 현재 위치 저장
            cameraRotation = playerTransform.eulerAngles; // 카메라의 회전값 저장
            originalSceneName = sceneName;
            Debug.Log($"플레이어 위치 저장: {playerPosition}, 카메라 회전값 저장: {cameraRotation}, 원래 씬: {sceneName}");
        }
        else
        {
            Debug.LogWarning("MainCamera 태그를 가진 오브젝트를 찾을 수 없습니다. 위치 및 회전값을 저장할 수 없습니다.");
        }
    }

    // 플레이어 위치 가져오기
    public Vector3 GetPlayerPosition()
    {
        return playerPosition;
    }

    // 카메라 회전값 가져오기
    public Vector3 GetCameraRotation()
    {
        return cameraRotation;
    }

    // 원래 씬 이름 가져오기
    public string GetOriginalSceneName()
    {
        return originalSceneName;
    }

    // Boolean 리스트에 값 추가
    public void AddBoolean(bool value)
    {
        booleanList.Add(value);
        Debug.Log($"Boolean 값 추가: {value}, 리스트 크기: {booleanList.Count}");
    }

    // Boolean 리스트에서 특정 인덱스의 값 가져오기
    public bool GetBoolean(int index)
    {
        if (index >= 0 && index < booleanList.Count)
        {
            return booleanList[index];
        }
        Debug.LogWarning($"잘못된 인덱스: {index}, 리스트 크기: {booleanList.Count}");
        return false; // 기본값 반환
    }

    // Boolean 리스트에서 특정 인덱스의 값 설정
    public void SetBoolean(int index, bool value)
    {
        if (index >= 0 && index < booleanList.Count)
        {
            booleanList[index] = value;
            Debug.Log($"Boolean 값 설정: 인덱스 {index} = {value}");
        }
        else
        {
            Debug.LogWarning($"잘못된 인덱스: {index}, 리스트 크기: {booleanList.Count}");
        }
    }

    // Boolean 리스트 초기화 (모든 값 제거)
    public void ClearBooleanList()
    {
        booleanList.Clear();
        Debug.Log("Boolean 리스트 초기화됨");
    }

    // Boolean 리스트 크기 반환
    public int GetBooleanListSize()
    {
        return booleanList.Count;
    }

    // 싱글톤 인스턴스에 접근하는 프로퍼티
    public static GameManager Instance
    {
        get { return instance; }
    }
}
