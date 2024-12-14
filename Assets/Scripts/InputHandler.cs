using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

namespace wwy
{

    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool b_Input;
        public bool sprintFlag;
        public bool rollFlag;
        public float rollInputTimer;
        public bool rb_Input;
        public bool rt_Input;

        public float durationForRoll2Sprint = 0.3f;

        PlayerControls inputActions;
        PlayerAttacker playerAttacker;
        PlayerInventory playerInventory;

        Vector2 movementInput;
        Vector2 cameraInput;


        private void Awake()
        {
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory= GetComponent<PlayerInventory>();   
        }

        private void OnEnable()
        {
            if(inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += (inputActions) =>
                { movementInput = inputActions.ReadValue<Vector2>(); };
                inputActions.PlayerMovement.Camera.performed += (inputActions) =>
                { cameraInput = inputActions.ReadValue<Vector2>(); };
            }
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
            HandleRollInput(delta);
            HandleAttackInput(delta);
        }

        private void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            //float inputMagnitude = Mathf.Clamp01(Mathf.Sqrt(horizontal * horizontal + vertical * vertical));
            //Debug.Log($"{moveAmount}, {inputMagnitude}");
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }
        private void HandleRollInput(float delta)
        {
            b_Input = inputActions.PlayerActions.Roll.IsPressed();
            if (b_Input)
            {
                rollInputTimer += delta;
                if (rollInputTimer >= durationForRoll2Sprint)
                {
                    sprintFlag = true;
                }
            }
            else
            {
                if(rollInputTimer > 0 && rollInputTimer < durationForRoll2Sprint)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }
                rollInputTimer = 0;
            }
        }
        
        private void HandleAttackInput(float delta)
        {
            inputActions.PlayerActions.RB.performed += i => rb_Input = true;
            inputActions.PlayerActions.RT.performed += i => rt_Input = true;

            if (rb_Input)
            {
                playerAttacker.HandleLigthAttack(playerInventory.rightWeapon);
            }
            if (rt_Input)
            {
                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            }
        }
    }
}
