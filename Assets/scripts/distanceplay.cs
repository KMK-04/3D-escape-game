using UnityEngine;
using System.Collections;

public class DistancePlay : MonoBehaviour
{
    public float activationDistance = 5f; // 음악 재생 기준 거리
    public AudioClip[] musicClips; // 재생할 음악 파일 리스트 (Inspector에서 설정)

    private Transform playerTransform;
    private AudioSource audioSource; // 오디오 재생을 위한 컴포넌트
    private bool isPlaying = false; // 음악 재생 상태 추적
    public float aaa = 0.1f;
    private bool isWaiting = false; // 대기 중인지 확인하는 플래그

    void Start()
    {
        // "Player" 태그를 가진 오브젝트 찾기
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            Debug.Log("Player 태그를 가진 오브젝트를 찾았습니다: " + player.name);
        }
        else
        {
            Debug.LogWarning("Player 태그를 가진 오브젝트를 찾을 수 없습니다.");
        }

        // AudioSource 컴포넌트 가져오기 또는 추가하기
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            Debug.Log("AudioSource 컴포넌트가 추가되었습니다.");
        }

        // AudioSource 기본 설정
        audioSource.loop = false; // 랜덤 재생을 위해 loop는 false로 설정
        audioSource.spatialBlend = 1.0f; // 3D 사운드로 설정
        audioSource.maxDistance = activationDistance; // 사운드 들리는 최대 거리 설정

        if (musicClips != null && musicClips.Length > 0)
        {
            Debug.Log("musicClips 리스트가 설정되었습니다. 총 " + musicClips.Length + "개의 곡이 있습니다.");
        }
        else
        {
            Debug.LogWarning("musicClips 리스트가 비어 있거나 할당되지 않았습니다.");
        }
    }

    void Update()
    {
        // playerTransform이 null이면 업데이트 중단
        if (playerTransform == null) return;

        // 본인 오브젝트와 플레이어 간의 거리 계산
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // 거리가 activationDistance 이하이면 음악 재생
        if (distance <= activationDistance)
        {
            if (!isPlaying && !isWaiting && musicClips != null && musicClips.Length > 0)
            {
                PlayRandomTrack();
                isPlaying = true;
                Debug.Log($"거리 {distance} <= {activationDistance}: 음악 재생 시작");
            }
            // 현재 곡이 끝났는지 확인하고 다음 랜덤 곡 재생 (대기 중이 아닐 때만)
            else if (isPlaying && !audioSource.isPlaying && !isWaiting)
            {
                StartCoroutine(WaitBeforeNextTrack(aaa)); // 5초 대기 후 다음 곡 재생
                Debug.Log("다음 랜덤 곡 재생 전 5초 대기 시작");
            }
        }
        else
        {
            if (isPlaying || isWaiting)
            {
                StopAllCoroutines(); // 대기 중인 코루틴 중지
                audioSource.Stop();
                isPlaying = false;
                isWaiting = false;
                Debug.Log($"거리 {distance} > {activationDistance}: 음악 재생 및 대기 중지");
            }
        }
    }

    // 랜덤으로 음악 트랙을 선택해 재생하는 메서드
    private void PlayRandomTrack()
    {
        if (musicClips.Length > 0)
        {
            int randomIndex = Random.Range(0, musicClips.Length);
            audioSource.clip = musicClips[randomIndex];
            audioSource.Play();
            Debug.Log($"랜덤 곡 재생: {audioSource.clip.name}");
        }
    }

    // 지정된 시간(초) 동안 대기 후 다음 곡을 재생하는 코루틴
    private IEnumerator WaitBeforeNextTrack(float waitTime)
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
        if (playerTransform != null && Vector3.Distance(transform.position, playerTransform.position) <= activationDistance)
        {
            PlayRandomTrack();
            Debug.Log("5초 대기 후 다음 랜덤 곡 재생");
        }
        else
        {
            isPlaying = false;
            Debug.Log("5초 대기 후 거리 밖에 있어 재생하지 않음");
        }
    }
}
