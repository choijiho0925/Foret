using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _02.Scripts.Player
{
    public class PlayerCtrl : MonoBehaviour 
    {

        private PlayerMovement playerMovement;
        private PlayerAttack playerAttack;
        private Animator animator;

        public float runSpeed = 40f;

        private Vector2 moveInput;
        bool jump = false;
        bool dash = false;

        private void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
            playerAttack = GetComponent<PlayerAttack>();
            animator = GetComponentInChildren<Animator>();
        }

        private void OnEnable()
        {
            playerMovement.OnLandEvent.AddListener(OnLanding);
            playerMovement.OnFallEvent.AddListener(OnFall);
        }

        private void OnDisable()
        {
            playerMovement.OnLandEvent.RemoveListener(OnLanding);
            playerMovement.OnFallEvent.RemoveListener(OnFall);
        }

        //bool dashAxis = false;
	
        void Update ()
        {

            //horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;


            if (Input.GetKeyDown(KeyCode.C))
            {
                dash = true;
            }

            /*if (Input.GetAxisRaw("Dash") == 1 || Input.GetAxisRaw("Dash") == -1) //RT in Unity 2017 = -1, RT in Unity 2019 = 1
        {
            if (dashAxis == false)
            {
                dashAxis = true;
                dash = true;
            }
        }
        else
        {
            dashAxis = false;
        }
        */

        }

        public void OnFall()
        {
            animator.SetBool("IsJumping", true);
        }

        private void OnLanding()
        {
            animator.SetBool("IsJumping", false);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                moveInput = context.ReadValue<Vector2>();
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
               moveInput = Vector2.zero;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                jump = true;
            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                dash = true;
            }
        }
        
        public void OnAttack(InputAction.CallbackContext context)
        {
            // 공격 키가 눌렸을 때
            if (context.phase == InputActionPhase.Started)
            {
                // PlayerAttack 스크립트에 현재 이동 방향(moveInput)을 전달하며 공격 요청
                playerAttack.PerformAttack(moveInput);
            }
        }

        private void FixedUpdate ()
        {
            playerMovement.Move(moveInput.x * runSpeed * Time.fixedDeltaTime, jump, dash);
            jump = false;
            dash = false;
        }
    }
}