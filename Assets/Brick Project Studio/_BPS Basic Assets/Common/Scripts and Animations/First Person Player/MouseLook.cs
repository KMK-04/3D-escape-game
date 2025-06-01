using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles
{
    public class MouseLook : MonoBehaviour
    {
        public float mouseXSensitivity = 100f;
        public Transform playerBody;
        float xRotation = 0f;
        private bool isCursorLocked = true; // 커서 잠금 상태를 추적

        // Start is called before the first frame update
        void Start()
        {
            // 초기 상태: 마우스 커서를 잠그고 숨김
            SetCursorLock(true);
        }

        // Update is called once per frame
        void Update()
        {
            // Esc 키로 커서 잠금/해제 토글
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isCursorLocked = !isCursorLocked;
                SetCursorLock(isCursorLocked);
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

        // 스크립트가 비활성화될 때 커서 잠금 해제
        void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
