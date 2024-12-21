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
        public bool comboFlag;
        public bool rollFlag;
        public float rollInputTimer;
        public bool rb_Input;
        public bool rt_Input;

        public bool d_Pad_Up;
        public bool d_Pad_Down;
        public bool d_Pad_Left;
        public bool d_Pad_Right;

        public float durationForRoll2Sprint = 0.3f;

        PlayerControls inputActions;
        PlayerAttacker playerAttacker;
        PlayerInventory playerInventory;
        PlayerManager playerManager;

        Vector2 movementInput;
        Vector2 cameraInput;


        private void Awake()
        {
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory= GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
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
            HandleQuickSlotsInput(delta);
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
                if (playerManager.canDoCombo)
                {
                    comboFlag = true;
                    playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                    comboFlag = false;
                }
                else
                {
                    if (playerManager.isInteracting) return;
                    if (playerManager.canDoCombo) return;
                    playerAttacker.HandleLigthAttack(playerInventory.rightWeapon);
                }
            }
            if (rt_Input)
            {
                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            }
        }
        private void HandleQuickSlotsInput(float delta)
        {
            inputActions.PlayerQuickSlots.D_Pad_Right.performed += i => { d_Pad_Right = true; };
            inputActions.PlayerQuickSlots.D_Pad_Up.performed += i => { d_Pad_Up = true; };
            inputActions.PlayerQuickSlots.D_Pad_Down.performed += i => { d_Pad_Down = true; };
            inputActions.PlayerQuickSlots.D_Pad_Left.performed += i => { d_Pad_Left = true; };
            if (d_Pad_Right)
            {
                playerInventory.ChangeRightWeapon();
            }

            else if(d_Pad_Left)
            {
                playerInventory.ChangeLeftWeapon();
            }
        }
    }
}
