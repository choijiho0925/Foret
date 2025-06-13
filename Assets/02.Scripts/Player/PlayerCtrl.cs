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


        public float runSpeed = 40f;

        private Vector2 moveInput;
        bool jump = false;
        bool dash = false;

        private void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
            playerAttack = GetComponent<PlayerAttack>();
        }
        

        //bool dashAxis = false;
        

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
            // 일반 공격
            if (context.phase == InputActionPhase.Started)
            {
                // PlayerAttack 스크립트에 현재 이동 방향(moveInput)을 전달하며 공격 요청
                playerAttack.PerformAttack(moveInput);
            }
        }   
        public void OnThrow(InputAction.CallbackContext context)
        {
            // 원거리 공격
            if (context.phase == InputActionPhase.Started)
            {
                // PlayerAttack 스크립트에 현재 이동 방향(moveInput)을 전달하며 공격 요청
                playerAttack.ThrowAttack();
            }
        }

        private void FixedUpdate()
        {
            playerMovement.Move(moveInput.x * runSpeed * Time.fixedDeltaTime, jump, dash);
            jump = false;
            dash = false;
        }
    }
}