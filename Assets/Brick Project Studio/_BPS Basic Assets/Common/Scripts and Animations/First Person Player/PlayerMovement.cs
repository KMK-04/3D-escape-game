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

        public bool canMove = true; // 이동 가능 여부 제어용

        void Start()
        {
            Vector3 savedPosition = GameManager.Instance.GetPlayerPosition();
            if (controller != null)
            {
                controller.enabled = false;
                transform.position = savedPosition;
                controller.enabled = true;
            }
            else
            {
                transform.position = savedPosition;
            }
        }

        void Update()
        {
            if (!canMove) return;

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        public void SetMovement(bool allow)
        {
            canMove = allow;
        }
    }
}
