using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles
{
    public class PlayerMovement : MonoBehaviour
    {
        public CharacterController controller;
        public float speed = 5f;
        public float gravity = -15f;
        Vector3 velocity;
        bool isGrounded;

        void Start()
        {
            // GameManager에서 저장된 플레이어 위치 가져오기
            Vector3 savedPosition = GameManager.Instance.GetPlayerPosition();
            // 플레이어를 저장된 위치로 이동
            if (controller != null)
            {
                controller.enabled = false; // 이동 전 CharacterController 비활성화
                transform.position = savedPosition;
                controller.enabled = true; // 이동 후 다시 활성화
                Debug.Log($"플레이어 위치 초기화: {savedPosition}");
            }
            else
            {
                transform.position = savedPosition;
                Debug.Log($"플레이어 위치 초기화 (CharacterController 없음): {savedPosition}");
            }
        }

        void Update()
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
        }
    }
}
