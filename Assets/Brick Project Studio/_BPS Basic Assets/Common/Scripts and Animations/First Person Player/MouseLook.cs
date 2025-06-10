using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles
{
    public class MouseLook : MonoBehaviour
    {
        public static MouseLook instance;
        public float mouseXSensitivity = 100f;
        public Transform playerBody;
        float xRotation = 0f;
        private bool isCursorLocked = false; // 커서 잠금 상태를 추적

        private void Awake()
        {
            if(instance == null)
                instance = this;
            else
               Destroy(instance);
        }
        // Start is called before the first frame update
        void Start()
        {
            // 초기 상태: 마우스 커서를 킴
            SetCursorLock(false);

            // GameManager에서 저장된 카메라 회전값 가져오기
            Vector3 savedRotation = GameManager.Instance.GetCameraRotation();
            Debug.Log($"저장된 카메라 회전값 로드: {savedRotation}");

            // 카메라의 X축 회전값 설정 (상하 회전)
            xRotation = savedRotation.x;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // 회전값 제한 적용

            // 카메라의 로컬 회전 설정 (상하 회전)
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // 플레이어 본체의 Y축 회전 설정 (좌우 회전)
            if (playerBody != null)
            {
                playerBody.rotation = Quaternion.Euler(0f, savedRotation.y, 0f);
            }
            else
            {
                Debug.LogWarning("playerBody가 할당되지 않았습니다. Y축 회전값을 적용할 수 없습니다.");
            }
        }

        // Update is called once per frame
        void Update()
        {
            
            // Esc 키로 커서 잠금/해제 토글
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!Dialogue_Manage.Instance.isEndLine()) //대화끝날때까지 막기.
                {
                    return;
                }
                ToggleLock();
            }

            // 커서가 잠겨 있을 때만 카메라 회전 처리
            if (isCursorLocked)
            {
                float mouseX = Input.GetAxis("Mouse X") * mouseXSensitivity * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * mouseXSensitivity * Time.deltaTime;

                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);

                transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                playerBody.Rotate(Vector3.up * mouseX);
            }
        }

        // 커서 잠금 상태 설정 메서드
        private void SetCursorLock(bool lockCursor)
        {
            if (lockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        public bool isLockOn()
        {
            if (isCursorLocked)
                return true;
            else
                return false;
        }
        public void ToggleLock()
        {
            Debug.Log("토글락 실행");
            isCursorLocked = !isCursorLocked;
            SetCursorLock(isCursorLocked);
        }
        // 스크립트가 비활성화될 때 커서 잠금 해제
        void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
