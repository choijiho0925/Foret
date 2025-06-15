using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _02.Scripts.Player
{
    public class PlayerCtrl : MonoBehaviour 
    {
        private PlayerMovement playerMovement;
        private PlayerStat playerStat;
        private PlayerAttack playerAttack;
        private PlayerInteract playerInteract;

        public bool canControl = true;      //플레이어 이동 가능 여부
        
        private Vector2 moveInput;
        private bool jump = false;
        private bool dash = false;
        
        private void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
            playerStat = GetComponent<PlayerStat>();
            playerAttack = GetComponent<PlayerAttack>();
            playerInteract = GetComponent<PlayerInteract>();
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
                playerAttack.ThrowAttack(moveInput);
            }
        }
        
        //'상호작용' 상태일 때는 모든 움직임 제한
        public void EnterInteraction()
        {
            canControl = false;
        }
        public void ExitInteraction()
        {
            canControl = true;
        }

        private void FixedUpdate()
        {
            if (!canControl)
            {
                jump = false;
                dash = false;
                return;
            }
            
            playerMovement.Move(moveInput.x * playerStat.CurrentMoveSpeed * Time.fixedDeltaTime, jump, dash);
            jump = false;
            dash = false;
        }
    }
}