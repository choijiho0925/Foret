using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _02.Scripts.Player
{
    public class PlayerCtrl : MonoBehaviour 
    {

        private PlayerMovement playerMovement;
        private Animator animator;

        public float runSpeed = 40f;

        float horizontalMove = 0f;
        bool jump = false;
        bool dash = false;

        private void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
            animator = GetComponentInChildren<Animator>();
        }

        private void OnEnable()
        {
            playerMovement.OnLandEvent.AddListener(OnLanding);
        }

        private void OnDisable()
        {
            playerMovement.OnLandEvent.RemoveListener(OnLanding);
        }

        //bool dashAxis = false;
	
        void Update ()
        {

            //horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            // 데드존 설정 
            if (Mathf.Abs(horizontalMove) < 0.01f)
            {
                horizontalMove = 0f;
            }

            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

            if (Input.GetKeyDown(KeyCode.Z))
            {
                jump = true;
            }

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
                horizontalMove  = context.ReadValue<Vector2>().x;
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                horizontalMove = 0;
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

        private void FixedUpdate ()
        {
            playerMovement.Move(horizontalMove * runSpeed * Time.fixedDeltaTime, jump, dash);
            jump = false;
            dash = false;
        }
    }
}