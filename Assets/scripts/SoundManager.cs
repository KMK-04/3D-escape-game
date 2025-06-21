using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource bgmSource;
    public AudioClip[] bgmClips; // Opening, Main, Puzzle, Ending 순

    // 각 그룹에 속하는 씬 이름 목록
    public List<string>[] sceneGroups;

    private int[] defaultVolume = { 5, 5, 5 }; // Main, BGM, Effect
    private bool[] defaultMute = { false, false, false };

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 씬 그룹 초기화 (Opening, Main, Puzzle, Ending)
        sceneGroups = new List<string>[4];
        // Opening 그룹
        sceneGroups[0] = new List<string> {
            "Opening", "Intro"
        };

        // Main 그룹
        sceneGroups[1] = new List<string> {
    "Scene_01"
        };

                // Puzzle 그룹
        sceneGroups[2] = new List<string> {
    "rushhourScene",
    "light",
    "성냥개비",
    "animal lock",
    "TileScene",
    "Pong",
    "scale",
    "flappy",
    "floor lock"
        };

            // Ending 그룹
            sceneGroups[3] = new List<string> {
        "ending1", "ending2", "gameoverEnding"
        };

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name);
        SetBGMForScene(scene.name);
        SyncWithGameManager();
    }

    void SetBGMForScene(string sceneName)
    {
        // sceneGroups의 인덱스와 bgmClips의 인덱스를 연결
        for (int i = 0; i < sceneGroups.Length; i++)
        {
            if (sceneGroups[i].Contains(sceneName))
            {
                if (bgmClips.Length > i && bgmClips[i] != null)
                {
                    bgmSource.clip = bgmClips[i];
                    bgmSource.Play();
                    Debug.Log($"BGM 재생: {bgmClips[i].name}");
                }
                else
                {
                    Debug.LogWarning($"BGM Clip이 설정되지 않았거나 인덱스 초과: {i}");
                }
                return;
            }
        }

        Debug.LogWarning($"BGM 설정 실패: 해당 씬({sceneName})이 어떤 그룹에도 속하지 않음");
    }

    void SyncWithGameManager()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            ApplyVolume(gm.volume);
            ApplyMute(gm.mute);
            Debug.Log("GameManager로부터 사운드 설정 반영");
        }
        else
        {
            ApplyVolume(defaultVolume);
            ApplyMute(defaultMute);
            Debug.Log("GameManager가 없어 기본값으로 사운드 설정");
        }
    }

    void ApplyVolume(int[] volume)
    {
        bgmSource.volume = volume[1] / 10f;
    }

    void ApplyMute(bool[] mute)
    {
        bgmSource.mute = mute[1];
    }

    public void UpdateGameManagerVolume()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.volume[1] = Mathf.RoundToInt(bgmSource.volume * 10f);
            gm.mute[1] = bgmSource.mute;
            Debug.Log("GameManager에 사운드 설정 저장");
        }
    }
}
