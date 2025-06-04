using UnityEngine;
using System.Collections;

public class DistancePlay : MonoBehaviour
{
    public float activationDistance = 5f; // ���� ��� ���� �Ÿ�
    public AudioClip[] musicClips; // ����� ���� ���� ����Ʈ (Inspector���� ����)

    private Transform playerTransform;
    private AudioSource audioSource; // ����� ����� ���� ������Ʈ
    private bool isPlaying = false; // ���� ��� ���� ����
    public float aaa = 0.1f;
    private bool isWaiting = false; // ��� ������ Ȯ���ϴ� �÷���

    void Start()
    {
        // "Player" �±׸� ���� ������Ʈ ã��
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            Debug.Log("Player �±׸� ���� ������Ʈ�� ã�ҽ��ϴ�: " + player.name);
        }
        else
        {
            Debug.LogWarning("Player �±׸� ���� ������Ʈ�� ã�� �� �����ϴ�.");
        }

        // AudioSource ������Ʈ �������� �Ǵ� �߰��ϱ�
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            Debug.Log("AudioSource ������Ʈ�� �߰��Ǿ����ϴ�.");
        }

        // AudioSource �⺻ ����
        audioSource.loop = false; // ���� ����� ���� loop�� false�� ����
        audioSource.spatialBlend = 1.0f; // 3D ����� ����
        audioSource.maxDistance = activationDistance; // ���� �鸮�� �ִ� �Ÿ� ����

        if (musicClips != null && musicClips.Length > 0)
        {
            Debug.Log("musicClips ����Ʈ�� �����Ǿ����ϴ�. �� " + musicClips.Length + "���� ���� �ֽ��ϴ�.");
        }
        else
        {
            Debug.LogWarning("musicClips ����Ʈ�� ��� �ְų� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    void Update()
    {
        // playerTransform�� null�̸� ������Ʈ �ߴ�
        if (playerTransform == null) return;

        // ���� ������Ʈ�� �÷��̾� ���� �Ÿ� ���
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // �Ÿ��� activationDistance �����̸� ���� ���
        if (distance <= activationDistance)
        {
            if (!isPlaying && !isWaiting && musicClips != null && musicClips.Length > 0)
            {
                PlayRandomTrack();
                isPlaying = true;
                Debug.Log($"�Ÿ� {distance} <= {activationDistance}: ���� ��� ����");
            }
            // ���� ���� �������� Ȯ���ϰ� ���� ���� �� ��� (��� ���� �ƴ� ����)
            else if (isPlaying && !audioSource.isPlaying && !isWaiting)
            {
                StartCoroutine(WaitBeforeNextTrack(aaa)); // 5�� ��� �� ���� �� ���
                Debug.Log("���� ���� �� ��� �� 5�� ��� ����");
            }
        }
        else
        {
            if (isPlaying || isWaiting)
            {
                StopAllCoroutines(); // ��� ���� �ڷ�ƾ ����
                audioSource.Stop();
                isPlaying = false;
                isWaiting = false;
                Debug.Log($"�Ÿ� {distance} > {activationDistance}: ���� ��� �� ��� ����");
            }
        }
    }

    // �������� ���� Ʈ���� ������ ����ϴ� �޼���
    private void PlayRandomTrack()
    {
        if (musicClips.Length > 0)
        {
            int randomIndex = Random.Range(0, musicClips.Length);
            audioSource.clip = musicClips[randomIndex];
            audioSource.Play();
            Debug.Log($"���� �� ���: {audioSource.clip.name}");
        }
    }

    // ������ �ð�(��) ���� ��� �� ���� ���� ����ϴ� �ڷ�ƾ
    private IEnumerator WaitBeforeNextTrack(float waitTime)
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
        if (playerTransform != null && Vector3.Distance(transform.position, playerTransform.position) <= activationDistance)
        {
            PlayRandomTrack();
            Debug.Log("5�� ��� �� ���� ���� �� ���");
        }
        else
        {
            isPlaying = false;
            Debug.Log("5�� ��� �� �Ÿ� �ۿ� �־� ������� ����");
        }
    }
}
